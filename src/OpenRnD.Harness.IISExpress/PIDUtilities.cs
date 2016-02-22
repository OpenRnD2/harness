using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenRnD.Harness.Core
{
    public static class PIDUtilities
    {
        public static void StorePID(string fileName, int processId)
        {
            File.WriteAllText(fileName, processId.ToString());
        }

        public static void KillByPID(string fileName, string processName)
        {
            if (File.Exists(fileName))
            {
                int pid = int.Parse(File.ReadAllText(fileName));
                Process[] processes = Process.GetProcessesByName(processName);
                Process matchingProcess = processes.SingleOrDefault(p => p.Id == pid);

                matchingProcess?.Kill();
            }
        }
    }

}
