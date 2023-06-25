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
            IClassificationSubjectService classificationSubjectService,
            IClassificationSubSubjectService classificationSubSubjectService,
            IArchiveService archiveService,
            IMediaStorageInActiveService mediaStorageInActiveService
            )
        {
            _archiveRentService = archiveRentService;
            _classificationSubjectService = classificationSubjectService;
            _classificationSubSubjectService = classificationSubSubjectService;
            _archiveService = archiveService;
            _MediaStorageInActiveService = mediaStorageInActiveService;
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
            ViewBag.listSubjectClassification = await BindSubjectClasscifications();
            ViewBag.listSubSubject = await BindSubSubjectClasscifications();
            ViewBag.listArchive = await BindArchivesInActive();
            ViewBag.listBorrower = await BindBorrower();

            ViewBag.Name = AppUsers.CurrentUser(User).EmployeeName;
            ViewBag.RoleName = AppUsers.CurrentUser(User).RoleName;
            ViewBag.NIK = AppUsers.CurrentUser(User).EmployeeNIK;
            ViewBag.Email = AppUsers.CurrentUser(User).EmployeeMail;
            ViewBag.Company = AppUsers.CurrentUser(User).CompanyName;
            ViewBag.Phone = AppUsers.CurrentUser(User).EmployeePhone;

            return View(GlobalConst.Form, model);
        }
        [HttpGet]
        public async Task<JsonResult> GetDetail(Guid Id)
        {
            try
            {
                var result = await _MediaStorageInActiveService.GetDetails(Id);
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
                var result = await _MediaStorageInActiveService.GetDetailArchive(Id);
                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Save(TrxArchiveRent model)
        {
            if (model != null)
            {
                
                var ArchiveId = new Guid(Request.Form["txtMdlArchiveId"]);
                var UserId = AppUsers.CurrentUser(User).UserId;
                var CreatedDate = DateTime.Now;
                var CreatedBy = AppUsers.CurrentUser(User).UserId;
                var Description = Request.Form["txtMdlDescription"];
                var requestedDate = Request.Form["txtMdlRequestedDate"];
                var requestedReturnDate = Request.Form["txtMdlRequestedReturnDate"];

                model.ArchiveId = ArchiveId;
                model.Description = Description;
                model.RequestedDate = Convert.ToDateTime(requestedDate);
                model.RequestedReturnDate = Convert.ToDateTime(requestedReturnDate);
                model.CreatedDate = CreatedDate;
                model.CreatedBy = CreatedBy;

                var borrower = new MstBorrower();
               
                borrower.BorrowerName = Request.Form["BorrowerName"].ToString();
                borrower.BorrowerCompany = Request.Form["BorrowerCompany"].ToString();
                borrower.BorrowerArchiveUnit = Request.Form["BorrowerArchiveUnit"].ToString();
                borrower.BorrowerPosition = Request.Form["BorrowerPosition"].ToString();
                borrower.BorrowerIdentityNumber = Request.Form["borrowerIdentityId"].ToString();
                borrower.BorrowerPhone = Request.Form["BorrowerPhone"].ToString();
                borrower.BorrowerEmail = Request.Form["BorrowerEmail"].ToString();
               

                if (Request.Form["borrowerType[]"].ToString() == "Baru")
                {
                    borrower.BorrowerId = new Guid();
                    borrower.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    borrower.CreatedDate = DateTime.Now;
                    await _archiveRentService.Insert(model, borrower);
                }
                else
                {
                    borrower.BorrowerId = new Guid(Request.Form["borrowerNameSelect"].ToString());
                    borrower.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    borrower.UpdatedDate = DateTime.Now;
                    await _archiveRentService.Update(model, borrower);
                }

                
            }

            return RedirectToIndex();
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _archiveRentService.GetById(Id);
            if (model != null)
            {
                ViewBag.ArchiveRentId = model.TrxArchiveRentId;
                ViewBag.ArchiveId = model.ArchiveId;
                ViewBag.TitleArchive = model.Archive.TitleArchive;
                ViewBag.SubSubJectClassificationId = model.Archive.SubSubjectClassification.SubjectClassificationId;
              //  ViewBag.SubSubJectClassificationName = model.FirstOrDefault().Archive.SubSubjectClassification.SubjectClassificationName;
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveRent, new { Area = GlobalConst.ArchiveInActive });
    }
}
