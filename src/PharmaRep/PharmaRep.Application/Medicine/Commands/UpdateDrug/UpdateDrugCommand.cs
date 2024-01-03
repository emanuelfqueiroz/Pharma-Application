using PharmaRep.Domain.Medicine.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace PharmaRep.Application.Medicine.Commands.RegisterDrug
{
    public class UpdateDrugCommand
    {
        public required int Id { get; set; }

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
}
