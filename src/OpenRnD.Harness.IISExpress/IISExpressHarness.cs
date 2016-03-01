using OpenRnD.Harness.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenRnD.Harness.IISExpress
{
    public class IISExpressHarness : IDisposable
    {
        public string ProjectPath { get; }

        public int ServerPort { get; }
        public Process ServerProcess { get; }


        public IISExpressHarness(string projectPath, int serverPort)
        {
            string fullName = new DirectoryInfo(Path.Combine(projectPath, "Web.config")).FullName;

            if (!File.Exists(fullName))
            {
                throw new Exception($"No Web.config file found in the project path directory ({projectPath}).");
            }

            if (serverPort < 1 || serverPort > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException($"The server port is invalid.");
            }

            ProjectPath = projectPath;
            ServerPort = serverPort;

            PIDUtilities.KillByPID("pid.txt", "iisexpress");
            ProcessStartInfo startInfo = CreateServerStartInfo();
            ServerProcess = StartServer(startInfo);
            PIDUtilities.StorePID("pid.txt", ServerProcess.Id);
        }

        private ProcessStartInfo CreateServerStartInfo()
        {
            string iisExpressPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) + @"\IIS Express\iisexpress.exe";

            string testAssemblyLocationPath = Assembly.GetExecutingAssembly().Location;
            string testAssemblyProjectPath = Path.GetDirectoryName(testAssemblyLocationPath);
            string targetProjectPath = Path.Combine(testAssemblyProjectPath, ProjectPath);
            string targetProjectPathNormalized = new DirectoryInfo(targetProjectPath).FullName;
            string iisExpressArguments = string.Format(@"/path:""{0}"" /port:{1}", targetProjectPathNormalized, ServerPort);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = iisExpressPath,
                Arguments = iisExpressArguments
            };

            return startInfo;
        }

        private Process StartServer(ProcessStartInfo startInfo)
        {
            return Process.Start(startInfo);
        }

        public void Dispose()
        {
            if (ServerProcess.HasExited == false)
            {
                ServerProcess.Kill();
                ServerProcess.WaitForExit();
            }
        }
    }
}
