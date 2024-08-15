using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

public interface IRepository<T>
{
    Task<List<T>> GetAllAsync(); // Загрузка всех данных
    Task<bool> ReplaceAllAsync(IEnumerable<T> entities); // Полное обновление данных
    Task<HttpResponseMessage> AddAsync(IEnumerable<T> entities); // Добавление данных
}
