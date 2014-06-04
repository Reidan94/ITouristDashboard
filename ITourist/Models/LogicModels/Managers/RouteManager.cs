using System;
using System.Collections.Generic;
using System.Linq;
using ITourist.Models.DataModels;

namespace ITourist.Models.LogicModels.Managers
{
    public class RouteManager : Manager
    {

        private int tempMax;

        public RouteManager(ITouristDatabaseEntities entities)
            : base(entities)
        {

        }


        public IEnumerable<Route> GetPublicRoutes(int page, int countriesPerPage, out int? max, Culture culture = Culture.En, string search = null, Order order = Order.Default)
        {
            var n = GetPublicRoutes((page - 1) * countriesPerPage, countriesPerPage, search, culture, order);
            max = tempMax;
            return n;
        }


        public IEnumerable<Route> GetPublicRoutes(int offset = 0, int count = 20, string search = null, Culture culture = Culture.En, Order order = Order.Default)
        {
            var routes = new List<Route>();
            var source = Data.Routes.Where(x => x.Status == RouteStatus.Public.Id);
            if (!string.IsNullOrWhiteSpace(search))
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (Route route in source)
                {
                    if (route.Contains(search))
                        routes.Add(route);
                }
            }
            else
                routes = source.ToList();
            tempMax = (int)Math.Ceiling((decimal)routes.Count() / count);
            IEnumerable<Route> result = routes;
            switch (order)
            {
                case Order.Name:
                    result = result.OrderBy(x => x.GetName(culture));
                    break;
                case Order.Popularity:
                    result = result.OrderByDescending(x => x.Rating);
                    break;
            }
            return result.Skip(offset).Take(count);
        }

        public ProcessResult MakeRoutePublic(int routeId, int userId, Translation translation)
        {
            User user = DataManager.Users.Find(userId);
            if (user == null)
                return ProcessResults.UserNotFound;
            if (user.HasLowExperience)
                return ProcessResults.LowExperience;
            Route route = GetRoute(routeId);
            if (route == null)
                return ProcessResults.RouteNotFound;
            if (user.Id != route.Author)
                return ProcessResults.InvalidAuthor;
            if (route.Status == RouteStatus.Public.Id)
                return ProcessResults.RouteAlreadyPublic;
            route.Status = (short)RouteStatus.Public.Id;
            route.Translation.Copy(translation);
            Data.SaveChanges();
            return ProcessResults.RouteIsPublicNow;
        }

        private Bookmark GetBookmark(int id)
        {
            return Data.Bookmarks.FirstOrDefault(x => x.Id == id);
        }

        private Bookmark GetBookmark(int userId, int routeId)
        {
            return Data.Bookmarks.FirstOrDefault(x => x.UserId == userId && x.RouteId == routeId);
        }

        public ProcessResult AddBookmark(int userId, int routeId)
        {
            User user = DataManager.Users.Find(userId);
            if (user == null)
                return ProcessResults.UserNotFound;
            Route route = GetRoute(routeId);
            if (route == null)
                return ProcessResults.RouteNotFound;
            if (route.Status != RouteStatus.Public.Id)
                return ProcessResults.RouteIsNotPublic;
            if (GetBookmark(userId, routeId) != null)
                return ProcessResults.BookmarkExists;
            Bookmark bookmark = Data.Bookmarks.Add(new Bookmark { RouteId = routeId, UserId = userId });
            Data.SaveChanges();
            ProcessResult result = ProcessResults.BookmarkAdded;
            result.AffectedObjectId = bookmark.Id;
            return result;
        }

        public ProcessResult RemoveBookmark(int id)
        {
            Bookmark bookmark = GetBookmark(id);
            if (bookmark == null)
                return ProcessResults.BookmarkNotFound;
            if (bookmark.Status == TrackingStatus.Current.Id)
                return ProcessResults.CannotRemoveCurrentBookmark;
            if (bookmark.Route.Status == RouteStatus.Private.Id)
            {
                Data.Routes.Remove(bookmark.Route);//Тут подействует CASCADE
            }
            Data.Bookmarks.Remove(bookmark);
            Data.SaveChanges();
            return ProcessResults.BookmarkRemoved;
        }

        //начать или пройти заново маршрут
        public ProcessResult StartTracking(int bookmarkId)
        {
            Bookmark bookmark = GetBookmark(bookmarkId);
            if (bookmark == null)
                return ProcessResults.BookmarkNotFound;
            if (bookmark.User.Bookmarks.Any(x => x.Status == TrackingStatus.Current.Id))
                return ProcessResults.AnotherRouteIsTracking;
            bookmark.Status = (byte)TrackingStatus.Current.Id;
            var checkIns = GetCheckInsOfTheRoute(bookmark.RouteId, bookmark.UserId).ToList();
            //если пользователь уже проходил маршрут
            if (checkIns.Any())
            {
                foreach (CheckIn checkIn in checkIns)
                {
                    checkIn.Time = null;
                }
            }
            else
            {
                checkIns = bookmark.Route.CheckPoints.Select(checkPoint => new CheckIn { CheckPointId = checkPoint.Id, UserId = bookmark.UserId }).ToList();
                Data.CheckIns.AddRange(checkIns);
            }
            Data.SaveChanges();
            return ProcessResults.TrackingStarted;
        }

        private IEnumerable<CheckIn> GetCheckInsOfTheRoute(int routeId, int userId)
        {
            return Data.CheckIns.Where(x => x.CheckPoint.RouteId == routeId && x.UserId == userId);
        }

