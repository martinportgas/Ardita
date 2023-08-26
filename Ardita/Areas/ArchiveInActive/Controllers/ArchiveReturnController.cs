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
    public class ArchiveReturnController : BaseController<TrxArchiveRent>
    {
        public ArchiveReturnController(
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
                var result = await _archiveRentService.GetReturnList(model);
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
        public async Task<IActionResult> ArchiveReturn(string Id)
        {
            Guid dataId = Guid.Empty;
            Guid.TryParse(Id, out dataId);
            var model = await _archiveRentService.GetById(dataId);
            if (model != null)
            {
                if (model.StatusId == (int)GlobalConst.STATUS.Retrieved)
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
                var result = await _archiveRentService.GetReturnByArchiveRentId(Id, form);

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
                var result = await _archiveRentService.GetReturnDetailByArchiveRentId(ArchiveId, sort);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
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
        public async Task<IActionResult> UpdateReturn(TrxArchiveRent model)
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
        [HttpGet]
        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            //TrxArchiveRent data = await _archiveRentService.GetById(Id);

            //var result = await _MediaStorageInActiveService.GetDetails(data.TrxArchiveRentDetails.FirstOrDefault().ArchiveId);

            //string FilePath = Path.Combine(_hostingEnvironment.WebRootPath, "BA_Peminjaman_Arsip.docx");
            //var file = Label.GenerateBARent(FilePath, data, result);

            //return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, $"{data.RentCode}.pdf");

            var settings = await _templateSettingService.GetAll();
            var setting = settings.Where(x => x.TemplateName == GlobalConst.TemplatePengembalianArsip).FirstOrDefault();

            var data = await _templateSettingService.GetDataView(setting.SourceData, Id);

            string FilePath = Path.Combine(_hostingEnvironment.WebRootPath, setting.Path);
            var file = Label.GenerateFromTemplate(setting.MstTemplateSettingDetails.ToList(), data, FilePath);

            TrxArchiveRent dataMain = await _archiveRentService.GetById(Id);
            return File(file, System.Net.Mime.MediaTypeNames.Application.Octet, $"{GlobalConst.TemplatePengembalianArsip.Replace(" ", "")}-{dataMain.RentCode}.pdf");
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveReturn, new { Area = GlobalConst.ArchiveInActive });
    }
}
