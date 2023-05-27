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
    public class ArchiveRetrievalController : BaseController<TrxArchiveRent>
    {
        public ArchiveRetrievalController(
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
                var result = await _archiveRentService.GetRetrievalList(model);
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

            return View(GlobalConst.Form, model);
        }
        [HttpGet]
        public async Task<JsonResult> GetDetail(Guid Id)
        {
            try
            {
                var result = await _archiveRentService.GetRetrievalByArchiveRentId(Id);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetSubDetail(Guid ArchiveId, int sort)
        {
            try
            {
                var result = await _archiveRentService.GetRetrievalDetailByArchiveRentId(ArchiveId, sort);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> UpdateRetrieval(Guid ArchiveRentId)
        {
            try
            {
                var result = await _archiveRentService.UpdateArchiveRent(ArchiveRentId, AppUsers.CurrentUser(User).UserId);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> ValidateQRBox(Guid ArchiveRentId, string MediaStorageInActiveCode)
        {
            try
            {
                var result = await _archiveRentService.ValidateQRBoxWithArchiveRentId(ArchiveRentId, MediaStorageInActiveCode);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveRetrieval, new { Area = GlobalConst.ArchiveInActive });
    }
}
