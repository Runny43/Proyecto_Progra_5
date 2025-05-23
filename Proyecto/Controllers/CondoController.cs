﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Models;

namespace Proyecto.Controllers
{
    public class CondoController : Controller
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
        // GET: CondoController
        
        public ActionResult Index()
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                ViewBag.User = user;

                List<Condominium> condoList = CondominiumHelper.getCondominiums().Result;

                ViewBag.Condominium = condoList;

                HttpContext.Session.SetString("condoList", JsonConvert.SerializeObject(condoList));

                return View();
            }
            TempData["Error"] = "Error.";
            return RedirectToAction("Index");
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

        public ActionResult CondoDetails()
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {

                List<Condominium> condoList = CondominiumHelper.getCondominiums().Result;

                ViewBag.Condominium = condoList;

                return View();
            }

            return RedirectToAction("Index", "Error");
        }
        public ActionResult EditCondominium(string name)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {

                ViewBag.Condo = CondominiumHelper.getCondominium(name).Result;

                return View();

            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult DeleteCondo(string uuid)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {

                try
                {
                    CondominiumHelper.DeleteCondo(uuid);

                    return RedirectToAction("Index", "Condo");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult EditCondominiumAction(string txtUuid, string txtName, string txtAddress, int txtCount, string txtPhoto)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {

                try
                {
                    CondominiumHelper.editCondo(txtUuid, txtName, txtAddress, txtCount, txtPhoto);

                    return RedirectToAction("Index", "Condo");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }
        // GET: CondoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CondoController/Create
        

        // POST: CondoController/Create
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

        // GET: CondoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CondoController/Edit/5
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

        // GET: CondoController/Delete/5
        public ActionResult Delete(IFormCollection form)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                try
                {
                    
                    return RedirectToAction("Index", "Profile");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }

        // POST: CondoController/Delete/5
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
