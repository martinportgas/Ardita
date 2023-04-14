using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ardita.Areas.MasterData.Controllers
{
    public class SubSubjectClassificationController : Controller
    {
        // GET: SubSubjectClassificationController
        public ActionResult Index()
        {
            return View();
        }

        // GET: SubSubjectClassificationController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubSubjectClassificationController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubSubjectClassificationController/Create
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

        // GET: SubSubjectClassificationController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubSubjectClassificationController/Edit/5
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

        // GET: SubSubjectClassificationController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubSubjectClassificationController/Delete/5
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
