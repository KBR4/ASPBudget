using Application;
using Bogus;
using Domain.Entities;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Processors;
using Infrastructure;
using Infrastructure.Repositories.BudgetRepository;
using Infrastructure.Repositories.BudgetRecordRepository;
using Infrastructure.Repositories.BudgetResultRepository;
using Infrastructure.Repositories.UserRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using Respawn;
using System.Reflection;
using MigrationRunner = Infrastructure.Database.MigrationRunner;
using Bogus.Extensions;

namespace ApplicationIntegrationTests
{
    public sealed class TestingFixture : IAsyncLifetime
    {
        private readonly Faker _faker;
        private Respawner _respawner = null!;

        public TestingFixture()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) => { config.AddJsonFile("appsettings.json"); })
                .ConfigureServices((context, services) =>
                {
                    services.AddInfrastructure();
                    services.AddApplication();

                    var connectionString = context.Configuration.GetConnectionString("PostgresDBIntegration");
                    if (string.IsNullOrWhiteSpace(connectionString))
                        throw new ApplicationException("PostgresDBIntegration connection string is empty");

                    services.AddSingleton(_ => new NpgsqlDataSourceBuilder(connectionString).Build());
                    services.AddTransient(sp =>
                    {
                        var dataSource = sp.GetRequiredService<NpgsqlDataSource>();
                        return dataSource.CreateConnection();
                    });

                    services
                        .AddFluentMigratorCore()
                        .ConfigureRunner(rb => rb
                            .AddPostgres()
                            .WithGlobalConnectionString(connectionString)
                            .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
                        .Configure<SelectingProcessorAccessorOptions>(options => { options.ProcessorId = "Postgres"; });
                })
                .Build();

            ServiceProvider = host.Services;
            _faker = new Faker();
        }

        public IServiceProvider ServiceProvider { get; }

        public async Task InitializeAsync()
        {           
            using var scope = ServiceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
            await connection.OpenAsync();

            var migrationRunner = scope.ServiceProvider.GetRequiredService<MigrationRunner>();
            migrationRunner.Run();

            _respawner = await Respawner.CreateAsync(connection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = ["public"],
                TablesToIgnore = ["VersionInfo"]
            });

        }

        public async Task<User> CreateUser()
        {
            using var scope = ServiceProvider.CreateScope();
            var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var userId = await userRepository.Create(new User
            {
                FirstName = _faker.Name.FirstName(),
                LastName = _faker.Name.LastName(),
                Email = _faker.Person.Email
            });

            var user = await userRepository.ReadById(userId);
            if (user == null) throw new Exception("Can't create user");
            return user;
        }

        public async Task<Budget> CreateBudget()
        {
            using var scope = ServiceProvider.CreateScope();
            var budgetRepository = scope.ServiceProvider.GetRequiredService<IBudgetRepository>();
            var user = await CreateUser();

            var budgetId = await budgetRepository.Create(new Budget
            {
                Name = _faker.Person.LastName + _faker.Person.FirstName + "LLC",
                StartDate = _faker.Date.Past(2),
                FinishDate = _faker.Date.Future(3),
                Description = _faker.Rant.Review("TV").ClampLength(10, ValidationConstants.MaxDescriptionLength),
                CreatorId = user.Id
            });

            var budget = await budgetRepository.ReadById(budgetId);
            if (budget == null) throw new Exception("Can't create budget");
            return budget;
        }
        public async Task<BudgetRecord> CreateBudgetRecord()
        {
            using var scope = ServiceProvider.CreateScope();
            var budgetRecordRepository = scope.ServiceProvider.GetRequiredService<IBudgetRecordRepository>();
            var budget = await CreateBudget();

            var recordId = await budgetRecordRepository.Create(new BudgetRecord
            {
                Name = _faker.Commerce.ProductName(),
                CreationDate = DateTime.Now,
                SpendingDate = _faker.Date.Future(3),
                BudgetId = budget.Id,
                Total = Math.Round(_faker.Random.Double(1, 1000), 2),
                Comment = _faker.Rant.Review().ClampLength(10, ValidationConstants.MaxCommentLength)
            });

            var record = await budgetRecordRepository.ReadById(recordId);
            if (record == null) throw new Exception("Can't create budget record");
            return record;
        }
        public async Task<BudgetResult> CreateBudgetResult()
        {
            using var scope = ServiceProvider.CreateScope();
            var budgetResultRepository = scope.ServiceProvider.GetRequiredService<IBudgetResultRepository>();
            var budget = await CreateBudget();

            var resultId = await budgetResultRepository.Create(new BudgetResult
            {
                BudgetId = budget.Id,
                TotalProfit = Math.Round(_faker.Random.Double(1000, 10000), 2)
            });

            var result = await budgetResultRepository.ReadById(resultId);
            if (result == null) throw new Exception("Can't create budget result");
            return result;
        }
        public async Task DisposeAsync() => await ResetDatabase();

        private async Task ResetDatabase()
        {
            using var scope = ServiceProvider.CreateScope();
            var connection = scope.ServiceProvider.GetRequiredService<NpgsqlConnection>();
            await connection.OpenAsync();
            await _respawner.ResetAsync(connection);
        }
    }
}
