using FlightManager.Api;
using FlightManager.Models;
using FlightManager.Utils;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class FlightRepository : IRepository<Flight>
{
    private readonly ApiClient _apiClient;

    public FlightRepository()
    {
        _apiClient = new ApiClient(ApiConfiguration.BaseAddress);
    }

    public async Task<List<Flight>> GetAllAsync()
    {
        var response = await _apiClient.GetAsync("Flights");

        if (response.IsSuccessStatusCode)
        {
            var flights = await response.Content.ReadFromJsonAsync<List<Flight>>();
            return flights ?? new List<Flight>();
        }

        return new List<Flight>();
    }

    public async Task<bool> ReplaceAllAsync(IEnumerable<Flight> flights)
    {
        var response = await _apiClient.PostAsJsonAsync("Flights/replace", flights);
        return response.IsSuccessStatusCode;
    }

    public async Task<HttpResponseMessage> AddAsync(IEnumerable<Flight> flights)
    {
        return await _apiClient.PostAsJsonAsync("Flights/add", flights);
    }

    public async Task<bool> SaveAsync(IEnumerable<Flight> flights)
    {
        return await ReplaceAllAsync(flights);
    }
}
