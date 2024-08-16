using System.Windows;

namespace FlightManager.Windows
{
    public partial class FlightsWindow : Window
    {
        public FlightsWindow()
        {
            InitializeComponent();
            // Установка DataContext в конструкторе для связи с ViewModel
            DataContext = new FlightsViewModel();
        }

        // Обработчик события для кнопки "Назад"
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }
    }
}
