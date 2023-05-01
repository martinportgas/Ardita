using Ardita.Repositories.Classess;
using Ardita.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ardita.Repositories;
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
        services.AddScoped<IPageDetailRepository, PageDetailRepository>();
        services.AddScoped<IClassificationRepository, ClassificationRepository>();
        services.AddScoped<IClassificationTypeRepository, ClassificationTypeRepository>();
        services.AddScoped<IClassificationSubjectRepository, ClassificationSubjectRepository>();
        services.AddScoped<IClassificationSubSubjectRepository, ClassificationSubSubjectRepository>();
        services.AddScoped<IClassificationPermissionRepository, ClassificationPermissionRepository>();
        services.AddScoped<IArchiveCreatorRepository, ArchiveCreatorRepository>();
        services.AddScoped<IArchiveUnitRepository, ArchiveUnitRepository>();
        services.AddScoped<IGmdRepository, GmdRepository>();
        services.AddScoped<ISecurityClassificationRepository, SecurityClassificationRepository>();
        services.AddScoped<ITypeStorageRepository, TypeStorageRepository>();
        services.AddScoped<IFloorRepository, FloorRepository>();
        services.AddScoped<IRoomRepository, RoomRepository>();
        services.AddScoped<IRackRepository, RackRepository>();
        services.AddScoped<ILevelRepository, LevelRepository>();
        services.AddScoped<IRowRepository, RowRepository>();
        services.AddScoped<IArchiveRepository, ArchiveRepository>();
        services.AddScoped<IArchiveExtendRepository, ArchiveExtendRepository>();
        services.AddScoped<IArchiveExtendDetailRepository, ArchiveExtendDetailRepository>();
        services.AddScoped<IArchiveDestroyRepository, ArchiveDestroyRepository>();
        services.AddScoped<IArchiveDestroyDetailRepository, ArchiveDestroyDetailRepository>();
        services.AddScoped<IArchiveMovementRepository, ArchiveMovementRepository>();
        services.AddScoped<IArchiveMovementDetailRepository, ArchiveMovementDetailRepository>();
        services.AddScoped<IArchiveRetentionRepository, ArchiveRetentionRepository>();
        services.AddScoped<IArchiveApprovalRepository, ArchiveApprovalRepository>();
        services.AddScoped<IFileArchiveDetailRepository, FileArchiveDetailRepository>();

        return services;
    }
}
