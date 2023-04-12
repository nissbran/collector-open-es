using System;
using EventStore.ClientAPI;

namespace DevOpen.Infrastructure.Configuration
{
    public static class EventStoreConnectionFactory
    {
        public static IEventStoreConnection Create(
            Uri connectionString,
            ILogger customLogger = null,
            string connectionName = null)
        {
            var connectionSettings = ConnectionSettings.Create()
                .UseConsoleLogger()
                .DisableTls()
                .DisableServerCertificateValidation();

            if (customLogger != null)
                connectionSettings.UseCustomLogger(customLogger);

            return EventStoreConnection.Create(connectionSettings, connectionString, connectionName);
        }
    }
}