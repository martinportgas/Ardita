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

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("UserManage")]
    public class EmployeeController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        private readonly IEmployeeService _employeeService;
        private readonly IPositionService _positionService;
        public EmployeeController(
            IHostingEnvironment hostingEnvironment,
            IEmployeeService employeeService,
            IPositionService positionService
            )
        {
            _hostingEnvironment = hostingEnvironment;
            _employeeService = employeeService;
            _positionService = positionService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<JsonResult> GetData()
        {
            try
            {
                var model = new DataTableModel();

                model.draw = Request.Form["draw"].FirstOrDefault();
                model.start = Request.Form["start"].FirstOrDefault();
                model.length = Request.Form["length"].FirstOrDefault();
                model.sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
                model.sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
                model.searchValue = Request.Form["search[value]"].FirstOrDefault();
                model.pageSize = model.length != null ? Convert.ToInt32(model.length) : 0;
                model.skip = model.start != null ? Convert.ToInt32(model.start) : 0;
                model.recordsTotal = 0;

                var result = await _employeeService.GetListEmployee(model);

                var jsonResult = new
                {
                    draw = result.draw,
                    recordsFiltered = result.recordsFiltered,
                    recordsTotal = result.recordsTotal,
                    data = result.data
                };
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> Add()
        {
            EmployeeInserViewModel viewModels = new EmployeeInserViewModel();

            var Positions = await _positionService.GetAll();
            var Employee = new MstEmployee();

            viewModels.Positions = Positions;
            viewModels.Employee = Employee;

            return View(viewModels);
        }

        public async Task<IActionResult> Update(Guid Id)
        {
            EmployeeInserViewModel viewModels = new EmployeeInserViewModel();

            var Positions = await _positionService.GetAll();
            var dataEmployee = await _employeeService.GetById(Id);
            var Employee = new MstEmployee();

            if (dataEmployee.Count() > 0)
            {
                Employee = dataEmployee.FirstOrDefault();

                viewModels.Positions = Positions;
                viewModels.Employee = Employee;

                return View(viewModels);
            }
            else
            {
                return RedirectToAction("Index", "Employee", new { Area = "Employee" });
            }
        }

        public async Task<IActionResult> Remove(Guid Id)
        {
            EmployeeInserViewModel viewModels = new EmployeeInserViewModel();

            var Positions = await _positionService.GetAll();
            var dataEmployee = await _employeeService.GetById(Id);
            var Employee = new MstEmployee();

            if (dataEmployee.Count() > 0)
            {
                Employee = dataEmployee.FirstOrDefault();

                viewModels.Positions = Positions;
                viewModels.Employee = Employee;

                return View(viewModels);
            }
            else
            {
                return RedirectToAction("Index", "Employee", new { Area = "Employee" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(EmployeeInserViewModel model)
        {
            int result = 0;
            if (model.Employee != null)
            {

                if (model.Employee.EmployeeId != Guid.Empty)
                {
                    model.Employee.UpdateBy = new Guid(User.FindFirst("UserId").Value);
                    model.Employee.UpdateDate = DateTime.Now;

                    result = await _employeeService.Update(model.Employee);
                }
                else
                {
                    model.Employee.CreatedBy = new Guid(User.FindFirst("UserId").Value);
                    model.Employee.CreatedDate = DateTime.Now;
                    result = await _employeeService.Insert(model.Employee);
                }

            }
            return RedirectToAction("Index", "Employee", new { Area = "Employee" });
        }

        public async Task<IActionResult> Delete(EmployeeInserViewModel model)
        {
            int result = 0;
            if (model.Employee != null)
            {

                if (model.Employee.EmployeeId != Guid.Empty)
                {
                 
                    model.Employee.UpdateBy = new Guid(User.FindFirst("UserId").Value);
                    model.Employee.UpdateDate = DateTime.Now;

                    result = await _employeeService.Delete(model.Employee);
                }


            }
            return RedirectToAction("Index", "Employee", new { Area = "Employee" });
        }

        public async Task<IActionResult> DownloadTemplate()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"EmployeeTemplate.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Employee");
                ISheet excelSheetPosition = workbook.CreateSheet("Position");

                IRow row = excelSheet.CreateRow(0);
                IRow rowPosition = excelSheetPosition.CreateRow(0);

                row.CreateCell(0).SetCellValue("NIK");
                row.CreateCell(1).SetCellValue("Name");
                row.CreateCell(2).SetCellValue("Email");
                row.CreateCell(3).SetCellValue("Gender");
                row.CreateCell(4).SetCellValue("PlaceOfBirth");
                row.CreateCell(5).SetCellValue("DateOfBirth");
                row.CreateCell(6).SetCellValue("Address");
                row.CreateCell(7).SetCellValue("Phone");
                row.CreateCell(8).SetCellValue("PositionCode");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("202304090001");
                row.CreateCell(1).SetCellValue("Sample Name");
                row.CreateCell(2).SetCellValue("Sample@Mail.com");
                row.CreateCell(3).SetCellValue("Male");
                row.CreateCell(4).SetCellValue("Jakarta");
                row.CreateCell(5).SetCellValue("1980-01-01");
                row.CreateCell(6).SetCellValue("Jl. Pangeran Antasari");
                row.CreateCell(7).SetCellValue("82175521432");
                row.CreateCell(8).SetCellValue("IT-0006");

                rowPosition.CreateCell(0).SetCellValue("Code");
                rowPosition.CreateCell(1).SetCellValue("Name");

                var dataPosition = await _positionService.GetAll();

                int no = 1;
                foreach (var item in dataPosition)
                {
                    rowPosition = excelSheetPosition.CreateRow(no);

                    rowPosition.CreateCell(0).SetCellValue(item.Code);
                    rowPosition.CreateCell(1).SetCellValue(item.Name);
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
            string sFileName = @"EmployeeData.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            var data = await _employeeService.GetAll();
            var positions = await _positionService.GetAll();

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("Employee");

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("NIK");
                row.CreateCell(1).SetCellValue("Name");
                row.CreateCell(2).SetCellValue("Email");
                row.CreateCell(3).SetCellValue("Gender");
                row.CreateCell(4).SetCellValue("PlaceOfBirth");
                row.CreateCell(5).SetCellValue("DateOfBirth");
                row.CreateCell(6).SetCellValue("Address");
                row.CreateCell(7).SetCellValue("Phone");
                row.CreateCell(8).SetCellValue("Position");

                int no = 1;
                foreach (var item in data) 
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(item.Nik);
                    row.CreateCell(1).SetCellValue(item.Name);
                    row.CreateCell(2).SetCellValue(item.Email);
                    row.CreateCell(3).SetCellValue(item.Gender);
                    row.CreateCell(4).SetCellValue(item.PlaceOfBirth);

                    DateTime? dtDob = item.DateOfBirth;
                    row.CreateCell(5).SetCellValue(dtDob?.ToString("dd/MM/yyyy"));
                    row.CreateCell(6).SetCellValue(item.Address);
                    row.CreateCell(7).SetCellValue(item.Phone);

                    var positionCode = positions.Where(x => x.PosittionId == item.PositionId).FirstOrDefault().Name;
                    row.CreateCell(8).SetCellValue(positionCode);
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

            var positions = await _positionService.GetAll();

            List<MstEmployee> employees = new();
            MstEmployee employee;

            foreach (DataRow row in result.Rows)
            {
                employee = new();
                employee.EmployeeId = Guid.NewGuid();
                employee.Nik = row[0].ToString();
                employee.Name = row[1].ToString();
                employee.Email = row[2].ToString();
                employee.Gender = row[3].ToString();
                employee.PlaceOfBirth = row[4].ToString();
                employee.DateOfBirth = Convert.ToDateTime(row[5].ToString());
                employee.Address = row[6].ToString();
                employee.Phone = row[7].ToString();

                employee.PositionId = positions.Where(x => x.Code == row[8].ToString()).FirstOrDefault().PosittionId;

                employee.IsActive = true;
                employee.CreatedBy = new Guid(User.FindFirst("UserId").Value);
                employee.CreatedDate = DateTime.Now;

                employees.Add(employee);
            }
            await _employeeService.InsertBulk(employees);

            return RedirectToAction("Index", "Employee", new { Area = "Employee" });
        }
    }
}
