using FlightManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FlightManager.Utils
{
    public class PassengerDataLoader : IDataLoader<Passenger>
    {
        public async Task<IEnumerable<Passenger>> LoadDataAsync(string filePath)
        {
            var passengers = new List<Passenger>();

            
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var parts = line.Split(',');

                    if (parts.Length == 4)
                    {
                        passengers.Add(new Passenger
                        {
                            PassengerId = int.Parse(parts[0]),
                            LastName = parts[1],
                            FirstName = parts[2],
                            MiddleName = parts[3]
                        });
                    }
                }
            }

            return passengers;
        }
    }
}
