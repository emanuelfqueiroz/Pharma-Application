using PharmaRep.Application.Common;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine.Aggregates;

namespace PharmaRep.Application.Medicine.Queries
{
    public record DrugByIdQuery(int id);
    public class DrugByIdQueryHandler(IDrugRepository repo)
        : IQueryHandler<DrugByIdQuery, DrugAggregate?>
    {
        private readonly IDrugRepository _repo = repo;

        public async Task<DrugAggregate?> HandleAsync(DrugByIdQuery query)
        {
            return await _repo.GetByIdAsync(query.id);
        }
    }

   
}
