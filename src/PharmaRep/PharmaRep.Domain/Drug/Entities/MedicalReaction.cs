namespace PharmaRep.Domain.Medicine.Entities;

public class MedicalReaction
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
}
