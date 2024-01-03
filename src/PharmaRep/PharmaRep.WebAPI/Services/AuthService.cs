//using Isopoh.Cryptography.Argon2;
//using Microsoft.IdentityModel.Tokens;
//using System.ComponentModel.DataAnnotations;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace PharmaRep.WebAPI.Services;

//public class LoginUser
//{
//    [Required]
//    public required string Email { get; set; }
//    [Required]
//    public required string Password { get; set; }
//}

//public class RegisterUser
//{
//    [Required]
//    public required string UserName { get; set; }
//    [Required]
//    public required string Email { get; set; }
//    [Required]
//    public required string Password { get; set; }
//}

//public record RegisteredUser(int Id);

//public interface IAuthRepository
//{
//    Task<UserAuth?> GetUserAsync(string email);
//    Task<UserAuth?> AddUserAsync(string name, string email, string encodedPassword);
//}
//public interface IAuthService
//{
//    /// <summary>
//    /// Returns JWT token if login was successful, otherwise null
//    /// </summary>
//    Task<TokenCredential?> LoginAsync(string email, string password);
//    /// <summary>
//    /// Returns userId if registration was successful, otherwise null
//    /// </summary>
//    Task<RegisteredUser?> RegisterAsync(string Name, string Email, string Password);
//}

//public class Role
//{
//    public const string Operator = "operator";
//    public const string Admin = "admin";
//}
//public record TokenCredential(string Token);
//public record UserAuth(int Id, string Name, string Email, string EncodedPassword);
//public class AuthService : IAuthService
//{
//    private readonly IConfiguration _configuration;
//    private readonly IAuthRepository _authRepository;
//    public AuthService(IConfiguration configuration, IAuthRepository authRepository)
//    {
//        _configuration = configuration!;
//        _authRepository = authRepository!;
//    }

//    /// <summary>
//    /// Returns JWT token if login was successful, otherwise null
//    /// </summary>
//    public async Task<TokenCredential?> LoginAsync(string email, string password)
//    {
//        // Search user in DB and verify password

//        var user = await _authRepository.GetUserAsync(email);

//        if (string.IsNullOrEmpty(user?.EncodedPassword) || Argon2.Verify(user.EncodedPassword, password) == false)
//        {
//            return null; //returning null intentionally to show that login was unsuccessful
//        }

//        // Create JWT token handler and get secret key

//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.ASCII.GetBytes(_configuration["JWT:SecretKey"]);

//        // Prepare list of user claims

//        var claims = new List<Claim>
//        {
//            new Claim(ClaimTypes.NameIdentifier, email),
//            new Claim(ClaimTypes.Role, Role.Operator),
//        };

//        // Create token descriptor

//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(claims),
//            IssuedAt = DateTime.UtcNow,
//            Issuer = _configuration["JWT:Issuer"],
//            Audience = _configuration["JWT:Audience"],
//            Expires = DateTime.UtcNow.AddDays(1),
//            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
//        };

//        // Create token and set it to user

//        var token = tokenHandler.CreateToken(tokenDescriptor);

//        return new TokenCredential(tokenHandler.WriteToken(token));

//    }

//    public async Task<RegisteredUser?> RegisterAsync(string Name, string Email, string Password)
//    {
//        var user = await _authRepository.AddUserAsync(Name, Email, Argon2.Hash(Password));
//        return user != null ? new RegisteredUser(user.Id) : null;
//    }

//}
