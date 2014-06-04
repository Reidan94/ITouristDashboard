using System.Linq;
using System.Web.Mvc;
using ITourist.App_Resources;
using ITourist.Models.DataModels;
using ITourist.Models.DataModels.Serializable;
using ITourist.Models.LogicModels.Managers;

namespace ITourist.Controllers
{
    public class SearchController : BaseController
    {
        private const int CountriesPerPage = 15;
        private const int RegionsPerPage = 15;
        private const int PlacesPerPage = 15;

        public ActionResult Countries(int page = 1, string search = null)
        {
            int? max;
            ViewBag.Current = page;
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            var countries = DataManager.Countries.GetCountries(page, CountriesPerPage, out max, culture, search);
            if (max.HasValue)
                ViewBag.Max = max.Value;
            ViewBag.Search = search;
            return View(countries);
        }



        public ActionResult Country(int id, int page = 1, string search = null)
        {
            int? max;
            ViewBag.Current = page;
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            Country country;
            var regions = DataManager.Regions.GetRegions(id, page, RegionsPerPage, out max, out country, culture, search);
            if (country == null)
                return View("ProcessError", model: Language.CountryNotExists);
            if (max.HasValue)
                ViewBag.Max = max.Value;
            country.Regions = regions.ToList();
            ViewBag.Search = search;
            return View(country);
        }

        public ActionResult Region(int id, int page = 1, string search = null)
        {
            int? max;
            ViewBag.Current = page;
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            Region region;
            var places = DataManager.Places.GetPlaces(id, page, PlacesPerPage, out max, out region, culture, search);
            if (region == null)
                return View("ProcessError", model: Language.RegionNotExist);
            if (max.HasValue)
                ViewBag.Max = max.Value;
            region.Places = places.ToList();
            ViewBag.Search = search;
            return View(DataManager.Regions.GetRegion(id));
        }

        public ActionResult Place(int id)
        {
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            ViewBag.User = DefineUser(HttpContext);
            Place place = DataManager.Places.GetPlace(id);
            if (place == null)
                return View("ProcessError", model: Language.PlaceNotExist);
            return View(place);
        }

        public string AddComment(int placeId, string message)
        {
            var user = DefineUser(HttpContext);
            Culture culture = DefineLanguage(HttpContext);
            ProcessResult result;
            if (user != null && user.HasUserAccess)
                result = DataManager.Places.AddComment(placeId, user.Id, message);
            else
                result = ProcessResults.UserNotFound;
            return ToJson(SProcessResult.Convert(result, culture));
        }

        public ActionResult Extended(ExtendedSearch search)
        {
            User user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn", "User");
            ViewBag.User = user;
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            int? max;
            search = Manager.ExtendedSearch(search, out max, culture);
            ViewBag.Max = max;
            return View(search);
        }
    }
}