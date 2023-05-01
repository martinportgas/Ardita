using Ardita.Controllers;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace Ardita.Areas.ArchiveActive.Controllers
{
    [CustomAuthorize]
    [Area(Const.ArchiveActive)]
    public class ArchiveRetentionController : BaseController<TrxArchive>
    {
        #region MEMBER AND CTR
        private readonly IArchiveRetentionService _archiveRetentionService;
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
            ViewBag.title = Const.TitleArchiveRetention;
            ViewBag.backController = Const.ArchiveRetention;
            ViewBag.backArea = Const.ArchiveActive;
            return PartialView(Const._ArchiveMonitoringDetail, model);
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
