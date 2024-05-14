using System;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using static Neodymium.WindowScreenInfo;

namespace Neodymium
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

        private Settings CurrentSettings;
        private string ConfigPath;

        public class Settings
        {
            // Define default modifier and keys for each shortcut
            public bool ModifierAlt { get; set; } = true;
            public bool ModifierCtrl { get; set; } = true;
            public bool ModifierShift { get; set; } = false;
            public bool ModifierWin { get; set; } = true;
            public UInt16 ResizeStep { get; set; } = 50;
            public int TopLeft { get; set; } = (int)Keys.U;
            public int TopRight { get; set; } = (int)Keys.I;
            public int BottomLeft { get; set; } = (int)Keys.J;
            public int BottomRight { get; set; } = (int)Keys.K;
            public int Left { get; set; } = (int)Keys.Left;
            public int Right { get; set; } = (int)Keys.Right;
            public int Top { get; set; } = (int)Keys.Up;
            public int Bottom { get; set; } = (int)Keys.Down;
            public int Center { get; set; } = (int)Keys.C;
            public int Full { get; set; } = (int)Keys.Enter;
            public int Wider { get; set; } = (int)Keys.W;
            public int Taller { get; set; } = (int)Keys.E;
            public int Narrower { get; set; } = (int)Keys.S;
            public int Shorter { get; set; } = (int)Keys.D;
            public int NextDisplay { get; set; } = (int)Keys.X;
        }

        enum Shortcuts
        {
            TOP_LEFT, TOP_RIGHT, BOTTOM_LEFT, BOTTOM_RIGHT, LEFT, RIGHT, CENTER, TOP, BOTTOM, FULL, WIDER, TALLER, NARROWER, SHORTER, NEXT_DISPLAY
        }

        public Main()
        {
            InitializeComponent();
            string homePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            ConfigPath = Path.Combine(homePath, ".neodymium.json");

            var fromConfig = HotkeysFromConfig();

            if (fromConfig != null)
            {
                CurrentSettings = fromConfig;
            }
            else
            {
                CurrentSettings = new Settings();

                string jsonString = JsonSerializer.Serialize(CurrentSettings, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(ConfigPath, jsonString);
            }
        }

        private Settings? HotkeysFromConfig()
        {
            if (File.Exists(ConfigPath))
            {
                try
                {
                    string jsonString = File.ReadAllText(ConfigPath);
                    return JsonSerializer.Deserialize<Settings>(jsonString);
                }
                catch { return null; }
            }
            return null;
        }

        private async void InitializeMinimizeAfterDelay()
        {
            await Task.Delay(2000);
            this.WindowState = FormWindowState.Minimized;
            this.appStatusLabel.Text = "Running!\nYou can minimize me now!";
        }

        private void Main_Load(object sender, EventArgs e)
        {
            RegisterHotkeys();
            InitializeMinimizeAfterDelay();
        }

        private void RegisterHotkeys()
        {
            // Alt = 1, Ctrl = 2, Shift = 4, Win = 8
            int modifier = CurrentSettings.ModifierAlt ? 1 : 0;
            modifier = CurrentSettings.ModifierCtrl ? modifier + 2 : modifier;
            modifier = CurrentSettings.ModifierShift ? modifier + 4 : modifier;
            modifier = CurrentSettings.ModifierWin ? modifier + 8 : modifier;

            RegisterHotKey(this.Handle, (int)Shortcuts.TOP_LEFT, modifier, CurrentSettings.TopLeft);
            RegisterHotKey(this.Handle, (int)Shortcuts.TOP_RIGHT, modifier, CurrentSettings.TopRight);
            RegisterHotKey(this.Handle, (int)Shortcuts.BOTTOM_LEFT, modifier, CurrentSettings.BottomLeft);
            RegisterHotKey(this.Handle, (int)Shortcuts.BOTTOM_RIGHT, modifier, CurrentSettings.BottomRight);
            RegisterHotKey(this.Handle, (int)Shortcuts.LEFT, modifier, CurrentSettings.Left);
            RegisterHotKey(this.Handle, (int)Shortcuts.RIGHT, modifier, CurrentSettings.Right);
            RegisterHotKey(this.Handle, (int)Shortcuts.TOP, modifier, CurrentSettings.Top);
            RegisterHotKey(this.Handle, (int)Shortcuts.BOTTOM, modifier, CurrentSettings.Bottom);
            RegisterHotKey(this.Handle, (int)Shortcuts.CENTER, modifier, CurrentSettings.Center);
            RegisterHotKey(this.Handle, (int)Shortcuts.FULL, modifier, CurrentSettings.Full);
            RegisterHotKey(this.Handle, (int)Shortcuts.WIDER, modifier, CurrentSettings.Wider);
            RegisterHotKey(this.Handle, (int)Shortcuts.TALLER, modifier, CurrentSettings.Taller);
            RegisterHotKey(this.Handle, (int)Shortcuts.NARROWER, modifier, CurrentSettings.Narrower);
            RegisterHotKey(this.Handle, (int)Shortcuts.SHORTER, modifier, CurrentSettings.Shorter);
            RegisterHotKey(this.Handle, (int)Shortcuts.NEXT_DISPLAY, modifier, CurrentSettings.NextDisplay);
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

            int inc = CurrentSettings.ResizeStep;

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

        private void OpenLink(string link)
        {
            Process.Start("explorer.exe", link);
        }

        private void openConfig_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("explorer.exe", " /select, " + ConfigPath));
        }

        private void keyReference_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("https://learn.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes");
        }

        private void gitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("https://github.com/richardstefun/Neodymium");
        }

        private void supportProjectLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenLink("https://ko-fi.com/richardstefun");
        }

        private void buttonSupport_Click(object sender, EventArgs e)
        {
            OpenLink("https://ko-fi.com/richardstefun");
        }
    }
}
