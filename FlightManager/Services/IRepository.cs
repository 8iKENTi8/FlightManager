public interface IRepository<T>
{
    Task<List<T>> GetAllAsync(); // Загрузка всех данных
    Task<bool> ReplaceAllAsync(IEnumerable<T> entities); // Полное обновление данных
    Task<bool> AddAsync(IEnumerable<T> entities); // Добавление данных
}
