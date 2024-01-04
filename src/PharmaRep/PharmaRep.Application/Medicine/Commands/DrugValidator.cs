using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Domain.Medicine.ValueObjects;

namespace PharmaRep.Application.Medicine.Commands
{
    public class DrugValidator
    {
        public static (bool result, string? message) ValidateDrugUpdate(Drug drug) => drug.DrugStatus switch
        {
            DrugStatusEnum.WaitingForApproval => (false, "Cannot update drug status to waiting for approval."),
            DrugStatusEnum.Active when !drug.AdverseReactions.Any() => (false, "Cannot activate a drug status without adverse reactions."),
            DrugStatusEnum.Active when !drug.Indications.Any() => (false, "Cannot activate a drug status without indications."),
            DrugStatusEnum.Active => (true, null),
            DrugStatusEnum.Disabled => (true, null),
            _ => (false, "Cannot update drug status: unsupported scenario.")
        };
    }
}
