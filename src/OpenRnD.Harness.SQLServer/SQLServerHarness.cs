using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRnD.Harness.SQLServer
{
    public abstract class SQLServerHarness : IDisposable
    {
        public SqlConnection Connection { get; }
        public string DatabaseName { get; }

        public SQLServerHarness(string connectionString)
        {
            if(string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            string databaseName;
            string instanceConnectionString = DatabaseHelper.RemoveInitialCatalog(connectionString, out databaseName);
            DatabaseName = databaseName;

            Connection = new SqlConnection(instanceConnectionString);
            Connection.Open();
        }

        public virtual void Dispose()
        {
            Connection.Dispose();
        }
    }
}
