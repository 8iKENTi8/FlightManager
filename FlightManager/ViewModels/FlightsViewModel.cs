using FlightManager.Models;
using FlightManager.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;

namespace FlightManager.ViewModels
{
    public class FlightsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Flight> _flights;
        private readonly FlightService _flightService;

        public FlightsViewModel()
        {
            _flights = new ObservableCollection<Flight>();
            _flightService = new FlightService();
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

        public async Task LoadFlightsAsync()
        {
            var flights = await _flightService.GetFlightsAsync();
            Flights.Clear();
            foreach (var flight in flights)
            {
                Flights.Add(flight);
            }
        }

        public async Task<bool> SaveFlightsAsync(IEnumerable<Flight> flights)
        {
            return await _flightService.PostFlightsAsync(flights);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
