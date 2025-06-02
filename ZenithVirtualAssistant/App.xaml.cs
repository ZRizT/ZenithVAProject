using System;
using System.Windows;
using System.Windows.Forms; // For NotifyIcon
using ZenithVirtualAssistant.Services;

namespace ZenithVirtualAssistant
{
    public partial class App : Application
    {
        private NotifyIcon _notifyIcon;
        private SpeechService _speechService;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize system tray icon
            _notifyIcon = new NotifyIcon
            {
                Icon = new System.Drawing.Icon("icon.jpg"), // Add an icon file to project
                Visible = true,
                Text = "Zenith Virtual Assistant"
            };
            _notifyIcon.DoubleClick += (s, args) =>
            {
                MainWindow.Show();
                MainWindow.Activate();
            };

            // Create context menu for system tray
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Exit", null, (s, args) => Shutdown());
            _notifyIcon.ContextMenuStrip = contextMenu;

            // Initialize services
            _speechService = new SpeechService();
            _speechService.StartListening();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _notifyIcon.Dispose();
            _speechService.Dispose();
            base.OnExit(e);
        }
    }
}