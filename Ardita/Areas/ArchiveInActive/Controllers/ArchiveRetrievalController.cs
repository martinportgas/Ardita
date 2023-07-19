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
            _MediaStorageInActiveService = mediaStorageInActiveService;
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
        public async Task<JsonResult> GetDataHistory(Guid Id)
        {
            try
            {
                var result = await _archiveRentService.GetByBorrowerId(Id);
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
            ViewBag.QR = TempData[GlobalConst.NotFound];
            ViewBag.Notif = TempData[GlobalConst.Notification];
            TempData.Clear();

            return View(GlobalConst.Form, model);
        }
        public async Task<IActionResult> ArchiveRetrieval(string Id)
        {
            Guid dataId = Guid.Empty;
            Guid.TryParse(Id, out dataId);
            var model = await _archiveRentService.GetById(dataId);
            if (model != null)
            {
                if (model.StatusId == (int)GlobalConst.STATUS.WaitingForRetrieval)
                    return View(GlobalConst.Form, model);
                else
                {
                    TempData[GlobalConst.NotFound] = Id;
                    TempData[GlobalConst.Notification] = GlobalConst.NotFound;
                    return RedirectToAction(GlobalConst.Add);
                }
            }
            else
            {
                TempData[GlobalConst.NotFound] = Id;
                TempData[GlobalConst.Notification] = GlobalConst.NotFound;
                return RedirectToAction(GlobalConst.Add);
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _archiveRentService.GetById(Id);
            if (model != null)
            {
                return View("../" + GlobalConst.ArchiveRentApproval + "/" + GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDataRentBox(Guid Id)
        {
            try
            {
                var result = await _archiveRentService.GetArchiveRentBoxById(Id);
                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDataRentBoxDetail(Guid Id, int Sort)
        {
            try
            {
                var dataDetail = await _MediaStorageInActiveService.GetDetailStorages(Id, Sort);
                var dataLocation = await _MediaStorageInActiveService.GetById(Id);
                var result = new
                {
                    main = dataLocation,
                    detail = dataDetail
                };
                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDetail(Guid Id, string form)
        {
            try
            {
                var result = await _archiveRentService.GetRetrievalByArchiveRentId(Id, form);

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
        public async Task<IActionResult> UpdateRetrieval(TrxArchiveRent model)
        {
            try
            {
                await _archiveRentService.UpdateArchiveRent(model.TrxArchiveRentId, AppUsers.CurrentUser(User).UserId);
                return RedirectToIndex();
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
