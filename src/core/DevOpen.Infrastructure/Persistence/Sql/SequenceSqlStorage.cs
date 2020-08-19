using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DevOpen.Application.Repositories;

namespace DevOpen.Infrastructure.Persistence.Sql
{
    public class CreditNumberSqlStorage : SqlStorage, ICreditNumberRepository
    {
        public async Task<long> GetCurrentAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();
            const string sql = "SELECT current_value FROM sys.sequences WHERE name = 'CreditNumberSequence'";

            using var command = new SqlCommand(sql, connection);
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt64(result);
        }

        public async Task<long> GetNextAsync()
        {
            using var connection = new SqlConnection(ConnectionString);
            await connection.OpenAsync();
            const string sql = "SELECT NEXT VALUE FOR CreditNumberSequence";
            using var command = new SqlCommand(sql, connection);
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt64(result);
        }
    }
}