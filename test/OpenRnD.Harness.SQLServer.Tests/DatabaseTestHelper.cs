using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenRnD.Harness.SQLServer.DatabaseHelper;

namespace OpenRnD.Harness.SQLServer.Tests
{
    internal static class DatabaseTestHelper
    {
        private const string databaseName = "TestDB";
        private const string tableName = "TestTABLE";

        public static SqlConnection CreateConnection(string connectionString)
        {
            string instanceConnectionString = RemoveInitialCatalog(connectionString);
            SqlConnection connection = new SqlConnection(instanceConnectionString);
            connection.Open();

            return connection;
        }

        public static void CreateDatabase(SqlConnection connection)
        {
            DatabaseHelper.DropDatabase(connection, databaseName);

            ExecuteNonQuery(connection, $"CREATE DATABASE [{databaseName}];");
            ExecuteNonQuery(connection, $"USE [{databaseName}]; CREATE TABLE [{tableName}] (Id INT);");
        }

        public static void InsertValues(SqlConnection connection)
        {
            ExecuteNonQuery(connection, $"INSERT INTO [{tableName}](Id) VALUES (1), (2), (3);");
        }

        public static void DropDatabase(SqlConnection connection)
        {
            DatabaseHelper.DropDatabase(connection, databaseName);
        }

        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["database"].ConnectionString;
        }

        public static void AssertValuesAreGone(SqlConnection connection)
        {
            int count = ExecuteScalar<int>(connection, $"SELECT COUNT(*) FROM {tableName};");

            Assert.AreEqual(0, count);
        }
    }
}
