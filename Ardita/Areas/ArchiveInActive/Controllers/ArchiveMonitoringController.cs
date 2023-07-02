using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveInActive.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.ArchiveInActive)]
    public class ArchiveMonitoringController : BaseController<TrxArchive>
    {
        public ArchiveMonitoringController(
        IArchiveCreatorService archiveCreatorService,
        IRowService rowService,
        IArchiveService archiveService,
        IFileArchiveDetailService fileArchiveDetailService)
        {
            _archiveCreatorService = archiveCreatorService;
            _rowService = rowService;
            _fileArchiveDetailService = fileArchiveDetailService;
            _archiveService = archiveService;
        }
        public override async Task<ActionResult> Index()
        {
            ViewBag.ListArchiveCreator = await BindArchiveCreators();
            ViewBag.listRow = await BindRowsWithDetails();
            return View();
        }
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.whereClause = GlobalConst.WhereClauseArchiveMonitoring;
                model.IsArchiveActive = false;
                if (AppUsers.CurrentUser(User).RoleCode == GlobalConst.ROLE.USV.ToString())
                    model.PositionId = AppUsers.CurrentUser(User).PositionId;
                var result = await _archiveService.GetList(model);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _archiveService.GetById(Id);
            ViewBag.title = GlobalConst.TitleArchiveMonitoring;
            ViewBag.backController = GlobalConst.ArchiveMonitoring;
            ViewBag.backArea = GlobalConst.ArchiveActive;
            return PartialView(GlobalConst._ArchiveMonitoringDetail, model);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            var model = await _fileArchiveDetailService.GetById(Id);
            string path = model.FilePath;

            if (System.IO.File.Exists(path))
            {
                return File(System.IO.File.OpenRead(path), "application/octet-stream", Path.GetFileName(path));
            }
            return NotFound();
        }
    }
}
