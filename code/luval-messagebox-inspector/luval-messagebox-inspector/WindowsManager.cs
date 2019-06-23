using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public static string CaptureScreen()
        {
            using (var bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                            Screen.PrimaryScreen.Bounds.Height))
            {
                using (var g = Graphics.FromImage(bmp))
                {
                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                     Screen.PrimaryScreen.Bounds.Y,
                                     0, 0,
                                     bmp.Size,
                                     CopyPixelOperation.SourceCopy);
                }
                var fileName = GetFileName();
                bmp.Save(fileName, ImageFormat.Png);
                CleanOldCaptures();
                return fileName;
            }
        }

        private static void CleanOldCaptures()
        {
            var files = Directory.GetFiles(Environment.CurrentDirectory, "*.png").Reverse().ToList();
            if(files.Count > 30)
            {
                var topFiles = files.Take(5);
                foreach(var file in files)
                {
                    if (!topFiles.Contains(file)) File.Delete(file);
                }
            }
        }
        private static string GetFileName()
        {
            var start = new DateTime(2019, 6, 1);
            var number = Convert.ToInt64(DateTime.UtcNow.Subtract(start).TotalSeconds);
            return string.Format("CAPTURE-{0}.png", number);
        }
    }
}
