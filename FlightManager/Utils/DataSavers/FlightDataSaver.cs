using FlightManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FlightManager.Utils
{
    public class FlightDataSaver : IDataSaver<Flight>
    {
        public async Task SaveDataAsync(IEnumerable<Flight> data, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var flight in data)
                {
                    var line = $"{flight.FlightId},{flight.FlightNumber},{flight.DepartureTime},{flight.ArrivalTime}";
                    await writer.WriteLineAsync(line);
                }
            }
        }
    }
}
