using FlightManager.Utils;
using FlightManager.Utils.Helpers;
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
            var filePath = DialogHelper.ShowOpenFileDialog();
            if (filePath != null)
            {
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
            var filePath = DialogHelper.ShowOpenFileDialog();
            if (filePath != null)
            {
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

        private async void SaveData_Click(object sender, RoutedEventArgs e)
        {
            var filePath = DialogHelper.ShowSaveFileDialog(defaultFileName: "flights_data");
            if (filePath != null)
            {
                var saver = new FlightDataSaver();

                try
                {
                    // Сохранение данных в выбранный файл
                    await saver.SaveDataAsync(_viewModel.Flights, filePath);
                    MessageBox.Show("Данные успешно сохранены в файл.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных в файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
