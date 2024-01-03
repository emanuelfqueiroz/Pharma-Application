using PharmaRep.Application.Common;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.Application.Medicine.Queries
{
    public record DrugQuery(string? name = null, string? brandName = null, int? status = null, string? orderby = null, int pageSize = 20);
    public class DrugQueryHandler(IDrugRepository repo)
        : IQueryHandler<DrugQuery, IEnumerable<DrugInformation>>
    {
        private readonly IDrugRepository _repo = repo;

        public async Task<IEnumerable<DrugInformation>> HandleAsync(DrugQuery query)
        {
            return await _repo.ListAsync(query.pageSize, query.name, query.brandName, query.status, query.orderby);
        }
    }

}
