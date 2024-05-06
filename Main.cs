using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using static WinWin.WindowScreenInfo;

namespace WinWin
{
    public partial class Main : Form
    {
        [DllImport("user32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);
        [DllImport("user32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [Serializable]
        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
            public System.Drawing.Point ptMinPosition;
            public System.Drawing.Point ptMaxPosition;
            public System.Drawing.Rectangle rcNormalPosition;
        }

        public static void DeMaximizeActiveWindow(nint hWnd)
        {
            if (hWnd != IntPtr.Zero)
            {
                WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
                placement.length = Marshal.SizeOf(placement);

                if (GetWindowPlacement(hWnd, ref placement))
                {
                    if (placement.showCmd == SW_SHOWMAXIMIZED)
                    {
                        ShowWindow(hWnd, SW_SHOWNORMAL);
                    }
                }
            }
        }

        WindowScreenInfo windowScreenInfo = new WindowScreenInfo();

        private const int SW_SHOWNORMAL = 1;
        private const int SW_SHOWMINIMIZED = 2;
        private const int SW_SHOWMAXIMIZED = 3;

        const int MODIFIER = 1 + 2 + 8; // Alt = 1, Ctrl = 2, Shift = 4, Win = 8

        enum Shortcuts
        {
            TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT, LEFT, RIGHT, CENTER, TOP, BOTTOM, FULL, WIDER, TALLER, NARROWER, SHORTER, NEXT_DISPLAY
        }

        public Main()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKey(this.Handle, (int)Shortcuts.TOP_LEFT, MODIFIER, (int)Keys.U);
            RegisterHotKey(this.Handle, (int)Shortcuts.TOP_RIGHT, MODIFIER, (int)Keys.I);
            RegisterHotKey(this.Handle, (int)Shortcuts.BOTTOM_LEFT, MODIFIER, (int)Keys.J);
            RegisterHotKey(this.Handle, (int)Shortcuts.BOTTOM_RIGHT, MODIFIER, (int)Keys.K);
            RegisterHotKey(this.Handle, (int)Shortcuts.LEFT, MODIFIER, (int)Keys.Left);
            RegisterHotKey(this.Handle, (int)Shortcuts.RIGHT, MODIFIER, (int)Keys.Right);
            RegisterHotKey(this.Handle, (int)Shortcuts.TOP, MODIFIER, (int)Keys.Up);
            RegisterHotKey(this.Handle, (int)Shortcuts.BOTTOM, MODIFIER, (int)Keys.Down);
            RegisterHotKey(this.Handle, (int)Shortcuts.CENTER, MODIFIER, (int)Keys.C);
            RegisterHotKey(this.Handle, (int)Shortcuts.FULL, MODIFIER, (int)Keys.Enter);
            RegisterHotKey(this.Handle, (int)Shortcuts.WIDER, MODIFIER, (int)Keys.W);
            RegisterHotKey(this.Handle, (int)Shortcuts.TALLER, MODIFIER, (int)Keys.E);
            RegisterHotKey(this.Handle, (int)Shortcuts.NARROWER, MODIFIER, (int)Keys.S);
            RegisterHotKey(this.Handle, (int)Shortcuts.SHORTER, MODIFIER, (int)Keys.D);
            RegisterHotKey(this.Handle, (int)Shortcuts.NEXT_DISPLAY, MODIFIER, (int)Keys.X);
        }


        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0312)
            {
                DoResizeWindow((Shortcuts)m.WParam.ToInt32());

            }
            base.WndProc(ref m);
        }

        private Screen NextScreen(Screen screen)
        {
            Screen[] allScreens = Screen.AllScreens;
            int currentIndex = Array.IndexOf(allScreens, screen);

            if (currentIndex == -1)
            {
                throw new ArgumentException("The provided screen is not part of the AllScreens collection.");
            }

            int nextIndex = (currentIndex + 1) % allScreens.Length;
            return allScreens[nextIndex];
        }

