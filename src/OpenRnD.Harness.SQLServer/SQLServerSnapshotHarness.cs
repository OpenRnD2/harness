using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static OpenRnD.Harness.SQLServer.DatabaseHelper;

namespace OpenRnD.Harness.SQLServer
{
    public class SQLServerSnapshotHarness : SQLServerHarness
    {
        public string SnapshotName { get; }
        public string SnapshotPath { get; }

        public SQLServerSnapshotHarness(string connectionString)
            : base(connectionString)
        {
            SnapshotName = Assembly.GetCallingAssembly().GetName().Name;
            SnapshotPath = Path.Combine(Environment.CurrentDirectory, SnapshotName + ".ss");

            CreateSnapshot();
        }

        private void CreateSnapshot()
        {
            ExecuteNonQuery(Connection, $@"
                IF (DB_ID('{SnapshotName}') IS NOT NULL)
                BEGIN
                    USE master;
                    ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    RESTORE DATABASE [{DatabaseName}] from DATABASE_SNAPSHOT='{SnapshotName}';
                    ALTER DATABASE [{DatabaseName}] SET MULTI_USER;
                    DROP DATABASE [{SnapshotName}];
                END");

            ExecuteNonQuery(Connection, $@"
                CREATE DATABASE [{SnapshotName}] ON ( 
                    NAME = [{DatabaseName}],
                    FILENAME='{SnapshotPath}')
                AS SNAPSHOT OF [{DatabaseName}]");
        }

        private void RestoreSnapshot()
        {
            ExecuteNonQuery(Connection, $@"
                USE master; 
                ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [{DatabaseName}] FROM DATABASE_SNAPSHOT = '{SnapshotName}';");
        }

        public override void Dispose()
        {
            RestoreSnapshot();

            base.Dispose();
        }
    }
}
