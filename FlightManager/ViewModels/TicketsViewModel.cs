using FlightManager.Models;
using FlightManager.Repositories.Implementations;
using FlightManager.Utils;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace FlightManager.ViewModels
{
    public class TicketsViewModel
    {
        public ObservableCollection<Ticket> Tickets { get; set; }

        private readonly TicketRepository _ticketRepository;

        public TicketsViewModel()
        {
            Tickets = new ObservableCollection<Ticket>();
            _ticketRepository = new TicketRepository();
        }

        public async Task<bool> ReplaceTicketsInDatabaseAsync(IEnumerable<Ticket> tickets)
        {
            return await _ticketRepository.ReplaceAllAsync(tickets);
        }

        public async Task<bool> AddTicketsToDatabaseAsync(IEnumerable<Ticket> tickets)
        {
            var response = await _ticketRepository.AddAsync(tickets);
            return response.IsSuccessStatusCode;
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
    }
}
