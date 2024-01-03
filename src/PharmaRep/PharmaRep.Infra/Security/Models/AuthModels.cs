using System.ComponentModel.DataAnnotations;

namespace PharmaRep.Infra.Security.Models;

public class LoginUser
{
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}


public record RegisterUser
{
    [Required]
    public required string FullName { get; set; }
    [Required]
    public required string Email { get; set; }
    [Required]
    public required string Password { get; set; }
}
public record RegisteredUser(int Id, string Email);
public record TokenCredential(string Token);
