using System.Windows;

namespace FlightManager.Windows
{
    public partial class PassengersWindow : Window
    {
        public PassengersWindow()
        {
            InitializeComponent();
            DataContext = ((ViewModelLocator)Application.Current.Resources["ViewModelLocator"]).PassengersViewModel;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
