using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITourist.App_Resources;
using ITourist.Models.DataModels;
using ITourist.Models.DataModels.Serializable;
using ITourist.Models.LogicModels.Managers;

namespace ITourist.Controllers
{
    public class RouteProcessResult
    {
        public bool Succeeded { get; private set; }
        public bool IsPlaceChosen { get; private set; }
        public string Message { get; private set; }

        public RouteProcessResult(bool succeeded, bool isPlaceInRoute, string message)
        {
            Succeeded = succeeded;
            IsPlaceChosen = isPlaceInRoute;
            Message = message;
        }
    }

    public class RouteController : BaseController
    {
        private const string PlacesCookie = "Places";
        private const string CountryCookie = "Country";
        private const int RoutesPerPage = 20;

        private List<string> GetChosenPlaces(HttpContextBase context)
        {
            var cookie = context.Request.Cookies[PlacesCookie];
            if (cookie == null)
                return new List<string>();
            return cookie.Value.Split(' ').ToList();
        }

        private HttpCookie SetChosenPlaces(IEnumerable<string> places)
        {
            var cookie = new HttpCookie(PlacesCookie)
            {
                Expires = DateTime.Now.AddYears(100),
                Value = String.Join(" ", places),
                Path = "/",
                Shareable = true
            };
            return cookie;
        }


        private string GetCountry(HttpContextBase context)
        {
            var cookie = context.Request.Cookies[CountryCookie];
            return cookie == null ? null : cookie.Value;
        }

        private HttpCookie SetCountry(string id)
        {
            var cookie = new HttpCookie(CountryCookie)
            {
                Expires = DateTime.Now.AddYears(100),
                Value = id,
                Path = "/",
                Shareable = true
            };
            return cookie;
        }

        public ActionResult Error(string error)
        {
            ViewBag.Message = error;
            return View();
        }

        public ActionResult ChosenPlaces()
        {
            ViewBag.Culture = DefineLanguage(HttpContext);
            var user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn", "User");
            return View(DataManager.Places.GetPlaces(GetChosenPlaces(HttpContext)));
        }

        public string AddPlaceToChosen(string placeId)
        {
            if (!HttpContext.Request.IsAjaxRequest())
                return ToJson(SProcessResult.Convert(ProcessResults.Error));
            DefineLanguage(HttpContext);
            var places = GetChosenPlaces(HttpContext);
            if (places.Contains(placeId))
                return ToJson(new RouteProcessResult(false, true, Language.PlaceIsAlreadyInTheRoute));
            var country = GetCountry(HttpContext);
            string placeCountry = DataManager.Places.GetPlace(Convert.ToInt32(placeId)).Region.CountryId.ToString();
            if (country != null && country != placeCountry)
                return ToJson(new RouteProcessResult(false, false, Language.CountriesNotMatched));
            HttpContext.Response.SetCookie(SetCountry(placeCountry));
            places.Add(placeId);
            HttpContext.Response.SetCookie(SetChosenPlaces(places));
            return ToJson(new RouteProcessResult(true, true, Language.PlaceWasAddedToTheRoute));
        }

        public string RemovePlaceFromChosen(string placeId)
        {
            if (!HttpContext.Request.IsAjaxRequest())
                return ToJson(SProcessResult.Convert(ProcessResults.Error));
            DefineLanguage(HttpContext);
            var places = GetChosenPlaces(HttpContext);
            if (!places.Any())
                return ToJson(new RouteProcessResult(false, false, Language.RouteEmpty));
            if (!places.Contains(placeId))
                return ToJson(new RouteProcessResult(false, false, Language.InvalidPlaceID));
            places.Remove(placeId);
            HttpContext.Response.SetCookie(SetChosenPlaces(places));
            return ToJson(new RouteProcessResult(true, false, Language.PlaceRemovedSuccessfully));
        }

        public bool IsPlaceChosen(string id)
        {
            var places = GetChosenPlaces(HttpContext);
            bool b = places.Contains(id);
            return b;
        }


        public ActionResult ViewOnMap(int id)
        {
            DefineLanguage(HttpContext);
            Route route = DataManager.Routes.GetRoute(id);
            if (route != null && (route.Status == RouteStatus.Public.Id || route.Author == DefineUser(HttpContext).Id))
                return View(route.Places);
            return View("ProcessError", model: Language.RouteNotFound);
        }

