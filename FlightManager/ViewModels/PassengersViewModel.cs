using FlightManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FlightManager.ViewModels
{
    public class PassengersViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Passenger> _passengers;

        public PassengersViewModel()
        {
            _passengers = new ObservableCollection<Passenger>
            {
                new Passenger { PassengerId = 1, LastName = "Ivanov", FirstName = "Ivan", MiddleName = "Ivanovich" },
                new Passenger { PassengerId = 2, LastName = "Petrov", FirstName = "Petr", MiddleName = "Petrovich" }
            };
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
