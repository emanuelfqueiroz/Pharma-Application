using System.Text.Json.Serialization;

namespace PharmaRep.Infra.Security.Models;

public class UserAuth
{
    public UserAuth(int id, string name, string email, string encodedPassword)
    {
        Id = id;
        Name = name;
        Email = email;
        EncodedPassword = encodedPassword;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    [JsonIgnore]
    public string EncodedPassword { get; set; }
}