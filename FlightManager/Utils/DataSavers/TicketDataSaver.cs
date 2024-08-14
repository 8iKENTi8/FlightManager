using FlightManager.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace FlightManager.Utils
{
    public class TicketDataSaver : IDataSaver<Ticket>
    {
        public async Task SaveDataAsync(IEnumerable<Ticket> data, string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var ticket in data)
                {
                    var line = $"{ticket.TicketId},{ticket.PassengerId},{ticket.FlightId}";
                    await writer.WriteLineAsync(line);
                }
            }
        }
    }
}
