using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Mic;
using Newtonsoft.Json;
using System.Text;


namespace Proyecto.Controllers
{
    public class SecurityController : Controller
    {
        public UserModel GetSessionInfo()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                {
                    UserModel? user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("userSession"));

                    if (user.Type.Equals("security"))
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


        // GET: SecurityController
        

        public ActionResult SetCount(int count)
        {
            ViewBag.Count = count;

            return View("IndexSecurity");
        }

        public ActionResult IndexSecurity()
        {
            UserModel? user = GetSessionInfo();
            if (user != null)
            {
                ViewBag.CondoList = CondominiumHelper.getCondominiums().Result;
                ViewBag.SecurityList = UserHelper.getSecurity().Result;

                return View();
            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult EditSecurity(string id)
        {
            UserModel? sessionUser = GetSessionInfo();
            if (sessionUser != null)
            {
                ViewBag.CondoList = CondominiumHelper.getCondominiums().Result;

                var userInfo = UserHelper.getUserInfo(id).Result;
                if (userInfo != null)
                {
                    ViewBag.Security = userInfo;
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }

        [HttpPost]
        public ActionResult EditSecurityAction(string txtUuid, string txtEmail, string displayName, string txtCard, string selCondo)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                try
                {
                    UserHelper.editSecurity(txtUuid, txtEmail, displayName, txtCard, selCondo);
                    return RedirectToAction("IndexSecurity", "Security");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }



        //public ActionResult IndexSecurity()
        //{
        //    UserModel? user = GetSessionInfo();
        //    if (user != null)
        //    {
        //        ViewBag.CondoList = CondominiumHelper.getCondominiums().Result;
        //        ViewBag.SecurityList = UserHelper.getSecurity().Result;

        //        return View();
        //    }

        //    return RedirectToAction("Index", "Error");
        //}

        //public ActionResult EditSecurityAction(string txtUuid, string txtEmail, string displayName, string txtCard, string selCondo)
        //{
        //    UserModel? user = GetSessionInfo();

        //    if (user != null)
        //    {
        //        try
        //        {
        //            UserHelper.editSecurity(txtUuid, txtEmail, displayName, txtCard, selCondo);

        //            //return RedirectToAction ("IndexSecurity", "Profile");
        //            return RedirectToAction("IndexSecurity", "Profile", new { id = txtUuid });
        //        }
        //        catch
        //        {
        //            return RedirectToAction("Index", "Error");
        //        }
        //    }

        //    return RedirectToAction("Index", "Error");
        //}

        //public ActionResult EditSecurity(string id)
        //{

        //    UserModel? sessionUser = GetSessionInfo();

        //    if (sessionUser != null)
        //    {
        //        ViewBag.CondoList = CondominiumHelper.getCondominiums().Result;

        //        var userInfo = UserHelper.getUserInfo(id).Result;
        //        if (userInfo != null)
        //        {
        //            ViewBag.Security = userInfo;
        //            return View();
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index", "Error"); // <- mover aquí
        //        }
        //    }

        //    return RedirectToAction("Index", "Error");
        //}


        // GET: SecurityController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SecurityController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SecurityController/Create
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

        // GET: SecurityController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SecurityController/Edit/5
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

        // GET: SecurityController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SecurityController/Delete/5
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
