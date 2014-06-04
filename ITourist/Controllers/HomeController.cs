using System.Web.Mvc;
using ITourist.Models.DataModels;
using ITourist.Models.LogicModels.Managers;

namespace ITourist.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            return View(DataManager.Places.GetPopularPlaces(culture));
        }

        public ActionResult About()
        {
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            return View();
        }
    }
}