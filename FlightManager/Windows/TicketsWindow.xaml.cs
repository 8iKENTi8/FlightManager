using System.Windows;
using FlightManager.ViewModels;

namespace FlightManager.Windows
{
    public partial class TicketsWindow : Window
    {
        public TicketsWindow()
        {
            InitializeComponent();
            var viewModel = new TicketsViewModel();
            DataContext = viewModel;
            // Загрузка данных из базы данных при инициализации
            viewModel.LoadTicketsAsync().ConfigureAwait(false);
        }

        // Обработчик события для кнопки "Назад"
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
