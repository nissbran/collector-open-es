using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DevOpen.Framework.Infrastructure.Subscriptions;

namespace DevOpen.Infrastructure.Persistence.Sql
{
    public class SubscriptionCheckpointStorage : SqlStorage
    {
        public SubscriptionCheckpoint Load(Guid subscriptionId)
        {
            var previousCheckpoint = GetCheckpoint(subscriptionId) ?? CreateNewSubscriptionCheckpoint(subscriptionId);

            return previousCheckpoint;
        }

        public async Task Save(SubscriptionCheckpoint checkpoint)
        {
            var newCheckpointDate = DateTime.UtcNow;
            
            using (var connection = new SqlConnection(ConnectionString))
            {
                const string sql = "UPDATE SubscriptionCheckpoint SET LastProcessedPosition = @checkpoint, EventsProcessed = @eventsProcessed, UpdatedUtc = @updated WHERE SubscriptionId = @subscriptionId";
                
                await connection.OpenAsync();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@subscriptionId", checkpoint.SubscriptionId);
                    command.Parameters.AddWithValue("@checkpoint", checkpoint.LastProcessedPosition);
                    command.Parameters.AddWithValue("@eventsProcessed", checkpoint.EventsProcessed);
                    command.Parameters.AddWithValue("@updated", checkpoint.UpdatedUtc);

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        
        private SubscriptionCheckpoint GetCheckpoint(Guid subscriptionId)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                const string sql = "SELECT * FROM SubscriptionCheckpoint WHERE SubscriptionId = @subscriptionId";
                
                connection.Open();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@subscriptionId", subscriptionId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            
                            return new SubscriptionCheckpoint(
                                Guid.Parse(Convert.ToString(reader["SubscriptionId"])),
                                Convert.ToInt64(reader["LastProcessedPosition"]),
                                Convert.ToInt64(reader["EventsProcessed"]),
                                Convert.ToDateTime(reader["UpdatedUtc"]));
                        }

                        return null;
                    }
                }
            }
        }

        private SubscriptionCheckpoint CreateNewSubscriptionCheckpoint(Guid subscriptionId)
        {
            var newCheckpointDate = DateTime.UtcNow;
            
            using (var connection = new SqlConnection(ConnectionString))
            {
                const string sql = "INSERT INTO SubscriptionCheckpoint (SubscriptionId, LastProcessedPosition, EventsProcessed, UpdatedUtc) VALUES (@subscriptionId, 0, 0, @updated)";
                
                connection.Open();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@subscriptionId", subscriptionId);
                    command.Parameters.AddWithValue("@updated",newCheckpointDate);

                    command.ExecuteNonQuery();
                }
            }
            
            return new SubscriptionCheckpoint(subscriptionId, 0, 0, newCheckpointDate);
        }
    }
}