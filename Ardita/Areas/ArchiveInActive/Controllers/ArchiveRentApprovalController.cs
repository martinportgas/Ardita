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
    public class ArchiveRentApprovalController : BaseController<TrxArchiveRent>
    {
        public ArchiveRentApprovalController(
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
                var result = await _archiveRentService.GetApprovalList(model);
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
        public override async Task<IActionResult> Approval(Guid Id, int Level)
        {
            var model = await _archiveRentService.GetById(Id);
            if (model != null)
            {
                ViewBag.ArchiveRentId = model.TrxArchiveRentId;
                ViewBag.ArchiveId = model.ArchiveId;
                ViewBag.TitleArchive = model.TrxArchiveRentDetails.FirstOrDefault().Archive.TitleArchive;
                ViewBag.SubSubJectClassificationId = model.TrxArchiveRentDetails.FirstOrDefault().Archive.SubSubjectClassification.SubjectClassificationId;
                //  ViewBag.SubSubJectClassificationName = model.FirstOrDefault().Archive.SubSubjectClassification.SubjectClassificationName;
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _archiveRentService.GetById(Id);
            if (model != null)
            {
                ViewBag.ArchiveRentId = model.TrxArchiveRentId;
                ViewBag.ArchiveId = model.ArchiveId;
                ViewBag.TitleArchive = model.TrxArchiveRentDetails.FirstOrDefault().Archive.TitleArchive;
                ViewBag.SubSubJectClassificationId = model.TrxArchiveRentDetails.FirstOrDefault().Archive.SubSubjectClassification.SubjectClassificationId;
                //  ViewBag.SubSubJectClassificationName = model.FirstOrDefault().Archive.SubSubjectClassification.SubjectClassificationName;
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDetailArchive(Guid Id)
        {
            try
            {
                var result = await _MediaStorageInActiveService.GetDetailArchive(Id);
                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public async Task<JsonResult> GetDetail(string archiveName, Guid subSubjectId)
        {
            try
            {
                var result = await _MediaStorageInActiveService.GetDetails(archiveName, subSubjectId);
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
        public override async Task<IActionResult> SubmitApproval(TrxArchiveRent model)
        {
            var ArchiveRentId = model.TrxArchiveRentId;
            var Description = Request.Form["txtDescription"];
            int Status = Request.Form["btnApproval"] == "Approve" ? 3 : 4;
            Guid UserId = AppUsers.CurrentUser(User).UserId;

            await _archiveRentService.Approval(ArchiveRentId, Description, Status, UserId);

            return RedirectToIndex();
        }
        
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveRentApproval, new { Area = GlobalConst.ArchiveInActive });
    }
}
