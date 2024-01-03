using PharmaRep.Application.Common;
using PharmaRep.Domain.Brand;

namespace PharmaRep.Application.Medicine.Commands.DeactivateDrug;

public record DectivateDrugCommand(int Id);

public class DeactivateDrugCommandHandler(IDrugRepository repo, IIdentifierService identifier) : ICommandHandler<DectivateDrugCommand, DeactivatedEntity>
{
    public async Task<CommandResponse<DeactivatedEntity?>> HandleAsync(DectivateDrugCommand command)
    {
        var byUserId = identifier.GetUserId();
        await repo.DeactivateAsync(command.Id, byUserId);
        return CommandResponse<DeactivatedEntity?>.Success(
            new DeactivatedEntity(command.Id, byUserId));
    }
}
