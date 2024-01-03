using PharmaRep.Infra.Security.Models;

namespace PharmaRep.Infra.Security;

public interface IAuthRepository
{
    Task<UserAuth?> GetUserAsync(string email);
    Task<UserAuth?> AddUserAsync(string name, string email, string encodedPassword);
}
