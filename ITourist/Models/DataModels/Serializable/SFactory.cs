using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITourist.Models.DataModels.Serializable
{
    public class SFactory
    {
        public static SRegion Create(HttpContextBase context, Region region, Culture culture, bool details)
        {
            if (region == null) return null;
            return details ? new SRegionExtended(context, region, culture) : new SRegion(context, region, culture);
        }

        public static IEnumerable<SRegion> Create(HttpContextBase context, IEnumerable<Region> regions, Culture culture, bool details)
        {
            var enumerable = regions as List<Region> ?? regions.ToList();
            if (!enumerable.Any()) return null;
            return enumerable.Select(region => Create(context, region, culture, details));
        }

        public static SPlace Create(HttpContextBase context, Place place, Culture culture, bool details)
        {
            if (place == null) return null;
            return details ? new SPlaceExtended(context, place, culture) : new SPlace(context, place, culture);
        }

        public static IEnumerable<SPlace> Create(HttpContextBase context, IEnumerable<Place> places, Culture culture, bool details)
        {
            var enumerable = places as List<Place> ?? places.ToList();
            if (!enumerable.Any()) return null;
            return enumerable.Select(place => Create(context, place, culture, details));
        }

        public static SRoute Create(HttpContextBase context, Route route, Culture culture, bool details)
        {
            if (route == null) return null;
            return details ? new SRouteExtended(route, Create(context, route.CheckPoints, culture, true), culture)
                : new SRoute(route, Create(context, route.CheckPoints, culture, false), culture);
        }

        public static IEnumerable<SRoute> Create(HttpContextBase context, IEnumerable<Route> routes, Culture culture, bool details)
        {
            var enumerable = routes as List<Route> ?? routes.ToList();
            if (!enumerable.Any()) return null;
            return enumerable.Select(route => Create(context, route, culture, details));
        }

        public static SBookmark Create(HttpContextBase context, Bookmark bookmark, Culture culture, bool details)
        {
            if (bookmark == null) return null;
            return new SBookmark(bookmark, Create(context,bookmark.Route, culture, details), culture);
        }

        public static IEnumerable<SBookmark> Create(HttpContextBase context, IEnumerable<Bookmark> bookmarks, Culture culture, bool details)
        {
            var enumerable = bookmarks as List<Bookmark> ?? bookmarks.ToList();
            if (!enumerable.Any()) return null;
            return enumerable.Select(bookmark => Create(context, bookmark, culture, details));
        }

        public static SCheckPoint Create(HttpContextBase context, CheckPoint checkPoint, Culture culture, bool details)
        {
            if (checkPoint == null) return null;
            return new SCheckPoint(checkPoint, Create(context, checkPoint.Place, culture, details));
        }

        public static IEnumerable<SCheckPoint> Create(HttpContextBase context, IEnumerable<CheckPoint> bookmarks, Culture culture, bool details)
        {
            var enumerable = bookmarks as List<CheckPoint> ?? bookmarks.ToList();
            if (!enumerable.Any()) return null;
            return enumerable.Select(bookmark => Create(context, bookmark, culture, details));
        }
    }
}