namespace BlazorFront.Services
{
    public interface IRepository
    {
        Task<T> GetAsync<T>(string url);
        Task<object> PostAsync<T>(string url, T entity);
        Task<object> PutAsync<T>(string url, T entity);
        Task<T> GetByIDAsync<T>(string url, int id);
        Task<T> GetByIDAsync<T>(string url, string id);
        Task<object> DeleteAsync(string url);

    }
}
