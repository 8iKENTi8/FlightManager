using FlightManager.Models;
using FlightManager.Repositories.Implementations;
using FlightManager.Utils;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using FlightManager.Utils.Helpers;
using System.ComponentModel;
using System.Windows;

namespace FlightManager.ViewModels
{
    public class TicketsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Ticket> _tickets;
        private readonly TicketRepository _ticketRepository;
        private readonly TicketDataLoader _dataLoader;
        private readonly TicketDataSaver _dataSaver;

        public ICommand ReplaceDataCommand { get; }
        public ICommand AddDataCommand { get; }
        public ICommand SaveDataCommand { get; }

        public TicketsViewModel()
        {
            _tickets = new ObservableCollection<Ticket>();
            _ticketRepository = new TicketRepository();
            _dataLoader = new TicketDataLoader();
            _dataSaver = new TicketDataSaver();

            ReplaceDataCommand = new RelayCommand(async () => await ReplaceDataAsync());
            AddDataCommand = new RelayCommand(async () => await AddDataAsync());
            SaveDataCommand = new RelayCommand(async () => await SaveDataAsync());

            // Загрузка данных из базы данных при инициализации
            LoadTicketsAsync().ConfigureAwait(false);
        }

        public ObservableCollection<Ticket> Tickets
        {
            get => _tickets;
            set
            {
                if (_tickets != value)
                {
                    _tickets = value;
                    OnPropertyChanged(nameof(Tickets));
                }
            }
        }

        public async Task LoadTicketsAsync()
        {
            var tickets = await _ticketRepository.GetAllAsync();
            Tickets.Clear();
            foreach (var ticket in tickets)
            {
                Tickets.Add(ticket);
            }
        }

        private async Task ReplaceDataAsync()
        {
            var filePath = DialogHelper.ShowOpenFileDialog();
            if (filePath != null)
            {
                try
                {
                    var tickets = await _dataLoader.LoadDataAsync(filePath);
                    Tickets.Clear();
                    foreach (var ticket in tickets)
                    {
                        Tickets.Add(ticket);
                    }

                    var success = await ReplaceTicketsInDatabaseAsync(Tickets);
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

        private async Task AddDataAsync()
        {
            var filePath = DialogHelper.ShowOpenFileDialog();
            if (filePath != null)
            {
                try
                {
                    var tickets = await _dataLoader.LoadDataAsync(filePath);
                    var success = await AddTicketsToDatabaseAsync(tickets);
                    if (success)
                    {
                        MessageBox.Show("Новые билеты успешно добавлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        await LoadTicketsAsync();
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

        private async Task SaveDataAsync()
        {
            var filePath = DialogHelper.ShowSaveFileDialog(defaultFileName: "tickets_data");
            if (filePath != null)
            {
                try
                {
                    await _dataSaver.SaveDataAsync(Tickets, filePath);
                    MessageBox.Show("Данные успешно сохранены в файл.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                    MessageBox.Show("Произошла ошибка при сохранении данных в файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task<bool> ReplaceTicketsInDatabaseAsync(IEnumerable<Ticket> tickets)
        {
            return await _ticketRepository.ReplaceAllAsync(tickets);
        }

        private async Task<bool> AddTicketsToDatabaseAsync(IEnumerable<Ticket> tickets)
        {
            var response = await _ticketRepository.AddAsync(tickets);
            return response.IsSuccessStatusCode;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
