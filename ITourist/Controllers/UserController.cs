using System;
using System.Web;
using System.Web.Mvc;
using ITourist.App_Resources;
using ITourist.Models.DataModels;
using ITourist.Models.LogicModels.Managers;
using ITourist.Models.LogicModels.Services;
using ITourist.Models.ViewModels;

namespace ITourist.Controllers
{
    public class UserController : BaseController
    {
        public ActionResult Index()
        {
            User user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn");
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            return View(user);
        }

        public ActionResult Info(int id)
        {
            User user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn");
            if (user.Id == id)
                return RedirectToAction("Index");
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            user = DataManager.Users.Find(id);
            if (user == null)
                return View("ProcessError", model: Language.UserNotFound);
            return View(user);
        }


        public ActionResult Logout()
        {
            var userCookie = new HttpCookie("UserId")
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            var keyCookie = new HttpCookie("Key")
            {
                Expires = DateTime.Now.AddDays(-1)
            };

            HttpContext.Response.Cookies.Set(userCookie);
            HttpContext.Response.Cookies.Set(keyCookie);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Authenticate(AuthenticationProvider provider, string id, string name, string lastName, string image)
        {
            User user = DataManager.Users.Authenticate(provider, id, name, lastName, image);
            SetUser(user, SecurityManager.GetHashString(id));
            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogIn(int? result)
        {
            User user = DefineUser(HttpContext);
            if (user != null)
                return RedirectToAction("Index");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            return View();
        }

        [HttpPost]
        public ActionResult ManageLogIn(string email, string password)
        {
            User user;
            ProcessResult result = DataManager.Users.LogInUser(email, password, out user);
            if (result.Succeeded && user != null)
            {
                SetUser(user.Id, user.Password);
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("LogIn", "User", new { result = result.Id });
        }

        public ActionResult Registration(int? result)
        {
            User user = DefineUser(HttpContext);
            if (user != null)
                return RedirectToAction("Index");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            return View();
        }

        [HttpPost]
        public ActionResult RegistrateUser(RegistrationModel registrationModel, HttpPostedFileBase imageUpload)
        {
            Culture culture = DefineLanguage(HttpContext);
            ProcessResult result = DataManager.Users.RegistrateUser(HttpContext, registrationModel, Server, imageUpload,culture);
            if (!result.Succeeded)
                return RedirectToAction("Registration", "User", new { result = result.Id });
            ViewBag.Culture = culture;
            return View();
        }

        public ActionResult Confirm(string hash)
        {
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            if (DataManager.Users.ConfirmRegistration(hash))
                ViewBag.Message = Language.RegistrationConfirmed;
            else return RedirectToAction("Error");
            return View();
        }

        private void SetUser(User user, string hashedKey)
        {
            if (user == null) return;
            var cookieUser = new HttpCookie("UserId")
            {
                Value = Convert.ToString(user.Id),
                Expires = DateTime.MaxValue
            };
            var cookieKey = new HttpCookie("Key")
            {
                Value = hashedKey,
                Expires = DateTime.MaxValue
            };
            HttpContext.Response.Cookies.Remove("UserId");
            HttpContext.Response.Cookies.Remove("Key");
            HttpContext.Response.SetCookie(cookieUser);
            HttpContext.Response.SetCookie(cookieKey);
        }

        private void SetUser(int userId, string hashedKey)
        {
            SetUser(DataManager.Users.Find(userId), hashedKey);
        }
    }
}