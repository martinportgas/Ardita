using Ardita.Controllers;
using Ardita.Extensions;
using Ardita.Models.DbModels;
using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.Log.Controllers
{
    [CustomAuthorizeAttribute]
    [Area(GlobalConst.Log)]
    public class LogLoginController : BaseController<LogLogin>
    {
        public LogLoginController(ILogLoginService logLoginService) 
        {
            _logLoginService = logLoginService;
        }
        public override async Task<ActionResult> Index() => await base.Index();
        public override async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _logLoginService.GetByFilterModel(model);
                return Json(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
