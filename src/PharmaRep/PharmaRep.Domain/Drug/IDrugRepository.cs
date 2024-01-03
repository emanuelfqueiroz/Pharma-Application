using PharmaRep.Domain.Medicine.Aggregates;
using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Domain.Medicine.ValueObjects;

namespace PharmaRep.Domain.Brand;

public interface IDrugRepository
{
    public Task<int> AddAsync(Drug drug);
    public Task<bool> UpdateAsync(Drug drug);

    public Task<DrugAggregate?> GetByIdAsync(int id);
    Task<IList<DrugInformation>> ListAsync(int pageSize, string? name = null, string? brandName =null, int? status = null, string? orderBy = null);
    Task<int> DeactivateAsync(int id, int byUserId);
    Task UpsertDrugReactionsAsync(int drugId, List<int> reactionsIds);
    Task UpsertDrugIndicationsAsync(int drugId, List<int> indicationsIds);
    Task<int> UpdateStatusAsync(int id, DrugStatusEnum drugStatus);
}
