﻿using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.MasterData)]
    public class ClassificationTypeController : BaseController<MstTypeClassification>
    {
        #region MEMBER AND CTR
        private readonly IClassificationTypeService _classificationTypeService;
        public ClassificationTypeController(IClassificationTypeService classificationTypeService)
        {
            _classificationTypeService = classificationTypeService;
        }

        #endregion
        #region MAIN ACTION
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _classificationTypeService.GetListClassificationType(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {
            return View(GlobalConst.Form, new MstTypeClassification());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _classificationTypeService.GetById(Id);
            if (data != null)
            {
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _classificationTypeService.GetById(Id);
            if (data != null)
            {
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }

        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _classificationTypeService.GetById(Id);
            if (data != null)
            {
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(MstTypeClassification model)
        {
            int result = 0;
            if (model != null)
            {
                if (model.TypeClassificationId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    result = await _classificationTypeService.Update(model);
                }

                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    result = await _classificationTypeService.Insert(model);
                }
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(MstTypeClassification model)
        {
            int result = 0;
            if (model != null && model.TypeClassificationId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                result = await _classificationTypeService.Delete(model);
            }
            return RedirectToIndex();
        }
        public async Task<IActionResult> UploadForm()
        {
            await Task.Delay(0);
            ViewBag.errorCount = TempData["errorCount"] == null ? -1 : TempData["errorCount"];
            return View();
        }
        public async Task<IActionResult> DownloadTemplate()
        {
            try
            {
                string fileName = $"{GlobalConst.Template}-{nameof(MstTypeClassification).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstTypeClassification).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(MstTypeClassification.TypeClassificationCode));
                row.CreateCell(2).SetCellValue(nameof(MstTypeClassification.TypeClassificationName));

                using (var exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    byte[] bytes = exportData.ToArray();
                    return File(bytes, GlobalConst.EXCEL_FORMAT_TYPE, $"{fileName}.xlsx");
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<IActionResult> Export()
        {
            try
            {
                string fileName = nameof(TrxSubjectClassification).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var data = await _classificationTypeService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxSubjectClassification).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(2).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));

                int no = 1;
                foreach (var item in data)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(item.TypeClassificationCode);
                    row.CreateCell(2).SetCellValue(item.TypeClassificationName);
                    no += 1;
                }

                using (var exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    byte[] bytes = exportData.ToArray();
                    return File(bytes, GlobalConst.EXCEL_FORMAT_TYPE, $"{fileName}.xlsx");
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<IActionResult> Upload()
        {
            try
            { 
                IFormFile file = Request.Form.Files[0];
                if (file.Length > 0)
                {
                    var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, string.Empty);
                    if (result.Rows.Count > 0)
                    {
                        List<MstTypeClassification> models = new();
                        MstTypeClassification model;


                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");

                        var classificationTypeDetail = await _classificationTypeService.GetAll();

                        
                        foreach (DataRow row in result.Rows)
                        {
                            string error = string.Empty;

                            if (classificationTypeDetail.Where(x => x.TypeClassificationCode == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_Kode Tipe Klasifikasi sudah ada";
                            }

                            if(valid) 
                            {
                                model = new();
                                model.TypeClassificationId = Guid.NewGuid();
                                model.TypeClassificationCode = row[1].ToString();
                                model.TypeClassificationName = row[2].ToString();
                                model.IsActive = true;
                                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                model.CreatedDate = DateTime.Now;

                                models.Add(model);
                            }
                            else
                            {
                                errorCount++;
                            }
                            row["Keterangan"] = error;


                        }
                        ViewBag.result = JsonConvert.SerializeObject(result);
                        ViewBag.errorCount = errorCount;

                        if (valid)
                            await _classificationTypeService.InsertBulk(models);
                    }
                    return View(GlobalConst.UploadForm);
                }
                else
                {
                    TempData["errorCount"] = 100000001;
                    return RedirectToAction(GlobalConst.UploadForm);
                }
            
            } 
            catch (Exception ex) 
            {
                TempData["errorCount"] = 100000001;
                return RedirectToAction(GlobalConst.UploadForm);
            }
        }
        #endregion
        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ClassificationType, new { Area = GlobalConst.MasterData });
        #endregion
    }
}
