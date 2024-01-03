using PharmaRep.Application.Common;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine;
using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.Application.Brand.Queries;

public record BrandQuery(string? filterName=null);
public class BrandQueryHandler(IBrandRepository repo) 
    : IQueryHandler<BrandQuery, IEnumerable<Domain.Brand.Entities.Brand>>
{
    private readonly IBrandRepository _repo = repo;

    public async Task<IEnumerable<Domain.Brand.Entities.Brand>> HandleAsync(BrandQuery query)
    {
         return await _repo.ListAsync(query.filterName);
    }
}

