using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Ardita.Areas.ArchiveInActive.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.ArchiveInActive)]
    public class ArchiveDestroyController : BaseController<TrxArchiveDestroy>
    {
        #region MEMBER AND CTR
        public ArchiveDestroyController(
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
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                model.IsArchiveActive = false;
                model.SessionUser = User;
                var result = await _archiveDestroyService.GetList(model);

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

            var model = new TrxArchiveDestroy();
            model.DestroyCode = GlobalConst.InitialCode;
            model.DocumentCode = GlobalConst.InitialCode;
            Guid Id = Guid.Empty;
            ViewBag.subDetail = await _archiveDestroyService.GetDetailByMainId(Id);
            ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveExtend);
            return View(GlobalConst.Form, model);
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var model = await _archiveDestroyService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown(false);

                ViewBag.subDetail = await _archiveDestroyService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveDestroy);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var model = await _archiveDestroyService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown();

                ViewBag.subDetail = await _archiveDestroyService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveDestroy);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _archiveDestroyService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown();

                ViewBag.subDetail = await _archiveDestroyService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveDestroy);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Preview(Guid Id)
        {
            var model = await _archiveDestroyService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown();

                ViewBag.subDetail = await _archiveDestroyService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveDestroy);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Approval(Guid Id, int Level)
        {
            var model = await _archiveDestroyService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown();

                ViewBag.level = Level;
                ViewBag.subDetail = await _archiveDestroyService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveDestroy);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }


        public async Task<IActionResult> Destroy(Guid Id)
        {
            var model = await _archiveDestroyService.GetById(Id);
            if (model != null)
            {
                await BindAllDropdown();

                ViewBag.subDetail = await _archiveDestroyService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, GlobalConst.ArchiveDestroy);
                return View(GlobalConst.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitDestroy(TrxArchiveDestroy model)
        {
            if (model != null && model.ArchiveDestroyId != Guid.Empty)
            {
                model.StatusId = (int)GlobalConst.STATUS.Musnah;
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                model.DestroySchedule = DateTime.Now;
                await _archiveDestroyService.Submit(model);

                var modelDetail = await _archiveDestroyService.GetDetailByMainId(model.ArchiveDestroyId);
                if (modelDetail.Any())
                {
                    foreach (var item in modelDetail)
                    {
                        var mediaStorage = await _mediaStorageService.GetDetailByArchiveId(item.ArchiveId);
                        if (mediaStorage != null)
                        {
                            mediaStorage.IsActive = false;
                            mediaStorage.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                            mediaStorage.UpdatedDate = DateTime.Now;

                            await _mediaStorageService.UpdateDetail(mediaStorage);
                        }

                        var archive = await _archiveService.GetById(item.ArchiveId);
                        if (archive != null)
                        {
                            archive.IsActive = false;
                            archive.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                            archive.UpdatedDate = DateTime.Now;

                            await _archiveService.Update(archive, "", new string[] { });
                        }
                    }
                }
            }
            return RedirectToIndex();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxArchiveDestroy model)
        {
            int result = 0;
            if (model != null)
            {
                var listApproval = Request.Form["approval[]"].ToArray();

                if (model.ArchiveDestroyId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;

                    var old = await _archiveDestroyService.GetById(model.ArchiveDestroyId);
                    model.DestroyCode = old.DestroyCode;

                    result = await _archiveDestroyService.Update(model);
                }
                else
                {
                    model.IsArchiveActive = false;
                    model.StatusId = (int)GlobalConst.STATUS.Draft;
                    model.ApproveLevel = 1;
                    model.ApproveMax = listApproval.Length;
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    result = await _archiveDestroyService.Insert(model);
                }

                if (listApproval.Length > 0)
                {
                    result = await _archiveApprovalService.DeleteByTransIdandApprovalCode(model.ArchiveDestroyId, GlobalConst.ArchiveDestroy);

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
                            modelApr.TransId = model.ArchiveDestroyId;
                            modelApr.ApprovalCode = GlobalConst.ArchiveDestroy;
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
                var listReason = Request.Form["reason[]"].ToArray();
                if (listArchive.Length > 0)
                {
                    result = await _archiveDestroyService.DeleteDetailByMainId(model.ArchiveDestroyId);

                    List<TrxArchiveDestroyDetail> modelsDetail = new();
                    TrxArchiveDestroyDetail modelDetail;

                    for (int i = 0; i < listArchive.Length; i++)
                    {
                        var ars = listArchive[i];
                        Guid archiveId = Guid.Empty;
                        Guid.TryParse(ars, out archiveId);

                        if (!string.IsNullOrEmpty(ars))
                        {
                            modelDetail = new();
                            modelDetail.ArchiveDestroyId = model.ArchiveDestroyId;
                            modelDetail.ArchiveId = archiveId;
                            modelDetail.Reason = listReason[i];
                            modelDetail.IsActive = true;
                            modelDetail.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            modelDetail.CreatedDate = DateTime.Now;

                            await _archiveDestroyService.InsertDetail(modelDetail);
                        }
                    }
                }
            }
            return RedirectToIndex();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxArchiveDestroy model)
        {
            if (model != null && model.ArchiveDestroyId != Guid.Empty)
            {
                await _archiveDestroyService.DeleteDetailByMainId(model.ArchiveDestroyId);

                model.IsActive = false;
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveDestroyService.Submit(model);
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Submit(TrxArchiveDestroy model)
        {
            if (model != null && model.ArchiveDestroyId != Guid.Empty)
            {
                model.StatusId = (int)GlobalConst.STATUS.ApprovalProcess;
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveDestroyService.Submit(model);
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> SubmitApproval(TrxArchiveDestroy model)
        {
            if (model != null && model.ArchiveDestroyId != Guid.Empty)
            {
                var ApprovalAction = Request.Form[GlobalConst.Submit];
                if (ApprovalAction == GlobalConst.Approve)
                {
                    if (model.ApproveLevel == model.ApproveMax)
                        model.StatusId = (int)GlobalConst.STATUS.UsulMusnah;
                    else
                        model.ApproveLevel += 1;
                }
                if (ApprovalAction == GlobalConst.Reject)
                {
                    model.StatusId = (int)GlobalConst.STATUS.Rejected;
                }
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveDestroyService.Submit(model);

                if(model.StatusId == (int)GlobalConst.STATUS.Approved)
                {
                    var modelDetail = await _archiveDestroyService.GetDetailByMainId(model.ArchiveDestroyId);
                    if (modelDetail.Any())
                    {
                        foreach (var item in modelDetail)
                        {
                            var mediaStorage = await _mediaStorageService.GetDetailByArchiveId(item.ArchiveId);
                            if (mediaStorage != null)
                            {
                                mediaStorage.IsActive = false;
                                mediaStorage.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                                mediaStorage.UpdatedDate = DateTime.Now;

                                await _mediaStorageService.UpdateDetail(mediaStorage);
                            }

                            var archive = await _archiveService.GetById(item.ArchiveId);
                            if(archive != null)
                            {
                                archive.IsActive = false;
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
        [HttpGet]
        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            return File(new byte[] { }, "application/octet-stream", "BeritaAcaraPemusnahan.pdf");
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
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveDestroy, new { Area = GlobalConst.ArchiveInActive });
        #endregion
    }
}
