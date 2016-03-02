using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.SqlClient;
using static OpenRnD.Harness.SQLServer.Tests.DatabaseTestHelper;

namespace OpenRnD.Harness.SQLServer.Tests
{
    [TestClass]
    public class BackupHarnessTests
    {
        [TestMethod]
        public void BackupHarness()
        {
            SqlConnection connection = CreateConnection(GetConnectionString());
            CreateDatabase(connection);

            using (SQLServerBackupHarness harness = new SQLServerBackupHarness(GetConnectionString(), "C:/backups/harnessBackup.bak"))
            {
                InsertValues(connection);
            }

            AssertValuesAreGone(connection);

            DropDatabase(connection);
        }
    }
}
