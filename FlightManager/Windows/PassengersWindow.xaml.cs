using FlightManager.Utils;
using FlightManager.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        private async void ReplaceData_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var loader = new PassengerDataLoader();

                try
                {
                    var passengers = await loader.LoadDataAsync(filePath);
                    _viewModel.Passengers.Clear();
                    foreach (var passenger in passengers)
                    {
                        _viewModel.Passengers.Add(passenger);
                    }

                    var success = await _viewModel.ReplacePassengersInDatabaseAsync(_viewModel.Passengers);
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
                var loader = new PassengerDataLoader();

                try
                {
                    var passengers = await loader.LoadDataAsync(filePath);
                    var success = await _viewModel.AddPassengersToDatabaseAsync(passengers);
                    if (success)
                    {
                        MessageBox.Show("Новые пассажиры успешно добавлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка при добавлении новых пассажиров в базу данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FileName = "passengers_data"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                var saver = new PassengerDataSaver();

                try
                {
                    await saver.SaveDataAsync(_viewModel.Passengers, filePath);
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
