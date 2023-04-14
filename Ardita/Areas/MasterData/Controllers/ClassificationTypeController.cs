using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers
{
    public class ClassificationTypeController : Controller
    {
        // GET: ClassificationTypeController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ClassificationTypeController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClassificationTypeController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClassificationTypeController/Create
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

        // GET: ClassificationTypeController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ClassificationTypeController/Edit/5
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

        // GET: ClassificationTypeController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ClassificationTypeController/Delete/5
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
