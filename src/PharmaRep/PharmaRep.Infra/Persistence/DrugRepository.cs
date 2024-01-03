using Microsoft.Data.SqlClient;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine.Aggregates;
using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Domain.Medicine.ValueObjects;
using PharmaRep.Infra.Persistence.Common;
using static PharmaRep.Domain.Medicine.Aggregates.DrugAggregate;

namespace PharmaRep.Infra.Persistence
{


    public class DrugRepository(ISqlConnectionFactory connManager) : IDrugRepository
    {
        public required ISqlConnectionFactory connManager { get; set; } = connManager;

        public async Task<int> AddAsync(Drug drug)
        {
            using var command = connManager.CreateCommand(
                @"INSERT INTO [Drug] (Name, Description, BrandId, DrugStatus, DateCreated, UserCreatedId)
                    VALUES (@Name, @Description, @BrandId, @DrugStatus, @DateCreated, @UserCreatedId);
                    SELECT SCOPE_IDENTITY()");
            command.Parameters.AddWithValue("@Name", drug.Name);
            command.Parameters.AddWithValue("@Description", drug.Description);
            command.Parameters.AddWithValue("@BrandId", drug.BrandId);
            command.Parameters.AddWithValue("@DrugStatus", (int)drug.DrugStatus);
            command.Parameters.AddWithValue("@DateCreated", drug.DateCreated);
            command.Parameters.AddWithValue("@UserCreatedId", drug.UserCreatedId);

            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result);
        }

        public async Task<IList<DrugInformation>> ListAsync(int pageSize, string? name, string? brandName, int? status, string? orderBy = "Drug.Id desc")
        {
            var results = new List<DrugInformation>();
            using var connection = connManager.GetConnection();

            var query = new SqlBuilder(
                @"SELECT Top(@pageSize) Drug.Id, Drug.Name, Drug.Description, Drug.DrugStatus, Drug.DateCreated,
                    Brand.BrandName, Brand.Id as BrandId
                    FROM Drug 
                    INNER JOIN Brand ON Drug.BrandId = Brand.Id",
                orderBy: orderBy);

            var command = new SqlCommand();

            command.Parameters.AddWithValue("@pageSize", pageSize);
            query.AddIf(name, () =>
            {
                command.Parameters.AddWithValue("@name", name);
                return "drug.[Name] LIKE @name + '%'";
            });
            query.AddIf(brandName, () =>
            {
                command.Parameters.AddWithValue("@brandName", brandName);
                return "brand.[BrandName] LIKE @brandName + '%'";
            });

            query.AddIf(status, () =>
            {
                command.Parameters.AddWithValue("@status", status);
                return "drug.[DrugStatus] = @status";
            });


            command.CommandText = query.ToString();
            command.Connection = connection;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var item = new DrugInformation
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Description = reader.GetString(reader.GetOrdinal("Description")),
                        BrandName = reader.GetString(reader.GetOrdinal("BrandName")),
                        BrandId = reader.GetInt32(reader.GetOrdinal("BrandId")),
                        DrugStatus = (DrugStatusEnum)reader.GetInt32(reader.GetOrdinal("DrugStatus")),
                        DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    };

                    results.Add(item);
                }
            }
            return results;
        }

        public async Task<DrugAggregate?> GetByIdAsync(int id)
        {
            using var command = connManager.CreateCommand(
                @"SELECT Id, Name, Description, DrugStatus, DateCreated,
                            BrandName, BrandId,
                            UserCreatedName, UserCreatedId,
                            MedicalConditionData,
                            MedicalReactionData
                    FROM DrugAggregateView
                    WHERE Id = @id");
            command.Parameters.AddWithValue("@id", id);

            using var reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                return new DrugAggregate
                {
                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                    Name = reader.GetString(reader.GetOrdinal("Name")),
                    Description = reader.GetString(reader.GetOrdinal("Description")),


                    DrugStatus = (DrugStatusEnum)reader.GetInt32(reader.GetOrdinal("DrugStatus")),
                    DateCreated = reader.GetDateTime(reader.GetOrdinal("DateCreated")),
                    UserCreated = new UserData(

                        Id: reader.GetInt32(reader.GetOrdinal("UserCreatedId")),
                        Name: reader.GetString(reader.GetOrdinal("UserCreatedName"))
                    ),
                    Brand = new BrandData(

                        Id: reader.GetInt32(reader.GetOrdinal("BrandId")),
                        Name: reader.GetString(reader.GetOrdinal("BrandName"))
                    ),
                    Indications = reader.GetStructuredData<MedicalConditionData>("MedicalConditionData").AsReadOnly(),
                    AdverseReactions = reader.GetStructuredData<MedicalReactionData>("MedicalReactionData").AsReadOnly()
                };
            }


            return null;
        }
        public async Task<bool> UpdateAsync(Drug drug)
        {
            using var command = connManager.CreateCommand(
                @"UPDATE Drug SET Name = @Name, Description = @Description,DrugStatus = @DrugStatus WHERE Id = @Id");

            command.Parameters.AddWithValue("@Id", drug.Id);
            command.Parameters.AddWithValue("@Name", drug.Name);
            command.Parameters.AddWithValue("@Description", drug.Description);
            command.Parameters.AddWithValue("@DrugStatus", (int)drug.DrugStatus);

            return await command.ExecuteNonQueryAsync() == 1;

        }

        public async Task<int> DeactivateAsync(int id, int byUserId)
        {
            var rows = await UpdateStatusAsync(id, DrugStatusEnum.Disabled);
            if (rows == 1)
            {
                return id;
            }
            throw new Exception("Drug Deactivation Failed on Storage");
        }

        public async Task<int> UpdateStatusAsync(int id, DrugStatusEnum drugStatus)
        {
            using var command = connManager.CreateCommand("Update [Drug] Set DrugStatus = @drugstatus Where id = @id");
            command.Parameters.AddWithValue("@drugstatus", (int)drugStatus);
            command.Parameters.AddWithValue("@id", id);

            return await command.ExecuteNonQueryAsync();
        }

        public async Task UpsertDrugIndicationsAsync(int drugId, List<int> indicationsIds)
        {
            //Remove All
            using var delCommand = connManager.CreateCommand("DELETE FROM DrugIndication WHERE DrugId = @DrugId");
            delCommand.Parameters.AddWithValue("@DrugId", drugId);
            await delCommand.ExecuteNonQueryAsync();

            //Insert All
            foreach (var indicationId in indicationsIds.Distinct())
            {
                using var command = connManager.CreateCommand("INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (@DrugId, @IndicationId)");
                command.Parameters.AddWithValue("@DrugId", drugId);
                command.Parameters.AddWithValue("@IndicationId", indicationId);
                await command.ExecuteNonQueryAsync();
            }
        }
        public async Task UpsertDrugReactionsAsync(int drugId, List<int> reactionsIds)
        {
            //Remove All
            using var delCommand = connManager.CreateCommand("DELETE FROM DrugReaction WHERE DrugId = @DrugId");
            delCommand.Parameters.AddWithValue("@DrugId", drugId);
            await delCommand.ExecuteNonQueryAsync();

            //Insert All
            foreach (var reactionId in reactionsIds.Distinct())
            {
                using var command = connManager.CreateCommand("INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (@DrugId, @ReactionId)");
                command.Parameters.AddWithValue("@DrugId", drugId);
                command.Parameters.AddWithValue("@ReactionId", reactionId);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

}
