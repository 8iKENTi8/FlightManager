using System.Collections.Generic;
using System.Threading.Tasks;

namespace FlightManager.Utils
{
    public interface IDataSaver<T>
    {
        Task SaveDataAsync(IEnumerable<T> data, string filePath);
    }
}
