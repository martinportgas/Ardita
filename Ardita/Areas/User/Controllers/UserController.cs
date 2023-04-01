using Ardita.Areas.User.Models;
using Ardita.Models.DbModels;
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

        [HttpGet]
        public async Task<JsonResult> GetData()
        {
            var userResult = await _userService.GetAll();
            var usersList = userResult.ToList();

            var employeeResult = await _employeeService.GetAll();
            var employeeList = employeeResult.ToList();

            var positionResult = await _positionService.GetAll();
            var positionList = positionResult.ToList();


            var data = (from user in userResult
                        join employee in employeeList on user.EmployeeId equals employee.EmployeeId
                        join position in positionList on employee.PositionId equals position.PosittionId
                        select new
                        {
                            UserId = user.UserId,
                            UserName = user.Username,
                            EmployeeName = employee.Name,
                            EmployeePosition = position.Name,
                            IsActive = user.IsActive
                        }).ToList();

            var JsonResults = JsonConvert.SerializeObject(data);

            return Json(new { data = data });
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
