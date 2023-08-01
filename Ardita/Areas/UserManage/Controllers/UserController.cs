using Ardita.Controllers;
using Ardita.Extensions;

using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.UserManage)]
    public class UserController : BaseController<MstUser>
    {
        public UserController(
            IUserService userService, 
            IEmployeeService employeeService,
            IArchiveUnitService archiveUnitService
           )
        {
            _userService = userService;
            _employeeService = employeeService;
            _archiveUnitService = archiveUnitService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        [HttpPost]
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {

            try
            {
                var result = await _userService.GetListUsers(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<JsonResult> CheckUsername(string id)
        {
            try
            {
                var result = await _userService.GetAll();
                bool duplicate = false;

                if (result.Where(x => x.Username == id).Count() > 0) 
                {
                    duplicate = true;
                }
                return Json(duplicate);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Add()
        {

            ViewBag.listEmployees = await BindEmployee();
            ViewBag.listArchiveUnit = await BindAllArchiveUnits();
            ViewBag.subDetail = new List<IdxUserArchiveUnit>();

            return View(GlobalConst.Form, new MstUser());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _userService.GetById(Id);

            if (data != null)
            {
                ViewBag.listEmployees = await BindEmployee();
                ViewBag.listArchiveUnit = await BindAllArchiveUnits();
                ViewBag.subDetail = await _userService.GetIdxUserArchiveUnitByUserId(Id);
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Delete(MstUser model) 
        {
            if (model != null)
            {
                if (model.UserId != Guid.Empty)
                {
                    model.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdateDate = DateTime.Now;

                    await _userService.Delete(model);
                }
            }
            return RedirectToIndex();
        }
        public override async Task<IActionResult> Save(MstUser model)
        {
            if (model != null)
            {
                model.Password = Global.Encode(model.Password);
                //string[] archiveUnitIds = Request.Form["archiveUnitIds[]"].ToArray();
                string[] archiveUnitIds = new string[] { };

                if (model.UserId != Guid.Empty)
                {
                    model.UpdateBy = AppUsers.CurrentUser(User).UserId;
                    model.UpdateDate = DateTime.Now;

                    await _userService.Update(model, archiveUnitIds);
                }
                else
                {
                    model.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    model.CreatedDate = DateTime.Now;
                    model.UserId = Guid.NewGuid();
                    await _userService.Insert(model, archiveUnitIds);
                }

            }
            return RedirectToIndex();
        }
        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _userService.GetById(Id);

            if (data != null)
            {
                ViewBag.listEmployees = await BindEmployee();
                ViewBag.listArchiveUnit = await BindAllArchiveUnits();
                ViewBag.subDetail = await _userService.GetIdxUserArchiveUnitByUserId(Id);
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _userService.GetById(Id);

            if (data != null)
            {
                ViewBag.listEmployees = await BindEmployee();
                ViewBag.listArchiveUnit = await BindAllArchiveUnits();
                ViewBag.subDetail = await _userService.GetIdxUserArchiveUnitByUserId(Id);
                return View(GlobalConst.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
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
                string fileName = $"{GlobalConst.Template}-{nameof(MstUser).ToCleanNameOf()}";
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstUser).ToCleanNameOf());
                ISheet excelSheetEmployee = workbook.CreateSheet(nameof(MstEmployee).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);
                IRow rowEmployee = excelSheetEmployee.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(MstUser.Username));
                row.CreateCell(2).SetCellValue(nameof(MstEmployee.Nik));


                //Employee
                rowEmployee.CreateCell(0).SetCellValue(GlobalConst.No);
                rowEmployee.CreateCell(1).SetCellValue(nameof(MstEmployee.Nik));
                rowEmployee.CreateCell(2).SetCellValue(nameof(MstEmployee.Name));
                rowEmployee.CreateCell(3).SetCellValue(nameof(MstEmployee.Email));
                rowEmployee.CreateCell(4).SetCellValue(nameof(MstEmployee.Gender));
                rowEmployee.CreateCell(5).SetCellValue(nameof(MstEmployee.PlaceOfBirth));
                rowEmployee.CreateCell(6).SetCellValue(nameof(MstEmployee.DateOfBirth));
                rowEmployee.CreateCell(7).SetCellValue(nameof(MstEmployee.Address));
                rowEmployee.CreateCell(8).SetCellValue(nameof(MstEmployee.Phone));
                rowEmployee.CreateCell(9).SetCellValue(nameof(MstPosition).ToCleanNameOf() + " " + nameof(MstPosition.Name));

                var dataEmployees = await _employeeService.GetAll();

                int no = 1;
                foreach (var item in dataEmployees)
                {
                    rowEmployee = excelSheetEmployee.CreateRow(no);

                    rowEmployee.CreateCell(0).SetCellValue(no);
                    rowEmployee.CreateCell(1).SetCellValue(item.Nik);
                    rowEmployee.CreateCell(2).SetCellValue(item.Name);
                    rowEmployee.CreateCell(3).SetCellValue(item.Email);
                    rowEmployee.CreateCell(4).SetCellValue(item.Gender);
                    rowEmployee.CreateCell(5).SetCellValue(item.PlaceOfBirth);
                    rowEmployee.CreateCell(6).SetCellValue(item.DateOfBirth.ToString());
                    rowEmployee.CreateCell(7).SetCellValue(item.Address);
                    rowEmployee.CreateCell(8).SetCellValue(item.Phone);
                    rowEmployee.CreateCell(9).SetCellValue(item.Position.Name);
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
                string fileName = nameof(MstUser).ToCleanNameOf();
                fileName = fileName.ToFileNameDateTimeStringNow(fileName);

                var data = await _userService.GetAll();

                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet(nameof(MstUser).ToCleanNameOf());

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue(GlobalConst.No);
                row.CreateCell(1).SetCellValue(nameof(MstUser.Username));
                row.CreateCell(2).SetCellValue(nameof(MstEmployee.Nik));
                row.CreateCell(3).SetCellValue(nameof(MstEmployee.Name));
                row.CreateCell(4).SetCellValue(nameof(MstEmployee.Email));
                row.CreateCell(5).SetCellValue(nameof(MstEmployee.Gender));
                row.CreateCell(6).SetCellValue(nameof(MstEmployee.PlaceOfBirth));
                row.CreateCell(7).SetCellValue(nameof(MstEmployee.DateOfBirth));
                row.CreateCell(8).SetCellValue(nameof(MstEmployee.Address));
                row.CreateCell(9).SetCellValue(nameof(MstEmployee.Phone));
                row.CreateCell(10).SetCellValue(nameof(MstPosition) + nameof(MstPosition.Name));

                int no = 1;
                foreach (var item in data)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(no);
                    row.CreateCell(1).SetCellValue(item.Username);
                    row.CreateCell(2).SetCellValue(item.Employee.Nik);
                    row.CreateCell(3).SetCellValue(item.Employee.Name);
                    row.CreateCell(4).SetCellValue(item.Employee.Email);
                    row.CreateCell(5).SetCellValue(item.Employee.Gender);
                    row.CreateCell(6).SetCellValue(item.Employee.PlaceOfBirth);
                    row.CreateCell(7).SetCellValue(item.Employee.DateOfBirth.ToString());
                    row.CreateCell(8).SetCellValue(item.Employee.Address);
                    row.CreateCell(9).SetCellValue(item.Employee.Phone);
                    row.CreateCell(10).SetCellValue(item.Employee.Position.Name);

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
                var result = Extensions.Global.ImportExcel(file, GlobalConst.Upload, string.Empty);
                var employees = await _employeeService.GetAll();

                List<MstUser> users = new();
                MstUser user;

                foreach (DataRow row in result.Rows)
                {
                    user = new();
                    user.UserId = Guid.NewGuid();
                    user.Username = row[1].ToString();
                    user.Password = Global.Encode(GlobalConst.Password);
                    user.EmployeeId = employees.Where(x => x.Nik.Contains(row[2].ToString())).FirstOrDefault().EmployeeId;

                    user.IsActive = true;
                    user.CreatedBy = AppUsers.CurrentUser(User).UserId;
                    user.CreatedDate = DateTime.Now;

                    users.Add(user);
                }
                await _userService.InsertBulk(users);
                return RedirectToIndex();
            }
            catch (Exception)
            {

                throw new Exception();
            }
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.User, new { Area = GlobalConst.UserManage });
    }
}
