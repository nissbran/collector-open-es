using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.Exceptions;
using Serilog;

namespace DevOpen.Framework.Infrastructure.Subscriptions
{
    public abstract class EventStoreCatchUpAllSubscriber : EventStoreSubscriber
    {
        private readonly string _subscriptionName;
        private readonly CatchAllSubscriptionSettings _settings;

        private readonly object _subscriptionModificationLock = new object();

        private int _retryAttempts;
        private int _subscriptionEpoch;
        private bool _subscriptionTerminatedRequested;
        private bool _subscriptionEpochStopping;

        private EventStoreAllCatchUpSubscription _catchUpSubscription;

        protected long LastCommitPosition { get; set; }

        public event EventHandler<SubscriptionTerminatedEventArgs> SubscriptionTerminated;

        public string SubscriptionName => $"{_subscriptionName}_{_subscriptionEpoch}";

        protected EventStoreCatchUpAllSubscriber(
            string subscriptionName,
            IEventStoreConnection connection,
            CatchAllSubscriptionSettings settings) : base(connection)
        {
            _subscriptionName = subscriptionName;
            _settings = settings;

            LastCommitPosition = settings.LastCommitPosition;
        }

        private void StartNewSubscriptionEpoch()
        {
            Task.Run(() =>
            {
                lock (_subscriptionModificationLock)
                {
                    if (_subscriptionTerminatedRequested)
                        return;

                    BeforeSubscriptionEpochStarts();

                    StopSubscriptionEpoch();

                    Interlocked.Increment(ref _subscriptionEpoch);

                    var subscriptionSettings = new CatchUpSubscriptionSettings(
                        maxLiveQueueSize: 500,
                        readBatchSize: _settings.ReadBatchSize,
                        verboseLogging: false,
                        resolveLinkTos: _settings.ResolveLinkEvents,
                        subscriptionName: SubscriptionName);

                    Log.Information("Starting a new subscription epoch {Epoch} from commit position {LastCommitPosition} with name {Name}", _subscriptionEpoch, LastCommitPosition,
                        SubscriptionName);

                    _catchUpSubscription = Connection.SubscribeToAllFrom(
                        lastCheckpoint: new Position(LastCommitPosition, LastCommitPosition),
                        settings: subscriptionSettings,
                        eventAppeared: EventAppeared,
                        liveProcessingStarted: LiveProcessingStarted,
                        subscriptionDropped: SubscriptionDropped);
                }
            });
        }

        public void StopSubscription()
        {
            TerminateSubscription();
        }

        protected virtual void BeforeSubscriptionEpochStarts()
        {
        }

        private void TerminateSubscription()
        {
            lock (_subscriptionModificationLock)
            {
                if (_subscriptionTerminatedRequested)
                    return;

                _subscriptionTerminatedRequested = true;

                StopSubscriptionEpoch();

                OnSubscriptionTerminated();
            }
        }

        private void StopSubscriptionEpoch()
        {
            if (_catchUpSubscription == null)
                return;

            try
            {
                _subscriptionEpochStopping = true;

                Log.Information("Stopping catch all subscription epoch {Name}! Waiting for queued events to process.", SubscriptionName);

                var stopSubscriptionWatch = Stopwatch.StartNew();

                _catchUpSubscription.Stop(TimeSpan.FromMilliseconds(_settings.StopSubscriptionTimeout));

                Log.Information("Subscription epoch {Name} stopped after {Elapsed} ms!", SubscriptionName, stopSubscriptionWatch.ElapsedMilliseconds);
            }
            catch (TimeoutException e)
            {
                Log.Error(e, "Could not stop the subscription epoch {Name} after {Timeout} ms!", SubscriptionName, _settings.StopSubscriptionTimeout);
            }
            finally
            {
                _subscriptionEpochStopping = false;
            }
        }

