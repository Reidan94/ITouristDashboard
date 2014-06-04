using ITourist.Models.DataModels;

namespace ITourist.Models.LogicModels.Managers
{
    public class DataManager
    {
        private static readonly ITouristDatabaseEntities Data = new ITouristDatabaseEntities();
        public static UserManager Users = new UserManager(Data);
        public static CountryManager Countries = new CountryManager(Data);
        public static RegionManager Regions = new RegionManager(Data);
        public static PlaceManager Places = new PlaceManager(Data);
        public static RouteManager Routes = new RouteManager(Data);
    }
}