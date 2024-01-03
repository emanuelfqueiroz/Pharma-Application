namespace PharmaRep.Domain.Brand.Entities;

public class Brand
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? WebSite { get; set; }
}
