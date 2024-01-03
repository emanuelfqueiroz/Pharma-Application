using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PharmaRep.Infra.Security;
using PharmaRep.Infra.Security.Models;

namespace PharmaRep.Infra.Persistence;

internal class AuthRepository(IConfiguration config) : IAuthRepository
{
    public required IConfiguration Config { get; set; } = config;
    private string connectionString => Config.GetConnectionString("DefaultConnection")!;

    public async Task<UserAuth?> AddUserAsync(string name, string email, string encodedPassword)
    {
        using (var connection = new SqlConnection(connectionString))
        {
          
            await connection.OpenAsync();
            var command = new SqlCommand("INSERT INTO [PharmaUser] (FullName, Email, EncodedPassword) VALUES (@fullname, @email, @encoded); SELECT SCOPE_IDENTITY();", connection);
            command.Parameters.AddWithValue("@fullname", name);
            command.Parameters.AddWithValue("@email", email);
            command.Parameters.AddWithValue("@encoded", encodedPassword);

            var result = await command.ExecuteScalarAsync();
            
            if (result != null)
            {
                return new UserAuth(
                    id: Convert.ToInt32(result),
                    name: name,
                    email: email,
                    encodedPassword: encodedPassword
                );
            }
        }

        return null;
    }

    public async Task<UserAuth?> GetUserAsync(string email)
    {
        using (var connection = new SqlConnection(connectionString))
        {
            var command = new SqlCommand("SELECT Id, FullName, Email, EncodedPassword FROM [PharmaUser] WHERE Email = @email", connection);
            command.Parameters.AddWithValue("@email", email);

            await connection.OpenAsync();

            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    return new UserAuth(

                        id: reader.GetInt32(reader.GetOrdinal("Id")),
                        name: reader.GetString(reader.GetOrdinal("FullName")),
                        email: reader.GetString(reader.GetOrdinal("Email")),
                        encodedPassword: reader.GetString(reader.GetOrdinal("EncodedPassword"))
                    );
                    
                }
            }
        }

        return null;
    }
}