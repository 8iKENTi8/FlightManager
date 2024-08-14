using FlightManager.Utils;
using FlightManager.ViewModels;
using System;
using System.Windows;

namespace FlightManager.Windows
{
    public partial class PassengersWindow : Window
    {
        private PassengersViewModel _viewModel;

        public PassengersWindow()
        {
            InitializeComponent();
            _viewModel = new PassengersViewModel();
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
                var loader = new PassengerDataLoader(); 
                var passengers = await loader.LoadDataAsync(openFileDialog.FileName);
                _viewModel.Passengers.Clear();
                foreach (var passenger in passengers)
                {
                    _viewModel.Passengers.Add(passenger);
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
                var saver = new PassengerDataSaver();
                await saver.SaveDataAsync(_viewModel.Passengers, saveFileDialog.FileName);
            }
        }
    }
}
