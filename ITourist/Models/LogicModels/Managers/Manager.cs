using System.IO;
using System.Web;
using ITourist.Models.DataModels;

namespace ITourist.Models.LogicModels.Managers
{
    public abstract class Manager
    {
        private const int ItemsPerPage = 10;
        protected readonly ITouristDatabaseEntities Data;

        protected Manager(ITouristDatabaseEntities entities)
        {
            Data = entities;
        }

        protected string SaveImage(int id, string folder, HttpPostedFileBase imageUpload, HttpServerUtilityBase server)
        {
            string relativePath = folder + id + Path.GetExtension(imageUpload.FileName);
            imageUpload.SaveAs(server.MapPath("~") + relativePath);
            return relativePath;
        }

        protected void DeleteImage(string virtualPath, HttpServerUtilityBase server)
        {
            if (virtualPath != null)
            {
                string path = server.MapPath("~") + virtualPath;
                var file = new FileInfo(path);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
        }

        public static ExtendedSearch ExtendedSearch(ExtendedSearch search, out int? max, Culture culture)
        {
            max = 0;
            if (search == null)
                return new ExtendedSearch(culture);
            if (!search.NeedToSearch)
                return search;
            int? type = search.CurrentPlaceType;
            if (type == -1)
                type = null;
            switch (search.CurrentSearchItem)
            {
                case (int)SearchItem.Region:
                    Country country;
                    search.Regions = DataManager.Regions.GetRegions(null, search.Page, ItemsPerPage, out max, out country, culture, search.Name, SearchOrder.GetOrder(search.CurrentSearchOrder));
                    return search;
                case (int)SearchItem.Place:
                    Region region;
                    search.Places = DataManager.Places.GetPlaces(null, search.Page, ItemsPerPage, out max, out region, culture, search.Name, type, SearchOrder.GetOrder(search.CurrentSearchOrder));
                    return search;
                case (int)SearchItem.Route:
                    search.Routes = DataManager.Routes.GetPublicRoutes(search.Page, ItemsPerPage, out max, culture, search.Name, SearchOrder.GetOrder(search.CurrentSearchOrder));
                    return search;
                default:
                    search.Countries = DataManager.Countries.GetCountries(search.Page, ItemsPerPage, out max, culture, search.Name, SearchOrder.GetOrder(search.CurrentSearchOrder));
                    return search;
            }
        }
    }
}