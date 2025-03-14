using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            services.AddTransient<BudgetRecordService, BudgetRecordService>();
            services.AddTransient<IBudgetResultService, BudgetResultService>();
            return services;
        }
    }
}
