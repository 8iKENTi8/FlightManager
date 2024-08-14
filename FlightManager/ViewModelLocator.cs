using FlightManager.ViewModels;

namespace FlightManager
{
    public class ViewModelLocator
    {
        public FlightsViewModel FlightsViewModel { get; } = new FlightsViewModel();
        public PassengersViewModel PassengersViewModel { get; } = new PassengersViewModel();
        public TicketsViewModel TicketsViewModel { get; } = new TicketsViewModel();
    }
}
