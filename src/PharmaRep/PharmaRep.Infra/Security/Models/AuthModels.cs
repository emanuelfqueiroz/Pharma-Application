using System.ComponentModel.DataAnnotations;

namespace PharmaRep.Infra.Security.Models;

public class LoginUser
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}


public record RegisterUser
{
    [Required]
    public required string FullName { get; set; }
    [Required]
    [EmailAddress]
    public required string Email { get; set; }
    [Required]
    [Length(5, 50)]
    public required string Password { get; set; }
}

public class RegisterUserResponse
{
    public RegisteredUser? RegisteredUser { get; set; }
    public bool IsSuccess { get; set; } 
    public string? ErrorMessage { get; set; }

    public RegisterUserResponse( bool isSuccess, string? errorMessage = null, RegisteredUser? registeredUser = null)
    {
        RegisteredUser = registeredUser;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    internal static RegisterUserResponse? Error(string errorMessage)
    {
        return new RegisterUserResponse(false, errorMessage);
    }
    internal static RegisterUserResponse? Success(RegisteredUser user)
    {
        return new RegisterUserResponse(true, registeredUser: user);
    }
}
public record RegisteredUser(int Id, string Email);
public record TokenCredential(string Token);
