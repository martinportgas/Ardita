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
    public class ArchiveExtendController : BaseController<TrxArchiveExtend>
    {
        #region MEMBER AND CTR
        public ArchiveExtendController(
            IArchiveDestroyService archiveDestroyService,
            IArchiveExtendService archiveExtendService,
            IArchiveMovementService archiveMovementService,
            IArchiveService archiveService,
            IEmployeeService employeeService,
            IArchiveRetentionService archiveRetentionService,
            IArchiveApprovalService archiveApprovalService,
            IMediaStorageService mediaStorageService,
            IArchiveUnitService archiveUnitService,
            IClassificationSubSubjectService classificationSubSubjectService,
        IClassificationService classificationService,
        IClassificationSubjectService classificationSubjectService,
        IFloorService floorService,
        IRoomService roomService,
        IRackService rackService,
        ILevelService levelService,
        IRowService rowService,
        IArchiveCreatorService archiveCreatorService)
        {
            _archiveExtendService = archiveExtendService;
            _employeeService = employeeService;
            _archiveRetentionService = archiveRetentionService;
            _archiveApprovalService = archiveApprovalService;
            _archiveService = archiveService;
            _archiveDestroyService = archiveDestroyService;
            _archiveMovementService = archiveMovementService;
            _mediaStorageService = mediaStorageService;
            _archiveUnitService = archiveUnitService;
            _classificationSubSubjectService = classificationSubSubjectService;
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
        #endregion
        #region MAIN ACTION
        public override async Task<ActionResult> Index()
        {
            await AllViewBagIndex();
            return await base.Index();
        }
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.IsArchiveActive = false;
                model.SessionUser = User;
                var result = await _archiveExtendService.GetList(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public async Task<JsonResult> GetDetailArchive(String Id)
        {
            try
            {
                var result = await _archiveService.GetById(Guid.Parse(Id));

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {
            await BindAllDropdown(false);

            var model = new TrxArchiveExtend();
            model.ExtendCode = GlobalConst.InitialCode;
            model.DocumentCode = GlobalConst.InitialCode;
            Guid Id = Guid.Empty;
            ViewBag.subDetail = await _archiveExtendService.GetDetailByMainId(Id);
            ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveExtend);
            return View(GlobalConst.Form, model);
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var model = await _archiveExtendService.GetById(Id);

            if (model != null)
            {
                await BindAllDropdown(false);

                ViewBag.subDetail = await _archiveExtendService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveExtend);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var model = await _archiveExtendService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown();

                ViewBag.subDetail = await _archiveExtendService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveExtend);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _archiveExtendService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown();

                ViewBag.subDetail = await _archiveExtendService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveExtend);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Preview(Guid Id)
        {
            var model = await _archiveExtendService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown();

                ViewBag.subDetail = await _archiveExtendService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveExtend);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Approval(Guid Id, int Level)
        {
            var model = await _archiveExtendService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown();

                ViewBag.level = Level;
                ViewBag.subDetail = await _archiveExtendService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveExtend);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxArchiveExtend model)
        {
            int result = 0;
            if (model != null)
            {
                var listApproval = Request.Form["approval[]"].ToArray();

                if (model.ArchiveExtendId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;

                    var old = await _archiveExtendService.GetById(model.ArchiveExtendId);
                    model.ExtendCode = old.ExtendCode;

                    result = await _archiveExtendService.Update(model);
                }
                else
                {
                    model.IsArchiveActive = false;
                    model.StatusId = (int)GlobalConst.STATUS.Draft;
                    model.ApproveLevel = 1;
                    model.ApproveMax = listApproval.Length;
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    result = await _archiveExtendService.Insert(model);
                }

                if (listApproval.Length > 0)
                {
                    result = await _archiveApprovalService.DeleteByTransIdandApprovalCode(model.ArchiveExtendId, GlobalConst.ArchiveExtend);

                    List<TrxApproval> modelsApr = new();
                    TrxApproval modelApr;

                    int approvalLevel = 1;
                    for (int i = 0; i < listApproval.Length; i++)
                    {
                        var emp = listApproval[i];
                        Guid employeeId = Guid.Empty;
                        Guid.TryParse(emp, out employeeId);

                        if (!string.IsNullOrEmpty(emp))
                        {
                            modelApr = new();
                            modelApr.TransId = model.ArchiveExtendId;
                            modelApr.ApprovalCode = GlobalConst.ArchiveExtend;
                            modelApr.EmployeeId = employeeId;
                            modelApr.ApprovalLevel = approvalLevel;
                            modelApr.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            modelApr.CreatedDate = DateTime.Now;

                            await _archiveApprovalService.Insert(modelApr);
                            approvalLevel++;
                        }
                    }
                }

                var listArchive = Request.Form["archive[]"].ToArray();
                var listRetentionBefore = Request.Form["retentionBefore[]"].ToArray();
                var listRetentionAfter = Request.Form["retentionAfter[]"].ToArray();
                var listReason = Request.Form["reasonExtend[]"].ToArray();
                if (listArchive.Length > 0)
                {
                    result = await _archiveExtendService.DeleteDetailByMainId(model.ArchiveExtendId);

                    List<TrxArchiveExtendDetail> modelsDetail = new();
                    TrxArchiveExtendDetail modelDetail;

                    for (int i = 0; i < listArchive.Length; i++)
                    {
                        var ars = listArchive[i];
                        Guid archiveId = Guid.Empty;
                        Guid.TryParse(ars, out archiveId);

                        if (!string.IsNullOrEmpty(ars))
                        {
                            modelDetail = new();
                            modelDetail.ArchiveExtendId = model.ArchiveExtendId;
                            modelDetail.ArchiveId = archiveId;
                            modelDetail.RetentionBefore = int.Parse(listRetentionBefore[i]);
                            modelDetail.RetensionAfter = int.Parse(listRetentionAfter[i]);
                            modelDetail.Reason = listReason[i];
                            modelDetail.IsActive = true;
                            modelDetail.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            modelDetail.CreatedDate = DateTime.Now;

                            await _archiveExtendService.InsertDetail(modelDetail);
                        }
                    }
                }
            }
            return RedirectToIndex();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxArchiveExtend model)
        {
            if (model != null && model.ArchiveExtendId != Guid.Empty)
            {
                await _archiveExtendService.DeleteDetailByMainId(model.ArchiveExtendId);

                model.IsActive = false;
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveExtendService.Submit(model);
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Submit(TrxArchiveExtend model)
        {
            if (model != null && model.ArchiveExtendId != Guid.Empty)
            {
                model.StatusId = (int)GlobalConst.STATUS.ApprovalProcess;
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveExtendService.Submit(model);
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> SubmitApproval(TrxArchiveExtend model)
        {
            if (model != null && model.ArchiveExtendId != Guid.Empty)
            {
                var ApprovalAction = Request.Form[GlobalConst.Submit];
                if (ApprovalAction == GlobalConst.Approve)
                {
                    if (model.ApproveLevel == model.ApproveMax)
                        model.StatusId = (int)GlobalConst.STATUS.Approved;
                    else
                        model.ApproveLevel += 1;
                }
                if (ApprovalAction == GlobalConst.Reject)
                {
                    model.StatusId = (int)GlobalConst.STATUS.Rejected;
                }
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveExtendService.Submit(model);

                if (model.StatusId == (int)GlobalConst.STATUS.Approved)
                {
                    var modelDetail = await _archiveExtendService.GetDetailByMainId(model.ArchiveExtendId);
                    if (modelDetail.Any())
                    {
                        foreach (var item in modelDetail)
                        {
                            var archive = await _archiveService.GetById(item.ArchiveId);
                            if (archive != null)
                            {
                                archive.InactiveRetention = item.RetensionAfter == null ? archive.InactiveRetention : (int)item.RetensionAfter;
                                archive.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                                archive.UpdatedDate = DateTime.Now;

                                await _archiveService.Update(archive, "", new string[] { });
                            }
                        }
                    }
                }
            }
            return RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveApproval, new { Area = GlobalConst.ArchiveInActive });
        }
        #endregion
        #region HELPER
        protected async Task BindAllDropdown(bool isAll = true)
        {
            if (isAll)
            {
                ViewBag.listArchiveUnit = await BindAllArchiveUnits();
                ViewBag.listSubSubject = await BindAllSubSubjectClasscifications();
            }
            else
            {
                ViewBag.listArchiveUnit = await BindArchiveUnits();
                ViewBag.listSubSubject = await BindSubSubjectClasscifications();
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
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveExtend, new { Area = GlobalConst.ArchiveInActive });
        #endregion
    }
}
