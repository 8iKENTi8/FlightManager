using System.Windows;

namespace FlightManager.Windows
{
    public partial class PassengersWindow : Window
    {
        public PassengersWindow()
        {
            InitializeComponent();
            // Установка DataContext в конструкторе для связи с ViewModel
            DataContext = new PassengersViewModel();
        }

        // Обработчик события для кнопки "Назад"
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close(); 
        }
    }
}
