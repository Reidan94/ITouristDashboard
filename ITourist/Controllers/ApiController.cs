using System.Web.Mvc;
using ITourist.Models.DataModels;
using ITourist.Models.DataModels.Serializable;
using ITourist.Models.LogicModels.Managers;

namespace ITourist.Controllers
{
    public class ApiController : BaseController
    {
        private const string Token = "52249ed0f060d93f0376a87f7474274b";

        private bool CheckToken(string token)
        {
            return Token == token;
        }

        public ActionResult Help(string token)
        {
            if (!CheckToken(token))
                return View("Error");
            return View();
        }

        #region Countries

        public string GetCountries(string token, int offset = 0, int count = 20, string search = null, Culture culture = Culture.En, Order order = Order.Default)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SCountry.Convert(HttpContext, DataManager.Countries.GetCountries(offset, count, culture, search, order), culture));
        }

        public string GetCountry(string token, int id, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SCountry.Convert(HttpContext, DataManager.Countries.GetCountry(id), culture));
        }

        #endregion

        #region Regions

        public string GetRegions(string token, int? countryId = null, int offset = 0, int count = 20, string search = null, Culture culture = Culture.En, Order order = Order.Default, bool details = false)
        {
            if (!CheckToken(token)) return null;
            var regions = DataManager.Regions.GetRegions(countryId, offset, count, culture, search, order);
            return ToJson(SFactory.Create(HttpContext, regions, culture, details));
        }

        public string GetRegion(string token, int id, Culture culture = Culture.En, bool details = false)
        {
            if (!CheckToken(token)) return null;
            var region = DataManager.Regions.GetRegion(id);
            return ToJson(SFactory.Create(HttpContext, region, culture, details));
        }

        #endregion

        #region Places

        public string GetPlaces(string token, int? regionId = null, int offset = 0, int count = 20, string search = null, Culture culture = Culture.En, Order order = Order.Default, bool details = false)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SFactory.Create(HttpContext, DataManager.Places.GetPlaces(regionId, offset, count, search, culture, order: order), culture, details));
        }

        public string GetPlace(string token, int id, Culture culture = Culture.En, bool details = false)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SFactory.Create(HttpContext, DataManager.Places.GetPlace(id), culture, details));
        }

        #endregion

        #region Users

        public string GetUser(string token, int id, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SUser.Convert(DataManager.Users.Find(id), HttpContext, culture));
        }

        public string LogIn(string token, string email, string password, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            User user;//заглушка
            return ToJson(SProcessResult.Convert(DataManager.Users.LogInUser(email, password, out user), culture));
        }

        public string Authenticate(string token, AuthenticationProvider provider, string id, string name, string lastName, string image, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SUser.Convert(DataManager.Users.Authenticate(provider, id, name, lastName, image), HttpContext, culture));
        }

        #endregion

        #region Routes

        public string GetUserBookmarks(string token, int userId, int offset = 0, int count = 20, string search = null, Culture culture = Culture.En, bool details = false)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SFactory.Create(HttpContext, DataManager.Routes.GetUserBookmarks(userId, offset, count, search, culture), culture, details));
        }

        public string AddBookmark(string token, int userId, int routeId, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SProcessResult.Convert(DataManager.Routes.AddBookmark(userId, routeId), culture));
        }

        public string RemoveBookmark(string token, int bookmarkId, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SProcessResult.Convert(DataManager.Routes.RemoveBookmark(bookmarkId), culture));
        }

        public string StartTracking(string token, int bookmarkId, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SProcessResult.Convert(DataManager.Routes.StartTracking(bookmarkId), culture));
        }

        public string AbortTracking(string token, int bookmarkId, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SProcessResult.Convert(DataManager.Routes.AbortTracking(bookmarkId), culture));
        }

        public string CheckIn(string token, int bookmarkId, int checkPointId, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SProcessResult.Convert(DataManager.Routes.CheckIn(bookmarkId, checkPointId), culture));
        }

        public string GetRoute(string token, int id, Culture culture = Culture.En, bool details = false)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SFactory.Create(HttpContext, DataManager.Routes.GetRoute(id), culture, details));
        }

        public string GetPublicRoutes(string token, int offset = 0, int count = 20, string search = null, Culture culture = Culture.En, Order order = Order.Default, bool details = false)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SFactory.Create(HttpContext, DataManager.Routes.GetPublicRoutes(offset, count, search, culture, order), culture, details));
        }

        public string CreateRoute(string token, Route route, int[] places, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SProcessResult.Convert(DataManager.Routes.CreateRoute(route, places), culture));
        }

        public string MakeRoutePublic(string token, int routeId, int userId, Translation translation, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SProcessResult.Convert(DataManager.Routes.MakeRoutePublic(routeId, userId, translation), culture));
        }

        public string RemovePublicRoute(string token, int routeId, int userId, Culture culture = Culture.En)
        {
            if (!CheckToken(token)) return null;
            return ToJson(SProcessResult.Convert(DataManager.Routes.RemovePublicRoute(routeId, userId), culture));
        }

        #endregion
    }
}