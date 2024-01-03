using System.ComponentModel.DataAnnotations;

namespace PharmaRep.Domain.User.Enitities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string EncodedPassword { get; set; }
    public string Email { get; set; }

    public User(string name, string email, string encodedPassword)
    {
        Name = name;
        Email = email;
        EncodedPassword = encodedPassword;
    }
}