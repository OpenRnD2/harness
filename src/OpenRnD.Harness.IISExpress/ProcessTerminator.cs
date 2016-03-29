using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenRnD.Harness.Core
{
    internal static class ProcessTerminator
    {
        public static IntPtr FindMainWindow(int processId)
        {
            IntPtr mainWindowHandle = IntPtr.Zero;

            NativeMethods.EnumWindows((hWnd, lParam) =>
            {
                int windowProcessId;
                NativeMethods.GetWindowThreadProcessId(hWnd, out windowProcessId);

                if (windowProcessId == processId)
                {
                    mainWindowHandle = hWnd;
                    return false;
                }

                return true;
            }, IntPtr.Zero);

            return mainWindowHandle;
        }

        internal static void Terminate(Process process)
        {
            // Try to abort the process in a civilized manner
            if (process.HasExited == false)
            {
                IntPtr mainWindowHandle = FindMainWindow(process.Id);
                NativeMethods.PostMessage(mainWindowHandle, NativeMethods.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                process.WaitForExit(3000);
            }

            // If the process hasn't complied after a reasonable timeout, kill it
            if (process.HasExited == false)
            {
                process.Kill();
                process.WaitForExit();
            }

        }
    }
}