        private async Task EventAppeared(EventStoreCatchUpSubscription subscription, ResolvedEvent resolvedEvent)
        {
            if (_subscriptionEpochStopping ||
                !IsValidEvent(resolvedEvent))
                return;

            // ReSharper disable once PossibleInvalidOperationException
            var commitPosition = resolvedEvent.OriginalPosition.Value.CommitPosition;

            try
            {
                await EventAppeared(resolvedEvent, commitPosition);
            }
            catch (Exception e)
            {
                if (Retry())
                {
                    Log.Warning(e, "Exception received for subscription epoch {Name}. Retrying...", SubscriptionName);
                    throw;
                }

                if (_settings.StopOnException)
                {
                    TerminateSubscriptionDueToError(resolvedEvent, e);
                    return;
                }

                ExceptionReceivedAfterMaxRetries(resolvedEvent, e);
            }

            await OnUpdateLastCommitPosition(commitPosition);
        }

        private bool Retry()
        {
            _retryAttempts++;

            if (_retryAttempts >= _settings.MaxRetryAttempts)
            {
                _retryAttempts = 0;
                return false;
            }

            return true;
        }

        private void TerminateSubscriptionDueToError(ResolvedEvent resolvedEvent, Exception exception)
        {
            SubscriptionTerminatedDueToException(resolvedEvent, exception);
            _subscriptionEpochStopping = true;
            Task.Run(() =>
            {
                TerminateSubscription();
                SubscriptionTerminated?.Invoke(this, new SubscriptionTerminatedEventArgs());
            });
        }

        private void SubscriptionDropped(EventStoreCatchUpSubscription subscription, SubscriptionDropReason reason, Exception exception)
        {
            OnSubscriptionDropped(subscription, reason, exception);

            if (reason == SubscriptionDropReason.UserInitiated)
                return;

            if (_subscriptionTerminatedRequested)
                return;

            if (reason == SubscriptionDropReason.EventHandlerException ||
                reason == SubscriptionDropReason.CatchUpError ||
                reason == SubscriptionDropReason.ProcessingQueueOverflow)
            {
                if (exception is ServerErrorException serverError)
                {
                    OnServerError(serverError);
                    return;
                }

                StartNewSubscriptionEpoch();
            }

            if (reason == SubscriptionDropReason.ConnectionClosed)
            {
                Log.Information(
                    "Subscription {Name} with epoch {Epoch} dropped with {Reason}, which means that either the single node is dead and no other node found through gossip. Waiting for reconnect to start a new epoch.",
                    _subscriptionName,
                    _subscriptionEpoch,
                    SubscriptionDropReason.ConnectionClosed);
                _subscriptionEpochStopping = true;
                _catchUpSubscription.Stop();
                return;
            }

            Log.Information("Subscription dropped with reason {Reason}", reason);
        }

        protected virtual void LiveProcessingStarted(EventStoreCatchUpSubscription subscription)
        {
        }

        protected virtual void OnServerError(ServerErrorException serverError)
        {
            Log.Error(serverError, "An server error occurred when receiving an event from EventStore");
        }

        protected virtual void SubscriptionTerminatedDueToException(ResolvedEvent resolvedEvent, Exception exception)
        {
        }

        protected virtual void ExceptionReceivedAfterMaxRetries(ResolvedEvent resolvedEvent, Exception exception)
        {
            Log.Error(exception,
                "An error occurred when receiving {EventType} with {EventNumber} from {Stream} from EventStore. Retried {RetryAttempts} times.",
                resolvedEvent.Event.EventType,
                resolvedEvent.Event.EventNumber,
                resolvedEvent.Event.EventStreamId,
                _settings.MaxRetryAttempts);
        }

        protected virtual void OnSubscriptionDropped(EventStoreCatchUpSubscription subscription, SubscriptionDropReason reason, Exception exception)
        {
        }

        protected virtual void OnSubscriptionTerminated()
        {
        }

        protected virtual Task OnUpdateLastCommitPosition(long newPosition)
        {
            LastCommitPosition = newPosition;

            return Task.CompletedTask;
        }

        protected override void OnConnected(object sender, ClientConnectionEventArgs clientConnectionEventArg)
        {
            StartNewSubscriptionEpoch();
        }

        protected override void OnDisconnected(object sender, ClientConnectionEventArgs clientConnectionEventArgs)
        {
            _catchUpSubscription?.Stop();

            if (!_subscriptionTerminatedRequested)
                Log.Information("Subscription {Name} with epoch {Epoch}, disconnected from EventStore. Waiting for reconnect to start a new epoch.", _subscriptionName, _subscriptionEpoch);
        }
    }

    public class SubscriptionTerminatedEventArgs
    {
    }
}