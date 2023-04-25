using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Globals;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorizeAttribute]
[Area("MasterData")]
public class ClassificationController : BaseController<TrxClassification>
{
    private IHostingEnvironment _hostingEnvironment;
    public ClassificationController(IHostingEnvironment hostingEnvironment, IClassificationService classificationService, IClassificationTypeService classificationTypeService)
    {
        _classificationService = classificationService;
        _classificationTypeService = classificationTypeService;
        _hostingEnvironment = hostingEnvironment;
    }
    // GET: ClassificationController
    public override async Task<ActionResult> Index() => await base.Index();

    public override async Task<JsonResult> GetData(DataTablePostModel model)
    {
        try
        {
            var result = await _classificationService.GetListClassification(model);

            return Json(result);

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public override async Task<IActionResult> Add()
    {
        var classificationTypeData = await _classificationTypeService.GetAll();

        ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
        return View(Const.Form, new TrxClassification());
    }
    public override async Task<IActionResult> Update(Guid Id)
    {
        var data = await _classificationService.GetById(Id);
        var model = data.Where(x => x.ClassificationId == Id).FirstOrDefault();
        if (model != null)
        {
            var classificationTypeData = await _classificationTypeService.GetAll();

            ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            return View(Const.Form, model);
        }
        else
        {
            return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
        }
    }
    public override async Task<IActionResult> Remove(Guid Id)
    {
        var data = await _classificationService.GetById(Id);
        var model = data.Where(x => x.ClassificationId == Id).FirstOrDefault();
        if (model != null)
        {
            var classificationTypeData = await _classificationTypeService.GetAll();

            ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            return View(Const.Form, model);
        }
        else
        {
            return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
        }

    }
    public override async Task<IActionResult> Detail(Guid Id)
    {
        var data = await _classificationService.GetById(Id);
        var model = data.Where(x => x.ClassificationId == Id).FirstOrDefault();
        if (model != null)
        {
            var classificationTypeData = await _classificationTypeService.GetAll();

            ViewBag.listClassificationType = new SelectList(classificationTypeData, "TypeClassificationId", "TypeClassificationName");
            return View(Const.Form, model);
        }
        else
        {
            return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
        }
    }

    // POST: ClassificationController/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Save(TrxClassification model)
    {
        int result = 0;
        if (model != null)
        {
            if (model.ClassificationId != Guid.Empty)
            {
                model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                model.UpdatedDate = DateTime.Now;
                result = await _classificationService.Update(model);
            }

            else
            {
                model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                model.CreatedDate = DateTime.Now;
                result = await _classificationService.Insert(model);
            }
        }
        return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
    }

    // POST: ClassificationController/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public override async Task<IActionResult> Delete(TrxClassification model)
    {
        int result = 0;
        if (model != null && model.ClassificationId != Guid.Empty)
        {
            model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
            result = await _classificationService.Delete(model);
        }
        return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
    }
    public async Task<IActionResult> DownloadTemplate()
    {
        string sWebRootFolder = _hostingEnvironment.WebRootPath;
        string sFileName = @"ClassificationTemplate.xlsx";
        string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
        FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
        var memory = new MemoryStream();
        using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
        {
            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet("Classification");
            ISheet excelSheetPosition = workbook.CreateSheet("ClassificationType");

            IRow row = excelSheet.CreateRow(0);
            IRow rowPosition = excelSheetPosition.CreateRow(0);

            row.CreateCell(0).SetCellValue("ClassificationCode");
            row.CreateCell(1).SetCellValue("ClassificationName");
            row.CreateCell(2).SetCellValue("TypeClassificationCode");

            row = excelSheet.CreateRow(1);
            row.CreateCell(0).SetCellValue("Sample Code");
            row.CreateCell(1).SetCellValue("Sample Name");
            row.CreateCell(2).SetCellValue("Sample Classification Type Code");

            rowPosition.CreateCell(0).SetCellValue("Code");
            rowPosition.CreateCell(1).SetCellValue("Name");

            var dataclassificationType = await _classificationTypeService.GetAll();

            int no = 1;
            foreach (var item in dataclassificationType)
            {
                rowPosition = excelSheetPosition.CreateRow(no);

                rowPosition.CreateCell(0).SetCellValue(item.TypeClassificationCode);
                rowPosition.CreateCell(1).SetCellValue(item.TypeClassificationName);
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
    public async Task<IActionResult> Export()
    {
        string sWebRootFolder = _hostingEnvironment.WebRootPath;
        string sFileName = @"ClassificationData.xlsx";
        string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
        var data = await _classificationService.GetAll();
        var type = await _classificationTypeService.GetAll();

        FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
        var memory = new MemoryStream();
        using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
        {
            IWorkbook workbook;
            workbook = new XSSFWorkbook();
            ISheet excelSheet = workbook.CreateSheet("Classification");

            IRow row = excelSheet.CreateRow(0);

            row.CreateCell(0).SetCellValue("ClassificationCode");
            row.CreateCell(1).SetCellValue("ClassificationName");
            row.CreateCell(2).SetCellValue("TypeClassificationCode");
            row.CreateCell(3).SetCellValue("TypeClassificationName");

            int no = 1;
            foreach (var item in data)
            {
                row = excelSheet.CreateRow(no);
                row.CreateCell(0).SetCellValue(item.ClassificationCode);
                row.CreateCell(1).SetCellValue(item.ClassificationName);
                var typeData = type.Where(x => x.TypeClassificationId == item.TypeClassificationId).FirstOrDefault();
                row.CreateCell(2).SetCellValue(typeData.TypeClassificationCode);
                row.CreateCell(3).SetCellValue(typeData.TypeClassificationName);
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

        List<TrxClassification> models = new();
        TrxClassification model;

        foreach (DataRow row in result.Rows)
        {
            model = new();
            model.ClassificationId = Guid.NewGuid();
            model.ClassificationCode = row[0].ToString();
            model.ClassificationName = row[1].ToString();
            model.TypeClassificationId = type.Where(x => x.TypeClassificationCode == row[2].ToString()).FirstOrDefault().TypeClassificationId;
            model.IsActive = true;
            model.CreatedBy = AppUsers.CurrentUser(User).UserId;
            model.CreatedDate = DateTime.Now;

            models.Add(model);
        }
        await _classificationService.InsertBulk(models);

        return RedirectToAction("Index", "Classification", new { Area = "MasterData" });
    }
}
