using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace luval_messagebox_inspector
{
    public class WindowManager
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);


        public static int SendMessage(IntPtr hWnd, string lParam)
        {
            return SendMessage(hWnd, 0x000C, 0, lParam);
        }

        public static IntPtr FindWindow(string caption)
        {
            return FindWindow(String.Empty, caption);
        }

        public static IntPtr FindWindowByCaption(string caption)
        {
            return FindWindowByCaption(IntPtr.Zero, caption);
        }
    }
}
