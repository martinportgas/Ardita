using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ardita.Services;

public static class ServiceExtension
{
    public static IServiceCollection AddDIServices(this IServiceCollection services, IConfiguration configuration)
    {
        //Register service Here
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ICompanyService, CompanyService>();
        services.AddScoped<IArchiveUnitService, ArchiveUnitService>();
        services.AddScoped<IArchiveCreatorService, ArchiveCreatorService>();
        services.AddScoped<IGmdService, GmdService>();
        services.AddScoped<ISecurityClassificationService, SecurityClassificationService>();
        services.AddScoped<ITypeStorageService, TypeStorageService>();
        services.AddScoped<IPositionService, PositionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<ISubMenuService, SubMenuService>();
        services.AddScoped<IUserRoleService, UserRoleService>();
        services.AddScoped<IPageService, PageService>();
        services.AddScoped<IRolePageService, RolePageService>();
        services.AddScoped<IClassificationService, ClassificationService>();
        services.AddScoped<IClassificationTypeService, ClassificationTypeService>();
        services.AddScoped<IClassificationSubjectService, ClassificationSubjectService>();
        services.AddScoped<IClassificationSubSubjectService, ClassificationSubSubjectService>();
        services.AddScoped<IFloorService, FloorService>();
        services.AddScoped<IRoomService, RoomService>();
        services.AddScoped<IRackService, RackService>();
        services.AddScoped<ILevelService, LevelService>();
        services.AddScoped<IRowService, RowService>();
        services.AddScoped<IArchiveService, ArchiveService>();
        services.AddScoped<IArchiveExtendService, ArchiveExtendService>();
        services.AddScoped<IArchiveDestroyService, ArchiveDestroyService>();
        services.AddScoped<IArchiveMovementService, ArchiveMovementService>();
        services.AddScoped<IArchiveRetentionService, ArchiveRetentionService>();
        services.AddScoped<IArchiveApprovalService, ArchiveApprovalService>();
        services.AddScoped<IFileArchiveDetailService, FileArchiveDetailService>();
        services.AddScoped<IMediaStorageService, MediaStorageService>();
        services.AddScoped<IArchiveReceivedService, ArchiveReceivedService>();
        services.AddScoped<IMediaStorageInActiveService, MediaStorageInActiveService>();
        services.AddScoped<IArchiveOwnerService, ArchiveOwnerService>();
        services.AddScoped<IArchiveTypeService, ArchiveTypeService>();
        services.AddScoped<IArchiveRentService, ArchiveRentService>();
        services.AddScoped<ISubTypeStorageService, SubTypeStorageService>();
        services.AddScoped<IArchiveOutIndicatorService, ArchiveOutIndicatorService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<ILogLoginService, LogLoginService>();
        services.AddScoped<ILogChangesService, LogChangesService>();
        services.AddScoped<ILogActivityService, LogActivityService>();
        services.AddScoped<ITemplateSettingService, TemplateSettingService>();

        services.AddScoped<IGeneralSettingsService, GeneralSettingsService>();

        return services;
    }
}
