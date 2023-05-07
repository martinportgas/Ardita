﻿using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;

namespace Ardita.Areas.ArchiveActive.Controllers
{
    [CustomAuthorize]
    [Area(Const.ArchiveActive)]
    public class ArchiveMovementController : BaseController<TrxArchiveMovement>
    {
        #region MEMBER AND CTR
        public ArchiveMovementController(
            IArchiveDestroyService archiveDestroyService,
            IArchiveExtendService archiveExtendService,
            IArchiveMovementService archiveMovementService,
            IArchiveService archiveService,
            IEmployeeService employeeService,
            IArchiveRetentionService archiveRetentionService,
            IArchiveApprovalService archiveApprovalService,
            ITypeStorageService typeStorageService)
        {
            _archiveExtendService = archiveExtendService;
            _employeeService = employeeService;
            _archiveRetentionService = archiveRetentionService;
            _archiveApprovalService = archiveApprovalService;
            _archiveService = archiveService;
            _archiveDestroyService = archiveDestroyService;
            _archiveMovementService = archiveMovementService;
            _typeStorageService = typeStorageService;
        }
        #endregion
        #region MAIN ACTION
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _archiveMovementService.GetList(model);

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
        [HttpPost]
        public async Task<JsonResult> GetDetailTypeStorage(Guid Id)
        {
            try
            {
                var result = await _typeStorageService.GetById(Id);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {
            ViewBag.listTypeStorage = await BindTypeStorageByCompanyId(AppUsers.CurrentUser(User).CompanyId);
            var model = new TrxArchiveMovement();
            model.MovementCode = Const.InitialCode;
            Guid Id = Guid.Empty;
            ViewBag.subDetail = await _archiveMovementService.GetDetailByMainId(Id);
            ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, Const.ArchiveExtend);
            return View(Const.Form, model);
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var model = await _archiveMovementService.GetById(Id);
            if (model != null)
            {
                ViewBag.listTypeStorage = await BindTypeStorageByCompanyId(AppUsers.CurrentUser(User).CompanyId);
                ViewBag.subDetail = await _archiveMovementService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, Const.ArchiveMovement);
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var model = await _archiveMovementService.GetById(Id);
            if (model != null)
            {
                ViewBag.listTypeStorage = await BindTypeStorageByCompanyId(AppUsers.CurrentUser(User).CompanyId);
                ViewBag.subDetail = await _archiveMovementService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, Const.ArchiveMovement);
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var model = await _archiveMovementService.GetById(Id);
            if (model != null)
            {
                ViewBag.listTypeStorage = await BindTypeStorageByCompanyId(AppUsers.CurrentUser(User).CompanyId);
                ViewBag.subDetail = await _archiveMovementService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, Const.ArchiveMovement);
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Preview(Guid Id)
        {
            var model = await _archiveMovementService.GetById(Id);
            if (model != null)
            {
                ViewBag.listTypeStorage = await BindTypeStorageByCompanyId(AppUsers.CurrentUser(User).CompanyId);
                ViewBag.subDetail = await _archiveMovementService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, Const.ArchiveMovement);
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Approval(Guid Id, int Level)
        {
            var model = await _archiveMovementService.GetById(Id);
            if (model != null)
            {
                ViewBag.level = Level;
                ViewBag.listTypeStorage = await BindTypeStorageByCompanyId(AppUsers.CurrentUser(User).CompanyId);
                ViewBag.subDetail = await _archiveMovementService.GetDetailByMainId(Id);
                ViewBag.approval = await _archiveApprovalService.GetByTransIdandApprovalCode(Id, Const.ArchiveMovement);
                return View(Const.Form, model);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(TrxArchiveMovement model)
        {
            int result = 0;
            if (model != null)
            {
                var listApproval = Request.Form["approval[]"].ToArray();

                if (model.ArchiveMovementId != Guid.Empty)
                {
                    model.StatusId = (int)Const.Status.Draft;
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    result = await _archiveMovementService.Update(model);
                }
                else
                {
                    model.StatusId = (int)Const.Status.Draft;
                    model.ApproveLevel = 1;
                    model.ApproveMax = listApproval.Length;
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    result = await _archiveMovementService.Insert(model);
                }

                if (listApproval.Length > 0)
                {
                    result = await _archiveApprovalService.DeleteByTransIdandApprovalCode(model.ArchiveMovementId, Const.ArchiveMovement);

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
                            modelApr.TransId = model.ArchiveMovementId;
                            modelApr.ApprovalCode = Const.ArchiveMovement;
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
                if (listArchive.Length > 0)
                {
                    result = await _archiveMovementService.DeleteDetailByMainId(model.ArchiveMovementId);

                    TrxArchiveMovementDetail modelDetail;

                    for (int i = 0; i < listArchive.Length; i++)
                    {
                        var ars = listArchive[i];
                        Guid archiveId = Guid.Empty;
                        Guid.TryParse(ars, out archiveId);

                        if (!string.IsNullOrEmpty(ars))
                        {
                            modelDetail = new();
                            modelDetail.ArchiveMovementId = model.ArchiveMovementId;
                            modelDetail.ArchiveId = archiveId;
                            modelDetail.IsActive = true;
                            modelDetail.CreatedBy = AppUsers.CurrentUser(User).UserId;
                            modelDetail.CreatedDate = DateTime.Now;

                            await _archiveMovementService.InsertDetail(modelDetail);
                        }
                    }
                }
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(TrxArchiveMovement model)
        {
            if (model != null && model.ArchiveMovementId != Guid.Empty)
            {
                await _archiveMovementService.DeleteDetailByMainId(model.ArchiveMovementId);

                model.IsActive = false;
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveMovementService.Submit(model);
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Submit(TrxArchiveMovement model)
        {
            if (model != null && model.ArchiveMovementId != Guid.Empty)
            {
                model.StatusId = (int)Const.Status.ApprovalProcess;
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveMovementService.Submit(model);
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> SubmitApproval(TrxArchiveMovement model)
        {
            if (model != null && model.ArchiveMovementId != Guid.Empty)
            {
                if (model.ApproveLevel == model.ApproveMax)
                    model.StatusId = (int)Const.Status.ApprovalProcess;
                else
                    model.ApproveLevel += 1;
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                await _archiveMovementService.Submit(model);

                if (model.StatusId == (int)Const.Status.Approved)
                {
                    var modelDetail = await _archiveMovementService.GetDetailByMainId(model.ArchiveMovementId);
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
                        }
                    }
                }
            }
            return RedirectToAction(Const.Index, Const.ArchiveApproval, new { Area = Const.ArchiveActive });
        }
        [HttpGet]
        public async Task<IActionResult> DownloadFile(Guid Id)
        {
            return File(new byte[] { }, "application/octet-stream", "BeritaAcaraPemindahan.pdf");
        }
        #endregion
        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.ArchiveMovement, new { Area = Const.ArchiveActive });
        #endregion
    }
}