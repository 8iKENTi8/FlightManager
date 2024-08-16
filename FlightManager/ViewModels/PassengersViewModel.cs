using FlightManager.Models;
using FlightManager.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;

public class PassengersViewModel : INotifyPropertyChanged
{
    // Коллекция пассажиров, используемая для связывания с представлением (UI)
    private ObservableCollection<Passenger> _passengers;

    // Репозиторий для работы с пассажирами через API
    private readonly PassengerRepository _passengerService;

    // Утилита для загрузки данных пассажиров из файлов
    private readonly PassengerDataLoader _dataLoader;

    // Утилита для сохранения данных пассажиров в файлы
    private readonly PassengerDataSaver _dataSaver;

    // Конструктор ViewModel, инициализирует коллекцию и репозиторий, загружает данные из базы данных
    public PassengersViewModel()
    {
        _passengers = new ObservableCollection<Passenger>();
        _passengerService = new PassengerRepository();
        _dataLoader = new PassengerDataLoader();
        _dataSaver = new PassengerDataSaver();

        // Асинхронная загрузка пассажиров из базы данных при инициализации ViewModel
        LoadPassengersFromDatabaseAsync().ConfigureAwait(false);
    }

    // Свойство для привязки коллекции пассажиров к UI
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
    public async Task<bool> ReplacePassengersInDatabaseAsync(IEnumerable<Passenger> passengers)
    {
        return await _passengerService.ReplaceAllAsync(passengers); // Замена данных через репозиторий
    }

    // Асинхронный метод для добавления новых пассажиров в базу данных
    public async Task<bool> AddPassengersToDatabaseAsync(IEnumerable<Passenger> passengers)
    {
        var response = await _passengerService.AddAsync(passengers); // Добавление данных через репозиторий
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            // Проверка на конфликт данных
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                // Обработка конфликта, вывод сообщения с деталями
                var conflictResponse = JsonConvert.DeserializeObject<dynamic>(content);
                var message = conflictResponse?.Message;
                var existingPassengers = conflictResponse?.ExistingPassengers;

                MessageBox.Show(message, "Конфликт данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                // Обновление списка пассажиров после успешного добавления
                await LoadPassengersFromDatabaseAsync();
            }
            return true;
        }
        return false;
    }

    // Событие для оповещения об изменении свойств в ViewModel (реализация INotifyPropertyChanged)
    public event PropertyChangedEventHandler PropertyChanged;

    // Метод для вызова события PropertyChanged
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
