using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.ArchiveInActive.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.ArchiveInActive)]
    public class ArchiveRentController : BaseController<TrxArchiveRent>
    {
        public ArchiveRentController(
            IArchiveRentService archiveRentService,
            IClassificationSubSubjectService classificationSubSubjectService,
            IArchiveService archiveService,
            IMediaStorageInActiveService mediaStorageInActiveService
            )
        {
            _archiveRentService = archiveRentService;
            _classificationSubSubjectService = classificationSubSubjectService;
            _archiveService = archiveService;
            MediaStorageInActiveService = mediaStorageInActiveService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _archiveRentService.GetList(model);
                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {
            var model = new TrxArchiveRent();
            ViewBag.listSubSubject = await BindSubSubjectClasscifications();
            ViewBag.listArchive = await BindArchives();
            return View(GlobalConst.Form, model);
        }
        [HttpGet]
        public async Task<JsonResult> GetDetail(Guid Id)
        {
            try
            {
                var result = await MediaStorageInActiveService.GetDetails(new Guid("34965E22-F1F8-4DAC-951B-00E3B8D3144D"));
                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDetailArchive(Guid Id)
        {
            try
            {
                var result = await MediaStorageInActiveService.GetDetailArchive(Id);
                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Save(TrxArchiveRent model)
        {
            var ArchiveId = new Guid(Request.Form["txtMdlArchiveId"]);
            var UserId = AppUsers.CurrentUser(User).UserId;
            var CreatedDate = DateTime.Now;
            var CreatedBy = AppUsers.CurrentUser(User).UserId;
            var Description = Request.Form["txtMdlDescription"];
            var requestedDate = Request.Form["txtMdlRequestedDate"];
            var requestedReturnDate = Request.Form["txtMdlRequestedReturnDate"];

            model.ArchiveId = ArchiveId;
            model.UserId = UserId;
            model.Description = Description;
            model.RequestedDate = Convert.ToDateTime(requestedDate);
            model.RequestedReturnDate = Convert.ToDateTime(requestedReturnDate);
            model.CreatedDate = CreatedDate;
            model.CreatedBy = CreatedBy;

            await _archiveRentService.Insert(model);

            return RedirectToIndex();
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveRent, new { Area = GlobalConst.ArchiveInActive });
    }
}
