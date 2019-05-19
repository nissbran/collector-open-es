using System.Data.SqlClient;

namespace DevOpen.Infrastructure.Persistence.Sql
{
    public abstract class SqlStorage
    {
        protected readonly string ConnectionString;
        
        protected SqlStorage()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = "localhost", 
                UserID = "sa", 
                Password = "GqMxaCiCKEzWTT6hdMwBDArb7ZnHboVmawNgL7YFkNU2A", 
                InitialCatalog = "readmodel"
            };

            ConnectionString = builder.ConnectionString;
        }
    }
}