using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class MstCompanyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
