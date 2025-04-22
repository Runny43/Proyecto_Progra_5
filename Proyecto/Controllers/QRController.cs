using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Mic;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class QRController : Controller
    {
        // GET: QRController
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Generate()

        {
            /// Generamos el código EasyPass
            string code = AppHelper.CreateEasyPassCode();

            // Guardamos el código en Firestore
            await FirebaseHelper.GuardarEasyPass(code);

            // Generamos el código QR con el mismo código generado para EasyPass
            string qrCodePath = QRGenerator.GenerateQRCode(code);

            // Pasamos el path del QR generado a la vista para mostrarlo
            ViewBag.QRCode = qrCodePath;

            return View("Index");
        }



        // GET: Mostrar formulario de validación
        [HttpGet]
        public ActionResult ValidateCode()
        {
            return View();
        }

        // POST: Validar el código enviado por el formulario
        [HttpPost]
        public async Task<ActionResult> ValidateCode(string code)
        {
            bool isValid = await EasyPass.ValidateEasyPassCode(code);

            if (isValid)
            {
                ViewBag.Message = "El código es válido.";
                return View();
            }
            else
            {
                ViewBag.ErrorMessage = "El código es inválido o ha expirado.";
                return View();
            }
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
