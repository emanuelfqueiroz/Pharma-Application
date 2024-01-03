using Microsoft.Data.SqlClient;
using PharmaRep.Domain.Medicine;
using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Infra.Persistence.Common;

namespace PharmaRep.Infra.Persistence
{
    public class MedicalConditionRepository(ISqlConnectionFactory connFactory) : IMedicalConditionRepository 
    { 
        public required ISqlConnectionFactory connFactory { get; set; } = connFactory;

        public async Task<IList<MedicalCondition>> ListAsync(string? filterName)
        {
            var results = new List<MedicalCondition>();
            using (var connection = connFactory.GetConnection())
            {
                var query = new SqlBuilder("SELECT Id, [Name], Description FROM [MedicalCondition]");
                var command = new SqlCommand();

                query.AddIf(filterName, () =>
                {
                    command.Parameters.AddWithValue("@name", filterName);
                    return "[Name] LIKE @name + '%'";
                });


                command.CommandText = query.ToString();
                command.Connection = connection;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var item = new MedicalCondition
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Description = reader.GetString(reader.GetOrdinal("Description")),
                        };

                        results.Add(item);
                    }
                }
            }
            return results;
        }
    }
}
