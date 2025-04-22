using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Models;
using Proyecto.Mic;
using Newtonsoft.Json;
using System.Text;
using static QRCoder.PayloadGenerator;
using System.Drawing.Drawing2D;
using Firebase.Auth;

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
            TempData["Error"] = "Error.";
            return RedirectToAction("Index");
        }

        public ActionResult SetCount(int count)
        {
            ViewBag.Count = count;

            return View("Index");
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

        


        
        public ActionResult CreateOwner(string txtEmail, string txtName, string txtCard, string txtPlate, string txtBrand, string txtModel, string txtColor, string selCondo, int selCondoNumber)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                try
                {
                    UserHelper.postUserWithEmailAndPassword(txtCard, txtPlate, txtBrand, txtModel, txtColor, txtEmail,  AppHelper.CreatePassword(), txtName, "owner", selCondo, selCondoNumber);
                   

                    return RedirectToAction("Index", "Profile");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
            
            
        }

        
        public ActionResult EditOwner(string txtUuid, string txtCard, string txtEmail, string txtName, string txtPlate, string txtBrand, string txtModel, string txtColor, string selCondo, int selCondoNumber)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                try
                {
                    UserHelper.editOwner(txtUuid, txtPlate, txtBrand, txtModel, txtColor, txtCard, txtEmail, txtName, selCondo, selCondoNumber);

                    return RedirectToAction("Index", "Profile");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
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

        public ActionResult CreateSecurity(string txtEmail, string txtName, string txtCard, string selCondo)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                try
                {

                    UserHelper.postSecurityWithEmailAndPassword(txtEmail, AppHelper.CreatePassword(), txtName, txtCard, "security", selCondo);


                    return RedirectToAction("IndexSecurity", "Profile");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult EditSecurity(string id)
        {

            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                ViewBag.CondoList = CondominiumHelper.getCondominiums().Result;
                ViewBag.Security = UserHelper.getUserInfo(id).Result;

                return View();
            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult EditSecurityAction(string txtUuid, string txtEmail, string displayName, string txtCard, string selCondo)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                try
                {
                    UserHelper.editSecurity(txtUuid, txtEmail, displayName, txtCard, selCondo);

                    return RedirectToAction("EditSecurity", "Profile");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }




        // GET: ProfileController/Details/5
        public ActionResult Details(int id)
        {
            return View();
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
        //public async Task<IActionResult> DeleteUser(string uuid)
        //{
        //    await DeleteUserAsync(uuid); // El uuid recibido desde el form
        //    return RedirectToAction("Profile"); // Redirige a donde quieras después de la eliminación
        //}

    }
}
