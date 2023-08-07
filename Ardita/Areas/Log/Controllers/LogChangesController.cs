using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json.Nodes;

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
                if (!string.IsNullOrEmpty(data.NewValue))
                {
                    JsonNode header = JsonConvert.DeserializeObject<JsonNode>(data.NewValue);

                   // ViewBag.DtNewHeader = Global.JsonToDataTable(header.header);
                }

                return View(GlobalConst.Detail);
            }
            else
            {
                return RedirectToIndex();
            }
        }

        private RedirectToActionResult RedirectToIndex() => RedirectToAction(GlobalConst.Index, GlobalConst.LogChanges, new { Area = GlobalConst.Log });
    }
}
