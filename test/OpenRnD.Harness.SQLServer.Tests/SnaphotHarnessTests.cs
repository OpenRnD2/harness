using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using System.Data.SqlClient;
using static OpenRnD.Harness.SQLServer.Tests.DatabaseTestHelper;

namespace OpenRnD.Harness.SQLServer.Tests
{
    [TestClass]
    public class SnaphotHarnessTests
    {
        [TestMethod]
        public void SnaphotHarness()
        {
            SqlConnection connection = CreateConnection(GetConnectionString());
            CreateDatabase(connection);

            using (SQLServerSnapshotHarness harness = new SQLServerSnapshotHarness(GetConnectionString()))
            {
                Assert.AreEqual(expected: "TestDB", actual: harness.DatabaseName);
                Assert.AreEqual(expected: "OpenRnD.Harness.SQLServer.Tests", actual: harness.SnapshotName);

                InsertValues(connection);
            }
            
            DropDatabase(connection);
        }
    }
}
