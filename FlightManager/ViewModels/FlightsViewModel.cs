using FlightManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FlightManager.ViewModels
{
    public class FlightsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Flight> _flights;
        private ObservableCollection<Passenger> _passengers;

        public FlightsViewModel()
        {
            // Инициализация данных
            _flights = new ObservableCollection<Flight>
            {
                new Flight { FlightId = 1, FlightNumber = "F123", DepartureTime = DateTime.Now, ArrivalTime = DateTime.Now.AddHours(2) },
                new Flight { FlightId = 2, FlightNumber = "F456", DepartureTime = DateTime.Now.AddDays(1), ArrivalTime = DateTime.Now.AddDays(1).AddHours(3) }
            };

            _passengers = new ObservableCollection<Passenger>
            {
                new Passenger { PassengerId = 1, LastName = "Ivanov", FirstName = "Ivan", MiddleName = "Ivanovich" },
                new Passenger { PassengerId = 2, LastName = "Petrov", FirstName = "Petr", MiddleName = "Petrovich" }
            };
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

        public ObservableCollection<Passenger> Passengers
        {
            get => _passengers;
            set
            {
                if (_passengers != value)
                {
                    _passengers = value;
                    OnPropertyChanged(nameof(Passengers));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
