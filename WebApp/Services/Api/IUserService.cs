using WebApp.Models;

namespace WebApp.Services.Api;

public interface IUserService
{
    Task<UserInfoDto?> GetCurrentUserAsync();
}
