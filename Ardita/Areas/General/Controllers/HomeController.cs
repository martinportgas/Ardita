using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Ardita.Services.Interfaces;
using Ardita.Extensions;
using Ardita.Services.Classess;
using Ardita.Report;
using Ardita.Models;
using Ardita.Models.DbModels;

namespace Ardita.Areas.General.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.General)]
    public class HomeController : Controller
    {
        private IArchiveUnitService _archiveUnitService;
        private IRackService _rackService;
        private IRoomService _roomService;
        private IArchiveService _archiveService;
        private IFileArchiveDetailService _fileArchiveDetailService;
        private IGmdService _gmdService;
        private ICompanyService _companyService;
        private IUserRoleService _userRoleService;
        private IArchiveCreatorService _archiveCreatorService;
        private ITypeStorageService _typeStorageService;
        private IRowService _rowService;
        private IArchiveRetentionService _archiveRetentionService;
        private IArchiveRentService _archiveRentService;
        private IMediaStorageService _mediaStorageService;
        private IMediaStorageInActiveService _mediaStorageInActiveService;
        private IGeneralSettingsService _generalSettingsService;
        private int statusSubmit = (int)GlobalConst.STATUS.Submit;
        public HomeController(
            IArchiveUnitService archiveUnitService, 
            IRoomService roomService, IArchiveService archiveService, 
            IFileArchiveDetailService fileArchiveDetailService, 
            IGmdService gmdService, 
            ICompanyService companyService, 
            IUserRoleService userRoleService, 
            IArchiveCreatorService archiveCreatorService,
            ITypeStorageService typeStorageService,
            IRowService rowService,
            IArchiveRetentionService archiveRetentionService,
            IArchiveRentService archiveRentService,
            IMediaStorageService mediaStorageService,
            IMediaStorageInActiveService mediaStorageInActiveService,
            IRackService rackService,
            IGeneralSettingsService generalSettingsService)
        {
            _archiveUnitService = archiveUnitService;
            _roomService = roomService;
            _archiveService = archiveService;
            _fileArchiveDetailService = fileArchiveDetailService;
            _gmdService = gmdService;
            _companyService = companyService;
            _userRoleService = userRoleService;
            _archiveCreatorService = archiveCreatorService;
            _typeStorageService = typeStorageService;
            _rowService = rowService;
            _archiveRetentionService = archiveRetentionService;
            _archiveRentService = archiveRentService;
            _mediaStorageService = mediaStorageService;
            _mediaStorageInActiveService = mediaStorageInActiveService;
            _rackService = rackService;
            _generalSettingsService = generalSettingsService;
        }
        public async Task<IActionResult> SearchAll(string keyword)
        {
            string param = $" TitleArchive.ToLower().Contains(\"{keyword.ToLower()}\") ";
            var dataArchive = await _archiveService.GetByParams(param);
            ViewBag.Data = dataArchive.ToList();
            return View("Search");
        }
        public async Task<IActionResult> SearchActive(string keyword)
        {
            string param = $" IsArchiveActive == true && TitleArchive.ToLower().Contains(\"{keyword.ToLower()}\") ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataArchive = await _archiveService.GetByParams(param);
            ViewBag.Data = dataArchive.ToList();
            return View("Search");
        }
        public async Task<IActionResult> SearchInActive(string keyword)
        {
            string param = $" IsArchiveActive == false && TitleArchive.ToLower().Contains(\"{keyword.ToLower()}\") ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataArchive = await _archiveService.GetByParams(param);
            ViewBag.Data = dataArchive.ToList();
            return View("Search");
        }
        public async Task<IActionResult> Index()
        {
            var xx = Request.Cookies["MyCookie"];
            if(AppUsers.CurrentUser(User).RoleCode == GlobalConst.ROLE.ADM.ToString())
            {
                return await InitFormAdmin();
            }
            else if (AppUsers.CurrentUser(User).RoleCode == GlobalConst.ROLE.UPL.ToString())
            {
                return await InitFormActive();
            }
            else if (AppUsers.CurrentUser(User).RoleCode == GlobalConst.ROLE.UKP.ToString())
            {
                return await InitFormInActive();
            }
            else
            {
                return View();
            }   
        }
        private async Task<IActionResult> InitFormAdmin()
        {
            var companies = await _companyService.GetAll();
            var location = await _archiveUnitService.GetAll();
            var userRole = await _userRoleService.GetAll();

            ViewBag.totalCompanies = companies.Count();
            ViewBag.totalLokasi = location.Count();
            ViewBag.userPengolah = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.UPL.ToString()).Count();
            ViewBag.userKearsipan = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.UKP.ToString()).Count();
            ViewBag.userView = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.USV.ToString()).Count();

            return View(GlobalConst.Admin);
        }
        private async Task<IActionResult> InitFormActive()
        {
            var userRole = await _userRoleService.GetAll();
            var room = await _roomService.GetAll();
            var creator = await _archiveCreatorService.GetAll();

            string param = $" StatusId == \"{statusSubmit}\" ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && TypeStorage.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && SubjectClassification.Classification.CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var storage = await _mediaStorageService.GetAll(param);

            ViewBag.userPengolah = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.UPL.ToString()).ToList();
            ViewBag.totalRoom = room.Where(x => x.ArchiveRoomType == GlobalConst.UnitPengolah).ToList();
            ViewBag.totalCreator = creator.ToList();
            ViewBag.totalStorage = storage.Count();

            return View(GlobalConst.Active);
        }
        private async Task<IActionResult> InitFormInActive()
        {
            var location = await _archiveUnitService.GetAll();
            var userRole = await _userRoleService.GetAll();
            var room = await _roomService.GetAll();

            string param = $" StatusId == \"{statusSubmit}\" ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && TypeStorage.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && SubSubjectClassification.CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var storage = await _mediaStorageInActiveService.GetAll(param);

            param = $" TrxMediaStorageInActiveDetails.FirstOrDefault() != null && TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.StatusId == \"{statusSubmit}\" ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var archive = await _archiveService.GetAll(param);

            ViewBag.totalLokasi = location.ToList();
            ViewBag.totalArsip = archive.ToList();
            ViewBag.userKearsipan = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.UKP.ToString()).ToList();
            ViewBag.totalRoom = room.Where(x => x.ArchiveRoomType == GlobalConst.UnitKearsipan).ToList();
            ViewBag.totalStorage = storage.ToList();

            return View(GlobalConst.InActive);
        }
        [HttpGet]
        public async Task<JsonResult> BindMaps()
        {
            var data = await _archiveUnitService.GetAll();
            var result = data
                .Where(x => x.Latitude != null && x.Longitude != null)
                .Select(x => new {
                    name = x.ArchiveUnitName,
                    address = x.ArchiveUnitAddress,
                    lat = x.Latitude,
                    lng = x.Longitude
                }).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalRoom()
        {
            var dataRoom = await _roomService.GetAll();
            var data = await _archiveUnitService.GetAll();
            var result = data
                .Where(x => x.Latitude != null && x.Longitude != null)
                .Select(x => new {
                    name = x.ArchiveUnitName,
                    totalRoom = dataRoom.Where(y => y.Floor.ArchiveUnitId == x.ArchiveUnitId).Count(),
                }).OrderByDescending(x => x.totalRoom).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalArhive()
        {
            //var dataArchive = await _archiveService.GetAll();
            //var data = await _archiveUnitService.GetAll();
            //var results = data
            //    .Select(x => new {
            //        name = x.ArchiveUnitName,
            //        totalArchive = dataArchive.Where(y => y.Creator.ArchiveUnitId == x.ArchiveUnitId && y.TrxFileArchiveDetails.FirstOrDefault() == null).Count(),
            //        totalArchiveDigital = dataArchive.Where(y => y.Creator.ArchiveUnitId == x.ArchiveUnitId && y.TrxFileArchiveDetails.FirstOrDefault() != null).Count(),
            //    }).ToList();
            //var result = results.Select(x => new
            //{
            //    x.name,
            //    x.totalArchive,
            //    x.totalArchiveDigital,
            //    total = x.totalArchive + x.totalArchiveDigital
            //}).OrderByDescending(x => x.total);
            var search = new GlobalSearchModel();
            search.StatusId = statusSubmit;
            var data = await _archiveUnitService.GetArchiveUnitGroupByArchiveCount(search);
            return Json(data.ToList());
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalGMD()
        {
            //var dataArchive = await _archiveService.GetAll();
            //var data = await _gmdService.GetAll();
            var search = new GlobalSearchModel();
            search.StatusId = statusSubmit;
            var data = await _gmdService.GetGMDGroupByArchiveCount(search);
            //var result = data
            //    .Select(x => new {
            //        name = x.nam,
            //        totalArchive = dataArchive.Where(y => y.GmdId == x.GmdId).Count(),
            //    }).OrderByDescending(x => x.totalArchive).ToList();
            return Json(data.ToList());
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalGMDActive()
        {
            var search = new GlobalSearchModel();
            search.StatusId = statusSubmit;
            search.IsArchiveActive = true;
            //string param = $" StatusId == \"{statusSubmit}\" && IsArchiveActive == true ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                search.ArchiveUnitId = AppUsers.CurrentUser(User).ArchiveUnitId;
                //param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                search.CreatorId = AppUsers.CurrentUser(User).CreatorId;
            //param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";

            var data = await _gmdService.GetGMDGroupByArchiveCount(search);

            //var dataArchives = await _archiveService.GetAll(param);
                
            //var data = await _gmdService.GetAll();
            //var result = data
            //    .Select(x => new {
            //        name = x.GmdName,
            //        totalArchive = dataArchives.Where(y => y.GmdId == x.GmdId).Count(),
            //    }).OrderByDescending(x => x.totalArchive).ToList();
            return Json(data.ToList());
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalGMDInActive()
        {
            var search = new GlobalSearchModel();
            search.StatusId = statusSubmit;
            search.IsArchiveActive = false;
            //string param = $" StatusId == \"{statusSubmit}\" && IsArchiveActive == true ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                search.ArchiveUnitId = AppUsers.CurrentUser(User).ArchiveUnitId;
            //param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                search.CreatorId = AppUsers.CurrentUser(User).CreatorId;
            //param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";

            var data = await _gmdService.GetGMDGroupByArchiveCount(search);
            //var dataArchives = await _archiveService.GetAll(param);
            //var data = await _gmdService.GetAll();
            //var result = data
            //    .Select(x => new {
            //        name = x.GmdName,
            //        totalArchive = dataArchives.Where(y => y.GmdId == x.GmdId).Count(),
            //    }).OrderByDescending(x => x.totalArchive).ToList();
            return Json(data.ToList());
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalStorageUse()
        {
            var dataStorage = await _mediaStorageService.GetCount();
            var dataRow = await _rowService.GetAll();
            int use = dataStorage == null ? 0 : dataStorage;
            int total = dataRow.Where(x => x.Level.Rack.Room.ArchiveRoomType == GlobalConst.UnitPengolah).Count();

            var result = new
            {
                totalUse = use,
                totalAvailable =  total - use,
            };
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalStorageUseInActive()
        {
            var dataStorage = await _mediaStorageInActiveService.GetCount();
            var dataRow = await _rowService.GetAll();
            int use = dataStorage == null ? 0 : dataStorage;
            int total = dataRow.Where(x => x.Level.Rack.Room.ArchiveRoomType == GlobalConst.UnitKearsipan).Count();

            var result = new
            {
                totalUse = use,
                totalAvailable = total - use,
            };
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalRoomUseDetail()
        {
            var dataRack = await _rackService.GetAll();

            string param = $" StatusId == \"{statusSubmit}\" && Row.Level.Rack.Room.ArchiveRoomType == \"{GlobalConst.UnitPengolah}\" ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && TypeStorage.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && SubjectClassification.Classification.CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataStorage = await _mediaStorageService.GetAll(param);

            var result = dataRack.Where(x => x.Room.ArchiveRoomType == GlobalConst.UnitPengolah)
                .Select(x => new {
                    name = x.RackName,
                    room = x.Room.RoomName,
                    totalUse = dataStorage.Where(y => y.Row.Level.RackId == x.RackId).Count(),
                }).OrderByDescending(x => x.totalUse).ToList();
            return Json(result.Where(x => x.totalUse > 0).ToList());
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalRoomUseDetailInActive()
        {
            var dataRack = await _rackService.GetAll();

            string param = $" StatusId == \"{statusSubmit}\" && Row.Level.Rack.Room.ArchiveRoomType == \"{GlobalConst.UnitKearsipan}\" ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && TypeStorage.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && SubSubjectClassification.CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataStorage = await _mediaStorageInActiveService.GetAll(param);

            var result = dataRack.Where(x => x.Room.ArchiveRoomType == GlobalConst.UnitKearsipan)
                .Select(x => new {
                    name = x.RackName,
                    room = x.Room.RoomName,
                    totalUse = dataStorage.Where(y => y.Row.Level.RackId == x.RackId).Count(),
                }).OrderByDescending(x => x.totalUse).ToList();
            return Json(result.Where(x => x.totalUse > 0).ToList());
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalArchiveType()
        {
            var dataRooms = await _roomService.GetAll();
            var dataRoom = dataRooms.Where(x => x.ArchiveRoomType == GlobalConst.UnitPengolah).ToList();

            string param = $" TrxMediaStorageDetails.FirstOrDefault().MediaStorage.StatusId == \"{statusSubmit}\" && IsArchiveActive == true && TrxMediaStorageDetails.FirstOrDefault() != null ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataArhives = await _archiveService.GetAll(param);

            var dataArhive = dataArhives.ToList();
            var results = dataRoom
                .Select(x => new {
                    name = x.RoomName,
                    totalInternal = dataArhive.Where(y => y.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.Rack.RoomId == x.RoomId && y.TypeSender == GlobalConst.Internal).Count(),
                    totalEksternal = dataArhive.Where(y => y.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.Rack.RoomId == x.RoomId && y.TypeSender == GlobalConst.Eksternal).Count(),
                }).ToList();
            var result = results
                .Select(x => new {
                    x.name,
                    x.totalInternal,
                    x.totalEksternal,
                    total = x.totalInternal + x.totalEksternal
                }).OrderByDescending(x => x.total).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalArchiveTypeInActive()
        {
            var dataRooms = await _roomService.GetAll();
            var dataRoom = dataRooms.Where(x => x.ArchiveRoomType == GlobalConst.UnitKearsipan).ToList();

            string param = $" TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.StatusId == \"{statusSubmit}\" && IsArchiveActive == false && TrxMediaStorageInActiveDetails.FirstOrDefault() != null ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataArhives = await _archiveService.GetAll(param);

            var dataArhive = dataArhives.ToList();
            var results = dataRoom
                .Select(x => new {
                    name = x.RoomName,
                    totalInternal = dataArhive.Where(y => y.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RoomId == x.RoomId && y.TypeSender == GlobalConst.Internal).Count(),
                    totalEksternal = dataArhive.Where(y => y.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RoomId == x.RoomId && y.TypeSender == GlobalConst.Eksternal).Count(),
                }).ToList();
            var result = results
                .Select(x => new {
                    x.name,
                    x.totalInternal,
                    x.totalEksternal,
                    total = x.totalInternal + x.totalEksternal
                }).OrderByDescending(x => x.total).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalRetensi()
        {
            var statusReceived = (int)GlobalConst.STATUS.ArchiveReceived;

            string param = $" IsArchiveActive == true && IsActive == true";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataArchive = await _archiveService.GetAll(param);

            param = $" 1=1 ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataRetensi = await _archiveRetentionService.GetAll(param);

            var result = new
            {
                active = dataArchive.Count(),
                move = dataArchive.Where(x => x.TrxArchiveMovementDetails.FirstOrDefault() != null).ToList().Where(x => x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.StatusId == statusReceived).Count(),
                retensi = dataRetensi.Count()
            };
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalRetensiInActive()
        {
            var statusMusnah = (int)GlobalConst.STATUS.Musnah;

            string param = $" IsArchiveActive == false && IsActive == true";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataArchive = await _archiveService.GetAll(param);

            param = $" 1=1 ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataRetensi = await _archiveRetentionService.GetInActiveAll(param);
            var result = new
            {
                active = dataArchive.Count() - dataRetensi.Count(),
                destroy = dataArchive.Where(x => x.TrxArchiveDestroyDetails.FirstOrDefault() != null).ToList().Where(x => x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.StatusId == statusMusnah && x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.IsArchiveActive == false).Count(),
                retensi = dataRetensi.Count()
            };
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalRent(int period)
        {
            var statusMusnah = (int)GlobalConst.STATUS.Musnah;
            int statusReturn = (int)GlobalConst.STATUS.Return;

            string param = $" 1=1 ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && Creator.ArchiveUnitId == \"{AppUsers.CurrentUser(User).ArchiveUnitId}\" ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && CreatorId == \"{AppUsers.CurrentUser(User).CreatorId}\" ";
            var dataArchive = await _archiveService.GetAll(param);

            param = $" 1=1 ";
            if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
                param += $" && TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId ==  \"{AppUsers.CurrentUser(User).ArchiveUnitId}\") ";
            if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
                param += $" && TrxArchiveRentDetails.Any(x => x.Archive.CreatorId ==  \"{AppUsers.CurrentUser(User).CreatorId}\") ";
            var dataRent = await _archiveRentService.GetAll(param);

            //if (AppUsers.CurrentUser(User).ArchiveUnitId != Guid.Empty)
            //    dataRent = dataRent.Where(x => x.TrxArchiveRentDetails.Any(x => x.Archive.Creator.ArchiveUnitId == AppUsers.CurrentUser(User).ArchiveUnitId)).ToList();
            //if (AppUsers.CurrentUser(User).CreatorId != Guid.Empty)
            //    dataRent = dataRent.Where(x => x.TrxArchiveRentDetails.Any(x => x.Archive.CreatorId == AppUsers.CurrentUser(User).CreatorId)).ToList();
            if (period != 0)
            {
                dataArchive = dataArchive.Where(x => ((DateTime)x.CreatedDateArchive).Year == period).ToList();
                dataRent = dataRent.Where(x => x.ApprovalReturnDate != null).ToList().Where(x => ((DateTime)x.ApprovalReturnDate).Year == period).ToList();
            }
            var result = new
            {
                rent = dataRent.Where(x => x.StatusId != statusReturn).Count(),
                destroy = dataArchive.Where(x => x.TrxArchiveDestroyDetails.FirstOrDefault() != null).ToList().Where(x => x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.StatusId == statusMusnah && x.TrxArchiveDestroyDetails.FirstOrDefault().ArchiveDestroy.IsArchiveActive == false).Count(),
                available = dataArchive.Where(x => x.IsArchiveActive == false).Count() - dataRent.Where(x => x.StatusId != statusReturn).Count(),
            };
            return Json(result);
        }
        public async Task<IActionResult> BindFileArchive(Guid Id, bool IsDownload = true)
        {
            var data = await _fileArchiveDetailService.GetById(Id);
            if (data != null)
            {
                var path = string.Concat(data.FilePath, data.FileNameEncrypt);
                if (System.IO.File.Exists(path))
                {
                    var bytes = System.IO.File.ReadAllBytes(path);
                    if (IsDownload)
                        return File(bytes, data.FileType, data.FileName);
                    else
                        return File(bytes, data.FileType);
                }
            }
            return File(new byte[] { }, "application/octet-stream", "FileNotFound.txt");
        }

        public async Task<FileResult> BindLogoCompany()
        {
            var data = await _generalSettingsService.GetExistingSettings();
            return File(Convert.FromBase64String(data.CompanyLogoContent), "application/octet-stream", data.CompanyLogoFileName);
        }

        [HttpGet]
        public async Task<JsonResult> BindTitleApplication()
        {
            var data = await _generalSettingsService.GetExistingSettings();

            if (data != null)
                return Json(new { data = data.AplicationTitle });
            else
            {
                return Json(new { data = "Arsip Digital Tata Graha" });
            }

        }
        [HttpGet]
        public async Task<JsonResult> BindFooter()
        {
            var data = await _generalSettingsService.GetExistingSettings();

            if (data != null)
                return Json(new { data = data.Footer });
            else
            {
                return Json(new { data = "<strong>Copyright</strong> Ardita MANIS &copy; 2023" });
            }

        }
    }
}
