using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace luval_messagebox_inspector
{
    public static class ScreenResolution
    {

        public static Rectangle GetPrimaryScreenBounds()
        {
            return GetScaledArea(Screen.PrimaryScreen.Bounds);
        }

        public static Rectangle GetPrimaryScreenWorkingArea()
        {
            return GetScaledArea(Screen.PrimaryScreen.WorkingArea);
        }

        public static Rectangle GetScaledArea(Rectangle source)
        {
            var scalingFactor = GetScalingFactor();
            return new Rectangle()
            {
                X = 0,
                Y = 0,
                Width = Convert.ToInt32(source.Width * scalingFactor),
                Height = Convert.ToInt32(source.Height * scalingFactor)
            };
        }

        [DllImport("gdi32.dll")]
        static extern int GetDeviceCaps(IntPtr hdc, int nIndex);
        public enum DeviceCap
        {
            VERTRES = 10,
            DESKTOPVERTRES = 117,
        }


        public static float GetScalingFactor()
        {
            var g = Graphics.FromHwnd(IntPtr.Zero);
            var desktop = g.GetHdc();
            var LogicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.VERTRES);
            var PhysicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DESKTOPVERTRES);

            var ScreenScalingFactor = (float)PhysicalScreenHeight / (float)LogicalScreenHeight;

            return ScreenScalingFactor; // 1.25 = 125%
        }
    }
}
