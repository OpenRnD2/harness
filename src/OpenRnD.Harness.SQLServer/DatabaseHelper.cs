using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRnD.Harness.SQLServer
{
    public static class DatabaseHelper
    {
        public static void ExecuteNonQuery(SqlConnection connection, string commandText)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }

        public static SqlDataReader ExecuteQuery(SqlConnection connection, string commandText)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            SqlDataReader dataReader = command.ExecuteReader();

            return dataReader;
        }

        public static T ExecuteScalar<T>(SqlConnection connection, string commandText)
        {
            SqlCommand command = connection.CreateCommand();
            command.CommandText = commandText;
            T result = (T)command.ExecuteScalar();

            return result;
        }

        public static void DropDatabase(SqlConnection connection, string databaseName)
        {
            bool exists = ExecuteScalar<object>(connection, $"SELECT DB_ID('{databaseName}');") != DBNull.Value;
            
            if(exists)
            {
                List<string> snapshots = new List<string>();

                using (SqlDataReader reader = ExecuteQuery(connection, $@"
                SELECT name FROM sys.databases WHERE source_database_id = (SELECT database_id FROM sys.databases WHERE name = '{databaseName}');"))
                {
                    while (reader.Read())
                    {
                        snapshots.Add(reader.GetString(0));
                    }
                }

                foreach(string snapshot in snapshots)
                {
                    DropDatabase(connection, snapshot);
                }

                ExecuteNonQuery(connection, $"USE master; DROP DATABASE [{databaseName}];");
            }
        }

        public static string RemoveInitialCatalog(string connectionString)
        {
            string initialCatalog;
            return RemoveInitialCatalog(connectionString, out initialCatalog);
        }

        public static string RemoveInitialCatalog(string connectionString, out string initialCatalog)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder(connectionString);
            initialCatalog = builder.InitialCatalog;
            builder.Remove("Initial Catalog");
            string instanceConnectionString = builder.ToString();

            return instanceConnectionString;
        }
    }
}
