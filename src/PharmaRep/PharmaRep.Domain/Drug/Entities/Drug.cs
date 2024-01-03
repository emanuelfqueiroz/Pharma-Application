using PharmaRep.Domain.Medicine.ValueObjects;

namespace PharmaRep.Domain.Medicine.Entities;

public class Drug
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int UserCreatedId { get; set; }
    public DateTime DateCreated { get; set; }
    public DrugStatusEnum DrugStatus { get; set; }
    public required List<int> AdverseReactions { get; set; }
    public required List<int> Indications { get; set; }
}

public class DrugInformation
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int BrandId { get; set; }
    public required string BrandName { get; set; }
    public DateTime DateCreated { get; set; }
    public DrugStatusEnum DrugStatus { get; set; }
}
