using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;
using FluentValidation.AspNetCore;
using FluentValidation;
using System.Reflection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBudgetService, BudgetService>();
            services.AddTransient<IBudgetRecordService, BudgetRecordService>();
            services.AddTransient<IBudgetResultService, BudgetResultService>();
            services.AddTransient<IPasswordHasher, BCryptHasher>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
