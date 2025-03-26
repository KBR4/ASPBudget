using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using Application.Services;

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
            
            return services;
        }
    }
}
