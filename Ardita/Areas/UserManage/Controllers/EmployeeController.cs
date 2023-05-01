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
using Ardita.Globals;
using Ardita.Controllers;
using Ardita.Extensions;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(Const.UserManage)]
    public class EmployeeController : BaseController<MstEmployee>
    {
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

            return View(Const.Form, new MstEmployee());
        }

        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _employeeService.GetById(Id);

            if (data != null)
            {
                ViewBag.listPositions = await BindPositions();
                return View(Const.Form, data);
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
                return View(Const.Form, data);
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
                return View(Const.Form, data);
            }
            else
            {
                return RedirectToIndex();
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
                    model.Employee.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.Employee.UpdateDate = DateTime.Now;

                    result = await _employeeService.Update(model.Employee);
                }
                else
                {
                    model.Employee.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.Employee.CreatedDate = DateTime.Now;
                    result = await _employeeService.Insert(model.Employee);
                }

            }
            return RedirectToIndex();
        }

        public async Task<IActionResult> Delete(EmployeeInserViewModel model)
        {
            int result = 0;
            if (model.Employee != null)
            {

                if (model.Employee.EmployeeId != Guid.Empty)
                {
                 
                    model.Employee.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.Employee.UpdateDate = DateTime.Now;

                    result = await _employeeService.Delete(model.Employee);
                }


            }
            return RedirectToIndex();
        }

        public async Task DownloadTemplate()
        {
            try
            {
                string fileName = $"{Const.Template}-{nameof(MstEmployee).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstEmployee).ToCleanNameOf());
                ISheet excelSheetPosition = workbook.CreateSheet(nameof(MstPosition).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);
                IRow rowPosition = excelSheetPosition.CreateRow(0);

                row.CreateCell(0).SetCellValue(Const.No);
                row.CreateCell(1).SetCellValue(nameof(MstEmployee.Nik));
                row.CreateCell(2).SetCellValue(nameof(MstEmployee.Name));
                row.CreateCell(3).SetCellValue(nameof(MstEmployee.Email));
                row.CreateCell(4).SetCellValue(nameof(MstEmployee.Gender));
                row.CreateCell(5).SetCellValue(nameof(MstEmployee.PlaceOfBirth));
                row.CreateCell(6).SetCellValue(nameof(MstEmployee.DateOfBirth));
                row.CreateCell(7).SetCellValue(nameof(MstEmployee.Address));
                row.CreateCell(8).SetCellValue(nameof(MstEmployee.Phone));
                row.CreateCell(9).SetCellValue(nameof(MstPosition.Code));



                rowPosition.CreateCell(0).SetCellValue(Const.No);
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
                workbook.WriteExcelToResponse(HttpContext, fileName);
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }
        public async Task Export()
        {
            try
            {
                string fileName = nameof(MstEmployee).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var data = await _employeeService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(TrxLevel).Replace(Const.Trx, string.Empty));

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(Const.No);
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
                workbook.WriteExcelToResponse(HttpContext, fileName);
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
                var result = Extensions.Global.ImportExcel(file, Const.Upload, _hostingEnvironment.WebRootPath);
                var positions = await _positionService.GetAll();

                List<MstEmployee> employees = new();
                MstEmployee employee;

                foreach (DataRow row in result.Rows)
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
                await _employeeService.InsertBulk(employees);
                return RedirectToIndex();
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.Employee, new { Area = Const.UserManage });
    }
}
