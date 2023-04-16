using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Services
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Register service Here
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IPositionService, PositionService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<ISubMenuService, SubMenuService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IPageService, PageService>();
            services.AddScoped<IRolePageService, RolePageService>();
            services.AddScoped<IClassificationService, ClassificationService>();
            services.AddScoped<IFloorService, FloorService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRackService, RackService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<IRowService, RowService>();

            return services;
        }
    }
}
