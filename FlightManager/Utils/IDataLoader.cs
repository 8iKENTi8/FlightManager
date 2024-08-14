namespace FlightManager.Utils
{
    public interface IDataLoader<T>
    {
        Task<IEnumerable<T>> LoadDataAsync(string filePath);
    }
}
