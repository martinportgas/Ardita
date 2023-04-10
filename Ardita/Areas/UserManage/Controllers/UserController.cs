using Ardita.Areas.UserManage.Models;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;
using System.Dynamic;
using dbModel = Ardita.Models.DbModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.UserManage.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("UserManage")]
    public class UserController : Controller
    {
        private IHostingEnvironment _hostingEnvironment;

        private readonly IUserService _userService;
        private readonly IEmployeeService _employeeService;
        private readonly IPositionService _positionService;

        public UserController(
            IHostingEnvironment hostingEnvironment,
            IUserService userService, 
            IEmployeeService employeeService,
            IPositionService positionService
           )
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
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

                var result = await _userService.GetListUsers(model);

                var jsonResult = new { 
                    draw = result.draw, 
                    recordsFiltered = result.recordsFiltered, 
                    recordsTotal = result.recordsTotal,
                    data = result.data 
                };
                return Json(jsonResult);
            } 
            catch(Exception ex)
            {
                throw;
            }
        }
        public async Task<IActionResult> Add()
        {
            InsertViewModels viewModels = new InsertViewModels();

            var Employees = await _employeeService.GetAll();
            var Users = new dbModel.MstUser();

            viewModels.Employees = Employees;
            viewModels.User = Users;

            return View(viewModels);
        }
        public async Task<IActionResult> Update(Guid Id)
        {
            InsertViewModels viewModels = new InsertViewModels();

            var Employees = await _employeeService.GetAll();
            var lisUser = await _userService.GetById(Id);
            if (lisUser.Count() > 0)
            {
                var Users = new MstUser();
                Users = lisUser.FirstOrDefault();

                viewModels.Employees = Employees;
                viewModels.User = Users;
                return View(viewModels);
            }
            else
            {
                return RedirectToAction("Index", "User", new { Area = "UserManage" });
            }
             
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            InsertViewModels viewModels = new InsertViewModels();

            var Employees = await _employeeService.GetAll();
            var lisUser = await _userService.GetById(Id);
            if (lisUser.Count() > 0)
            {
                var Users = new MstUser();
                Users = lisUser.FirstOrDefault();

                viewModels.Employees = Employees;
                viewModels.User = Users;
                return View(viewModels);
            }
            else
            {
                return RedirectToAction("Index", "User", new { Area = "UserManage" });
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(InsertViewModels model)
        {
            int result = 0;
            if (model.User != null) 
            {
                model.User.Password = Extensions.Global.Encode(model.User.Password);

                if (model.User.UserId != Guid.Empty)
                {
                    model.User.UpdateBy = new Guid(User.FindFirst("UserId").Value);
                    model.User.UpdateDate = DateTime.Now;
                    result = await _userService.Update(model.User);
                }
                else
                {
                    model.User.CreatedBy = new Guid(User.FindFirst("UserId").Value);
                    model.User.CreatedDate = DateTime.Now;
                    result = await _userService.Insert(model.User);
                }

            }
            return RedirectToAction("Index", "User", new { Area = "UserManage" });
        }
        public async Task<IActionResult> Delete(InsertViewModels model)
        {
            int result = 0;
            if (model.User != null)
            {
                if (model.User.UserId != Guid.Empty)
                {
                    model.User.Password = Extensions.Global.Encode("Default");
                    model.User.UpdateBy = new Guid(User.FindFirst("UserId").Value);
                    model.User.UpdateDate = DateTime.Now;

                    result = await _userService.Delete(model.User);
                }
            }
            return RedirectToAction("Index", "User", new { Area = "UserManage" });
        }
        public async Task<IActionResult> Upload() 
        {
            IFormFile file = Request.Form.Files[0];
            var result = Extensions.Global.ImportExcel(file, "Upload", _hostingEnvironment.WebRootPath);

            var employees = await _employeeService.GetAll();

            List<MstUser> users = new();
            MstUser user;

            foreach (DataRow row in result.Rows)
            {
                user = new();
                user.UserId = Guid.NewGuid();
                user.Username = row[0].ToString();
                user.EmployeeId = employees.Where(x => x.Nik == row[1].ToString()).FirstOrDefault().EmployeeId;
                user.Password = Extensions.Global.Encode("Default");
                user.IsActive = true;
                user.CreatedBy = new Guid(User.FindFirst("UserId").Value);
                user.CreatedDate = DateTime.Now;

                users.Add(user);
            }
            await _userService.InsertBulk(users);

            return RedirectToAction("Index", "User", new { Area = "UserManage" });
        }
        public async Task<IActionResult> DownloadTemplate()
        {
            string sWebRootFolder = _hostingEnvironment.WebRootPath;
            string sFileName = @"UserTemplate.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("User");
                ISheet excelSheetPosition = workbook.CreateSheet("Employee");

                IRow row = excelSheet.CreateRow(0);
                IRow rowPosition = excelSheetPosition.CreateRow(0);

                row.CreateCell(0).SetCellValue("Username");
                row.CreateCell(1).SetCellValue("NIK");
                row.CreateCell(2).SetCellValue("Name");

                row = excelSheet.CreateRow(1);
                row.CreateCell(0).SetCellValue("ExampleUserName");
                row.CreateCell(1).SetCellValue("ExampleNIK");
                row.CreateCell(2).SetCellValue("Name");

                rowPosition.CreateCell(0).SetCellValue("NIK");
                rowPosition.CreateCell(1).SetCellValue("Name");

                var dataEmployee = await _employeeService.GetAll();

                int no = 1;
                foreach (var item in dataEmployee)
                {
                    rowPosition = excelSheetPosition.CreateRow(no);

                    rowPosition.CreateCell(0).SetCellValue(item.Nik);
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
            string sFileName = @"UserData.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            var users = await _userService.GetAll();
            var employees = await _employeeService.GetAll();

            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            var memory = new MemoryStream();
            using (var fs = new FileStream(Path.Combine(sWebRootFolder, sFileName), FileMode.Create, FileAccess.Write))
            {
                IWorkbook workbook;
                workbook = new XSSFWorkbook();
                ISheet excelSheet = workbook.CreateSheet("User");

                IRow row = excelSheet.CreateRow(0);

                row.CreateCell(0).SetCellValue("Username");
                row.CreateCell(1).SetCellValue("NIK");
                row.CreateCell(2).SetCellValue("Name");

                int no = 1;
                foreach (var item in users)
                {
                    row = excelSheet.CreateRow(no);
                    row.CreateCell(0).SetCellValue(item.Username);

                    var employeeNIK = employees.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().Nik;
                    row.CreateCell(1).SetCellValue(employeeNIK);

                    var employeeName = employees.Where(x => x.EmployeeId == item.EmployeeId).FirstOrDefault().Name;
                    row.CreateCell(2).SetCellValue(employeeNIK);

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
    }
}
