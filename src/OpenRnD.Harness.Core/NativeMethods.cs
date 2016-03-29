using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenRnD.Harness.Core
{
    internal static class NativeMethods
    {
        internal delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern bool EnumWindows(EnumThreadWindowsCallback lpEnumFunc, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        [DllImport("User32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

        public const int WM_CLOSE = 0x0010;
    }
}
