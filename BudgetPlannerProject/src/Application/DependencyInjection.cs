using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using System.Reflection;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;

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
            
            //services.AddFluentValidationAutoValidation();
            //services.AddFluentValidationClientsideAdapters();
            //services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
