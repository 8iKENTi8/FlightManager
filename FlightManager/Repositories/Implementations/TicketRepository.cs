using FlightManager.Api;
using FlightManager.Models;
using FlightManager.Utils;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FlightManager.Repositories.Implementations
{
    public class TicketRepository : IRepository<Ticket>
    {
        private readonly ApiClient _apiClient;

        public TicketRepository()
        {
            _apiClient = new ApiClient(ApiConfiguration.BaseAddress); // Базовый адрес из конфигурации
        }

        // Метод для получения всех билетов
        public async Task<List<Ticket>> GetAllAsync()
        {
            var response = await _apiClient.GetAsync("Tickets"); // Эндпоинт для получения всех билетов

            if (response.IsSuccessStatusCode)
            {
                var tickets = await response.Content.ReadFromJsonAsync<List<Ticket>>();
                return tickets ?? new List<Ticket>();
            }

            return new List<Ticket>(); // Возвращаем пустой список в случае ошибки
        }

        // Метод для замены всех данных о билетах
        public async Task<bool> ReplaceAllAsync(IEnumerable<Ticket> tickets)
        {
            var response = await _apiClient.PostAsJsonAsync("Tickets/replace", tickets); // Эндпоинт для замены данных о билетах
            return response.IsSuccessStatusCode;
        }

        // Метод для добавления новых данных о билетах
        public async Task<HttpResponseMessage> AddAsync(IEnumerable<Ticket> tickets)
        {
            return await _apiClient.PostAsJsonAsync("Tickets/add", tickets); // Эндпоинт для добавления новых билетов
        }
    }
}
