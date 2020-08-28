using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DevOpen.ReadModel;
using Serilog;

namespace DevOpen.Infrastructure.Persistence
{
    public class RebuildCoordinator
    {
        private readonly List<IReadModelBuilder> _readModelBuilders;
        private readonly Stopwatch _rebuildWatch = new Stopwatch();
        
        public bool IsRebuilding { get; private set; } = false;

        public RebuildCoordinator(IEnumerable<IReadModelBuilder> readModelBuilders)
        {
            _readModelBuilders = readModelBuilders.ToList();
        }

        public void ClearModels()
        {
            foreach (var readModelBuilder in _readModelBuilders)
            {
                readModelBuilder.ClearModel();
            }
        }
        
        public void StartRebuild()
        {
            Log.Information("Starting new catchup subscription from start, building view models in memory...");
            
           // FindAllDeletedStreams();
            
            //Log.Information("Reducing loglevel to only log errors while rebuilding view models to avoid spam. (Check Trace for information)");
            //_logger.LogOnlyErrors();
            
            //_switchHandler.Prepare();
            _rebuildWatch.Start();
            IsRebuilding = true;
        }
        
        public void FinishRebuild()
        {
            Log.Information("Rebuild finished! Switching from in memory to persistent storage! Time elapsed: {Elapsed}", _rebuildWatch.Elapsed);
            // _logger.TurnLoggingOn();
            // _logger.LogInformation("Serilogger turned back on");
            //_switchHandler.Switch();
            foreach (var readModelBuilder in _readModelBuilders)
            {
                readModelBuilder.Switch();
            }
            _rebuildWatch.Reset();
            IsRebuilding = false;
        }

        public void ReportRebuildProgress(long eventsProcessed)
        {
            
        }
    }
}