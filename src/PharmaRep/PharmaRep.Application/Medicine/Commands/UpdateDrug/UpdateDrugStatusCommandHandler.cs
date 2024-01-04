using PharmaRep.Application.Common;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine.Aggregates;
using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.Application.Medicine.Commands.RegisterDrug;

public class UpdateDrugStatusCommandHandler(IDrugRepository repo, IIdentifierService identifier, IPharmaUnitOfWork unitOfWork)
    : ICommandHandler<UpdateDrugStatusCommand, EntityUpdated>
{
   
    public async Task<CommandResponse<EntityUpdated?>> HandleAsync(UpdateDrugStatusCommand command)
    {
        var aggregate = await repo.GetByIdAsync(command.Id);
        if (aggregate is null)
            return CommandResponse<EntityUpdated?>.NotFound();

        aggregate.DrugStatus = command.NewStatus;

        var validationResult = DrugValidator.ValidateDrugUpdate(MapToDrug(aggregate));
        if (!validationResult.result)
        {
            return CommandResponse<EntityUpdated?>.Error(validationResult.message!);
        }

        await repo.UpdateStatusAsync(aggregate.Id, aggregate.DrugStatus);
        return CommandResponse<EntityUpdated?>.Success(new EntityUpdated { Id = aggregate.Id });
    }
    public Drug MapToDrug(DrugAggregate aggregate)
    {
        return new Drug()
        {
            AdverseReactions = aggregate.AdverseReactions.Select(p => p.Id).ToList(),
            Indications = aggregate.Indications!.Select(p => p.Id).ToList(),
            Description = aggregate.Description,
            Name = aggregate.Name,
            BrandId = aggregate.Brand.Id,
            DrugStatus = aggregate.DrugStatus,
            DateCreated = aggregate.DateCreated,
            Id = aggregate.Id,
            UserCreatedId = aggregate.UserCreated.Id
        };
    }
}