        public IEnumerable<Bookmark> GetUserBookmarks(int userId, int offset = 0, int count = 20, string search = null, Culture culture = Culture.En)
        {
            var routes = new List<Bookmark>();
            var source = Data.Bookmarks.Where(x => x.UserId == userId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (Bookmark bookmark in source)
                {
                    if (bookmark.Route.Contains(search, culture))
                        routes.Add(bookmark);
                }
            }
            else
                routes = source.ToList();
            return routes.Skip(offset).Take(count);
        }

        public Route GetRoute(int routeId, bool notHidden = true)
        {
            return notHidden
                ? Data.Routes.FirstOrDefault(x => x.Id == routeId && x.Status != RouteStatus.Hidden.Id)
                : Data.Routes.FirstOrDefault(x => x.Id == routeId);
        }

        public ProcessResult CreateRoute(Route route, string[] places)
        {
            return CreateRoute(route, places.Select(place => Convert.ToInt32(place)).ToArray());
        }

        public ProcessResult CreateRoute(Route route, int[] places)
        {
            try
            {
                User user = DataManager.Users.Find(route.Author);
                if (user == null) return ProcessResults.InvalidAuthor;
                if (Translation.IsInValid(route.Translation))
                    return ProcessResults.InvalidTranslations;
                if (places == null || places.Count() < 2)
                    return ProcessResults.TooFewCheckPoints;
                Data.Translations.Add(route.Translation); //переводу приписвается id
                Data.SaveChanges();
                route.Name = route.Translation.Id;
                Data.Routes.Add(route);
                Data.SaveChanges();//route приписывается id
                var checkPoints = places.Select(place => new CheckPoint { PlaceId = place, RouteId = route.Id }).ToList();
                Data.CheckPoints.AddRange(checkPoints);
                Data.Bookmarks.Add(new Bookmark { RouteId = route.Id, UserId = user.Id });
                Data.SaveChanges();
                ProcessResult result = ProcessResults.RouteCreated;
                result.AffectedObjectId = route.Id;
                return result;
            }
            catch (Exception)
            {
                return ProcessResults.Error;
            }
        }

        public ProcessResult AbortTracking(int bookmarkId)
        {
            Bookmark bookmark = GetBookmark(bookmarkId);
            if (bookmark == null)
                return ProcessResults.BookmarkNotFound;
            if (bookmark.Status != TrackingStatus.Current.Id)
                return ProcessResults.RouteIsNotTracking;
            bookmark.Status = (byte)TrackingStatus.NotStarted.Id;
            Data.SaveChanges();
            return ProcessResults.TrackingAborted;
        }

        public ProcessResult CheckIn(int bookmarkId, int checkPointId)
        {
            Bookmark bookmark = GetBookmark(bookmarkId);
            if (bookmark == null)
                return ProcessResults.BookmarkNotFound;
            if (bookmark.Status != TrackingStatus.Current.Id)
                return ProcessResults.RouteIsNotTracking;
            Route route = bookmark.Route;
            var checkPoints = route.CheckPoints.ToList();
            var checkPoint = checkPoints.FirstOrDefault(x => x.Id == checkPointId);
            if (checkPoint == null)
                return ProcessResults.CheckPointNotFound;
            var checkIn = checkPoint.CheckIns.FirstOrDefault(x => x.UserId == bookmark.UserId);
            if (checkIn == null)
                return ProcessResults.CheckInNotFound;
            if (checkIn.Time != null)
                return ProcessResults.CheckinAlreadyChecked;
            DateTime time = DateTime.Now;
            int index = checkPoints.IndexOf(checkPoint);
            if (index == 0)
                checkPoint.SetNewAverageTime(0);
            else
            {
                var prevCheckIn = checkPoints[index - 1].CheckIns.FirstOrDefault(x => x.UserId == bookmark.UserId);
                if (prevCheckIn == null || !prevCheckIn.Time.HasValue)
                    return ProcessResults.InvalidPreviousCheckIn;
                checkPoint.SetNewAverageTime(time.Subtract(prevCheckIn.Time.Value).TotalMinutes);
            }
            checkIn.Time = time;
            route.Rating++;
            checkPoint.Place.Rating++;
            checkIn.User.Experience++;
            Data.SaveChanges();
            if (checkIn.CheckPoint == checkPoints.Last())
            {
                bookmark.Status = (byte)TrackingStatus.Finished.Id;
                Data.SaveChanges();
                return ProcessResults.TrackingStopped;
            }
            return ProcessResults.CheckInSuccessful;
        }

        public ProcessResult RemovePublicRoute(int routeId, int userId)
        {
            User user = DataManager.Users.Find(userId);
            if (user == null) return ProcessResults.UserNotFound;
            Route route = GetRoute(routeId);
            if (route == null) return ProcessResults.RouteNotFound;
            if (route.Status != RouteStatus.Public.Id)
                return ProcessResults.RouteIsNotPublic;
            if (user.Id != route.Author)
                return ProcessResults.InvalidAuthor;
            Bookmark bookmark = GetBookmark(userId, routeId);
            if (bookmark != null)
            {
                if (bookmark.Status == TrackingStatus.Current.Id)
                    return ProcessResults.CannotRemoveCurrentBookmark;
                Data.Bookmarks.Remove(bookmark);
            }
            //скрываем маршрут
            route.Status = (short)RouteStatus.Hidden.Id;
            Data.SaveChanges();
            return ProcessResults.RouteDeleted;
        }
    }
}