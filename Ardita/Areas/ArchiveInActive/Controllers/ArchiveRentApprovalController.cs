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
            IClassificationSubjectService classificationSubjectService,
            IClassificationSubSubjectService classificationSubSubjectService,
            IArchiveService archiveService,
            IMediaStorageInActiveService mediaStorageInActiveService,
            IArchiveUnitService archiveUnitService,
        IClassificationService classificationService,
        IFloorService floorService,
        IRoomService roomService,
        IRackService rackService,
        ILevelService levelService,
        IRowService rowService,
        IArchiveCreatorService archiveCreatorService
            )
        {
            _archiveRentService = archiveRentService;
            _classificationSubSubjectService = classificationSubSubjectService;
            _archiveService = archiveService;
            _MediaStorageInActiveService = mediaStorageInActiveService;
            _archiveUnitService = archiveUnitService;
            _classificationSubSubjectService = classificationSubSubjectService;
            _classificationSubjectService = classificationSubjectService;
            _levelService = levelService;
            _rowService = rowService;
            _archiveCreatorService = archiveCreatorService;
            _classificationService = classificationService;
            _rackService = rackService;
            _roomService = roomService;
            _floorService = floorService;
        }
        public override async Task<ActionResult> Index()
        {
            await AllViewBagIndex();
            return await base.Index();
        }
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.SessionUser = User;
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
                //ViewBag.ArchiveRentId = model.TrxArchiveRentId;
                //ViewBag.ArchiveId = model.ArchiveId;
                //ViewBag.TitleArchive = model.TrxArchiveRentDetails.FirstOrDefault().Archive.TitleArchive;
                //ViewBag.SubSubJectClassificationId = model.TrxArchiveRentDetails.FirstOrDefault().Archive.SubSubjectClassification.SubjectClassificationId;
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
                //ViewBag.ArchiveRentId = model.TrxArchiveRentId;
                //ViewBag.ArchiveId = model.ArchiveId;
                //ViewBag.TitleArchive = model.TrxArchiveRentDetails.FirstOrDefault().Archive.TitleArchive;
                //ViewBag.SubSubJectClassificationId = model.TrxArchiveRentDetails.FirstOrDefault().Archive.SubSubjectClassification.SubjectClassificationId;
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
        public async Task AllViewBagIndex()
        {
            ViewBag.ListArchiveUnit = await BindAllArchiveUnits();
            ViewBag.ListFloor = await BindFloors();
            ViewBag.ListRoom = await BindRooms();
            ViewBag.ListRack = await BindRacks();
            ViewBag.ListLevel = await BindLevels();
            ViewBag.ListRow = await BindRows();
            ViewBag.ListArchiveCreator = await BindArchiveCreators();
            ViewBag.ListClassification = await BindClasscifications();
            ViewBag.ListSubjectClassification = await BindSubjectClasscifications();
        }

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveRentApproval, new { Area = GlobalConst.ArchiveInActive });
    }
}
