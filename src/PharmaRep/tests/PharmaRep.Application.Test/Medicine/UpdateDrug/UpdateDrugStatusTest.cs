using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using PharmaRep.Application.Common;
using PharmaRep.Application.Medicine.Commands.RegisterDrug;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine.Aggregates;
using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Domain.Medicine.ValueObjects;


namespace PharmaRep.Application.Test.Medicine
{
    public class UpdateDrugStatusTest
    {
        private const int newDrugId = 3;
        private const int currentUserId = 10;
        private readonly Fixture fixture;
        private readonly Mock<IDrugRepository> drugRepo;
        private readonly Mock<IPharmaUnitOfWork> unitOfWork;
        private readonly Mock<IIdentifierService> identifier;

        public UpdateDrugStatusTest()
        {
            var config = new AutoMoqCustomization()
            {
                ConfigureMembers = true
            };

            drugRepo = new Mock<IDrugRepository>();
            unitOfWork = new Mock<IPharmaUnitOfWork>();
            identifier = new Mock<IIdentifierService>();
            identifier.Setup(p => p.GetUserId()).Returns(currentUserId);

            fixture = new Fixture();
            fixture.Customize(config);
            fixture.Inject(drugRepo.Object);
            fixture.Inject(unitOfWork.Object);
            fixture.Inject(identifier.Object);

            //Customize Default Command
            //Set Default Status
            fixture.Customize<UpdateDrugStatusCommand>(p => p.With(p => p.NewStatus, DrugStatusEnum.Active));

            //Default GetById Object
            var aggregateSample = fixture.Build<DrugAggregate>()
              .With(p => p.DrugStatus, DrugStatusEnum.Active)
              .Create();

            drugRepo
                .Setup(p => p.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(aggregateSample);
        }

        [Fact]
        public async Task When_UpdateDrugStatus_Then_Return_EnittyUpdated()
        {
            var commandHandler = fixture.Create<UpdateDrugStatusCommandHandler>();
            var response = await commandHandler.HandleAsync(fixture.Create<UpdateDrugStatusCommand>());

            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Data?.Id);
        }

        [Fact]
        public async Task When_UpdateDrugStatus_Ensure_All_Data_Updated()
        {
            var commandHandler = fixture.Create<UpdateDrugStatusCommandHandler>();
            var command = fixture.Create<UpdateDrugStatusCommand>();
            var response = await commandHandler.HandleAsync(command);

            drugRepo.Verify(p => p.UpdateStatusAsync(It.IsAny<int>(), It.IsAny<DrugStatusEnum>()), Times.Once);
        }

        [Theory]
        [InlineData(DrugStatusEnum.WaitingForApproval, false)]
        [InlineData(DrugStatusEnum.Active, true)]
        [InlineData(DrugStatusEnum.Disabled, true)]
        public async Task When_UpdateDrugStatus_ShouldNot_Accept_WaitingForApproval(DrugStatusEnum newStatus, bool expectedResult)
        {
            var commandHandler = fixture.Create<UpdateDrugStatusCommandHandler>();
            var command = fixture.Build<UpdateDrugStatusCommand>()
                .With(p => p.NewStatus, newStatus)
                .Create();

            var response = await commandHandler.HandleAsync(command);

            Assert.Equal(expectedResult, response.IsSuccess);
        }
    }
}