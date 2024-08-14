using FlightManager.Utils;
using FlightManager.ViewModels;
using System;
using System.Windows;

namespace FlightManager.Windows
{
    public partial class FlightsWindow : Window
    {
        private FlightsViewModel _viewModel;

        public FlightsWindow()
        {
            InitializeComponent();
            _viewModel = new FlightsViewModel();
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
                var loader = new FlightDataLoader();
                var flights = await loader.LoadDataAsync(openFileDialog.FileName);
                _viewModel.Flights.Clear();
                foreach (var flight in flights)
                {
                    _viewModel.Flights.Add(flight);
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
           
            this.Close();
        }
    }
}
