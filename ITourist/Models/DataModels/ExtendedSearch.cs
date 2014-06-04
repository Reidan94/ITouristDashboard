using System.Collections.Generic;
using System.Linq;

namespace ITourist.Models.DataModels
{
    public class ListItem : Translatable
    {
        public int Id { get; private set; }
        public Translation Name { get; private set; }

        public ListItem(int id, Translation name)
        {
            Id = id;
            Name = name;
        }

        public string GetName(Culture culture)
        {
            return GetTranslation(culture, Name);
        }
    }

    public enum SearchItem
    {
        Country,
        Region,
        Place,
        Route
    }
    public sealed class ExtendedSearch
    {
        public IEnumerable<ListItem> SearchItems { get; set; }
        public IEnumerable<Status> SearchOrders { get; set; }
        public IEnumerable<PlaceType> SearchPlaceTypes { get; set; }
        public int CurrentSearchItem { get; set; }
        public int CurrentSearchOrder { get; set; }
        public int CurrentPlaceType { get; set; }

        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<Region> Regions { get; set; }
        public IEnumerable<Place> Places { get; set; }
        public IEnumerable<Route> Routes { get; set; }

        public int Page { get; set; }
        public string Name { get; set; }
        public bool NeedToSearch { get; set; }

        public ExtendedSearch()
        {
            Page = 1;
            CurrentPlaceType = -1;
            SearchItems = new[]
            {
                new ListItem((int)SearchItem.Country, new Translation("countries", "страны")),
                new ListItem((int)SearchItem.Region, new Translation("regions","регионы")),
                new ListItem((int)SearchItem.Place, new Translation("places","места")),
                new ListItem((int)SearchItem.Route, new Translation("routes","маршруты")) 
            };
            SearchOrders = SearchOrder.GetAll(Culture.En);
            var list = PlaceTypes.GetAllPlaceTypes().ToList();
            list.Add(new PlaceType(-1, new Translation("all", "все")));
            SearchPlaceTypes = list.OrderBy(x => x.Id);
            NeedToSearch = false;          
        }

        public ExtendedSearch(Culture culture, int searchItem = 0, int searchOrder = 0, int placeType = -1,string name = null,int page = 1)
        {
            SearchItems = new[]
            {
                new ListItem((int)SearchItem.Country, new Translation("countries", "страны")),
                new ListItem((int)SearchItem.Region, new Translation("regions","регионы")),
                new ListItem((int)SearchItem.Place, new Translation("places","места")),
                new ListItem((int)SearchItem.Route, new Translation("routes","маршруты")) 
            };
            SearchOrders = SearchOrder.GetAll(culture);
            var list = PlaceTypes.GetAllPlaceTypes().ToList();
            list.Add(new PlaceType(-1, new Translation("All", "Все")));
            SearchPlaceTypes = list.OrderBy(x => x.Id);
            CurrentSearchItem = searchItem;
            CurrentSearchOrder = searchOrder;
            CurrentPlaceType = placeType;
            Name = name;
            Page = page;
            NeedToSearch = false;
        }
    }
}