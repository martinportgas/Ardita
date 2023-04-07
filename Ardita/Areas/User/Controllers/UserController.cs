using Ardita.Areas.User.Models;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Dynamic;
using dbModel = Ardita.Models.DbModels;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Ardita.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
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
                var Users = new dbModel.MstUser();
                Users.UserId = lisUser.FirstOrDefault().UserId;
                Users.EmployeeId = lisUser.FirstOrDefault().EmployeeId;
                Users.Username = lisUser.FirstOrDefault().Username;
                Users.Password = lisUser.FirstOrDefault().Password;

                viewModels.Employees = Employees;
                viewModels.User = Users;
                return View(viewModels);
            }
            else
            {
                return RedirectToAction("Index", "User", new { Area = "User" });
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
                    result = await _userService.Update(model.User);
                else
                    result = await _userService.Insert(model.User);

            }
            return RedirectToAction("Index", "User", new { Area = "user"});
        }
        public async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _userService.GetById(Id);
            var objUser = new MstUser();
            objUser.UserId = data.FirstOrDefault().UserId;

            await _userService.Delete(objUser);

            return RedirectToAction("Index", "User", new { Area = "user" });
        }
        public ActionResult Upload() 
        {
            IFormFile file = Request.Form.Files[0];
            var result = Extensions.Global.ImportExcel(file, "Upload", _hostingEnvironment.WebRootPath);

            MstUser model;
            

            foreach (DataRow row in result.Rows)
            {
                model = new MstUser();
                model.Username = row["Username"].ToString();
                model.Password = Extensions.Global.Encode(row["Password"].ToString());

                Guid EmplGuid = Guid.Parse(row["EmployeeId"].ToString().ToLower());
                model.EmployeeId = EmplGuid;
                model.IsActive = Convert.ToBoolean(row["IsActive"]);
                _userService.Upload(model);
            }

            return RedirectToAction("Index", "User", new { Area = "User" });
        }
    }
}
