using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luval_messagebox_inspector
{
    class Program
    {
        static void Main(string[] args)
        {
            var textToFind = "SAP GUI for Windows 750";
            while (true)
            {
                var winHandle = WindowManager.FindWindowByCaption(textToFind);
                LogMessage("Result: {0}", winHandle);
                if (winHandle != IntPtr.Zero)
                {
                    var focus = WindowManager.SetForegroundWindow(winHandle);
                    LogMessage("Windows found, focus set status: {0}", focus);
                    if (focus)
                    {
                        SendKeys.SendWait("N");
                        LogMessage("Message sent to focused window");
                    }
                }
                Thread.Sleep(3000);
            }
        }

        private static void LogMessage(string format, params object[] p)
        {
            Console.WriteLine(format, p);
        }
    }
}
