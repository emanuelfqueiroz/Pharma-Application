using PharmaRep.Application.Common;
using PharmaRep.Domain.Medicine;
using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.Application.Medicine.Queries
{
    public record MedicalConditionQuery(string? filterName = null);
    public class MedicalConditionQueryHandler(IMedicalConditionRepository repo)
        : IQueryHandler<MedicalConditionQuery, IEnumerable<MedicalCondition>>
    {
        private readonly IMedicalConditionRepository repo = repo;

        public async Task<IEnumerable<MedicalCondition>> HandleAsync(MedicalConditionQuery query)
        {
            return await repo.ListAsync(query.filterName);
        }
    }
}
