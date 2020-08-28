using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using DevOpen.Domain.Model;
using DevOpen.Domain.Model.Credits;
using DevOpen.ReadModel.Credits;

namespace DevOpen.Infrastructure.Persistence.Sql
{
    public class CreditSqlLookup : SqlStorage, ICreditLookup
    {
        public async Task<IList<CreditId>> GetCreditsForOrganisationNumber(OrganisationNumber organisationNumber)
        {
            var listOfCredits = new List<CreditId>();
            
            using (var connection = new SqlConnection(ConnectionString))
            {
                const string sql = "SELECT CreditId FROM CreditLookup WHERE OrganisationNumber = @organisationNumber";
                
                await connection.OpenAsync();

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@organisationNumber", organisationNumber.Number);

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            listOfCredits.Add(CreditId.Parse(reader.GetGuid(0)));
                        }
                    }
                }
            }

            return listOfCredits;
        }

        public async Task AddCreditToLookup(CreditId creditId, OrganisationNumber organisationNumber)
        {
            using var connection = new SqlConnection(ConnectionString);
            
            const string sql = "INSERT INTO CreditLookup (CreditId, OrganisationNumber, CreatedDate) VALUES (@creditId,@organisationNumber,@createdDate)";
                
            await connection.OpenAsync();

            using var command = new SqlCommand(sql, connection);
            
            command.Parameters.AddWithValue("@creditId", (Guid) creditId);
            command.Parameters.AddWithValue("@organisationNumber", organisationNumber.Number);
            command.Parameters.AddWithValue("@createdDate", DateTimeOffset.Now);

            await command.ExecuteNonQueryAsync();
        }

        public void ClearAll()
        {
            using var connection = new SqlConnection(ConnectionString);
            
            const string sql = "TRUNCATE TABLE CreditLookup";
            
            connection.Open();

            using var command = new SqlCommand(sql, connection);

            command.ExecuteNonQuery();
        }
    }
}