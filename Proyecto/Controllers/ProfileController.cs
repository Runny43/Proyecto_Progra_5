using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Mic;
using Newtonsoft.Json;
using System.Text;

namespace Proyecto.Controllers
{
    public class ProfileController : Controller
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

        // GET: ProfileController
        public ActionResult Index()
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                ViewBag.CondoList = CondominiumHelper.getCondominiums().Result;
                ViewBag.OwnerList = UserHelper.getOwners().Result;

                return View();
            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult SetCount(int count)
        {
            ViewBag.Count = count;

            return View("Index");
        }

        // GET: ProfileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult CreateOwner(string txtEmail, string txtName, string selCondo, int selCondoNumber)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                try
                {
                    UserHelper.postUserWithEmailAndPassword(txtEmail, AppHelper.CreatePassword(), txtName, "owner", selCondo, selCondoNumber);

                    return RedirectToAction("Index", "Profile");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
            //try
            //{
            //    UserHelper userHelper = new UserHelper();
            //    userHelper.postUserWithEmailAndPassword(txtEmail, AppHelper.CreatePassword(), txtName, "owner", selCondo, selCondoNumber);

            //    return RedirectToAction("Index", "Profile");
            //}
            //catch
            //{
            //    return RedirectToAction("Index", "Error");
            //}
        }


        public ActionResult EditOwner(string txtUuid, string txtEmail, string txtName, string selCondo, int selCondoNumber)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                try
                {
                    UserHelper.editOwner(txtUuid, txtEmail, txtName, selCondo, selCondoNumber);

                    return RedirectToAction("Index", "Profile");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }

      

        // GET: ProfileController/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Edit(string id)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                ViewBag.CondoList = CondominiumHelper.getCondominiums().Result;
                ViewBag.Owner = UserHelper.getUserInfo(id).Result;

                return View();
            }

            return RedirectToAction("Index", "Error");
        }

        // POST: ProfileController/Create
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

       

        // POST: ProfileController/Edit/5
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

        // GET: ProfileController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProfileController/Delete/5
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
