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
    [Area("MasterData")]
    public class ClassificationTypeController : BaseController<MstTypeClassification>
    {
        private IHostingEnvironment _hostingEnvironment;
        private readonly IClassificationTypeService _classificationTypeService;
        public ClassificationTypeController(
            IHostingEnvironment hostingEnvironment, IClassificationTypeService classificationTypeService)
        {
            _hostingEnvironment = hostingEnvironment;
            _classificationTypeService = classificationTypeService;
        }
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
                return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
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
                return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
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
                return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
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
            return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
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
            return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
        }
        public async Task<IActionResult> DownloadTemplate()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"ClassificationTypeTemplate.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("ClassificationType");

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("ClassificationTypeCode");
                row.CreateCell(1).SetCellValue("ClassificationTypeName");
                row.CreateCell(2).SetCellValue("Active");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("Sample Code");
                row.CreateCell(1).SetCellValue("Sample Name");
                row.CreateCell(2).SetCellValue("1");

                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public async Task<IActionResult> Export()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"ClassificationTypeData.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            var data = await _classificationTypeService.GetAll();

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("ClassificationType");

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("ClassificationTypeCode");
                row.CreateCell(1).SetCellValue("ClassificationTypeName");

                int no = 1;
                foreach (var item in data)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(item.TypeClassificationCode);
                    row.CreateCell(1).SetCellValue(item.TypeClassificationName);
                    no += 1;
                }
                workbook.Write(fs);
            }
            using (var stream = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", sFileName);
        }
        public async Task<ActionResult> Upload()
        {
            IFormFile file = Request.Form.Files[0];
            var result = Extensions.Global.ImportExcel(file, "Upload", _hostingEnvironment.WebRootPath);

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

            return RedirectToAction("Index", "ClassificationType", new { Area = "MasterData" });
        }
    }
}