        public ActionResult Preview(bool optimizeWaypoints = false, bool loopRoute = false)
        {
            DefineLanguage(HttpContext);
            var places = GetChosenPlaces(HttpContext);
            if (!places.Any())
                return View("ProcessError", model: Language.RouteEmpty);
            if (places.Count < 2)
                return View("ProcessError", model: Language.TwoPlacesNeeded);
            ViewBag.OptimizeWaypoints = optimizeWaypoints;
            if (loopRoute)
                places.Add(places[0]);
            return View("ViewOnMap", DataManager.Places.GetPlaces(places));

        }

        [HttpPost]
        public ActionResult Save(string name, bool optimizeWaypoints = false, bool loopRoute = false)
        {
            User user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn", "User");
            DefineLanguage(HttpContext);
            var places = GetChosenPlaces(HttpContext);
            if (!places.Any())
                return View("ProcessError", model: Language.RouteEmpty);
            if (places.Count < 2)
                return View("ProcessError", model: Language.TwoPlacesNeeded);
            ViewBag.OptimizeWaypoints = optimizeWaypoints;
            if (loopRoute)
                places.Add(places[0]);
            ViewBag.User = user;
            ViewBag.Name = name;
            return View(DataManager.Places.GetPlaces(places));
        }

        private void CleanCookies()
        {
            var country = new HttpCookie(CountryCookie)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            var places = new HttpCookie(PlacesCookie)
            {
                Expires = DateTime.Now.AddDays(-1)
            };
            HttpContext.Response.Cookies.Set(country);
            HttpContext.Response.Cookies.Set(places);
        }

        public string Create(string name, string ids)
        {
            Culture culture = DefineLanguage(HttpContext);
            if (!HttpContext.Request.IsAjaxRequest())
                return ToJson(SProcessResult.Convert(ProcessResults.Error,culture));
            User user = DefineUser(HttpContext);
            var places = ids.Split(' ');
            var translation = new Translation(name, name);
            ProcessResult result = DataManager.Routes.CreateRoute(new Route { Author = user.Id, Translation = translation }, places);
            if (result.Succeeded)
                CleanCookies();
            return ToJson(SProcessResult.Convert(result, culture));
        }


        public ActionResult PublicRoutes(int page = 1, string search = null)
        {
            User user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn", "User");
            int? max;
            ViewBag.Current = page;
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            var places = DataManager.Routes.GetPublicRoutes(page, RoutesPerPage, out max, culture, search);
            if (max.HasValue)
                ViewBag.Max = max.Value;
            ViewBag.Search = search;
            ViewBag.User = user;
            return View(places);
        }

        public ActionResult Bookmarks()
        {
            User user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn", "User");
            Culture culture = DefineLanguage(HttpContext);
            ViewBag.Culture = culture;
            ViewBag.User = user;
            ViewBag.Result = TempData["result"] as ProcessResult;
            return View(DataManager.Routes.GetUserBookmarks(user.Id, culture: culture));
        }

        public string AddToBookmarks(int routeId)
        {
            User user = DefineUser(HttpContext);
            Culture culture = DefineLanguage(HttpContext);
            if (user == null)
                return ToJson(SProcessResult.Convert(ProcessResults.UserNotFound,culture));
            ProcessResult result = DataManager.Routes.AddBookmark(user.Id, routeId);
            return ToJson(SProcessResult.Convert(result,culture));
        }

        public ActionResult RemoveBookmark(int id)
        {
            User user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn", "User");
            ProcessResult result = DataManager.Routes.RemoveBookmark(id);
            TempData["result"] = result;
            return RedirectToAction("Bookmarks");
        }

        public ActionResult MakeRoutePublic(int id,Translation translation)
        {
            User user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn", "User");
            ProcessResult result = DataManager.Routes.MakeRoutePublic(id, user.Id,translation);
            TempData["result"] = result;
            return RedirectToAction("Bookmarks");
        }

        public ActionResult RemovePublicRoute(int id)
        {
            User user = DefineUser(HttpContext);
            if (user == null)
                return RedirectToAction("LogIn", "User");
            ProcessResult result = DataManager.Routes.RemovePublicRoute(id, user.Id);
            TempData["result"] = result;
            return RedirectToAction("Bookmarks");
        }
    }
}