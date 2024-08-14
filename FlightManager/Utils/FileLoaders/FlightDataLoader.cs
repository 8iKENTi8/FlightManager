using FlightManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FlightManager.Utils
{
    public class FlightDataLoader : IDataLoader<Flight>
    {
        public async Task<IEnumerable<Flight>> LoadDataAsync(string filePath)
        {
            var flights = new List<Flight>();

           
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var parts = line.Split(',');

                    if (parts.Length == 4)
                    {
                        flights.Add(new Flight
                        {
                            FlightId = int.Parse(parts[0]),
                            FlightNumber = parts[1],
                            DepartureTime = DateTime.Parse(parts[2]),
                            ArrivalTime = DateTime.Parse(parts[3])
                        });
                    }
                }
            }

            return flights;
        }
    }
}
