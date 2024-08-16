using FlightManager.Models;
using FlightManager.Utils;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Windows.Input;
using FlightManager.Utils.Helpers;

public class FlightsViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Flight> _flights;
    private readonly FlightRepository _flightService;
    private readonly FlightDataLoader _dataLoader;
    private readonly FlightDataSaver _dataSaver;

    public ICommand ReplaceDataCommand { get; }
    public ICommand AddDataCommand { get; }
    public ICommand SaveDataCommand { get; }

    public FlightsViewModel()
    {
        _flights = new ObservableCollection<Flight>();
        _flightService = new FlightRepository();
        _dataLoader = new FlightDataLoader();
        _dataSaver = new FlightDataSaver();

        ReplaceDataCommand = new RelayCommand(async () => await ReplaceDataAsync());
        AddDataCommand = new RelayCommand(async () => await AddDataAsync());
        SaveDataCommand = new RelayCommand(async () => await SaveDataAsync());

        // Загрузка данных из базы данных при инициализации
        LoadFlightsFromDatabaseAsync().ConfigureAwait(false);
    }

    public ObservableCollection<Flight> Flights
    {
        get => _flights;
        set
        {
            if (_flights != value)
            {
                _flights = value;
                OnPropertyChanged(nameof(Flights));
            }
        }
    }

    private async Task LoadFlightsFromDatabaseAsync()
    {
        var flights = await _flightService.GetAllAsync();
        Flights.Clear();
        foreach (var flight in flights)
        {
            Flights.Add(flight);
        }
    }

    private async Task ReplaceDataAsync()
    {
        var filePath = DialogHelper.ShowOpenFileDialog();
        if (filePath != null)
        {
            try
            {
                var flights = await _dataLoader.LoadDataAsync(filePath);
                Flights.Clear();
                foreach (var flight in flights)
                {
                    Flights.Add(flight);
                }

                var success = await ReplaceFlightsInDatabaseAsync(Flights);
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
                var flights = await _dataLoader.LoadDataAsync(filePath);
                var success = await AddFlightsToDatabaseAsync(flights);
                if (success)
                {
                    MessageBox.Show("Новые рейсы успешно добавлены в базу данных.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    await LoadFlightsFromDatabaseAsync();
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

    private async Task SaveDataAsync()
    {
        var filePath = DialogHelper.ShowSaveFileDialog(defaultFileName: "flights_data");
        if (filePath != null)
        {
            try
            {
                await _dataSaver.SaveDataAsync(Flights, filePath);
                MessageBox.Show("Данные успешно сохранены в файл.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch
            {
                MessageBox.Show("Произошла ошибка при сохранении данных в файл.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async Task<bool> ReplaceFlightsInDatabaseAsync(IEnumerable<Flight> flights)
    {
        return await _flightService.ReplaceAllAsync(flights);
    }

    private async Task<bool> AddFlightsToDatabaseAsync(IEnumerable<Flight> flights)
    {
        var response = await _flightService.AddAsync(flights);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.Conflict)
            {
                var conflictResponse = JsonConvert.DeserializeObject<dynamic>(content);
                var message = conflictResponse?.Message;
                var existingFlights = conflictResponse?.ExistingFlights;
                MessageBox.Show(message, "Конфликт данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                await LoadFlightsFromDatabaseAsync();
            }
            return true;
        }
        return false;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
