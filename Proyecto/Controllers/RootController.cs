using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Newtonsoft.Json;


namespace Proyecto.Controllers
{
    public class RootController : Controller
    {

        public UserModel GetSessionInfo()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                {
                    UserModel? user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("userSession"));

                    if (user.Type.Equals("root"))
                    {
                        return user;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public ActionResult Main()
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                ViewBag.User = user;
                return View();
            }
            TempData["Error"] = "Error.";
			return RedirectToAction("Index");
        }

        // GET: RootController
        

        

        // GET: RootController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: RootController/Create
        

        // POST: RootController/Create
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

        // GET: RootController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: RootController/Edit/5
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

        // GET: RootController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: RootController/Delete/5
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
