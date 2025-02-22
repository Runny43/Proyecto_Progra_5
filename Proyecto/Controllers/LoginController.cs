using Firebase.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Proyecto.Firebase;
using Proyecto.Models;
using Newtonsoft.Json;


namespace MyWebApp.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                UserHelper userHelper = new UserHelper();

                //userHelper.postUserWithEmailAndPassword("jjtrejos@mycompany.com", "Admin$1234", "Juan Jose Trejos", "owner");

                UserCredential userCredential = await FirebaseAuthHelper.setFirebaseAuthClient().SignInWithEmailAndPasswordAsync(email, password);
                UserModel user = await userHelper.getUserInfo(email);

                HttpContext.Session.SetString("userSession", JsonConvert.SerializeObject(user));

                if (user.Type.Equals("root"))
                {
                    return RedirectToAction("Main", "Root");
                }

                if (user.Type.Equals("owner"))
                {
                    return RedirectToAction("Main", "Owner");
                }

                return RedirectToAction("Index", "Error");
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        public IActionResult LogOut(int id)
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // GET: LoginController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        // GET: LoginController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IFormCollection collection)
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

        // GET: LoginController/Edit/5
        public IActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, IFormCollection collection)
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

        // GET: LoginController/Delete/5
        public IActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
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