using FlightManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FlightManager.Utils
{
    public class PassengerDataSaver : IDataSaver<Passenger>
    {
        public async Task SaveDataAsync(IEnumerable<Passenger> data, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var passenger in data)
                {
                    var line = $"{passenger.PassengerId},{passenger.LastName},{passenger.FirstName},{passenger.MiddleName}";
                    await writer.WriteLineAsync(line);
                }
            }
        }
    }
}
