using System.Linq;
using System.Web;
using System.Web.Mvc;
using ITourist.Models.DataModels;
using ITourist.Models.LogicModels.Managers;

namespace ITourist.Controllers
{
    public class DashboardController : BaseController
    {
        //
        // GET: /Dashboard/
        public ActionResult Index()
        {
            return RedirectToAction("Countries");
        }

        public ActionResult NoPermission()
        {
            ViewBag.Culture = DefineLanguage(HttpContext);
            return View();
        }


        private bool HasNoAccess(User user)
        {
            return user == null || !user.HasModeratorAccess;
        }

        private bool HasNoAdminAccess(User user)
        {
            return user == null || !user.HasAdminAccess;
        }

        #region Countries

        public ActionResult Countries(int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Countries.GetAllCountries().ToList());
        }

        public ActionResult Country(int? result, int id)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Countries.GetCountry(id));
        }

        public ActionResult AddCountry(int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View();
        }

        public ActionResult EditCountry(int id, int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Countries.GetCountry(id));
        }

        public ActionResult DeleteCountry(int id, int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Countries.GetCountry(id));
        }

        [HttpPost]
        public ActionResult ManageCountryAdding(string nameRussian, string nameEnglish, HttpPostedFileBase imageUpload)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ProcessResult result = DataManager.Countries.AddCountry(nameRussian, nameEnglish, imageUpload, Server);
            if (result.Succeeded && result.AffectedObjectId.HasValue)
                return RedirectToAction("Country", new { id = result.AffectedObjectId.Value, result = result.Id });
            return RedirectToAction("AddCountry", new { result = result.Id });
        }

        [HttpPost]
        public ActionResult ManageCountryEditing(int id, string nameRus, string nameEn, HttpPostedFileBase imageUpload, bool deleteImage = false)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ProcessResult result = DataManager.Countries.EditCountry(id, nameRus, nameEn, imageUpload, deleteImage, Server);
            if (result.Succeeded)
                return RedirectToAction("Country", new { id, result = result.Id });
            return RedirectToAction("EditCountry", new { id, result = result.Id });
        }

        [HttpPost]
        public ActionResult ManageCountryDeleting(int id)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ProcessResult result = DataManager.Countries.DeleteCoutnry(id, Server);
            if (result.Succeeded)
                return RedirectToAction("Countries", new { result = result.Id });
            return RedirectToAction("DeleteCountry", new { id, result = result.Id });
        }

        #endregion

        #region Regions

        public ActionResult Regions(int? result, int countryId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Regions.GetRegionsOfTheCountry(countryId));
        }

        public ActionResult Region(int? result, int regionId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Regions.GetRegion(regionId));
        }

        public ActionResult AddRegion(int? result, int countryId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Countries.GetCountry(countryId));
        }

        public ActionResult EditRegion(int regionId, int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Regions.GetRegion(regionId));
        }

        public ActionResult DeleteRegion(int regionId, int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Regions.GetRegion(regionId));
        }

        [HttpPost]
        public ActionResult ManageRegionAdding(string nameRussian, string nameEnglish,
            string CountryId, HttpPostedFileBase imageUpload)
        {
            int countryId = int.Parse(CountryId);
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ProcessResult result = DataManager.Regions.AddRegion(nameRussian, nameEnglish, countryId, imageUpload, Server);
            if (result.Succeeded && result.AffectedObjectId.HasValue)
                return RedirectToAction("Region", new { regionId = result.AffectedObjectId.Value, result = result.Id });
            return RedirectToAction("AddRegion", new { result = result.Id, regionId = result.AffectedObjectId });
        }

        [HttpPost]
        public ActionResult ManageRegionEditing(int id, string nameRus, string nameEng, int countryId,HttpPostedFileBase imageUpload, bool deleteImage = false)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) 
                return RedirectToAction("NoPermission");
            ProcessResult result = DataManager.Regions.EditRegion(id, nameRus,nameEng,countryId, imageUpload, deleteImage, Server);
            return RedirectToAction(result.Succeeded ? "Region" : "EditRegion", new { regionId = id, result = result.Id });
        }

        [HttpPost]
        public ActionResult ManageRegionDeleting(int id, int countryId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ProcessResult result = DataManager.Regions.DeleteRegion(id, Server);
            if (result.Succeeded)
                return RedirectToAction("Country", new { result = result.Id, id = countryId });
            return RedirectToAction("DeleteRegion", new { id, result = result.Id });
        }

        #endregion

        #region Places

        public ActionResult Places(int? result, int regionId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Places.GetPlacesOfTheRegion(regionId));
        }

        public ActionResult Place(int? result, int placeId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Places.GetPlace(placeId));
        }

        public ActionResult AddPlace(int? result, int regionId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Regions.GetRegion(regionId));
        }

        public ActionResult EditPlace(int placeId, int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Places.GetPlace(placeId));
        }

        public ActionResult DeletePlace(int placeId, int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            return View(DataManager.Places.GetPlace(placeId));
        }

        public ActionResult AddPlacePhoto(int placeId, int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            var place = DataManager.Places.GetPlace(placeId);
            if (result.HasValue)
                ViewBag.Result = result;
            return View(place);
        }

        [HttpPost]
        public ActionResult ManagePlacePhotosAdding(HttpPostedFileBase imageUpload1,
            HttpPostedFileBase imageUpload2,
            HttpPostedFileBase imageUpload3,
            HttpPostedFileBase imageUpload4,
            HttpPostedFileBase imageUpload5, int placeId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ProcessResult processResult;
            if (imageUpload1 != null)
            {
                processResult = DataManager.Places.AddPlacePhoto(placeId, imageUpload1, Server);
                if (!processResult.Succeeded)
                    return RedirectToAction("AddPlacePhoto", new {placeId, result = processResult.Id });
            }
            if (imageUpload2 != null)
            {
                processResult = DataManager.Places.AddPlacePhoto(placeId, imageUpload2, Server);
                if (!processResult.Succeeded)
                    return RedirectToAction("AddPlacePhoto", new {placeId, result = processResult.Id });
            }
            if (imageUpload3 != null)
            {
                processResult = DataManager.Places.AddPlacePhoto(placeId, imageUpload3, Server);
                if (!processResult.Succeeded)
                    return RedirectToAction("AddPlacePhoto", new {placeId, result = processResult.Id });
            }
            if (imageUpload4 != null)
            {
                processResult = DataManager.Places.AddPlacePhoto(placeId, imageUpload4, Server);
                if (!processResult.Succeeded)
                    return RedirectToAction("AddPlacePhoto", new {placeId, result = processResult.Id });
            }
            if (imageUpload5 != null)
            {
                processResult = DataManager.Places.AddPlacePhoto(placeId, imageUpload5, Server);
                if (!processResult.Succeeded)
                    return RedirectToAction("AddPlacePhoto", new {placeId, result = processResult.Id });
            }
            return RedirectToAction("Place", new {placeId ,result=ProcessResults.PhotoAdded.Id});
        }

        public ActionResult ManagePlaceCommentDeleting(int commentId, int placeId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            var result = DataManager.Places.DeletePlaceComment(commentId);
            return RedirectToAction("Place", new {placeId, result = result.Id });
        }

        public ActionResult ManagePlacePhotoDeleting(int photoId, int placeId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            var result = DataManager.Places.DeletePlacePhoto(photoId, Server);
            return RedirectToAction("Place", new {placeId, result = result.Id });
        }

        [HttpPost]
        public ActionResult ManagePlaceAdding(Place place, HttpPostedFileBase imageUpload)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ProcessResult result = DataManager.Places.AddPlace(place,imageUpload, Server);
            if (result.Succeeded && result.AffectedObjectId.HasValue)
                return RedirectToAction("Place", new { placeId = result.AffectedObjectId.Value, result = result.Id });
            return RedirectToAction("AddPlace", new { result = result.Id, regionId = place.RegionId });
        }

        [HttpPost]
        public ActionResult ManagePlaceEditing(Place place, HttpPostedFileBase imageUpload, bool deleteImage = false)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ProcessResult result = DataManager.Places.EditPlace(place, deleteImage,imageUpload,Server);
            if (result.Succeeded)
                return RedirectToAction("Place", new { placeId = place.Id, result = result.Id });
            return RedirectToAction("EditPlace", new { placeId = place.Id, result = result.Id });
        }

        [HttpPost]
        public ActionResult ManagePlaceDeleting(int id, int regionId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ProcessResult result = DataManager.Places.DeletePlace(id, Server);
            if (result.Succeeded)
                return RedirectToAction("Region", new { result = result.Id, regionId  });
            return RedirectToAction("DeletePlace", new {id, result = result.Id });
        }

        #endregion

        #region Users

        public ActionResult Users(int? result)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            if (result.HasValue)
                ViewBag.Result = ProcessResults.GetById(result.Value);
            ViewBag.User = user;
            return View(DataManager.Users.GetAll());
        }

        [HttpPost]
        public ActionResult ManageUserEditing(int id, int type)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            var result = DataManager.Users.SetUserStatus(id, type);
            return RedirectToAction("Users", new { result = result.Id });
        }

        public ActionResult GetUser(int id)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ViewBag.User = user;
            return View(DataManager.Users.Find(id));
        }

        public ActionResult EditUserStatus(int id)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            ViewBag.User = user;
            return View(DataManager.Users.Find(id));
        }

        public ActionResult GetRoute(int routeId)
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            return View(DataManager.Routes.GetRoute(routeId));
        }

        public ActionResult GetPublicRoutes()
        {
            var user = DefineUser(HttpContext);
            if (HasNoAccess(user)) return RedirectToAction("NoPermission");
            return View(DataManager.Routes.GetPublicRoutes());
        }

        #endregion
    }
}