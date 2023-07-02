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
        public HomeController(IArchiveUnitService archiveUnitService, IRoomService roomService, IArchiveService archiveService, IFileArchiveDetailService fileArchiveDetailService, IGmdService gmdService, ICompanyService companyService, IUserRoleService userRoleService)
        {
            _archiveUnitService = archiveUnitService;
            _roomService = roomService;
            _archiveService = archiveService;
            _fileArchiveDetailService = fileArchiveDetailService;
            _gmdService = gmdService;
            _companyService = companyService;
            _userRoleService = userRoleService;

        }
        public async Task<IActionResult> Index()
        {
            var companies = await _companyService.GetAll();
            var location = await _archiveUnitService.GetAll();
            var userRole = await _userRoleService.GetAll();

            ViewBag.totalCompanies = companies.Count();
            ViewBag.totalLokasi = location.Count();
            ViewBag.userPengolah = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.UPL.ToString()).Count();
            ViewBag.userKearsipan = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.UKP.ToString()).Count();
            ViewBag.userView = userRole.Where(x => x.Role.Code == GlobalConst.ROLE.USV.ToString()).Count();

            return View();
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
                    totalArchive = dataArchive.Where(y => y.Creator.ArchiveUnitId == x.ArchiveUnitId && y.TrxMediaStorageDetails.Count() > 0).Count(),
                    totalArchiveDigital = dataArchive.Where(y => y.Creator.ArchiveUnitId == x.ArchiveUnitId && y.TrxMediaStorageDetails.Count() == 0).Count(),
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
    }
}