        private void DoResizeWindow(Shortcuts s)
        {
            IntPtr handle = GetForegroundWindow();
            ScreenInfo info = windowScreenInfo.GetScreenInfo(handle);

            int width = info.screen.WorkingArea.Width;
            int height = info.screen.WorkingArea.Height;

            int halfWidth = width / 2;
            int halfHeight = height / 2;

            int X = info.screen.WorkingArea.X;
            int Y = info.screen.WorkingArea.Y;

            int inc = 50;

            DeMaximizeActiveWindow(handle);

            switch (s)
            {
                case Shortcuts.TOP_LEFT:
                    MoveWindow(handle, X, Y, halfWidth, halfHeight, true);
                    break;
                case Shortcuts.TOP_RIGHT:
                    MoveWindow(handle, X + halfWidth, Y, halfWidth, halfHeight, true);
                    break;
                case Shortcuts.BOTTOM_LEFT:
                    MoveWindow(handle, X, Y + halfHeight, halfWidth, halfHeight, true);
                    break;
                case Shortcuts.BOTTOM_RIGHT:
                    MoveWindow(handle, X + halfWidth, Y + halfHeight, halfWidth, halfHeight, true);
                    break;
                case Shortcuts.LEFT:
                    MoveWindow(handle, X, Y, halfWidth, height, true);
                    break;
                case Shortcuts.RIGHT:
                    MoveWindow(handle, X + halfWidth, Y, halfWidth, height, true);
                    break;
                case Shortcuts.TOP:
                    MoveWindow(handle, X, Y, width, halfHeight, true);
                    break;
                case Shortcuts.BOTTOM:
                    MoveWindow(handle, X, Y + halfHeight, width, halfHeight, true);
                    break;
                case Shortcuts.CENTER:
                    MoveWindow(handle, (X + halfWidth) - (info.window.Width / 2), (Y + halfHeight) - (info.window.Height / 2), info.window.Width, info.window.Height, true);
                    break;
                case Shortcuts.FULL:
                    MoveWindow(handle, X, Y, width, height, true);
                    break;
                case Shortcuts.WIDER:
                    MakeWider(handle, info, width, inc);
                    break;
                case Shortcuts.TALLER:
                    MoveWindow(handle, info.window.Left, info.window.Top - (inc / 2), info.window.Width, info.window.Height + inc, true);
                    break;
                case Shortcuts.NARROWER:
                    MoveWindow(handle, info.window.Left + (inc / 2), info.window.Top, info.window.Width - inc, info.window.Height, true);
                    break;
                case Shortcuts.SHORTER:
                    MoveWindow(handle, info.window.Left, info.window.Top + (inc / 2), info.window.Width, info.window.Height - inc, true);
                    break;
                case Shortcuts.NEXT_DISPLAY:
                    NextDisplay(handle, info);
                    break;
                default:
                    break;
            }
        }

        private void NextDisplay(nint handle, ScreenInfo info)
        {
            if (Screen.AllScreens.Length == 1)
            {
                return; // Single screen, nowhere to move the window
            }
            var next = NextScreen(info.screen);
            MoveWindow(handle, next.WorkingArea.X, next.WorkingArea.Y, info.window.Width, info.window.Height, true);
            DoResizeWindow(Shortcuts.TOP_LEFT);
            DoResizeWindow(Shortcuts.CENTER);
        }

        private static void MakeWider(nint handle, ScreenInfo info, int width, int inc)
        {
            var newX = (info.window.Left - (inc / 2) >= info.screen.WorkingArea.X ? info.window.Left - (inc / 2) : info.screen.WorkingArea.Left);
            var newWidth = info.window.Width + inc;

            if (info.window.Right + inc > width)
            {
                newWidth = width - info.window.Left + (inc / 2);
            }
            MoveWindow(handle, newX, info.window.Top, newWidth, info.window.Height, true);
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

    }
}
