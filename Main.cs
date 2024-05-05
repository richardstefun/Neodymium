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

        WindowScreenInfo windowScreenInfo = new WindowScreenInfo();

        const int MODIFIER = 1 + 2 + 8; // Modifier keys codes: Alt = 1, Ctrl = 2, Shift = 4, Win = 8

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
            RegisterHotKey(this.Handle, (int)Shortcuts.TALLER, MODIFIER, (int)Keys.Q);
            RegisterHotKey(this.Handle, (int)Shortcuts.NARROWER, MODIFIER, (int)Keys.S);
            RegisterHotKey(this.Handle, (int)Shortcuts.SHORTER, MODIFIER, (int)Keys.A);
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
            // Get all available screens
            Screen[] allScreens = Screen.AllScreens;

            // Find the index of the current screen in the array
            int currentIndex = Array.IndexOf(allScreens, screen);

            if (currentIndex == -1)
            {
                // Current screen not found in the list (shouldn't happen with AllScreens, but handle if needed)
                throw new ArgumentException("The provided screen is not part of the AllScreens collection.");
            }

            // Determine the index of the next screen
            int nextIndex = (currentIndex + 1) % allScreens.Length;

            // Return the next screen
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

            int inc = 50;


            switch (s)
            {
                case Shortcuts.TOP_LEFT:
                    MoveWindow(handle, info.screen.Bounds.X, info.screen.Bounds.Y, halfWidth, halfHeight, true);
                    break;
                case Shortcuts.TOP_RIGHT:
                    MoveWindow(handle, info.screen.Bounds.X + halfWidth, info.screen.Bounds.Y, halfWidth, halfHeight, true);
                    break;
                case Shortcuts.BOTTOM_LEFT:
                    MoveWindow(handle, info.screen.Bounds.X, info.screen.Bounds.Y + halfHeight, halfWidth, halfHeight, true);
                    break;
                case Shortcuts.BOTTOM_RIGHT:
                    MoveWindow(handle, info.screen.Bounds.X + halfWidth, info.screen.Bounds.Y + halfHeight, halfWidth, halfHeight, true);
                    break;
                case Shortcuts.LEFT:
                    MoveWindow(handle, info.screen.Bounds.X, info.screen.Bounds.Y, halfWidth, height, true);
                    break;
                case Shortcuts.RIGHT:
                    MoveWindow(handle, info.screen.Bounds.X + halfWidth, info.screen.Bounds.Y, halfWidth, height, true);
                    break;
                case Shortcuts.TOP:
                    MoveWindow(handle, info.screen.Bounds.X, info.screen.Bounds.Y, width, halfHeight, true);
                    break;
                case Shortcuts.BOTTOM:
                    MoveWindow(handle, info.screen.Bounds.X, info.screen.Bounds.Y + halfHeight, width, halfHeight, true);
                    break;
                case Shortcuts.CENTER:
                    MoveWindow(handle, (info.screen.Bounds.X + halfWidth) - (info.window.Width / 2), (info.screen.Bounds.Y + halfHeight) - (info.window.Height / 2), info.window.Width, info.window.Height, true);
                    break;
                case Shortcuts.FULL:
                    MoveWindow(handle, info.screen.Bounds.X, info.screen.Bounds.Y, width, height, true);
                    break;
                case Shortcuts.WIDER:
                    var newX = (info.window.Left - (inc / 2) >= info.screen.Bounds.X ? info.window.Left - (inc/2) : info.screen.WorkingArea.Left);
                    var newWidth = info.window.Width + inc;
                    if (info.window.Right + inc > width)
                    {
                        newWidth = width - info.window.Left + (inc / 2);
                    }
                    MoveWindow(handle, newX, info.window.Top, newWidth, info.window.Height, true);
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
                    var next = NextScreen(info.screen);
                    //MoveWindow(handle, next.Bounds.X, next.Bounds.Y, info.window.Width, info.window.Height, true);
                    MoveWindow(handle, (next.WorkingArea.X + next.WorkingArea.Width/2) - (info.window.Width / 2), (next.WorkingArea.Y + next.WorkingArea.Height/2) - (info.window.Height / 2), info.window.Width, info.window.Height, true);
                    break;
            }
        }



        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Minimize to Tray App";
            notifyIcon1.BalloonTipText = "You have successfully minimized your form.";

            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

    }
}
