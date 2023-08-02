using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Areas.UserManage.Models;
using Ardita.Models.DbModels;
using Ardita.Areas.UserManage.Models;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using System.Data;

using Ardita.Controllers;
using Ardita.Extensions;
using ICSharpCode.SharpZipLib.Tar;
using Newtonsoft.Json;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.UserManage)]
    public class EmployeeController : BaseController<MstEmployee>
    {
        public EmployeeController(
            IHostingEnvironment hostingEnvironment,
            IEmployeeService employeeService,
            IPositionService positionService,
            ICompanyService companyService
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _employeeService = employeeService;
            _positionService = positionService;
            _companyService = companyService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {

            try
            {
                var result = await _employeeService.GetListEmployee(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {
            ViewBag.listPositions = await BindPositions();
            ViewBag.listCompany = await BindCompanies();

            return View(GlobalConst.Form, new MstEmployee());
        }

        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _employeeService.GetById(Id);

            if (data != null)
            {
                ViewBag.listPositions = await BindPositions();
                ViewBag.listCompany = await BindCompanies();
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _employeeService.GetById(Id);

            if (data != null)
            {
                ViewBag.listPositions = await BindPositions();
                ViewBag.listCompany = await BindCompanies();
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _employeeService.GetById(Id);

            if (data != null)
            {
                ViewBag.listPositions = await BindPositions();
                ViewBag.listCompany = await BindCompanies();
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(MstEmployee model)
        {
            if (model != null)
            {

                if (model.EmployeeId != Guid.Empty)
                {
                    model.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdateDate = DateTime.Now;

                    await _employeeService.Update(model);
                }
                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    await _employeeService.Insert(model);
                }

            }
            return RedirectToIndex();
        }

        public override async Task<IActionResult> Delete(MstEmployee model)
        {
            if (model != null)
            {

                if (model.EmployeeId != Guid.Empty)
                {
                 
                    model.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdateDate = DateTime.Now;

                    await _employeeService.Delete(model);
                }


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
                string fileName = $"{GlobalConst.Template}-{nameof(MstEmployee).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstEmployee).ToCleanNameOf());
                ISheet excelSheetPosition = workbook.CreateSheet(nameof(MstPosition).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);
                IRow rowPosition = excelSheetPosition.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(MstEmployee.Nik));
                row.CreateCell(2).SetCellValue(nameof(MstEmployee.Name));
                row.CreateCell(3).SetCellValue(nameof(MstEmployee.Email));
                row.CreateCell(4).SetCellValue(nameof(MstEmployee.Gender));
                row.CreateCell(5).SetCellValue(nameof(MstEmployee.PlaceOfBirth));
                row.CreateCell(6).SetCellValue(nameof(MstEmployee.DateOfBirth));
                row.CreateCell(7).SetCellValue(nameof(MstEmployee.Address));
                row.CreateCell(8).SetCellValue(nameof(MstEmployee.Phone));
                row.CreateCell(9).SetCellValue(nameof(MstPosition.Code));



                rowPosition.CreateCell(0).SetCellValue(GlobalConst.No);
                rowPosition.CreateCell(1).SetCellValue(nameof(MstPosition.Code));
                rowPosition.CreateCell(2).SetCellValue(nameof(MstPosition.Name));

                var dataPosition = await _positionService.GetAll();

                int no = 1;
                foreach (var item in dataPosition)
                {
                    rowPosition = excelSheetPosition.CreateRow(no);

                    rowPosition.CreateCell(0).SetCellValue(no);
                    rowPosition.CreateCell(1).SetCellValue(item.Code);
                    rowPosition.CreateCell(2).SetCellValue(item.Name);
                    no += 1;
                }
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
        public async Task<IActionResult> Export()
        {
            try
            {
                string fileName = nameof(MstEmployee).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var data = await _employeeService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstEmployee).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(MstEmployee.Nik));
                row.CreateCell(2).SetCellValue(nameof(MstEmployee.Name));
                row.CreateCell(3).SetCellValue(nameof(MstEmployee.Email));
                row.CreateCell(4).SetCellValue(nameof(MstEmployee.Gender));
                row.CreateCell(5).SetCellValue(nameof(MstEmployee.PlaceOfBirth));
                row.CreateCell(6).SetCellValue(nameof(MstEmployee.DateOfBirth));
                row.CreateCell(7).SetCellValue(nameof(MstEmployee.Address));
                row.CreateCell(8).SetCellValue(nameof(MstEmployee.Phone));
                row.CreateCell(9).SetCellValue(nameof(MstPosition) + nameof(MstPosition.Name));

                int no = 1;
                foreach (var item in data)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(item.Nik);
                    row.CreateCell(2).SetCellValue(item.Name);
                    row.CreateCell(3).SetCellValue(item.Email);
                    row.CreateCell(4).SetCellValue(item.Gender);
                    row.CreateCell(5).SetCellValue(item.PlaceOfBirth);
                    row.CreateCell(6).SetCellValue(item.DateOfBirth.ToString());
                    row.CreateCell(7).SetCellValue(item.Address);
                    row.CreateCell(8).SetCellValue(item.Phone);
                    row.CreateCell(9).SetCellValue(item.Position.Name);

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
        public async Task<ActionResult> Upload()
        {
            try
            {
                IFormFile file = Request.Form.Files[0];

                if (file.Length > 0)
                {
                    var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, _hostingEnvironment.WebRootPath);
                    var positions = await _positionService.GetAll();
                    var employeeDetail = await _employeeService.GetAll();

                    if (result.Rows.Count > 0)
                    {
                        List<MstEmployee> employees = new();
                        MstEmployee employee;

                        bool valid = true;
                        int errorCount = 0;

                        result.Columns.Add("Keterangan");

                        foreach (DataRow row in result.Rows)
                        {
                            DateTime dateOfBirth = DateTime.Now;
                            string error = string.Empty;

                            var positionDetail = positions.FirstOrDefault(x => x.Code == row[9].ToString());

                            if (positionDetail == null)
                            {
                                valid = false;
                                error = "_Position Tidak Valid";
                            }
                            else if (!DateTime.TryParse(row[6].ToString(), out dateOfBirth))
                            {
                                valid = false;
                                error = "_Tanggal Lahir Tidak Valid";
                            }
                            else if (result.AsEnumerable().Where(x => x.Field<string>("Nik") == row[1].ToString()).Count() > 1)
                            {
                                valid = false;
                                error = "_Terdapat Nik Yang sama";
                            }
                            else if (employeeDetail.Where(x => x.Nik == row[1].ToString()).Count() > 0)
                            {
                                valid = false;
                                error = "_NIK sudah ada di database";
                            }

                            if (valid)
                            {
                                employee = new();
                                employee.EmployeeId = Guid.NewGuid();
                                employee.Nik = row[1].ToString();
                                employee.Name = row[2].ToString();
                                employee.Email = row[3].ToString();
                                employee.Gender = row[4].ToString();
                                employee.PlaceOfBirth = row[5].ToString();
                                employee.DateOfBirth = Convert.ToDateTime(row[6].ToString());
                                employee.Address = row[7].ToString();
                                employee.Phone = row[8].ToString();

                                employee.PositionId = positions.Where(x => x.Code.Contains(row[9].ToString())).FirstOrDefault().PositionId;
                                employee.IsActive = true;
                                employee.CreatedBy = AppUsers.CurrentUser(User).UserId;
                                employee.CreatedDate = DateTime.Now;

                                employees.Add(employee);
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
                            await _employeeService.InsertBulk(employees);
                    }

                    return View(GlobalConst.UploadForm);
                }
                else
                {
                    TempData["errorCount"] = 100000001;
                    return RedirectToAction(GlobalConst.UploadForm);
                }
            }
            catch (Exception)
            {
                TempData["errorCount"] = 100000001;
                return RedirectToAction(GlobalConst.UploadForm);
            }
        }

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.Employee, new { Area = GlobalConst.UserManage });
    }
}
