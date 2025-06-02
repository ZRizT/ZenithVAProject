using System;
using System.Windows;
using ZenithVirtualAssistant.Services;
using ZenithVirtualAssistant.Models;

namespace ZenithVirtualAssistant.Views
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseService _databaseService;
        private readonly WidgetWindow _widgetWindow;

        public MainWindow(DatabaseService databaseService, WidgetWindow widgetWindow)
        {
            InitializeComponent();
            _databaseService = databaseService;
            _widgetWindow = widgetWindow;
            LoadHistory();
        }

        public void LoadHistory() // Change to public
        {
            HistoryListView.ItemsSource = _databaseService.GetCommandHistory();
        }

        private void ShowWidget_Click(object sender, RoutedEventArgs e)
        {
            _widgetWindow.Show();
            Hide();
        }
    }
}