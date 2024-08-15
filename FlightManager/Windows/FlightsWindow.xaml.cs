using FlightManager.Utils;
using FlightManager.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private async void ReplaceData_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var loader = new FlightDataLoader();

                try
                {
                    // Загрузка данных из выбранного файла
                    var flights = await loader.LoadDataAsync(filePath);
                    _viewModel.Flights.Clear();
                    foreach (var flight in flights)
                    {
                        _viewModel.Flights.Add(flight);
                    }

                    // Перезапись данных в базе
                    var success = await _viewModel.ReplaceFlightsInDatabaseAsync(_viewModel.Flights);
                    if (success)
                    {
                        MessageBox.Show("Данные успешно перезаписаны в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка при перезаписи данных в базу данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка при загрузке данных из файла.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async void AddData_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var loader = new FlightDataLoader();

                try
                {
                    // Загрузка данных из выбранного файла
                    var flights = await loader.LoadDataAsync(filePath);

                    // Добавление новых данных в базу
                    var success = await _viewModel.AddFlightsToDatabaseAsync(flights);
                    if (success)
                    {
                        MessageBox.Show("Новые рейсы успешно добавлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка при добавлении новых рейсов в базу данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка при загрузке данных из файла.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
