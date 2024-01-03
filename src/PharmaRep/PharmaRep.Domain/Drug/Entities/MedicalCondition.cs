namespace PharmaRep.Domain.Medicine.Entities;

public class MedicalCondition
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}