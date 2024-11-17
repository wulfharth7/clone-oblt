using System.Threading.Tasks;

namespace clone_oblt.Services.Interfaces
{
    public interface IApiServiceBase
    {
        Task<TResponse> SendRequestAsync<TRequest, TResponse>(TRequest requestBody, string endpoint);
    }
}
