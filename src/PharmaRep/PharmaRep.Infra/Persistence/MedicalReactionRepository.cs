using Microsoft.Data.SqlClient;
using PharmaRep.Domain.Medicine;
using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Infra.Persistence.Common;

namespace PharmaRep.Infra.Persistence
{

    public class MedicalReactionRepository(ISqlConnectionFactory connFactory) : IMedicalReactionRepository
    {
        public required ISqlConnectionFactory connFactory { get; set; } = connFactory;

        public async Task<IList<MedicalReaction>> ListAsync(string? filterName)
        {
            var results = new List<MedicalReaction>();
            using (var connection = connFactory.GetConnection())
            {
                var query = new SqlBuilder("SELECT Id, [Reaction], Description FROM [MedicalReaction]");
                var command = new SqlCommand();

                query.AddIf(filterName, () =>
                {
                    command.Parameters.AddWithValue("@name", filterName);
                    return "[Reaction] LIKE @name + '%'";
                });


                command.CommandText = query.ToString();
                command.Connection = connection;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new MedicalReaction
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Reaction")),
                            Description = reader.GetSafeString("Description"),
                        };

                        results.Add(item);
                    }
                }
            }
            return results;
        }
    }
}
