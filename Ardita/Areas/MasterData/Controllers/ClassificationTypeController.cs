using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting.Internal;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.MasterData)]
    public class ClassificationTypeController : BaseController<MstTypeClassification>
    {
        #region MEMBER AND CTR
        private readonly IClassificationTypeService _classificationTypeService;
        public ClassificationTypeController(
            IHostingEnvironment hostingEnvironment, IClassificationTypeService classificationTypeService)
        {
            _hostingEnvironment = hostingEnvironment;
            _classificationTypeService = classificationTypeService;
        }

        #endregion
        #region MAIN ACTION
        public override async Task<ActionResult> Index() => await base.Index();
        public async Task<JsonResult> GetData(DataTablePostModel model)
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
            return View(Const.Form, new MstTypeClassification());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _classificationTypeService.GetById(Id);
            if (data.Count() > 0)
            {
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _classificationTypeService.GetById(Id);
            if (data.Count() > 0)
            {
                return View(Const.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }

        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _classificationTypeService.GetById(Id);
            if (data.Count() > 0)
            {
                return View(Const.Form, data.FirstOrDefault());
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
        public async Task DownloadTemplate()
        {
            try
            {
                string fileName = $"{Const.Template}-{nameof(MstTypeClassification).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstTypeClassification).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(nameof(MstTypeClassification.TypeClassificationCode));
                row.CreateCell(1).SetCellValue(nameof(MstTypeClassification.TypeClassificationName));

                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task Export()
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

                row.CreateCell(0).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationCode));
                row.CreateCell(1).SetCellValue(nameof(TrxSubjectClassification.SubjectClassificationName));

                int no = 1;
                foreach (var item in data)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(item.TypeClassificationCode);
                    row.CreateCell(1).SetCellValue(item.TypeClassificationName);
                    no += 1;
                }
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
        }
        public async Task<IActionResult> Upload()
        {
            IFormFile file = Request.Form.Files[0];
            var result = Extensions.Global.ImportExcel(file, Const.Upload, _hostingEnvironment.WebRootPath);

            var type = await _classificationTypeService.GetAll();

            List<MstTypeClassification> models = new();
            MstTypeClassification model;

            foreach (DataRow row in result.Rows)
            {
                model = new();
                model.TypeClassificationId = Guid.NewGuid();
                model.TypeClassificationCode = row[0].ToString();
                model.TypeClassificationName = row[1].ToString();
                model.IsActive = true;
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;

                models.Add(model);
            }
            await _classificationTypeService.InsertBulk(models);

            return RedirectToIndex();
        }
        #endregion
        #region HELPER
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.ClassificationSubject, new { Area = Const.MasterData });
        #endregion
    }
}
