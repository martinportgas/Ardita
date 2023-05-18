namespace Ardita.Extensions;

public static class GlobalConst
{
    public const string BASE_PATH_ARCHIVE = "Path:Archive";
    public const string BASE_PATH_TEMP_ARCHIVE = "Path:TempArchive";
    public const string EXCEL_FORMAT_TYPE = "application/vnd.ms-excel";
    public enum STATUS
    {
        Draft = 1,
        ApprovalProcess = 2,
        Approved = 3,
        Rejected = 4,
        Submit = 5,
        ArchiveReceived = 6,
        ArchiveNotReceived = 7,
    }
    public const string Form = "Form";
    public const string Add = "Add";
    public const string UserId = "UserId";
    public const string Submit = "Submit";
    public const string SubMenuPageDetail = "SubMenuPageDetail";
    public const string SubMenuForm = "SubMenuForm";
    public const string PageForm = "PageForm";
    public const string AddPage = "AddPage";
    public const string SavePage = "SavePage";
    public const string PageUpdateForm = "PageUpdateForm";

    //Area
    public const string MasterData = "MasterData";
    public const string UserManage = "UserManage";
    public const string General = "General";
    public const string ArchiveActive = "ArchiveActive";
    public const string ArchiveInActive = "ArchiveInActive";

    //Controller
    public const string Company = "Company";
    public const string Home = "Home";
    public const string Classification = "Classification";
    public const string ClassificationType = "ClassificationType";
    public const string ClassificationSubject = "ClassificationSubject";
    public const string ClassificationSubSubject = "ClassificationSubSubject";
    public const string ArchiveUnit = "ArchiveUnit";
    public const string MediaStorage = "MediaStorage";
    public const string ArchiveCreator = "ArchiveCreator";
    public const string Gmd = "Gmd";
    public const string SecurityClassification = "SecurityClassification";
    public const string Floor = "Floor";
    public const string Room = "Room";
    public const string Rack = "Rack";
    public const string Level = "Level";
    public const string Row = "Row";
    public const string TypeStorage = "TypeStorage";
    public const string Archive = "Archive";
    public const string ArchiveMonitoring = "ArchiveMonitoring";
    public const string Employee = "Employee";
    public const string Position = "Position";
    public const string Menu = "Menu";
    public const string User = "User";
    public const string UserRole = "UserRole";
    public const string Page = "Page";
    public const string RolePage = "RolePage";
    public const string Role = "Role";

    public const string ArchiveRetention = "ArchiveRetention";
    public const string ArchiveExtend = "ArchiveExtend";
    public const string ArchiveDestroy = "ArchiveDestroy";
    public const string ArchiveMovement = "ArchiveMovement";
    public const string ArchiveApproval = "ArchiveApproval";
    public const string ArchiveReceived = "ArchiveReceived";

    //Action
    public const string Index = "Index";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string Detail = "Detail";
    public const string Preview = "Preview";
    public const string Remove = "Remove";
    public const string Save = "Save";
    public const string Create = "Create";
    public const string Approval = "Approval";
    public const string SubmitApproval = "SubmitApproval";
    public const string Export = "Export";
    public const string Upload = "Upload";
    public const string GetData = "GetData";
    public const string DownloadTemplate = "DownloadTemplate";
    public const string DownloadFile = "DownloadFile";
    public const string UpdateSubMenu = "UpdateSubMenu";
    public const string SaveSubMenu = "SaveSubMenu";
    public const string UpdatePage = "UpdatePage";
    public const string AcceptArchiveForm = "AcceptArchiveForm";

    //Html helper
    public const string Required = "required";
    public const string Disabled = "disabled";
    public const string Hidden = "hidden";
    public const string Approve = "Approve";
    public const string Reject = "Reject";
    public const string Cancel = "Cancel";
    public const string Back = "Back";
    public const string IconBack = "<i class='fa fa-chevron-circle-left'></i> ";
    public const string IconSave = "<i class='fa fa-save'></i> ";
    public const string IconSubmit = "<i class='fa fa-paper-plane'></i> ";
    public const string IconApprove = "<i class='fa fa-check-circle'></i> ";
    public const string IconReject = "<i class='fa fa-times-circle'></i> ";
    public const string IconRefresh = "<i class='fa fa-refresh'></i> ";
    public const string IconDelete = "<i class='fa fa-trash'></i> ";
    public const string BtnPrimary = "btn-primary";
    public const string BtnDanger = "btn-danger";

    //Other
    public const string ViewStart = "~/Views/Shared/_Layout.cshtml";
    public const string Template = "Template";
    public const string Files = "files";
    public const string DetailArray = "detail[]";
    public const string IdFileDeletedArray = "idFileDeleted[]";
    public const string InitialCode = "Auto Generated";

    //Prefix
    public const string Trx = "Trx";
    public const string No = "No";

    //Binding
    public const string BindFloors = "BindFloors";
    public const string BindFloorsByArchiveUnitId = "BindFloorsByArchiveUnitId";
    public const string BindArchiveCreators = "BindArchiveCreators";
    public const string BindSecurityClassifications = "BindSecurityClassifications";
    public const string BindSubSubjectClasscifications = "BindSubSubjectClasscifications";
    public const string BindGmds = "BindGmds";
    public const string BindLevels = "BindLevels";
    public const string BindArchiveUnits = "BindArchiveUnits";
    public const string BindRooms = "BindRooms";
    public const string BindRacks = "BindRacks";
    public const string BindCompanies = "BindCompanies";
    public const string BindClassificationSubjectIdByClassificationId = "BindClassificationSubjectIdByClassificationId";
    public const string BindClassificationIdByClassificationTypeId = "BindClassificationIdByClassificationTypeId";
    public const string BindArchivesBySubSubjectClassificationId = "BindArchivesBySubSubjectClassificationId";
    public const string BindTypeStorageByArchiveUnitId = "BindTypeStorageByArchiveUnitId";
    public const string BindRoomByFloorId = "BindRoomByFloorId";
    public const string BindRackByRoomId = "BindRackByRoomId";
    public const string BindLevelByRackId = "BindLevelByRackId";
    public const string BindRowByLevelId = "BindRowByLevelId";
    public const string BindTypeStorageById = "BindTypeStorageById";
    public const string BindDownload = "BindDownload";
    public const string BindArchiveRetention = "BindArchiveRetention";
    public const string BindArchiveRetentionByParam = "BindArchiveRetentionByParam";
    public const string BindEmployeeByParam = "BindEmployeeByParam";
    public const string BindQrCode = "BindQrCode";
    public const string BindLabel = "BindLabel";

    //Web Component
    public const string GET = "GET";
    public const string JSON = "JSON";
    public const string APPLICATIONJSON = "application/json";


    //Partial View
    public const string _ArchiveMonitoringDetail = "_ArchiveMonitoringDetail";

    //Title
    public const string TitleArchiveMonitoring = "Archive Monitoring";
    public const string TitleArchiveRetention = "Archive Retention";

    //Others
    public const string Password = "Password";

}
