using Ardita.Areas.UserManage.Models;
using Ardita.Controllers;
using Ardita.Globals;
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
    [Area(Const.UserManage)]
    public class UserController : BaseController<MstUser>
    {

        public UserController(
            IHostingEnvironment hostingEnvironment,
            IUserService userService, 
            IEmployeeService employeeService
           )
        {
            _hostingEnvironment = hostingEnvironment;
            _userService = userService;
            _employeeService = employeeService;
        }
        public override async Task<ActionResult> Index() => await base.Index();

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
        public override async Task<IActionResult> Add()
        {
            ViewBag.listEmployees = await BindEmployee();

            return View(Const.Form, new MstUser());
        }
        public override async Task<IActionResult> Update(Guid Id)
        {
            var data = await _userService.GetById(Id);

            if (data != null)
            {
                ViewBag.listEmployees = await BindEmployee();
                return View(Const.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        public override async Task<IActionResult> Remove(Guid Id)
        {
            var data = await _userService.GetById(Id);

            if (data != null)
            {
                ViewBag.listEmployees = await BindEmployee();
                return View(Const.Form, data);
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
                return View(Const.Form, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }
        private RedirectToActionResult RedirectToIndex() => RedirectToAction(Const.Index, Const.User, new { Area = Const.UserManage });
    }
}
