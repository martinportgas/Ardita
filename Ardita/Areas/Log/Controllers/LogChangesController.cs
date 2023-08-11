using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.CodeAnalysis.RulesetToEditorconfig;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;
using System;
using System.Data;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Web.Helpers;

namespace Ardita.Areas.Log.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.Log)]
    public class LogChangesController : BaseController<LogChange>
    {
        public LogChangesController(ILogChangesService logChangesService)
        {
            _logChangesService = logChangesService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _logChangesService.GetByFilterModel(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public override async Task<IActionResult> Detail(Guid Id)
        {
            var data = await _logChangesService.GetById(Id);
            if (data != null)
            {
                return View(GlobalConst.Detail, data);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.LogChanges, new { Area = GlobalConst.Log });
    }
}
