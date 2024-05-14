using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Neodymium
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]

        

        static void Main()
        {
            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "Neodymium", out createdNew))
            {
                if (createdNew)
                {
                    ApplicationConfiguration.Initialize();
                    Application.Run(new Main());
                }
                else
                {
                    MessageBox.Show("If the window is not available, check the tray icon.", "Neodymium is already running!");
                }
            }
            
        }
    }
}