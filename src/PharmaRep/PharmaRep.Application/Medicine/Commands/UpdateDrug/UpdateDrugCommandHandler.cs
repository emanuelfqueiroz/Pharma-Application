using PharmaRep.Application.Common;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine.Entities;

namespace PharmaRep.Application.Medicine.Commands.RegisterDrug;
public class UpdateDrugCommandHandler(IDrugRepository repo, IIdentifierService identifier, IPharmaUnitOfWork unitOfWork)
    : ICommandHandler<UpdateDrugCommand, EntityUpdated>
{ 
    public async Task<CommandResponse<EntityUpdated?>> HandleAsync(UpdateDrugCommand command)
    {
        try
        {
            var drug = MapToDrug(command);

            var validationResult = DrugValidator.ValidateDrugUpdate(drug);
            if (!validationResult.result)
            {
                return CommandResponse<EntityUpdated?>.Error(validationResult.message!);
            }

            unitOfWork.BeginTransaction();

            await Task.WhenAll(
                repo.UpdateAsync(drug),
                repo.UpsertDrugIndicationsAsync(command.Id, drug.Indications),
                repo.UpsertDrugReactionsAsync(command.Id, drug.AdverseReactions)
            );

            unitOfWork.Commit();
            return CommandResponse<EntityUpdated?>.Success(new EntityUpdated { Id = command.Id });
        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            throw;
        }
    }

   

    public Drug MapToDrug(UpdateDrugCommand command)
    {
        return new Drug
        {
            Id = command.Id,
            BrandId = command.BrandId,
            Name = command.Name,
            Description = command.Description,
            UserCreatedId = identifier.GetUserId(),
            DateCreated = DateTime.Now,
            AdverseReactions = command.AdverseReactions,
            DrugStatus = command.DrugStatus,
            Indications = command.Indications
        };
    }
    
}
