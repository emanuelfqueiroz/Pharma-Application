using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using PharmaRep.Application.Common;
using PharmaRep.Application.Medicine.Commands.RegisterDrug;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Domain.Medicine.ValueObjects;


namespace PharmaRep.Application.Test.Medicine.UpdateDrug
{
    public class UpdateDrugTest
    {
        private const int newDrugId = 3;
        private const int currentUserId = 10;
        private readonly Fixture fixture;
        private readonly Mock<IDrugRepository> drugRepo;
        private readonly Mock<IPharmaUnitOfWork> unitOfWork;
        private readonly Mock<IIdentifierService> identifier;

        public UpdateDrugTest()
        {
            var config = new AutoMoqCustomization()
            {
                ConfigureMembers = true
            };

            drugRepo = new Mock<IDrugRepository>();
            unitOfWork = new Mock<IPharmaUnitOfWork>();
            identifier = new Mock<IIdentifierService>();
            identifier.Setup(p => p.GetUserId()).Returns(currentUserId);
            drugRepo.Setup(p => p.UpdateAsync(It.IsAny<Drug>())).ReturnsAsync(true);


            fixture = new Fixture();
            fixture.Customize(config);
            fixture.Inject(drugRepo.Object);
            fixture.Inject(unitOfWork.Object);
            fixture.Inject(identifier.Object);

            //Set Default Status
            fixture.Customize<UpdateDrugCommand>(p => p.With(p => p.DrugStatus, DrugStatusEnum.Active));
        }


        [Fact]
        public async Task When_UpdateDrug_Ensure_All_Data_Updated()
        {
            var commandHandler = fixture.Create<UpdateDrugCommandHandler>();
            var response = await commandHandler.HandleAsync(fixture.Create<UpdateDrugCommand>());

            drugRepo.Verify(p => p.UpdateAsync(It.IsAny<Drug>()), Times.Once);
            drugRepo.Verify(p => p.UpsertDrugIndicationsAsync(It.IsAny<int>(), It.IsAny<List<int>>()), Times.Once);
            drugRepo.Verify(p => p.UpsertDrugReactionsAsync(It.IsAny<int>(), It.IsAny<List<int>>()), Times.Once);
        }

        [Theory]
        [InlineData(DrugStatusEnum.WaitingForApproval, false)]
        [InlineData(DrugStatusEnum.Active, true)]
        [InlineData(DrugStatusEnum.Disabled, true)]
        public async Task When_UpdateDrug_ShouldNot_Accept_WaitingForApproval(DrugStatusEnum newStatus, bool expectedResult)
        {
            var commandHandler = fixture.Create<UpdateDrugCommandHandler>();
            var command = fixture.Build<UpdateDrugCommand>()
                .With(p => p.DrugStatus, newStatus)
                .Create();

            var response = await commandHandler.HandleAsync(command);

            Assert.Equal(expectedResult, response.IsSuccess);
        }

        [Theory]
        [InlineData(DrugStatusEnum.Active, false)]
        [InlineData(DrugStatusEnum.Disabled, true)]
        public async Task When_UpdateDrug_ShouldNot_Accept_Active_Without_Indications(DrugStatusEnum newStatus, bool expectedResult)
        {
            var commandHandler = fixture.Create<UpdateDrugCommandHandler>();
            var command = fixture.Build<UpdateDrugCommand>()
                .With(p => p.DrugStatus, newStatus)
                .With(p => p.Indications, new List<int>())
                .Create();

            var response = await commandHandler.HandleAsync(command);

            Assert.Equal(expectedResult, response.IsSuccess);
        }
        [Theory]
        [InlineData(DrugStatusEnum.Active, false)]
        [InlineData(DrugStatusEnum.Disabled, true)]
        public async Task When_UpdateDrug_ShouldNot_Accept_Active_Without_Reactions(DrugStatusEnum newStatus, bool expectedResult)
        {
            var commandHandler = fixture.Create<UpdateDrugCommandHandler>();
            var command = fixture.Build<UpdateDrugCommand>()
                .With(p => p.DrugStatus, newStatus)
                .With(p => p.AdverseReactions, new List<int>())
                .Create();

            var response = await commandHandler.HandleAsync(command);

            Assert.Equal(expectedResult, response.IsSuccess);
        }

        [Fact]
        public async Task When_UpdateDrug_Then_Return_EnittyUpdated()
        {
            var commandHandler = fixture.Create<UpdateDrugCommandHandler>();
            var response = await commandHandler.HandleAsync(fixture.Create<UpdateDrugCommand>());

            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Data?.Id);
        }

        [Fact]
        public async Task When_UpdateDrug_Ensure_Transaction_LifeCycle()
        {
            var commandHandler = fixture.Create<UpdateDrugCommandHandler>();
            var response = await commandHandler.HandleAsync(fixture.Create<UpdateDrugCommand>());

            unitOfWork.Verify(p => p.BeginTransaction(), Times.Once);
            unitOfWork.Verify(p => p.Commit(), Times.Once);
            unitOfWork.Verify(p => p.Rollback(), Times.Never);
        }

        [Fact]
        public async Task OnError_UpdateDrug_Then_Rollback_And_ThrowsException()
        {
            var commandHandler = fixture.Create<UpdateDrugCommandHandler>();
            drugRepo.Setup(p => p.UpdateAsync(It.IsAny<Drug>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<Exception>(() => commandHandler.HandleAsync(fixture.Create<UpdateDrugCommand>()));
            unitOfWork.Verify(p => p.BeginTransaction(), Times.Once);
            unitOfWork.Verify(p => p.Rollback(), Times.Once);
            unitOfWork.Verify(p => p.Commit(), Times.Never);
        }
    }
}