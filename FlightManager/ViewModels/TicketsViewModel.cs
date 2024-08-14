using FlightManager.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace FlightManager.ViewModels
{
    public class TicketsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Ticket> _tickets;

        public TicketsViewModel()
        {
            // Имитируем загрузку данных
            _tickets = new ObservableCollection<Ticket>
            {
                new Ticket { TicketId = 1, PassengerId = 101, FlightId = 201 },
                new Ticket { TicketId = 2, PassengerId = 102, FlightId = 202 }
            };
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
