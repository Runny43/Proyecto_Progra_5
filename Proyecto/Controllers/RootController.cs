using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Newtonsoft.Json;
//using MyWebApp.Models;

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

            return RedirectToAction("Index", "Error");
        }

        // GET: RootController
        public ActionResult Index()
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                ViewBag.User = user;

                CondominiumHelper condominiumHelper = new CondominiumHelper();

                ViewBag.Condominium = condominiumHelper.getCondominiums().Result;

                return View();
            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult Create()
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                return View();
            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult CreateCondominium(string txtName, string txtAddress, int txtCount, string txtPhoto)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                CondominiumHelper condominiumHelper = new CondominiumHelper();

                bool result = condominiumHelper.saveCondominium(new Condominium
                {
                    Name = txtName,
                    Address = txtAddress,
                    Count = txtCount,
                    Photo = txtPhoto
                }).Result;

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index", "Error");
        }

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
