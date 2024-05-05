using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static WinWin.Main;
using static WinWin.WindowScreenInfo;

namespace WinWin
{
    public class WindowScreenInfo
    {
        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(nint hWnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern nint MonitorFromRect(ref RECT lprc, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern nint MonitorFromWindow(nint hWnd, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern bool GetMonitorInfo(nint hMonitor, ref MONITORINFO lpmi);

        [DllImport("user32.dll")]
        public static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        static extern bool GetMonitorInfo(nint hMonitor, ref MONITORINFOEX lpmi);


        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct MONITORINFO
        {
            public int cbSize;
            public RECT rcMonitor;
            public RECT rcWork;
            public uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct MONITORINFOEX
        {
            public int Size;
            public RECT Monitor;
            public RECT WorkArea;
            public uint Flags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEVMODE
        {
            private const int CCHDEVICENAME = 0x20;
            private const int CCHFORMNAME = 0x20;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public ScreenOrientation dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
            public string dmFormName;
            public short dmLogPixels;
            public int dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }

        private const uint MONITOR_DEFAULTTONEAREST = 0x00000002;

        [StructLayout(LayoutKind.Sequential)]
        public struct ScreenInfo
        {
            public Screen screen;
            public DEVMODE dmMode;
            public MONITORINFOEX monitorInfoEx;
            public Rectangle window;
            public RECT windowRECT;
        }

        public ScreenInfo GetScreenInfo(nint hWnd)
        {
            RECT rect;
            GetWindowRect(hWnd, out rect);

            nint hMonitor = MonitorFromRect(ref rect, MONITOR_DEFAULTTONEAREST);

            MONITORINFOEX mi = new MONITORINFOEX();
            mi.Size = Marshal.SizeOf(typeof(MONITORINFOEX));
            GetMonitorInfo(hMonitor, ref mi);

            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            EnumDisplaySettings(mi.DeviceName, -1, ref dm);

            ScreenInfo screenInfo = new ScreenInfo();

            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.DeviceName.Equals(mi.DeviceName))
                {
                    screenInfo.screen = screen;
                }
            }

            screenInfo.dmMode = dm;
            screenInfo.monitorInfoEx = mi;
            screenInfo.window = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top); 
            screenInfo.windowRECT = rect;

            return screenInfo;
        }
    }
}