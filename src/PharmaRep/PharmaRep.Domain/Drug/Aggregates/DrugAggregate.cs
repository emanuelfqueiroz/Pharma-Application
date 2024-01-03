using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Domain.Medicine.ValueObjects;
using PharmaRep.Domain.User.Enitities;
using System.Collections.ObjectModel;

namespace PharmaRep.Domain.Medicine.Aggregates;

public class DrugAggregate
{
    public record BrandData(int Id, string Name);
    public record UserData(int Id, string Name);
    public record MedicalReactionData(int Id, string Name);
    public record MedicalConditionData(int Id, string Name);

    public int Id { get; set; }
    public required BrandData Brand { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required UserData UserCreated { get; set; }
    public DateTime DateCreated { get; set; }
    public DrugStatusEnum DrugStatus { get; set; }
    public ReadOnlyCollection<MedicalReactionData> AdverseReactions { get; set; } = new List<MedicalReactionData>().AsReadOnly();
    public ReadOnlyCollection<MedicalConditionData>? Indications { get; set; } = new List<MedicalConditionData>().AsReadOnly();
}

