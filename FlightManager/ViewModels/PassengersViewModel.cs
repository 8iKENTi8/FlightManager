using FlightManager.Models;
using FlightManager.Utils;
using FlightManager.Utils.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;

public class PassengersViewModel : INotifyPropertyChanged
{
    // Коллекция пассажиров для привязки к UI
    private ObservableCollection<Passenger> _passengers;

    // Репозиторий для работы с пассажирами через API
    private readonly PassengerRepository _passengerService;

    // Утилиты для загрузки и сохранения данных пассажиров
    private readonly PassengerDataLoader _dataLoader;
    private readonly PassengerDataSaver _dataSaver;

    // Команды для выполнения действий
    public ICommand ReplaceDataCommand { get; }
    public ICommand AddDataCommand { get; }
    public ICommand SaveDataCommand { get; }

    // Конструктор ViewModel инициализирует коллекции, утилиты и команды
    public PassengersViewModel()
    {
        _passengers = new ObservableCollection<Passenger>();
        _passengerService = new PassengerRepository();
        _dataLoader = new PassengerDataLoader();
        _dataSaver = new PassengerDataSaver();

        // Инициализация команд
        ReplaceDataCommand = new RelayCommand(async () => await ReplaceDataAsync());
        AddDataCommand = new RelayCommand(async () => await AddDataAsync());
        SaveDataCommand = new RelayCommand(async () => await SaveDataAsync());

        // Загрузка данных при инициализации ViewModel
        LoadPassengersFromDatabaseAsync().ConfigureAwait(false);
    }

    // Свойство для доступа к коллекции пассажиров
    public ObservableCollection<Passenger> Passengers
    {
        get => _passengers;
        set
        {
            if (_passengers != value)
            {
                _passengers = value;
                OnPropertyChanged(nameof(Passengers)); // Оповещение об изменении свойства
            }
        }
    }

    // Асинхронный метод для загрузки всех пассажиров из базы данных
    public async Task LoadPassengersFromDatabaseAsync()
    {
        var passengers = await _passengerService.GetAllAsync();
        Passengers.Clear();
        foreach (var passenger in passengers)
        {
            Passengers.Add(passenger);
        }
    }

    // Асинхронный метод для полной замены данных пассажиров в базе данных
    private async Task ReplaceDataAsync()
    {
        var filePath = DialogHelper.ShowOpenFileDialog();
        if (filePath != null)
        {
            try
            {
                var passengers = await _dataLoader.LoadDataAsync(filePath);
                Passengers.Clear();
                foreach (var passenger in passengers)
                {
                    Passengers.Add(passenger);
                }

                var success = await ReplacePassengersInDatabaseAsync(Passengers);
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

    // Асинхронный метод для добавления новых пассажиров в базу данных
    private async Task AddDataAsync()
    {
        var filePath = DialogHelper.ShowOpenFileDialog();
        if (filePath != null)
        {
            try
            {
                var passengers = await _dataLoader.LoadDataAsync(filePath);
                var success = await AddPassengersToDatabaseAsync(passengers);
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

    // Асинхронный метод для сохранения данных пассажиров в файл
    private async Task SaveDataAsync()
    {
        var filePath = DialogHelper.ShowSaveFileDialog(defaultFileName: "passengers_data");
        if (filePath != null)
        {
            try
            {
                await _dataSaver.SaveDataAsync(Passengers, filePath);
                MessageBox.Show("Данные успешно сохранены в файл.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при сохранении данных в файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Асинхронный метод для полной замены данных пассажиров в базе данных
    public async Task<bool> ReplacePassengersInDatabaseAsync(IEnumerable<Passenger> passengers)
    {
        return await _passengerService.ReplaceAllAsync(passengers);
    }

    // Асинхронный метод для добавления новых пассажиров в базу данных
    public async Task<bool> AddPassengersToDatabaseAsync(IEnumerable<Passenger> passengers)
    {
        var response = await _passengerService.AddAsync(passengers);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                var conflictResponse = JsonConvert.DeserializeObject<dynamic>(content);
                var message = conflictResponse?.Message;
                var existingPassengers = conflictResponse?.ExistingPassengers;

                MessageBox.Show(message, "Конфликт данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                await LoadPassengersFromDatabaseAsync();
            }
            return true;
        }
        return false;
    }

    // Событие для оповещения об изменении свойств в ViewModel
    public event PropertyChangedEventHandler? PropertyChanged;

    // Метод для вызова события PropertyChanged
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
