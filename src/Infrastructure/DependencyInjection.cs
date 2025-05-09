using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.Repositories.BudgetRepository;
using Infrastructure.Repositories.BudgetResultRepository;
using Infrastructure.Repositories.BudgetRecordRepository;
using Microsoft.Extensions.Configuration;
using Npgsql;
using FluentMigrator.Runner;
using System.Reflection;
using Dapper;
using Infrastructure.Database.TypeMappings;
using Infrastructure.Repositories.AttachmentRepository;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("PostgresDB");
                return new NpgsqlDataSourceBuilder(connectionString).Build();
            });

            services.AddScoped(sp =>
            {
                var datasource = sp.GetRequiredService<NpgsqlDataSource>();
                return datasource.CreateConnection();
            });

            services.AddTransient<IUserRepository, UserPostgresRepository>();
            services.AddTransient<IBudgetRepository, BudgetPostgresRepository>();
            services.AddTransient<IBudgetRecordRepository, BudgetRecordPostgresRepository>();
            services.AddTransient<IBudgetResultRepository, BudgetResultPostgresRepository>();
            services.AddTransient<IAttachmentRepository, AttachmentPostgresRepository>();

            services.AddFluentMigratorCore()
                .ConfigureRunner(
                rb => rb.AddPostgres()
                .WithGlobalConnectionString("PostgresDB")
                .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations()
                )
                .AddLogging(lb => lb.AddFluentMigratorConsole());

            services.AddScoped<Database.MigrationRunner>();
            DapperConfig.Configure();

            return services;
        }
    }
}
