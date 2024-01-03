using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.Domain.Brand;
public interface IBrandRepository
{
    public Task<IList<Entities.Brand>> ListAsync(string? filterName);
}
