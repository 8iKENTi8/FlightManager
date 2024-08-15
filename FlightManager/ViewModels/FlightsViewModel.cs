using FlightManager.Models;
using FlightManager.Utils;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

public class FlightsViewModel : INotifyPropertyChanged
{
    private ObservableCollection<Flight> _flights;
    private readonly FlightRepository _flightService;
    private readonly FlightDataLoader _dataLoader;
    private readonly FlightDataSaver _dataSaver;

    public FlightsViewModel()
    {
        _flights = new ObservableCollection<Flight>();
        _flightService = new FlightRepository();
        _dataLoader = new FlightDataLoader();
        _dataSaver = new FlightDataSaver();

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

    public async Task LoadFlightsFromDatabaseAsync()
    {
        var flights = await _flightService.GetAllAsync();
        Flights.Clear();
        foreach (var flight in flights)
        {
            Flights.Add(flight);
        }
    }

    public async Task<bool> ReplaceFlightsInDatabaseAsync(IEnumerable<Flight> flights)
    {
        return await _flightService.ReplaceAllAsync(flights);
    }

    public async Task<bool> AddFlightsToDatabaseAsync(IEnumerable<Flight> flights)
    {
        var success = await _flightService.AddAsync(flights);
        if (success)
        {
            // Обновление списка рейсов после успешного добавления данных
            await LoadFlightsFromDatabaseAsync();
        }
        return success;
    }


    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
