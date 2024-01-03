using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.Domain.Medicine;

public interface IMedicalReactionRepository
{
    public Task<IList<MedicalReaction>> ListAsync(string? filterName);
}
