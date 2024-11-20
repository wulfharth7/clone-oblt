using clone_oblt.Models;
using Microsoft.AspNetCore.Mvc;

namespace clone_oblt.Services.Interfaces
{
    public interface ISessionApiService
    {
        Task<SessionResponse> CreateSessionAsync([FromBody] SessionRequest requestbody);
    }
}
