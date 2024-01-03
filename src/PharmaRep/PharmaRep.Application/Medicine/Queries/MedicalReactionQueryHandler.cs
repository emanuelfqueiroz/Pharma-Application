using PharmaRep.Application.Common;
using PharmaRep.Domain.Medicine;
using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.Application.Medicine.Queries
{
    public record MedicalReactionQuery(string? filterName = null);
    public class MedicalReactionQueryHandler(IMedicalReactionRepository repo)
        : IQueryHandler<MedicalReactionQuery, IEnumerable<MedicalReaction>>
    {
        private readonly IMedicalReactionRepository repo = repo;

        public async Task<IEnumerable<MedicalReaction>> HandleAsync(MedicalReactionQuery query)
        {
            return await repo.ListAsync(query.filterName);
        }
    }

}
