using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.Domain.Medicine;

public interface IMedicalConditionRepository
{
    public Task<IList<MedicalCondition>> ListAsync(string? filterName);
}
