using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

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
        WaitingForRetrieval = 8,
        Retrieved = 9,
        Return = 10,
        UsulMusnah = 11,
        Musnah = 12
    }
    public enum ROLE
    {
        ADM = 1,
        UPL = 2,
        UKP = 3,
        APR = 4,
        USV = 5
    }
    public const string Form = "Form";
    public const string Add = "Add";
    public const string UserId = "UserId";
    public const string Submit = "Submit";
    public const string SubmitDestroy = "SubmitDestroy";
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
    public const string Report = "Report";

    //Controller
    public const string Company = "Company";
    public const string ChangeRole = "ChangeRole";
    public const string ChangePassword = "ChangePassword";
    public const string Home = "Home";
    public const string Classification = "Classification";
    public const string ClassificationType = "ClassificationType";
    public const string ClassificationSubject = "ClassificationSubject";
    public const string ClassificationSubSubject = "ClassificationSubSubject";
    public const string ArchiveUnit = "ArchiveUnit";
    public const string MediaStorage = "MediaStorage";
    public const string MediaStorageInActive = "MediaStorageInActive";
    public const string ArchiveCreator = "ArchiveCreator";
    public const string Gmd = "Gmd";
    public const string SecurityClassification = "SecurityClassification";
    public const string Floor = "Floor";
    public const string Room = "Room";
    public const string Rack = "Rack";
    public const string Level = "Level";
    public const string Row = "Row";
    public const string TypeStorage = "TypeStorage";
    public const string SubTypeStorage = "SubTypeStorage";
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
    public const string ArchiveOwner = "ArchiveOwner";
    public const string ArchiveType = "ArchiveType";
    public const string ArchiveCirculation = "ArchiveCirculation";
    public const string ReportArchiveActive = "ReportArchiveActive";

    public const string ArchiveRetention = "ArchiveRetention";
    public const string ArchiveExtend = "ArchiveExtend";
    public const string ArchiveDestroy = "ArchiveDestroy";
    public const string ArchiveMovement = "ArchiveMovement";
    public const string ArchiveApproval = "ArchiveApproval";
    public const string ArchiveReceived = "ArchiveReceived";
    public const string ArchiveRent = "ArchiveRent";
    public const string ArchiveRentApproval = "ArchiveRentApproval";
    public const string ArchiveRetrieval = "ArchiveRetrieval";
    public const string ArchiveReturn = "ArchiveReturn";

    //Action
    public const string Index = "Index";
    public const string Update = "Update";
    public const string Delete = "Delete";
    public const string Detail = "Detail";
    public const string IsUsed = "IsUsed";
    public const string DetailArchive = "DetailArchive";
    public const string Preview = "Preview";
    public const string Remove = "Remove";
    public const string Destroy = "Destroy";
    public const string Save = "Save";
    public const string Create = "Create";
    public const string Approval = "Approval";
    public const string SubmitApproval = "SubmitApproval";
    public const string Export = "Export";
    public const string Import = "Import";
    public const string Upload = "Upload";
    public const string UploadForm = "UploadForm";
    public const string GetData = "GetData";
    public const string GetDetail = "GetDetail";
    public const string GetSubDetail = "GetSubDetail";
    public const string GetDetailArchive = "GetDetailArchive";
    public const string DownloadTemplate = "DownloadTemplate";
    public const string DownloadFile = "DownloadFile";
    public const string UpdateSubMenu = "UpdateSubMenu";
    public const string SaveSubMenu = "SaveSubMenu";
    public const string UpdatePage = "UpdatePage";
    public const string AcceptArchiveForm = "AcceptArchiveForm";
    public const string GetFileArchive = "GetFileArchive";
    public const string ValidateQRBox = "ValidateQRBox";
    public const string UpdateRetrieval = "UpdateRetrieval";
    public const string UpdateReturn = "UpdateReturn";
    public const string GetDataHistory = "GetDataHistory";
    public const string GetDataRentBox = "GetDataRentBox";
    public const string GetDataRentBoxDetail = "GetDataRentBoxDetail";
    public const string GenerateReport = "GenerateReport";
    public const string Admin = "Admin";
    public const string Active = "Active";
    public const string InActive = "InActive";

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
    public const string DetailIsUsedArray = "detailIsUsed[]";
    public const string UsedByArray = "usedBy[]";
    public const string UsedDateArray = "usedDate[]";
    public const string ReturnDateArray = "returnDate[]";
    public const string IdFileDeletedArray = "idFileDeleted[]";
    public const string listSts = "listSts[]";
    public const string listArchive = "listArchive[]";
    public const string InitialCode = "Auto Generated";
    public const string wwwroot = "wwwroot";
    public const string Internal = "Internal";
    public const string Eksternal = "Eksternal";
    public const string ArsipPemusnahan = "Pemusnahan Arsip";
    public const string ArsipPemindahan = "Pemindahan Arsip";

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
    public const string BindArchiveUnitsByParam = "BindArchiveUnitsByParam";
    public const string BindArchiveUnitsByCompanyIdAndParam = "BindArchiveUnitsByCompanyIdAndParam";
    public const string BindRooms = "BindRooms";
    public const string BindRoomActiveByFloorId = "BindRoomActiveByFloorId";
    public const string BindRoomInActiveByFloorId = "BindRoomInActiveByFloorId";
    public const string BindRacks = "BindRacks";
    public const string BindCompanies = "BindCompanies";
    public const string BindClassificationSubjectIdByClassificationId = "BindClassificationSubjectIdByClassificationId";
    public const string BindClassificationIdByClassificationTypeId = "BindClassificationIdByClassificationTypeId";
    public const string BindArchivesBySubSubjectClassificationId = "BindArchivesBySubSubjectClassificationId";
    public const string BindSubSubjectClassificationByClassificationId = "BindSubSubjectClassificationByClassificationId";
    public const string BindArchivesInActiveBySubSubjectClassificationId = "BindArchivesInActiveBySubSubjectClassificationId";
    public const string BindTypeStorageByArchiveUnitId = "BindTypeStorageByArchiveUnitId";
    public const string BindSubSubjectClassificationByArchiveUnitId = "BindSubSubjectClassificationByArchiveUnitId";
    public const string BindSubjectClassificationByArchiveUnitId = "BindSubjectClassificationByArchiveUnitId";
    public const string BindRoomByFloorId = "BindRoomByFloorId";
    public const string BindRackByRoomId = "BindRackByRoomId";
    public const string BindLevelByRackId = "BindLevelByRackId";
    public const string BindRowByLevelId = "BindRowByLevelId";
    public const string BindRowArchiveByLevelId = "BindRowArchiveByLevelId";
    public const string BindTypeStorageById = "BindTypeStorageById";
    public const string BindDownload = "BindDownload";
    public const string BindArchiveRetention = "BindArchiveRetention";
    public const string BindArchiveRetentionByParam = "BindArchiveRetentionByParam";
    public const string BindArchiveRetentionInActiveByParam = "BindArchiveRetentionInActiveByParam";
    public const string BindEmployeeByParam = "BindEmployeeByParam";
    public const string BindApprovalByParam = "BindApprovalByParam";
    public const string BindQrCode = "BindQrCode";
    public const string BindLabel = "BindLabel";
    public const string BindTypeStorageByParam = "BindTypeStorageByParam";
    public const string BindRowArchiveInActiveByLevelId = "BindRowArchiveInActiveByLevelId";
    public const string BindRowAvailableArchiveInActiveByLevelId = "BindRowAvailableArchiveInActiveByLevelId";
    public const string BindSubTypeStorageByTypeStorageId = "BindSubTypeStorageByTypeStorageId";
    public const string BindArchiveActiveBySubjectId = "BindArchiveActiveBySubjectId";
    public const string BindSubTypeStorageById = "BindSubTypeStorageById";
    public const string BindGmdDetailByGmdId = "BindGmdDetailByGmdId";
    public const string BindGmdDetailById = "BindGmdDetailById";
    public const string BindGMDDetailVolumeByTypeStorageId = "BindGMDDetailVolumeByTypeStorageId";
    public const string BindGMDDetailByTypeStorageId = "BindGMDDetailByTypeStorageId";
    public const string BindRoomActiveByArchiveUnitId = "BindRoomActiveByArchiveUnitId";
    public const string BindParamRackByRoomId = "BindParamRackByRoomId";
    public const string BindParamLevelByRackId = "BindParamLevelByRackId";
    public const string BindParamRowByLevelId = "BindParamRowByLevelId";
    public const string BindParamCreatorByArchiveUnitId = "BindParamCreatorByArchiveUnitId";
    public const string BindParamClassificationSubjectIdByClassificationId = "BindParamClassificationSubjectIdByClassificationId";

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
    public const string UnitPengolah = "Unit Pengolah";
    public const string UnitKearsipan = "Unit Kearsipan";
    public const string Notification = "Notification";
    public const string NotFound = "NotFound";
    public const string Success = "Success";
    public const string NothingSelected = "Tidak ada yang dipilih";
    public const string Failed = "Failed";
    public const string Yes = "Yes";
    public const string Error = "Error";
    public const string Used = "Digunakan";
    public const string Rent = "Dipinjam";
    public const string UsedBy = "UsedBy";
    public const string Available = "Tersedia";
    public static DateTime MinDate = DateTime.Parse("1900-01-01");
    public static DateTime MaxDate = DateTime.Now;

    //ClaimTypes
    public const string Username = "Username";
    public const string RoleId = "RoleId";
    public const string RoleCode = "RoleCode";
    public const string RoleName = "RoleName";
    public const string EmployeeNIK = "EmployeeNIK";
    public const string EmployeeName = "EmployeeName";
    public const string EmployeeMail = "EmployeeMail";
    public const string EmployeePhone = "EmployeePhone";
    public const string PositionId = "PositionId";
    public const string CompanyId = "CompanyId";
    public const string CompanyName = "CompanyName";
    public const string EmployeeId = "EmployeeId";
    public const string ArchiveUnitCode = "ArchiveUnitCode";

    //Excel Template Header From Json
    public const string name = "name";
    public const string column = "column";
    public static JObject ExcelTemplate(string templateName)
    {
        return JObject.Parse(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), wwwroot, $"{templateName}.json")));
    }

    //Datatable
    public static DataTable dataSender()
    {
        DataTable dtSender = new DataTable();
        dtSender.Columns.Add("no");
        dtSender.Columns.Add("tipe");
        DataRow dataRow = dtSender.NewRow();
        dataRow[0] = 1;
        dataRow[1] = Internal;
        dtSender.Rows.Add(dataRow);
        dataRow = dtSender.NewRow();
        dataRow[0] = 2;
        dataRow[1] = Eksternal;
        dtSender.Rows.Add(dataRow);
        return dtSender;
    }
    //Document Code
    public const string RetentionExtendDoc = "PR";
    public const string ArchiveDestroyDoc = "PH";
    public const string ArchiveMovementDoc = "PD";

    //where clause
    public const string WhereClauseArchiveMonitoring = @"archiveCode + creator.creatorName + archiveOwner.archiveOwnerName + typeSender + keyword 
    + titleArchive + subSubjectClassification.subjectClassification.subjectClassificationName + subSubjectClassification.subSubjectClassificationName 
    + securityClassification.securityClassificationName + documentNo + createdDateArchive.ToString() + archiveType.archiveTypeName 
    + trxMediaStorageDetails.FirstOrDefault().mediaStorage.typeStorage.typeStorageName
    + trxMediaStorageDetails.FirstOrDefault().mediaStorage.row.level.rack.rackName 
    + trxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.LevelName 
    + TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.RowName
    + volume.ToString()
    ";

    public const string WhereClauseArchiveRegist = @"archiveCode + documentNo + titleArchive + gmd.gmdName + securityClassification.securityClassificationName 
    + creator.creatorName + subSubjectClassification.subSubjectClassificationName";

}
