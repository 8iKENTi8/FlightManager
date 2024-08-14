using FlightManager.Utils;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FlightManager.Api
{
    public class ApiClient
    {
        private readonly HttpClient _httpClient;

        // Конструктор по умолчанию, использующий стандартный адрес из конфигурации
        public ApiClient() : this(ApiConfiguration.BaseAddress)
        {
        }

        // Конструктор с возможностью задания адреса
        public ApiClient(string baseAddress)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
        }

        // Метод для выполнения POST запросов
        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string requestUri, T content)
        {
            return await _httpClient.PostAsJsonAsync(requestUri, content);
        }

        // Метод для выполнения GET запросов
        public async Task<HttpResponseMessage> GetAsync(string requestUri)
        {
            return await _httpClient.GetAsync(requestUri);
        }
    }
}
