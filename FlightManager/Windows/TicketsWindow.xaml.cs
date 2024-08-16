using FlightManager.Utils;
using FlightManager.ViewModels;
using System.Windows;

namespace FlightManager.Windows
{
    public partial class TicketsWindow : Window
    {
        private readonly TicketsViewModel _viewModel;

        public TicketsWindow()
        {
            InitializeComponent();
            _viewModel = new TicketsViewModel();
            DataContext = _viewModel;
            LoadTickets();
        }

        private async void LoadTickets()
        {
            await _viewModel.LoadTicketsAsync();
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
                var loader = new TicketDataLoader();

                try
                {
                    // Загрузка и парсинг данных из файла
                    var tickets = await loader.LoadDataAsync(filePath); // Убедитесь, что это возвращает List<Ticket>

                    // Очистка текущего списка билетов и добавление новых данных
                    _viewModel.Tickets.Clear();
                    foreach (var ticket in tickets)
                    {
                        _viewModel.Tickets.Add(ticket);
                    }

                    // Отправка данных на сервер для замены
                    var success = await _viewModel.ReplaceTicketsInDatabaseAsync(tickets);
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
                var loader = new TicketDataLoader();

                try
                {
                    var tickets = await loader.LoadDataAsync(filePath);
                    var success = await _viewModel.AddTicketsToDatabaseAsync(tickets);
                    if (success)
                    {
                        MessageBox.Show("Новые билеты успешно добавлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка при добавлении новых билетов в базу данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                FileName = "tickets_data"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var filePath = saveFileDialog.FileName;
                var saver = new TicketDataSaver();

                try
                {
                    await saver.SaveDataAsync(_viewModel.Tickets, filePath);
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
