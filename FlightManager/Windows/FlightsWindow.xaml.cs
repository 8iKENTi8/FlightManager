using System.Windows;

namespace FlightManager.Windows
{
    public partial class FlightsWindow : Window
    {
        public FlightsWindow()
        {
            InitializeComponent();
            DataContext = ((ViewModelLocator)Application.Current.Resources["ViewModelLocator"]).FlightsViewModel;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
