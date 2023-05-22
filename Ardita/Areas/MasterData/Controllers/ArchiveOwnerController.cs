using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorize]
    [Area(GlobalConst.MasterData)]
    public class ArchiveOwnerController : BaseController<MstArchiveOwner>
    {
        public override async Task<ActionResult> Index() => await base.Index();
        public ArchiveOwnerController(IArchiveOwnerService archiveOwnerService)
        {
            _archiveOwnerService = archiveOwnerService;
        }

        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _archiveOwnerService.GetList(model);

                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public override async Task<IActionResult> Add()
        {
            return View(GlobalConst.Form, new MstArchiveOwner());
        }

        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _archiveOwnerService.GetById(Id);
            if (data.Any())
            {
                return View(GlobalConst.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _archiveOwnerService.GetById(Id);
            if (data.Any())
            {
                return View(GlobalConst.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _archiveOwnerService.GetById(Id);
            if (data.Any())
            {
                return View(GlobalConst.Form, data.FirstOrDefault());
            }
            else
            {
                return RedirectToIndex();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Save(MstArchiveOwner model)
        {
            if (model != null)
            {
                if (model.ArchiveOwnerId != Guid.Empty)
                {
                    model.UpdatedBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdatedDate = DateTime.Now;
                    await _archiveOwnerService.Update(model);
                }

                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _archiveOwnerService.Insert(model);
                }
            }
            return RedirectToIndex();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public override async Task<IActionResult> Delete(MstArchiveOwner model)
        {
            if (model != null && model.ArchiveOwnerId != Guid.Empty)
            {
                await _archiveOwnerService.Delete(model);
            }
            return RedirectToIndex();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];

                var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, string.Empty);
                var archiveOwner = await _archiveOwnerService.GetAll();


                List<MstArchiveOwner> mstArchiveOwners = new();
                MstArchiveOwner mstArchiveOwner;

                foreach (DataRow row in result.Rows)
                {
                    mstArchiveOwner = new();
                    mstArchiveOwner.ArchiveOwnerId = Guid.NewGuid();

                    mstArchiveOwner.ArchiveOwnerCode = row[1].ToString();
                    mstArchiveOwner.ArchiveOwnerName = row[2].ToString();

                    mstArchiveOwner.IsActive = true;
                    mstArchiveOwner.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    mstArchiveOwner.CreatedDate = DateTime.Now;

                    mstArchiveOwners.Add(mstArchiveOwner);
                }
                await _archiveOwnerService.InsertBulk(mstArchiveOwners);
                return RedirectToIndex();
            }
            catch (Exception)
            {

                throw new Exception();
            }

        }
        public async Task<IActionResult> Export()
        {
            try
            {
                string fileName = nameof(MstArchiveOwner).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var archiveOwners = await _archiveOwnerService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstArchiveOwner).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(MstArchiveOwner.ArchiveOwnerCode));
                row.CreateCell(2).SetCellValue(nameof(MstArchiveOwner.ArchiveOwnerName));

                int no = 1;
                foreach (var item in archiveOwners)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(item.ArchiveOwnerCode);
                    row.CreateCell(2).SetCellValue(item.ArchiveOwnerName);
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
        public async Task<IActionResult> DownloadTemplate()
        {
            try
            {
                string fileName = $"{GlobalConst.Template}-{nameof(MstArchiveOwner).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstArchiveOwner).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                //Archive Owner
                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(MstArchiveOwner.ArchiveOwnerCode));
                row.CreateCell(2).SetCellValue(nameof(MstArchiveOwner.ArchiveOwnerName));

                using (var exportData = new MemoryStream())
                {
                    workbook.Write(exportData);
                    byte[] bytes = exportData.ToArray();
                    return File(bytes, GlobalConst.EXCEL_FORMAT_TYPE, $"{fileName}.xlsx");
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.ArchiveOwner, new { Area = GlobalConst.MasterData });

    }
}
