using System;
using System.Windows;
using System.Windows.Input;
using ZenithVirtualAssistant.Services;

namespace ZenithVirtualAssistant.Views
{
    public partial class WidgetWindow : Window
    {
        private readonly SystemMonitorService _systemMonitorService;
        private readonly System.Timers.Timer _timer;

        public WidgetWindow(SystemMonitorService systemMonitorService)
        {
            InitializeComponent();
            _systemMonitorService = systemMonitorService;
            _timer = new System.Timers.Timer(1000);
            _timer.Elapsed += (s, e) => UpdateSystemStats();
            _timer.Start();

            MouseDown += (s, e) => DragMove();
        }

        public void UpdateStatus(string status)
        {
            Dispatcher.Invoke(() => StatusText.Text = status);
        }

        private void UpdateSystemStats()
        {
            Dispatcher.Invoke(() =>
            {
                CpuText.Text = $"CPU: {_systemMonitorService.GetCpuUsage():F1}%";
                MemoryText.Text = $"RAM: {_systemMonitorService.GetAvailableMemory():F1} MB";
                DiskText.Text = $"Disk: {_systemMonitorService.GetTotalDiskUsage():F1}%";
            });
        }
    }
}