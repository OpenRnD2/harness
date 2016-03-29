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
        public IISExpressBitness Bitness { get; }
        public bool ShowWindow { get; }

        public IISExpressHarness(string projectPath, int serverPort, IISExpressBitness bitness = IISExpressBitness.x86, bool showWindow = false)
        {
            string fullName = new DirectoryInfo(Path.Combine(projectPath, "Web.config")).FullName;

            if (!File.Exists(fullName))
            {
                throw new Exception($"No Web.config file found in the project path directory ({fullName}).");
            }

            if (serverPort < 1 || serverPort > ushort.MaxValue)
            {
                throw new ArgumentOutOfRangeException($"The server port is invalid.");
            }

            ProjectPath = projectPath;
            ServerPort = serverPort;
            Bitness = bitness;
            ShowWindow = showWindow;

            PIDUtilities.KillByPID("pid.txt", "iisexpress");
            ProcessStartInfo startInfo = CreateServerStartInfo();
            ServerProcess = StartServer(startInfo);
            PIDUtilities.StorePID("pid.txt", ServerProcess.Id);
        }

        private ProcessStartInfo CreateServerStartInfo()
        {
            string iisExpressPath = GetIISExpressPath(Bitness);
            CheckForIISExpressPath(iisExpressPath);

            string testAssemblyLocationPath = Assembly.GetExecutingAssembly().Location;
            string testAssemblyProjectPath = Path.GetDirectoryName(testAssemblyLocationPath);
            string targetProjectPath = Path.Combine(testAssemblyProjectPath, ProjectPath);
            string targetProjectPathNormalized = new DirectoryInfo(targetProjectPath).FullName;
            string iisExpressArguments = string.Format(@"/path:""{0}"" /port:{1}", targetProjectPathNormalized, ServerPort);

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = iisExpressPath,
                Arguments = iisExpressArguments,
            };

            if (!ShowWindow)
            {
                startInfo.UseShellExecute = false;
                startInfo.CreateNoWindow = true;
                startInfo.RedirectStandardOutput = true;
            }

            return startInfo;
        }

        private void CheckForIISExpressPath(string iisExpressPath)
        {
            if(!File.Exists(iisExpressPath))
            {
                throw new IOException($"IIS Express path not found ({iisExpressPath}).");
            }
        }

        private Process StartServer(ProcessStartInfo startInfo)
        {
            return Process.Start(startInfo);
        }

        public void Dispose()
        {
            ProcessTerminator.Terminate(ServerProcess);
        }

        private string GetIISExpressPath(IISExpressBitness bitness)
        {
            string folder;

            if(bitness == IISExpressBitness.x86)
            {
                folder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86);
            }
            else if(bitness == IISExpressBitness.x64)
            {
                if(!Environment.Is64BitProcess)
                {
                    // SpecialFolder.ProgramFiles would still point to the x86 folder in a 32-bit process
                    folder = Environment.GetEnvironmentVariable("ProgramW6432");
                }
                else
                {
                    folder = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
                }
            }
            else
            {
                throw new ArgumentException();
            }

            string iisExpressPath = Path.Combine(folder, @"IIS Express\iisexpress.exe");

            return iisExpressPath;
        }

        const string iisExpressPath = @"IIS Express\IISExpress.exe";
    }
}
