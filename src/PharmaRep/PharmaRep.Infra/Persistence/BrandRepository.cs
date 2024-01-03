using Microsoft.Data.SqlClient;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Brand.Entities;
using PharmaRep.Infra.Persistence.Common;

namespace PharmaRep.Infra.Persistence
{
    public class BrandRepository(ISqlConnectionFactory connFactory) : IBrandRepository
    {
        public required ISqlConnectionFactory connFactory { get; set; } = connFactory;
      
        public async Task<IList<Brand>> ListAsync(string? filterName)
        {
            var brands = new List<Brand>();
            using (var connection = connFactory.GetConnection())
            {
                 var query = new SqlBuilder("SELECT Id, BrandName, Description, WebSite FROM [Brand]");
                var command = new SqlCommand();

                query.AddIf(filterName, () =>
                {
                    command.Parameters.AddWithValue("@brandName", filterName);
                    return "BrandName LIKE @brandName + '%'";
                });
              

                command.CommandText = query.ToString();
                command.Connection = connection;

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var brand = new Brand
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("BrandName")),
                            Description = reader.GetSafeString("Description"),
                            WebSite = reader.GetSafeString("WebSite"),
                        };

                        brands.Add(brand);
                    }
                }
            }
            return brands;
        }
    }

}
