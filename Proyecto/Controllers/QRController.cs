using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Mic;

namespace Proyecto.Controllers
{
    public class QRController : Controller
    {
        // GET: QRController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Generate()
        {
            string path = QRGenerator.GenerateQRCode();

            ViewBag.QRCode = path;

            return View("Index");
        }



        // GET: QRController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: QRController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: QRController/Create
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

        // GET: QRController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QRController/Edit/5
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

        // GET: QRController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QRController/Delete/5
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
