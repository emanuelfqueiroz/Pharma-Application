using PharmaRep.Domain.Medicine.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PharmaRep.Application.Medicine.Commands.RegisterDrug
{
    public record UpdateDrugCommand
    {
        [JsonIgnore]
        public int Id { get; set; }

        public required int BrandId { get; set; }
        [MinLength(3)]
        public required string Name { get; set; }
        [MinLength(20)]
        public required string Description { get; set; }

        [EnumDataType(typeof(DrugStatusEnum))]
        public required DrugStatusEnum DrugStatus { get; set; }
        public required List<int> AdverseReactions { get; set; }
        public required List<int> Indications { get; set; }
    }
    public record UpdateDrugStatusCommand
    {
        [JsonIgnore]
        public int Id { get; set; }
        public required DrugStatusEnum NewStatus { get; set; }
    }
}
