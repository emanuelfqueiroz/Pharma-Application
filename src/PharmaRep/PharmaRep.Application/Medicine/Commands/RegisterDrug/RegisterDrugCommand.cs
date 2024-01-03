using PharmaRep.Application.Common;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Domain.Medicine.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace PharmaRep.Application.Medicine.Commands.RegisterDrug
{
    public record RegisterDrugCommand
    {
        [Required]
        public int BrandId { get; set; }
        [Required]
        public required string Name { get; set; }
        [Required]
        public required string Description { get; set; }
        [Required]
        public required List<int> Indications { get; set; }
        [Required]
        public required List<int> AdverseReactions { get; set; }
    }

    public class RegisterDrugCommandHandler(IDrugRepository repo, IIdentifierService identifier, IPharmaUnitOfWork unitOfWork) : ICommandHandler<RegisterDrugCommand, EntityCreated>
    {
        public async Task<CommandResponse<EntityCreated?>> HandleAsync(RegisterDrugCommand command)
        {
            try
            {
               
                unitOfWork.BeginTransaction();
                var drug = MapToDrug(command);

                await repo.AddAsync(drug).ContinueWith(id => drug.Id = id.Result);
                await Task.WhenAll(
                    repo.UpsertDrugIndicationsAsync(drug.Id, drug.Indications),
                    repo.UpsertDrugReactionsAsync(drug.Id, drug.AdverseReactions)
                ); ;

                
                unitOfWork.Commit();
                return CommandResponse<EntityCreated?>.Success(new EntityCreated { Id = drug.Id });
            }
            catch (Exception)
            {
                unitOfWork.Rollback();
                throw;
            }
           
        }
        public Drug MapToDrug(RegisterDrugCommand command)
        {
            return new Drug
            {
                BrandId = command.BrandId,
                Name = command.Name,
                Description = command.Description,
                Indications = command.Indications,
                AdverseReactions = command.AdverseReactions,
                UserCreatedId = identifier.GetUserId(),
                DateCreated = DateTime.Now,
                DrugStatus = DrugStatusEnum.WaitingForApproval                
            };
        }
    }
}
