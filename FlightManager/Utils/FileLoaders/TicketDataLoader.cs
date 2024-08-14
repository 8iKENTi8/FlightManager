using FlightManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FlightManager.Utils
{
    public class TicketDataLoader : IDataLoader<Ticket>
    {
        public async Task<IEnumerable<Ticket>> LoadDataAsync(string filePath)
        {
            var tickets = new List<Ticket>();

            
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    var parts = line.Split(',');

                    if (parts.Length == 3)
                    {
                        tickets.Add(new Ticket
                        {
                            TicketId = int.Parse(parts[0]),
                            PassengerId = int.Parse(parts[1]),
                            FlightId = int.Parse(parts[2])
                        });
                    }
                }
            }

            return tickets;
        }
    }
}
