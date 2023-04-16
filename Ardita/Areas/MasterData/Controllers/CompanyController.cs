using Ardita.Areas.UserManage.Models;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Classess;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers;

[CustomAuthorize]
[Area("MasterData")]
public class CompanyController : Controller
{
    private IWebHostEnvironment _webHostEnvironment;
    private readonly ICompanyService? _companyService;

    public CompanyController(IWebHostEnvironment webHostEnvironment, ICompanyService companyService)
    {
        _webHostEnvironment = webHostEnvironment;
        _companyService = companyService;
    }

    #region VIEW
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<JsonResult> GetData()
    {
        try
        {
            var model = new DataTableModel
            {
                draw = Request.Form["draw"].FirstOrDefault(),
                start = Request.Form["start"].FirstOrDefault(),
                length = Request.Form["length"].FirstOrDefault(),
                sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault(),
                sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault(),
                searchValue = Request.Form["search[value]"].FirstOrDefault()
            };

            model.pageSize = model.length != null ? Convert.ToInt32(model.length) : 0;
            model.skip = model.start != null ? Convert.ToInt32(model.start) : 0;
            model.recordsTotal = 0;

            var result = await _companyService.GetListCompanies(model);

            var jsonResult = new
            {
                draw = result.Draw,
                recordsFiltered = result.RecordsFiltered,
                recordsTotal = result.RecordsTotal,
                data = result.Data
            };

            return Json(jsonResult);

        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    #region CREATE
    public async Task<IActionResult> Add()
    {
        var Company = new MstCompany();
        await Task.Delay(0);

        return View(Company);
    }
    #endregion
}
