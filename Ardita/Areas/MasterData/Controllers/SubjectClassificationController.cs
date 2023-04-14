using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers
{
    public class SubjectClassificationController : Controller
    {
        // GET: SubjectClassificationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SubjectClassificationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubjectClassificationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubjectClassificationController/Create
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

        // GET: SubjectClassificationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubjectClassificationController/Edit/5
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

        // GET: SubjectClassificationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubjectClassificationController/Delete/5
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
