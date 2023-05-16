using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveInActive.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.ArchiveInActive)]
    public class ArchiveRetentionController : BaseController<TrxArchive>
    {
        #region MEMBER AND CTR
        public ArchiveRetentionController(
            IArchiveRetentionService archiveRetentionService,
            IArchiveService archiveService,
            IFileArchiveDetailService fileArchiveDetailService)
        {
            _archiveRetentionService = archiveRetentionService;
            _fileArchiveDetailService = fileArchiveDetailService;
            _archiveService = archiveService;
        }
        #endregion

        #region MAIN ACTION
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.IsArchiveActive = false;
                var result = await _archiveRetentionService.GetList(model);

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
            ViewBag.title = GlobalConst.TitleArchiveRetention;
            ViewBag.backController = GlobalConst.ArchiveRetention;
            ViewBag.backArea = GlobalConst.ArchiveInActive;
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
        #endregion
    }
}
