using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DevOpen.Framework.Application.Processes;

namespace DevOpen.Infrastructure.Persistence.Sql
{
    public class ProcessedEventsSqlStorage : SqlStorage, IProcessedEventsStorage
    {
        public async Task<bool> Exists(string processName, string eventType, Guid eventId)
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();
            const string sql = "SELECT COUNT(*) FROM ProcessedEvents WHERE EventId = @EventId AND EventType = @EventType AND ProcessName = @ProcessName";
            
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@EventId", eventId);
            command.Parameters.AddWithValue("@EventType", eventType);
            command.Parameters.AddWithValue("@ProcessName", processName);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result) > 0;
        }

        public async Task Add(string processName, string eventType, Guid eventId)
        {
            using var connection = new SqlConnection(ConnectionString);                
            await connection.OpenAsync();
            
            const string sql = "INSERT INTO ProcessedEvents (EventId, EventType, ProcessName, CreatedDate) VALUES (@eventId, @eventType, @processName, @createdDate)";

            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@eventId", eventId);
            command.Parameters.AddWithValue("@eventType", eventType);
            command.Parameters.AddWithValue("@processName", processName);
            command.Parameters.AddWithValue("@createdDate", DateTime.Now);

            await command.ExecuteNonQueryAsync();
        }
    }
}