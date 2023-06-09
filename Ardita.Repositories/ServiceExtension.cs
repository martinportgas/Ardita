﻿using Ardita.Models.DbModels;
using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            //Register repository Here
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IPositionRepository, PositionRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IMenuRepository, MenuRepository>();
            services.AddScoped<ISubMenuRepository, SubMenuRepository>();
            services.AddScoped<IUserRoleRepository, UserRoleRepository>();
            services.AddScoped<IPageRepository, PageRepository>();
            services.AddScoped<IRolePageRepository, RolePageRepository>();

            return services;
        }
    }
}
