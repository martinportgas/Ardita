using Ardita.Models.ViewModels;
using Ardita.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers
{
    [CustomAuthorizeAttribute]
    [Area("MasterData")]
    public class ClassificationController : Controller
    {
        private readonly IClassificationService _classificationService;
        public ClassificationController(IClassificationService classificationService)
        {
            _classificationService = classificationService;
        }
        // GET: ClassificationController
        public ActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> GetData(DataTablePostModel model)
        {
            try
            {
                var result = await _classificationService.GetListClassification(model);

                return Json(result);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // GET: ClassificationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClassificationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClassificationController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClassificationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClassificationController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClassificationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClassificationController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
