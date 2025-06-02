using System;
using System.Windows;
using ZenithVirtualAssistant.Services;
using ZenithVirtualAssistant.Utils;
using ZenithVirtualAssistant.Views;

namespace ZenithVirtualAssistant
{
    public partial class App : Application
    {
        private SystemTrayManager _systemTrayManager;
        private SpeechService _speechService;
        private OllamaService _ollamaService;
        private SystemControlService _systemControlService;
        private SystemMonitorService _systemMonitorService;
        private DatabaseService _databaseService;
        private WidgetWindow _widgetWindow;
        private MainWindow _mainWindow;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize services
            _databaseService = new DatabaseService();
            _ollamaService = new OllamaService("config.json");
            _systemControlService = new SystemControlService();
            _systemMonitorService = new SystemMonitorService();
            _speechService = new SpeechService("config.json", _ollamaService);
            _speechService.OnCommandRecognized += HandleCommand;

            // Initialize UI
            _widgetWindow = new WidgetWindow(_systemMonitorService);
            _mainWindow = new MainWindow(_databaseService, _widgetWindow);

            // System tray
            _systemTrayManager = new SystemTrayManager(
                () => _mainWindow.Show(),
                () => _widgetWindow.Show(),
                () => Shutdown()
            );

            // Show widget by default
            _widgetWindow.Show();
        }

        private async void HandleCommand(string command)
        {
            _widgetWindow.UpdateStatus($"Processing: {command}");
            string response;

            try
            {
                if (command.StartsWith("open ") || command == "shutdown")
                {
                    _systemControlService.ExecuteCommand(command);
                    response = $"Executed: {command}";
                }
                else
                {
                    response = await _ollamaService.ProcessCommandAsync(command);
                }
            }
            catch (Exception ex)
            {
                response = $"Error: {ex.Message}";
            }

            _speechService.Speak(response); // Line 69
            _databaseService.SaveCommand(command, response);
            _widgetWindow.UpdateStatus(response);
            _mainWindow.Dispatcher.Invoke(() => (_mainWindow as MainWindow)?.LoadHistory());
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _systemTrayManager.Dispose();
            _speechService.Stop(); // Line 78
            base.OnExit(e);
        }
    }
}