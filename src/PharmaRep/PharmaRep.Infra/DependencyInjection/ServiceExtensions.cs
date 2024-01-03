using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PharmaRep.Application.Brand.Queries;
using PharmaRep.Application.Common;
using PharmaRep.Application.Medicine.Commands.DeactivateDrug;
using PharmaRep.Application.Medicine.Commands.RegisterDrug;
using PharmaRep.Application.Medicine.Queries;
using PharmaRep.Domain.Brand;
using PharmaRep.Domain.Brand.Entities;
using PharmaRep.Domain.Medicine;
using PharmaRep.Domain.Medicine.Aggregates;
using PharmaRep.Domain.Medicine.Entities;
using PharmaRep.Infra.Persistence;
using PharmaRep.Infra.Persistence.Common;
using PharmaRep.Infra.Security;

namespace PharmaRep.Infra.DependencyInjection;

//create class DI    //create class DI that receive Initialize dependency injection
public static class ServiceExtensions
{
    //create method that receive IServiceCollection
    public static IServiceCollection Initialize(this IServiceCollection services, IConfiguration configuration)
    {

        
        //NOTE: For this project, we are not using MediatorR
        services.AddTransient<IQueryHandler<BrandQuery, IEnumerable<Brand>>, BrandQueryHandler>();
        services.AddTransient<IQueryHandler<MedicalConditionQuery, IEnumerable<MedicalCondition>>, MedicalConditionQueryHandler>();
        services.AddTransient<IQueryHandler<MedicalReactionQuery, IEnumerable<MedicalReaction>>, MedicalReactionQueryHandler>();
        services.AddTransient<IQueryHandler<DrugQuery, IEnumerable<DrugInformation>>, DrugQueryHandler>();
        services.AddTransient<IQueryHandler<DrugByIdQuery, DrugAggregate?>, DrugByIdQueryHandler>();
        services.AddTransient<ICommandHandler<DectivateDrugCommand, DeactivatedEntity>, DeactivateDrugCommandHandler>();
        services.AddTransient<ICommandHandler<UpdateDrugCommand, EntityUpdated>, UpdateDrugCommandHandler>();
        services.AddTransient<ICommandHandler<RegisterDrugCommand, EntityCreated?>, RegisterDrugCommandHandler>();
        //services.AddScoped<ICommandHandler<Command, Response, CommandHandler>>
        //services.AddScoped<IQueryHandler<Query, IEnumerable<>>, QueryHandler>();


        //add Services
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IIdentifierService, HttpIdentifier>();

        //Ensures the same scoped instance
        services.AddScoped<PharmaDbManager>()
            .AddScoped<ISqlConnectionFactory>(sp => sp.GetRequiredService<PharmaDbManager>())
            .AddScoped<IPharmaUnitOfWork>(sp => sp.GetRequiredService<PharmaDbManager>());
           

        //Repositories
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IMedicalConditionRepository, MedicalConditionRepository>();
        services.AddScoped<IMedicalReactionRepository, MedicalReactionRepository>();
        services.AddScoped<IDrugRepository, DrugRepository>();








        //services.AddScoped<IDrugRepository, DrugRepository>();
        //services.AddScoped<IUserRepository, UserRepository>();
        //services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddScoped<IPharmacyRepository, PharmacyRepository>();
        //services.AddScoped<ICustomerRepository, CustomerRepository>();
        //services.AddScoped<IOrderRepository, OrderRepository>();
        //services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
        //services.AddScoped<IPrescriptionDrugRepository, PrescriptionDrugRepository>();
        //services.AddScoped<IOrderDrugRepository, OrderDrugRepository>();
        //services.AddScoped<IBrandRepository, BrandRepository>();
        //services.AddScoped<IPharmacyDrugRepository, PharmacyDrugRepository>();
        //services.AddScoped<IEmailService, EmailService>();
        ////add scoped
        //services.AddDbContext<PharmaRepDbContext>(options =>
        //    options.UseSqlServer(configuration.GetConnectionString("PharmaRepDbContext")));
        ////add scoped
        //services.AddAutoMapper(typeof(MapperProfile));
        ////return services
        ///
        return services;
    }
}