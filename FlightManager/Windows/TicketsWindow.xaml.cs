using FlightManager.Utils;
using FlightManager.ViewModels;
using System;
using System.Windows;

namespace FlightManager.Windows
{
    public partial class TicketsWindow : Window
    {
        private TicketsViewModel _viewModel;

        public TicketsWindow()
        {
            InitializeComponent();
            _viewModel = new TicketsViewModel();
            DataContext = _viewModel;
        }

        private async void LoadData_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var loader = new TicketDataLoader(); 
                var tickets = await loader.LoadDataAsync(openFileDialog.FileName);
                _viewModel.Tickets.Clear();
                foreach (var ticket in tickets)
                {
                    _viewModel.Tickets.Add(ticket);
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
     
            this.Close();
        }

        private async void SaveData_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var saver = new TicketDataSaver();
                await saver.SaveDataAsync(_viewModel.Tickets, saveFileDialog.FileName);
            }
        }
    }
}
