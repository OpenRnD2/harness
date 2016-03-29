using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OpenRnD.Harness.SQLServer.DatabaseHelper;

namespace OpenRnD.Harness.SQLServer
{
    public class SQLServerBackupHarness : SQLServerHarness
    {
        public string BackupPath { get; }

        public SQLServerBackupHarness(string connectionString, string backupPath)
            : base(connectionString)
        {
            // TODO: Determine backup path from ???

            BackupPath = backupPath;

            CreateBackup();
        }

        private void CreateBackup()
        {
            string directoryPath = Path.GetDirectoryName(BackupPath);

            Directory.CreateDirectory(directoryPath);
             
            if (File.Exists(BackupPath))
            {
                RestoreBackup();
            }

            ExecuteNonQuery(Connection, $@"
                BACKUP DATABASE [{DatabaseName}] TO DISK='{BackupPath}' WITH FORMAT, 
                    MEDIANAME = 'SQLServerBackups', 
                    NAME = 'Harness backup of {DatabaseName}'");
        }

        private void RestoreBackup()
        {
            ExecuteNonQuery(Connection, $@"
                USE master;
                ALTER DATABASE [{DatabaseName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [{DatabaseName}] FROM DISK='{BackupPath}' WITH 
                    RECOVERY, 
                    REPLACE;
                ALTER DATABASE [{DatabaseName}] SET MULTI_USER;");
        }

        public override void Dispose()
        {
            RestoreBackup();
            File.Delete(BackupPath);

            base.Dispose();
        }
    }
}
