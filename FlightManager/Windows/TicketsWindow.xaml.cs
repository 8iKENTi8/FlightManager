using System.Windows;

namespace FlightManager.Windows
{
    public partial class TicketsWindow : Window
    {
        public TicketsWindow()
        {
            InitializeComponent();
            DataContext = ((ViewModelLocator)Application.Current.Resources["ViewModelLocator"]).TicketsViewModel;
        }
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
