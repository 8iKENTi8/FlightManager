using FlightManager.Models;
using FlightManager.Utils;
using FlightManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Windows;

namespace FlightManager.Windows
{
    public partial class FlightsWindow : Window
    {
        private FlightsViewModel _viewModel;
        private readonly HttpClient _httpClient;

        public FlightsWindow()
        {
            InitializeComponent();
            _viewModel = new FlightsViewModel();
            DataContext = _viewModel;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:44305/api/") 
            };
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

                // Отправляем данные в API сразу после загрузки
                await PostFlightsToApiAsync(_viewModel.Flights);
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
                var saver = new FlightDataSaver();
                await saver.SaveDataAsync(_viewModel.Flights, saveFileDialog.FileName);
            }
        }

        private async Task PostFlightsToApiAsync(ObservableCollection<Flight> flights)
        {
            var flightsList = new List<Flight>(flights);
            var response = await _httpClient.PostAsJsonAsync("Flights", flightsList);

            if (response.IsSuccessStatusCode)
            {
                MessageBox.Show("Данные успешно отправлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Произошла ошибка при отправке данных в базу данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
