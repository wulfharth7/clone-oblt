namespace clone_oblt.Services.Interfaces
{
    public interface IObiletApiService
    {
        Task<T> PostAsync<T>(object body);
    }
}
