using OpenRnD.Harness.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenRnD.Harness.DotNetCore
{
    public class DotNetCoreHarness : IDisposable
    {
        public string ProjectPath { get; }

        public int ServerPort { get; }
        public Process ServerProcess { get; }
        public string HostingEnvironment { get; }


        public DotNetCoreHarness(string projectPath, int serverPort, string hostingEnvironment)
        {
            string fullName = new DirectoryInfo(Path.Combine(projectPath, "project.json")).FullName;

            if (!File.Exists(fullName))
            {
                throw new Exception($"No project.json file found in the project path directory ({fullName}).");
            }

            if (serverPort < 1 || serverPort > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException($"The server port is invalid.");
            }

            ProjectPath = projectPath;
            ServerPort = serverPort;
            HostingEnvironment = hostingEnvironment;

            PIDUtilities.KillByPID("pid.txt", "dnx");
            ProcessStartInfo startInfo = CreateServerStartInfo();
            ServerProcess = StartServer(startInfo);
            PIDUtilities.StorePID("pid.txt", ServerProcess.Id);
        }

        private Process StartServer(ProcessStartInfo startInfo)
        {
            return Process.Start(startInfo);
        }

        private ProcessStartInfo CreateServerStartInfo()
        {
            string testAssemblyLocationPath = Assembly.GetExecutingAssembly().Location;
            string testAssemblyProjectPath = Path.GetDirectoryName(testAssemblyLocationPath);
            string targetProjectPath = Path.Combine(testAssemblyProjectPath, ProjectPath);
            string targetProjectPathNormalized = new DirectoryInfo(targetProjectPath).FullName;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                UseShellExecute = false,
                FileName = "dnx",
                Arguments = $"web --server.urls=http://localhost:{ServerPort}",
                WorkingDirectory = targetProjectPathNormalized
            };

            startInfo.EnvironmentVariables.Add("Hosting:Environment", HostingEnvironment);

            return startInfo;
        }
        
        public void Dispose()
        {
            ProcessTerminator.Terminate(ServerProcess);
        }
    }
}
