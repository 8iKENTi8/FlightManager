using FlightManager.Api;
using FlightManager.Models;
using FlightManager.Utils;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class PassengerRepository : IRepository<Passenger>
{
    private readonly ApiClient _apiClient;

    public PassengerRepository()
    {
        _apiClient = new ApiClient(ApiConfiguration.BaseAddress);
    }

    public async Task<List<Passenger>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync("Passengers");

        if (response.IsSuccessStatusCode)
        {
            var passengers = await response.Content.ReadFromJsonAsync<List<Passenger>>();
            return passengers ?? new List<Passenger>();
        }

        return new List<Passenger>();
    }

    public async Task<bool> ReplaceAllAsync(IEnumerable<Passenger> passengers)
    {
        var response = await _apiClient.PostAsJsonAsync("Passengers/replace", passengers);
        return response.IsSuccessStatusCode;
    }

    public async Task<HttpResponseMessage> AddAsync(IEnumerable<Passenger> passengers)
    {
        return await _apiClient.PostAsJsonAsync("Passengers/add", passengers);
    }
}
