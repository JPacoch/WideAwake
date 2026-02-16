using System;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Reflection;
using System.IO;

namespace WideAwake
{
    public class Program : Form
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern uint SetThreadExecutionState(uint esFlags);

        [DllImport("user32.dll")]
        static extern void mouse_event(uint dwFlags, int dx, int dy, uint dwData, uint dwExtraInfo);

        const uint ES_CONTINUOUS = 0x80000000;
        const uint ES_SYSTEM_REQUIRED = 0x00000001;
        const uint ES_DISPLAY_REQUIRED = 0x00000002;
        const uint MOUSEEVENTF_MOVE = 0x0001;

        private NotifyIcon trayIcon = null!;
        private System.Windows.Forms.Timer activityTimer = null!;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Program());
        }

        public Program()
        {
            SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED | ES_DISPLAY_REQUIRED);

            activityTimer = new System.Windows.Forms.Timer();
            activityTimer.Interval = 60000; 
            activityTimer.Tick += (s, e) => {
                mouse_event(MOUSEEVENTF_MOVE, 1, 0, 0, 0);
                mouse_event(MOUSEEVENTF_MOVE, -1, 0, 0, 0);
            };
            activityTimer.Start();

            InitializeTray();

            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;
        }

        private void InitializeTray()
        {
            trayIcon = new NotifyIcon();
            trayIcon.Text = "WideAwake (Active)";
            trayIcon.Visible = true;

            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = "WideAwake.app.ico"; 
            
            try 
            {
                using (Stream? stream = assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                        trayIcon.Icon = new Icon(stream);
                    else
                        trayIcon.Icon = SystemIcons.Application;
                }
            }
            catch 
            {
                trayIcon.Icon = SystemIcons.Application;
            }

            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, (s, e) => Application.Exit());
            trayIcon.ContextMenuStrip = contextMenu;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            activityTimer.Stop();
            SetThreadExecutionState(ES_CONTINUOUS);
            trayIcon.Dispose();
            base.OnFormClosing(e);
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }
    }
}