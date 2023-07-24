using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Ardita.Services.Interfaces;
using Ardita.Extensions;
using Ardita.Services.Classess;

namespace Ardita.Areas.General.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.General)]
    public class HomeController : Controller
    {
        private IArchiveUnitService _archiveUnitService;
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
            IMediaStorageInActiveService mediaStorageInActiveService)
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
        }
        public async Task<IActionResult> SearchActive(string keyword)
        {
            var dataArchive = await _archiveService.GetAll();
            ViewBag.Data = dataArchive.Where(x => x.IsArchiveActive == true && x.TitleArchive.ToLower().Contains(keyword.ToLower())).ToList();
            return View("Search");
        }
        public async Task<IActionResult> SearchInActive(string keyword)
        {
            var dataArchive = await _archiveService.GetAll();
            ViewBag.Data = dataArchive.Where(x => x.IsArchiveActive == false && x.TitleArchive.ToLower().Contains(keyword.ToLower())).ToList();
            return View("Search");
        }
        public async Task<IActionResult> Index()
        {
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
            var storage = await _mediaStorageService.GetAll();

            ViewBag.userPengolah = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.UPL.ToString()).ToList();
            ViewBag.totalRoom = room.Where(x => x.ArchiveRoomType == GlobalConst.UnitPengolah).ToList();
            ViewBag.totalCreator = creator.ToList();
            ViewBag.totalStorage = storage.Where(x => x.StatusId == statusSubmit).Count();

            return View(GlobalConst.Active);
        }
        private async Task<IActionResult> InitFormInActive()
        {
            var location = await _archiveUnitService.GetAll();
            var userRole = await _userRoleService.GetAll();
            var room = await _roomService.GetAll();
            var storage = await _mediaStorageInActiveService.GetAll();
            var archive = await _archiveService.GetAll();

            ViewBag.totalLokasi = location.ToList();
            ViewBag.totalArsip = archive.Where(x => x.TrxMediaStorageInActiveDetails.FirstOrDefault() != null).ToList().Where(x => x.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.StatusId == statusSubmit).ToList();
            ViewBag.userKearsipan = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.UKP.ToString()).ToList();
            ViewBag.totalRoom = room.Where(x => x.ArchiveRoomType == GlobalConst.UnitKearsipan).ToList();
            ViewBag.totalStorage = storage.Where(x => x.StatusId == statusSubmit).ToList();

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
            var dataArchive = await _archiveService.GetAll();
            var data = await _archiveUnitService.GetAll();
            var results = data
                .Select(x => new {
                    name = x.ArchiveUnitName,
                    totalArchive = dataArchive.Where(y => y.Creator.ArchiveUnitId == x.ArchiveUnitId && y.TrxFileArchiveDetails.FirstOrDefault() == null).Count(),
                    totalArchiveDigital = dataArchive.Where(y => y.Creator.ArchiveUnitId == x.ArchiveUnitId && y.TrxFileArchiveDetails.FirstOrDefault() != null).Count(),
                }).ToList();
            var result = results.Select(x => new
            {
                x.name,
                x.totalArchive,
                x.totalArchiveDigital,
                total = x.totalArchive + x.totalArchiveDigital
            }).OrderByDescending(x => x.total);
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalGMD()
        {
            var dataArchive = await _archiveService.GetAll();
            var data = await _gmdService.GetAll();
            var result = data
                .Select(x => new {
                    name = x.GmdName,
                    totalArchive = dataArchive.Where(y => y.GmdId == x.GmdId).Count(),
                }).OrderByDescending(x => x.totalArchive).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalGMDActive()
        {
            var dataArchives = await _archiveService.GetAll();
            var dataArchive = dataArchives.Where(x => x.StatusId == statusSubmit).ToList();
            var data = await _gmdService.GetAll();
            var result = data
                .Select(x => new {
                    name = x.GmdName,
                    totalArchive = dataArchive.Where(y => y.GmdId == x.GmdId && y.IsArchiveActive == true).Count(),
                }).OrderByDescending(x => x.totalArchive).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalGMDInActive()
        {
            var dataArchives = await _archiveService.GetAll();
            var dataArchive = dataArchives.Where(x => x.StatusId == statusSubmit).ToList();
            var data = await _gmdService.GetAll();
            var result = data
                .Select(x => new {
                    name = x.GmdName,
                    totalArchive = dataArchive.Where(y => y.GmdId == x.GmdId && y.IsArchiveActive == false).Count(),
                }).OrderByDescending(x => x.totalArchive).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalStorageUse()
        {
            var dataStorage = await _mediaStorageService.GetAll();
            int use = 0;
            int total = 0;
            dataStorage = dataStorage.Where(y => y.StatusId == statusSubmit && y.Row.Level.Rack.Room.ArchiveRoomType == GlobalConst.UnitPengolah).ToList();
            foreach(var row in dataStorage)
            {
                if (row.GmdDetailId != null)
                {
                    total += row.TypeStorage.TrxTypeStorageDetails.Where(x => x.GmdDetailId == row.GmdDetailId).FirstOrDefault().Size;
                    use += row.TrxMediaStorageDetails.Sum(i => i.Archive.Volume);
                } 
            }
            var result = new
            {
                totalUse = use,
                totalAvailable = total - use,
            };
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalStorageUseInActive()
        {
            var dataStorage = await _mediaStorageInActiveService.GetAll();
            int use = 0;
            int total = 0;
            dataStorage = dataStorage.Where(y => y.StatusId == statusSubmit && y.Row.Level.Rack.Room.ArchiveRoomType == GlobalConst.UnitKearsipan).ToList();
            foreach (var row in dataStorage)
            {
                if(row.GmdDetailId != null)
                {
                    total += row.TypeStorage.TrxTypeStorageDetails.Where(x => x.GmdDetailId == row.GmdDetailId).FirstOrDefault().Size;
                    use += row.TrxMediaStorageInActiveDetails.Sum(i => i.Archive.Volume);
                }
            }
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
            var dataRoom = await _roomService.GetAll();
            var dataStorage = await _mediaStorageService.GetAll();
            var result = dataRoom
                .Select(x => new {
                    name = x.RoomName,
                    totalUse = dataStorage.Where(y => y.StatusId == statusSubmit && y.Row.Level.Rack.RoomId == x.RoomId && y.Row.Level.Rack.Room.ArchiveRoomType == GlobalConst.UnitPengolah).Count(),
                }).OrderByDescending(x => x.totalUse).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalRoomUseDetailInActive()
        {
            var dataRoom = await _roomService.GetAll();
            var dataStorage = await _mediaStorageInActiveService.GetAll();
            var result = dataRoom
                .Select(x => new {
                    name = x.RoomName,
                    totalUse = dataStorage.Where(y => y.StatusId == statusSubmit && y.Row.Level.Rack.RoomId == x.RoomId && y.Row.Level.Rack.Room.ArchiveRoomType == GlobalConst.UnitKearsipan).Count(),
                }).OrderByDescending(x => x.totalUse).ToList();
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalArchiveType()
        {
            var dataRooms = await _roomService.GetAll();
            var dataRoom = dataRooms.Where(x => x.ArchiveRoomType == GlobalConst.UnitPengolah).ToList();
            var dataArhives = await _archiveService.GetAll();
            var dataArhive = dataArhives.Where(x => x.TrxMediaStorageDetails.FirstOrDefault() != null).ToList();
            var results = dataRoom
                .Select(x => new {
                    name = x.RoomName,
                    totalInternal = dataArhive.Where(y => y.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.StatusId == statusSubmit && y.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.Rack.RoomId == x.RoomId && y.TypeSender == GlobalConst.Internal).Count(),
                    totalEksternal = dataArhive.Where(y => y.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.StatusId == statusSubmit && y.TrxMediaStorageDetails.FirstOrDefault().MediaStorage.Row.Level.Rack.RoomId == x.RoomId && y.TypeSender == GlobalConst.Eksternal).Count(),
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
            var dataArhives = await _archiveService.GetAll();
            var dataArhive = dataArhives.Where(x => x.TrxMediaStorageInActiveDetails.FirstOrDefault() != null).ToList();
            var results = dataRoom
                .Select(x => new {
                    name = x.RoomName,
                    totalInternal = dataArhive.Where(y => y.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.StatusId == statusSubmit && y.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RoomId == x.RoomId && y.TypeSender == GlobalConst.Internal).Count(),
                    totalEksternal = dataArhive.Where(y => y.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.StatusId == statusSubmit && y.TrxMediaStorageInActiveDetails.FirstOrDefault().MediaStorageInActive.Row.Level.Rack.RoomId == x.RoomId && y.TypeSender == GlobalConst.Eksternal).Count(),
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
            var dataArchive = await _archiveService.GetAll();
            var dataRetensi = await _archiveRetentionService.GetAll();
            var result = new
            {
                active = dataArchive.Where(x => x.IsArchiveActive == true && x.IsActive == true).Count(),
                move = dataArchive.Where(x => x.TrxArchiveMovementDetails.FirstOrDefault() != null).ToList().Where(x => x.TrxArchiveMovementDetails.FirstOrDefault().ArchiveMovement.StatusId == statusReceived).Count(),
                retensi = dataRetensi.Count()
            };
            return Json(result);
        }
        [HttpGet]
        public async Task<JsonResult> BindTotalRetensiInActive()
        {
            var statusMusnah = (int)GlobalConst.STATUS.Musnah;
            var dataArchive = await _archiveService.GetAll();
            var dataRetensi = await _archiveRetentionService.GetInActiveAll();
            var result = new
            {
                active = dataArchive.Where(x => x.IsArchiveActive == false).Count() - dataRetensi.Count(),
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
            var dataArchive = await _archiveService.GetAll();
            var dataRent = await _archiveRentService.GetAll();
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
    }
}
