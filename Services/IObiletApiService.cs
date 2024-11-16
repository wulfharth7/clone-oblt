namespace clone_oblt.Services
{
    public interface IObiletApiService
    {
        Task<T> PostAsync<T>(object body);
    }
}
