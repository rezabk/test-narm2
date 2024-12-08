using Domain.Entities.UserAgg;

namespace Application.Services.Interface.Utils;

public interface ITokenService
{
    Task<string> CreateToken(ApplicationUser user);
}