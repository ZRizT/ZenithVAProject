using System;
using System.Drawing;   
using System.Windows.Forms; 

namespace ZenithVirtualAssistant.Utils
{
    public class SystemTrayManager
    {
        private readonly NotifyIcon _notifyIcon;
        private readonly Action _showMainWindow;
        private readonly Action _showWidget;
        private readonly Action _exitApplication;

        public SystemTrayManager(Action showMainWindow, Action showWidget, Action exitApplication)
        {
            _showMainWindow = showMainWindow;
            _showWidget = showWidget;
            _exitApplication = exitApplication;

            _notifyIcon = new NotifyIcon
            {
                Icon = new Icon("Assets\\ZenithIcon.ico"),
                Text = "Zenith Virtual Assistant",
                Visible = true
            };
            _notifyIcon.DoubleClick += (s, e) => _showMainWindow();
            _notifyIcon.ContextMenuStrip = CreateContextMenu();
        }

        private ContextMenuStrip CreateContextMenu()
        {
            var menu = new ContextMenuStrip();
            menu.Items.Add("Show Main Window", null, (s, e) => _showMainWindow());
            menu.Items.Add("Show Widget", null, (s, e) => _showWidget());
            menu.Items.Add("Exit", null, (s, e) => _exitApplication());
            return menu;
        }

        public void Dispose()
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }
    }
}