using System.Windows;

namespace FlightManager.Windows
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFlightsWindow_Click(object sender, RoutedEventArgs e)
        {
            var flightsWindow = new FlightsWindow();
            flightsWindow.Show();
        }

        private void OpenPassengersWindow_Click(object sender, RoutedEventArgs e)
        {
            var passengersWindow = new PassengersWindow();
            passengersWindow.Show();
        }

        private void OpenTicketsWindow_Click(object sender, RoutedEventArgs e)
        {
            var ticketsWindow = new TicketsWindow();
            ticketsWindow.Show();
        }
    }
}
