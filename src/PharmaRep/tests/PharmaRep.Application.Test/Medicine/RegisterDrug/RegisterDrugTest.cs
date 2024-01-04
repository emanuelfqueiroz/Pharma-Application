using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using PharmaRep.Application.Common;
using PharmaRep.Application.Medicine.Commands.RegisterDrug;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Medicine.Entities;


namespace PharmaRep.Application.Test.Medicine.RegisterDrug
{
    public class RegisterDrugTest
    {
        private const int newDrugId = 3;
        private const int currentUserId = 10;
        private readonly Fixture fixture;
        private readonly Mock<IDrugRepository> drugRepo;
        private readonly Mock<IPharmaUnitOfWork> unitOfWork;
        private readonly Mock<IIdentifierService> identifier;

        public RegisterDrugTest()
        {
            var config = new AutoMoqCustomization()
            {
                ConfigureMembers = true
            };

            drugRepo = new Mock<IDrugRepository>();
            unitOfWork = new Mock<IPharmaUnitOfWork>();
            identifier = new Mock<IIdentifierService>();
            identifier.Setup(p => p.GetUserId()).Returns(currentUserId);
            drugRepo.Setup(p => p.AddAsync(It.IsAny<Drug>())).ReturnsAsync(newDrugId);


            fixture = new Fixture();
            fixture.Customize(config);
            fixture.Inject(drugRepo.Object);
            fixture.Inject(unitOfWork.Object);
            fixture.Inject(identifier.Object);
        }


        [Fact]
        public async Task When_RegisterNewDrug_Ensure_All_Data_Inserted()
        {
            var commandHandler = fixture.Create<RegisterDrugCommandHandler>();
            var response = await commandHandler.HandleAsync(fixture.Create<RegisterDrugCommand>());


            drugRepo.Verify(p => p.AddAsync(It.IsAny<Drug>()), Times.Once);
            drugRepo.Verify(p => p.UpsertDrugIndicationsAsync(It.IsAny<int>(), It.IsAny<List<int>>()), Times.Once);
            drugRepo.Verify(p => p.UpsertDrugReactionsAsync(It.IsAny<int>(), It.IsAny<List<int>>()), Times.Once);
        }

        [Fact]
        public async Task When_RegisterNewDrug_Ensure_UserCreatedIdd()
        {
            var commandHandler = fixture.Create<RegisterDrugCommandHandler>();
            await commandHandler.HandleAsync(fixture.Create<RegisterDrugCommand>());
            drugRepo.Verify(p => p.AddAsync(It.Is<Drug>(d => d.UserCreatedId == currentUserId)));
        }


        [Fact]
        public async Task When_RegisterNewDrug_Then_Return_NewId()
        {
            var commandHandler = fixture.Create<RegisterDrugCommandHandler>();
            var response = await commandHandler.HandleAsync(fixture.Create<RegisterDrugCommand>());

            Assert.True(response.IsSuccess);
            Assert.Equal(newDrugId, response.Data!.Id);
        }

        [Fact]
        public async Task When_RegisterNewDrug_Ensure_Transaction_LifeCycle()
        {
            var commandHandler = fixture.Create<RegisterDrugCommandHandler>();
            var response = await commandHandler.HandleAsync(fixture.Create<RegisterDrugCommand>());

            unitOfWork.Verify(p => p.BeginTransaction(), Times.Once);
            unitOfWork.Verify(p => p.Commit(), Times.Once);
            unitOfWork.Verify(p => p.Rollback(), Times.Never);
        }

        [Fact]
        public async Task OnError_RegisterNewDrug_Then_Rollback_And_ThrowsException()
        {
            var commandHandler = fixture.Create<RegisterDrugCommandHandler>();
            drugRepo.Setup(p => p.AddAsync(It.IsAny<Drug>())).ThrowsAsync(new Exception());

            await Assert.ThrowsAsync<AggregateException>(() => commandHandler.HandleAsync(fixture.Create<RegisterDrugCommand>()));
            unitOfWork.Verify(p => p.BeginTransaction(), Times.Once);
            unitOfWork.Verify(p => p.Rollback(), Times.Once);
            unitOfWork.Verify(p => p.Commit(), Times.Never);
        }
    }
}