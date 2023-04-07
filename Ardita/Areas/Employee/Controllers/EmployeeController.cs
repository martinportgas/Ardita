using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Areas.Employee.Models;
using Ardita.Models.DbModels;
using Ardita.Areas.User.Models;

namespace Ardita.Areas.Employee.Controllers
{
    [Authorize]
    [Area("Employee")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IPositionService _positionService;
        public EmployeeController(
            IEmployeeService employeeService,
            IPositionService positionService
            )
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(EmployeeInserViewModel model)
        {
            int result = 0;
            if (model.Employee != null)
            {

                if (model.Employee.EmployeeId != Guid.Empty)
                    result = await _employeeService.Update(model.Employee);
                else
                    result = await _employeeService.Insert(model.Employee);

            }
            return RedirectToAction("Index", "Employee", new { Area = "Employee" });
        }
    }
}
