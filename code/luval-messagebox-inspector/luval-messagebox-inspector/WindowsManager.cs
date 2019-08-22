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

        public static string Capture(string fileLocation)
        {
            var res = TryCapture(ImageFormat.Jpeg, fileLocation);
            if (res == "FAILED") res = TryCapture(ImageFormat.Gif, fileLocation);
            if (res == "FAILED") res = TryCapture(ImageFormat.Png, fileLocation);
            return res;
        }

        public static string TryCapture(ImageFormat format, string fileLocation)
        {
            var res = string.Empty;
            try
            {
                res = Capture(format, fileLocation);
            }
            catch (CaptureException cEx)
            {
                Logger.WriteWarning(cEx.ToString());
                return "FAILED";
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("CAPTURE FAILED", ex);
            }
            return res;
        }


        public static string Capture(ImageFormat format, string fileLocation)
        {
            var fileName = GetFileName(format);
            var fullLocation = Path.Combine(fileLocation, fileName);
            try
            {
                var screen = ScreenResolution.GetPrimaryScreenBounds();
                using (var bmp = new Bitmap(screen.Width,
                                            screen.Height))
                {
                    using (var g = Graphics.FromImage(bmp))
                    {
                        g.CopyFromScreen(screen.X,
                                         screen.Y,
                                         0, 0,
                                         bmp.Size,
                                         CopyPixelOperation.SourceCopy);
                    }
                    bmp.Save(fullLocation, format);
                }
            }
            catch (Exception ex)
            {
                throw new CaptureException("Failed to take the screenshot", ex);
            }
            CleanOldCaptures();
            return fullLocation;
        }

        private static void CleanOldCaptures()
        {
            var files = Directory.GetFiles(Environment.CurrentDirectory, "*.png").Reverse().ToList();
            if (files.Count > 30)
            {
                var topFiles = files.Take(5);
                foreach (var file in files)
                {
                    if (!topFiles.Contains(file)) File.Delete(file);
                }
            }
        }

        private static string GetFileName(ImageFormat format)
        {
            var start = new DateTime(2019, 6, 1);
            var number = Convert.ToInt64(DateTime.UtcNow.Subtract(start).TotalSeconds);
            return string.Format("CAPTURE-{0}.{1}", number, GetFileExtension(format));
        }

        private static string GetFileExtension(ImageFormat format)
        {
            if (format == ImageFormat.Gif) return "gif";
            if (format == ImageFormat.Jpeg) return "jpeg";
            if (format == ImageFormat.Png) return "png";
            if (format == ImageFormat.Tiff) return "tiff";
            if (format == ImageFormat.Bmp) return "bmp";
            return "jpeg";
        }
    }

    public class CaptureException : Exception
    {
        public CaptureException()
        {

        }

        public CaptureException(string message) : base(message)
        {

        }

        public CaptureException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
