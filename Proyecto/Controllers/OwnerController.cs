using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Proyecto.Mic;
using Proyecto.Models;
using System.Xml.Linq;

namespace Proyecto.Controllers
{
    public class OwnerController : Controller
    {
        public UserModel GetSessionInfo()
        {
            try
            {
                if (!string.IsNullOrEmpty(HttpContext.Session.GetString("userSession")))
                {
                    UserModel? user = JsonConvert.DeserializeObject<UserModel>(HttpContext.Session.GetString("userSession"));

                    if (user.Type.Equals("owner"))
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

        // GET: OwnerController
        public ActionResult Index()
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


        public ActionResult Visits()
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                ViewBag.User = user;
                List<VisitModel> visitList = VisitHelper.getVisit(user.Name).Result;

                ViewBag.Visits = visitList;

                HttpContext.Session.SetString("visitList", JsonConvert.SerializeObject(visitList));

                return View();
            }
            TempData["Error"] = "Error.";
            return RedirectToAction("Index");
        }

        
        public ActionResult Deliverys()
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                ViewBag.User = user;
                List<VisitModel> deliveryList = VisitHelper.getDelivery(user.Name).Result;

                ViewBag.Delivery = deliveryList;

                HttpContext.Session.SetString("deliveryList", JsonConvert.SerializeObject(deliveryList));

                return View();
            }
            TempData["Error"] = "Error.";
            return RedirectToAction("Index");
        }

        public ActionResult CreateDelivery(string txtCard, string txtTo, string txtName)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                if (txtTo == user.Name)
                {
                    try
                    {
                        VisitHelper.postDelivery(txtTo, txtCard, txtName, "delivery");

                        return RedirectToAction("Main", "Owner");
                    }

                    catch
                    {
                        return RedirectToAction("Index", "Error");
                    }
                }
                
            }

            return RedirectToAction("Index", "Error");


        }
        public ActionResult CreateVisit(string txtCard, string txtTo, string txtName, string txtType, string txtPlate, string txtBrand, string txtModel, string txtColor)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                if (txtTo == user.Name) 
                {

                    List<VisitModel> visitsList = VisitHelper.getAllVisits().Result;

                    foreach (var visit in visitsList) 
                    {
                        if (txtName != visit.Name)
                        {
                            try
                            {
                                VisitHelper.postVisit(txtTo, txtCard, txtName, txtType, txtPlate, txtBrand, txtModel, txtColor);

                                return RedirectToAction("Main", "Owner");
                            }
                            catch
                            {
                                return RedirectToAction("Index", "Error");
                            }
                        }
                        
                    }
                }
                
            }

            return RedirectToAction("Index", "Error");


        }
        public ActionResult EditDelivery(string name)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                
                ViewBag.Delivery = VisitHelper.getDeliveryToEdit(name).Result;

                return View();
                
            }

            return RedirectToAction("Index", "Error");
        }
        public ActionResult EditVisit(string name)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {

                ViewBag.Visit = VisitHelper.getVisitToEdit(name).Result;

                return View();

            }

            return RedirectToAction("Index", "Error");
        }
        public ActionResult EditVisits(string txtUuid, string txtCard, string txtTo, string txtName, string txtType, string txtModel, string txtPlate, string txtBrand, string txtColor)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {

                try
                {
                    VisitHelper.editVisit(txtUuid, txtCard, txtTo, txtName, txtType, txtModel, txtBrand, txtPlate, txtColor);

                    return RedirectToAction("Visits", "Owner");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }
        public ActionResult EditDeliverys(string txtUuid, string txtCard, string txtTo, string txtName)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {
                
                try
                {
                    VisitHelper.editDelivery(txtUuid, txtTo, txtName);

                    return RedirectToAction("Main", "Owner");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
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

        public ActionResult DeleteDelivery(string uuid)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {

                try
                {
                    VisitHelper.DeleteDelivery(uuid);

                    return RedirectToAction("Main", "Owner");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }

        public ActionResult DeleteVisit(string uuid)
        {
            UserModel? user = GetSessionInfo();

            if (user != null)
            {

                try
                {
                    VisitHelper.DeleteVisit(uuid);

                    return RedirectToAction("Main", "Owner");
                }
                catch
                {
                    return RedirectToAction("Index", "Error");
                }
            }

            return RedirectToAction("Index", "Error");
        }

        


        // GET: OwnerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OwnerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OwnerController/Create
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

        // GET: OwnerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OwnerController/Edit/5
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

        // GET: OwnerController/Delete/5
        

        // POST: OwnerController/Delete/5
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
