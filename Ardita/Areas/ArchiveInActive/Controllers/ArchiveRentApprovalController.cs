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
        public override async Task<IActionResult> Approval(Guid Id, int Level)
        {
            var model = new TrxArchiveRent();
            ViewBag.listSubSubject = await BindSubSubjectClasscifications();
            ViewBag.listArchive = await BindArchives();

            ViewBag.Name = AppUsers.CurrentUser(User).EmployeeName;
            ViewBag.RoleName = AppUsers.CurrentUser(User).RoleName;
            ViewBag.NIK = AppUsers.CurrentUser(User).EmployeeNIK;
            ViewBag.Email = AppUsers.CurrentUser(User).EmployeeMail;
            ViewBag.Company = AppUsers.CurrentUser(User).CompanyName;
            ViewBag.Phone = AppUsers.CurrentUser(User).EmployeePhone;

            var rent = await _archiveRentService.GetById(Id);
            var archive = await _MediaStorageInActiveService.GetDetailArchive(rent.FirstOrDefault().ArchiveId);



            foreach (dynamic items in archive)
            {
                ViewBag.TrxArchiveRentId = Id;
                ViewBag.SubSubjectClassification = items.SubSubjectClassification.SubSubjectClassificationName;
                ViewBag.TitleArchive = items.TitleArchive;
                ViewBag.CreatorName = items.Creator.CreatorName;
                ViewBag.RequestedDate = DateTime.Now.ToString("dd/MM/yyyy");
                ViewBag.ReturnDate = DateTime.Now.AddDays(7).ToString("dd/MM/yyyy");
                ViewBag.ArchiveUnitName = items.Creator.ArchiveUnit.ArchiveUnitName;
                ViewBag.DescriptionByUser = rent.FirstOrDefault().Description;
                break;
            }

            

            return View(GlobalConst.Form, model);
        }

        public override async Task<IActionResult> SubmitApproval(TrxArchiveRent model)
        {
            var ArchiveRentId = new Guid(Request.Form["txtTrxArchiveRentId"].ToString());
            var Description = Request.Form["txtDescription"];
            int Status = Request.Form["btnApproval"] == "Approve" ? 3 : 4;
            Guid UserId = AppUsers.CurrentUser(User).UserId;

            await _archiveRentService.Approval(ArchiveRentId, Description, Status, UserId);

            return RedirectToIndex();
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveRentApproval, new { Area = GlobalConst.ArchiveInActive });
    }
}
