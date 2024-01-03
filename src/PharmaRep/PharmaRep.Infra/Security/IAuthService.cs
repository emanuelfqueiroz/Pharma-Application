using PharmaRep.Infra.Security.Models;

namespace PharmaRep.Infra.Security;

public interface IAuthService
{
    /// <summary>
    /// Returns JWT token if login was successful, otherwise null
    /// </summary>
    Task<TokenCredential?> LoginAsync(string email, string password);
    /// <summary>
    /// Returns userId if registration was successful, otherwise null
    /// </summary>
    Task<RegisteredUser?> RegisterAsync(string Name, string Email, string Password);
}
