using FlightManager.Api;
using FlightManager.Models;
using FlightManager.Utils;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FlightManager.Services
{
    public class FlightService
    {
        private readonly ApiClient _apiClient;

        // Конструктор FlightService инициализирует ApiClient с базовым адресом
        public FlightService()
        {
            _apiClient = new ApiClient(ApiConfiguration.BaseAddress);
        }

        // Метод для получения списка рейсов из API
        public async Task<List<Flight>> GetFlightsAsync()
        {
            // Отправляем GET запрос к API
            var response = await _apiClient.GetAsync("Flights");

            // Проверяем, что запрос успешен
            if (response.IsSuccessStatusCode)
            {
                // Читаем содержимое ответа как список рейсов
                // Если ответ пустой, возвращаем пустой список, чтобы избежать возвращения null
                var flights = await response.Content.ReadFromJsonAsync<List<Flight>>();
                return flights ?? new List<Flight>(); // Возвращаем пустой список, если flights равно null
            }
            else
            {
                // В случае ошибки возвращаем пустой список
                return new List<Flight>();
            }
        }

        // Метод для отправки списка рейсов в API
        public async Task<bool> PostFlightsAsync(IEnumerable<Flight> flights)
        {
            // Отправляем POST запрос с данными рейсов
            var response = await _apiClient.PostAsJsonAsync("Flights", flights);

            // Возвращаем true, если запрос успешен, иначе false
            return response.IsSuccessStatusCode;
        }
    }
}
