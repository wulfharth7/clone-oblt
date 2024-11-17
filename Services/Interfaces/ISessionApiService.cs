namespace clone_oblt.Services.Interfaces
{
    public interface ISessionApiService
    {
        Task<T> PostAsync<T>(object body);
    }
}
