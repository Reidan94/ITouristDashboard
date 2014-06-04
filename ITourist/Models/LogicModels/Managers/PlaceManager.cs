using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using ITourist.Models.DataModels;
using ITourist.Models.LogicModels.Services;

namespace ITourist.Models.LogicModels.Managers
{
    public class PlaceManager : Manager
    {
        private int tempMax;

        public PlaceManager(ITouristDatabaseEntities entities)
            : base(entities)
        {

        }

        public IEnumerable<Place> GetAllPlaces(int? placeTypeId = null, bool confirmedOnly = true)
        {
            if (placeTypeId.HasValue)
                return confirmedOnly ? Data.Places.Where(x => x.Confirmed && x.TypeId == placeTypeId.Value) : Data.Places.Where(x => x.TypeId == placeTypeId.Value);
            return confirmedOnly ? Data.Places.Where(x => x.Confirmed) : Data.Places;
        }

        public IQueryable<Place> GetPlacesOfTheRegion(int regionId, bool confirmedOnly = true)
        {
            return confirmedOnly
                ? Data.Places.Where(place => place.RegionId == regionId && place.Confirmed)
                : Data.Places.Where(place => place.RegionId == regionId);
        }

        public IEnumerable<Place> GetPlaces(int? regionId, int page, int regionsPerPage, out int? max, out Region region, Culture culture = Culture.En, string search = null, int? placeTypeId = null, Order order = Order.Default)
        {
            var n = GetPlaces(regionId, (page - 1) * regionsPerPage, regionsPerPage, search, culture, placeTypeId, true, order);
            max = tempMax;
            region = regionId.HasValue ? DataManager.Regions.GetRegion(regionId.Value) : null;
            return n;
        }

        public IEnumerable<Place> GetPlaces(int? regionId, int offset, int count, string search = null, Culture culture = Culture.En, int? placeTypeId = null, bool confirmedOnly = true, Order order = Order.Default)
        {
            var places = new List<Place>();
            var source = regionId.HasValue ? GetPlacesOfTheRegion(regionId.Value, confirmedOnly) : GetAllPlaces(placeTypeId, confirmedOnly);
            if (!string.IsNullOrWhiteSpace(search))
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (Place place in source)
                {
                    if (place.Contains(search))
                        places.Add(place);
                }
            }
            else
                places = source.ToList();
            tempMax = (int)Math.Ceiling((decimal)places.Count() / count);
            IEnumerable<Place> result = places;
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


        public IEnumerable<Place> GetPopularPlaces(Culture culture)
        {
            return GetPlaces(null, 0, 3, culture: culture, order: Order.Popularity);
        }

        public Place GetPlace(int placeId, bool confirmedOnly = true)
        {
            return confirmedOnly
                ? Data.Places.FirstOrDefault(place => place.Id == placeId && place.Confirmed)
                : Data.Places.FirstOrDefault(place => place.Id == placeId);
        }

        public IEnumerable<Place> GetPlaces(IEnumerable<int> placesId, bool confirmedOnly = true)
        {
            return confirmedOnly
                ? placesId.Select(id => Data.Places.FirstOrDefault(place => place.Id == id && place.Confirmed))
                : placesId.Select(id => Data.Places.FirstOrDefault(place => place.Id == id));
        }

        public IEnumerable<Place> GetPlaces(IEnumerable<string> places)
        {
            if (places == null)
                return null;
            return GetPlaces(places.Select(id => Convert.ToInt32(id)));
        }

        public ProcessResult AddComment(int placeId, int userId, string message)
        {
            if (String.IsNullOrWhiteSpace(message))
                return ProcessResults.CommentCannotBeEmpty;
            Place place = GetPlace(placeId);
            if (place == null)
                return ProcessResults.PlaceNotFound;
            Data.Comments.Add(new Comment { PlaceId = placeId, DateTime = DateTime.Now, UserId = userId, Message = message });
            Data.SaveChanges();
            return ProcessResults.CommentAdded;
        }

        public IEnumerable<Place> GetNearestPlaces(double x, double y, int count = 10)
        {
            return Data.Places.OrderBy(place => (place.X - x) * (place.X - x) + (place.Y - y) * (place.Y - y)).Take(count);
        }

        public IEnumerable<Photo> GetPhotosOfThePlace(int placeId, bool confirmedOnly = true)
        {
            return confirmedOnly
                ? Data.Photos.Where(p => p.PlaceId == placeId && p.Place.Confirmed)
                : Data.Photos.Where(p => p.PlaceId == placeId);
        }

        public IEnumerable<Photo> GetPhotosOfThePlace(int placeId, int offset, int count, bool confirmedOnly = true)
        {
            return GetPhotosOfThePlace(placeId, confirmedOnly).Skip(offset).Take(count);
        }


        public IEnumerable<LikesToPlace> GetLikesToThePlace(int placeId)
        {
            return Data.LikesToPlaces.Where(x => x.PlaceId == placeId && x.Place.Confirmed);
        }

        public IEnumerable<LikesToPlace> GetLikesToThePlace(int placeId, int offset, int count)
        {
            return Data.LikesToPlaces.Where(x => x.PlaceId == placeId && x.Place.Confirmed).Skip(offset).Take(count);
        }

        public IEnumerable<Comment> GetCommentsOfThePlace(int placeId)
        {
            return Data.Comments.Where(x => x.PlaceId == placeId && x.Place.Confirmed);
        }

        public IEnumerable<Comment> GetCommentsOfThePlace(int placeId, int offset, int count)
        {
            return Data.Comments.Where(x => x.PlaceId == placeId && x.Place.Confirmed).Skip(offset).Take(count);
        }

        public Place GetPlace(Translation translation, int regionId)
        {
            return Data.Places.FirstOrDefault(x => (x.Translation.En == translation.En || x.Translation.Ru == translation.Ru) && x.RegionId == regionId);
        }

        public ProcessResult AddPlace(Place place, HttpPostedFileBase imageUpload, HttpServerUtilityBase server)
        {
            if (Translation.IsInValid(place.Translation) || Translation.IsInValid(place.Translation1))
                return ProcessResults.InvalidTranslations;
            if (GetPlace(place.Translation, place.RegionId) != null)
                return ProcessResults.PlaceAlreadyExists;
            var t = Data.Translations.Add(place.Translation);
            Data.SaveChanges();
            place.Name = t.Id;
            t = Data.Translations.Add(t);
            Data.SaveChanges();
            place.Description = t.Id;
            place = Data.Places.Add(place);
            try
            {
                Data.SaveChanges();
            }
            catch (Exception)
            {
                Data.Places.Remove(place);
                Data.SaveChanges();
                return ProcessResults.Error;
            }

            if (imageUpload != null)
            {
                if (imageUpload.ContentLength <= 0 || !SecurityManager.IsImage(imageUpload))
                    return ProcessResults.InvalidImageFormat;
                place.Image = SaveImage(place.Id, StaticSettings.PlacesUploadFolderPath, imageUpload, server);
                Data.SaveChanges();
            }
            ProcessResult result = ProcessResults.PlaceAdded;
            result.AffectedObjectId = place.Id;
            return result;
        }

        public ProcessResult AddPlacePhoto(int placeId, HttpPostedFileBase imageUpload, HttpServerUtilityBase server)
        {
            if (imageUpload == null || imageUpload.ContentLength <= 0 || !SecurityManager.IsImage(imageUpload))
                return ProcessResults.InvalidImageFormat;
            var photo = new Photo
            {
                PlaceId = placeId,
                Image = ""
            };
            photo = Data.Photos.Add(photo);
            Data.SaveChanges();
            photo.Image = SavePlaceImage(placeId, photo.Id, StaticSettings.PlacesUploadFolderPath, imageUpload, server);
            Data.SaveChanges();
            return ProcessResults.ImageWasSuccessfullyAdded;
        }

        public ProcessResult EditPlace(Place place, bool deleteImage, HttpPostedFileBase imageUpload, HttpServerUtilityBase server)
        {
            if (Translation.IsInValid(place.Translation) || Translation.IsInValid(place.Translation1))
                return ProcessResults.TitleCannotBeEmpty;
            var p = GetPlace(place.Translation, place.RegionId);
            if (p != null && p.Id != place.Id)
                return ProcessResults.PlaceAlreadyExists;
            p = GetPlace(place.Id);
            if (p == null)
                return ProcessResults.PlaceNotFound;
            p.Translation.En = place.Translation.En;
            p.Translation.Ru = place.Translation.Ru;
            p.Translation1.En = place.Translation1.En;
            p.Translation1.Ru = place.Translation1.Ru;
            p.TypeId = place.TypeId;
            p.X = place.X;
            p.Y = place.Y;
            if (deleteImage && imageUpload == null)
            {
                DeleteImage(p.Image, server);
            }
            else if (imageUpload != null)
            {
                if (imageUpload.ContentLength <= 0 || !SecurityManager.IsImage(imageUpload))
                    return ProcessResults.InvalidImageFormat;
                p.Image = SaveImage(place.Id, StaticSettings.PlacesUploadFolderPath, imageUpload, server);
            }
            Data.SaveChanges();
            return ProcessResults.PlaceEdited;
        }

        public ProcessResult DeletePlace(int id, HttpServerUtilityBase server)
        {
            var place = GetPlace(id);
            if (place == null)
                return ProcessResults.PlaceNotFound;
            Data.Places.Remove(place);
            Data.SaveChanges();
            DeleteImage(place.Image, server);
            var imgDirectory = server.MapPath("~") + StaticSettings.PlacesUploadFolderPath + "/" + id;
            if (Directory.Exists(imgDirectory))
                Directory.Delete(imgDirectory, true);
            return ProcessResults.PlaceDeleted;
        }


        private string SavePlaceImage(int id, int photoId, string folder, HttpPostedFileBase imageUpload, HttpServerUtilityBase server)
        {
            var folderPath = server.MapPath("~") + folder + id;
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var relativePath = folder + id + "/" + photoId + Path.GetExtension(imageUpload.FileName);
            imageUpload.SaveAs(server.MapPath("~") + relativePath);
            return relativePath;
        }

        public Photo GetPhotoOfThePlace(int photoId)
        {
            return Data.Photos.FirstOrDefault(x => x.Id == photoId);
        }

        private Comment GetComment(int commentId)
        {
            return Data.Comments.FirstOrDefault(x => x.Id == commentId);
        }

        public ProcessResult DeletePlaceComment(int commentId)
        {
            var comment = GetComment(commentId);
            if (comment == null)
                return ProcessResults.CommentNotFound;

            Data.Comments.Remove(comment);
            Data.SaveChanges();
            return ProcessResults.CommentWasDeleted;
        }

        public ProcessResult DeletePlacePhoto(int photoId, HttpServerUtilityBase server)
        {
            var photo = GetPhotoOfThePlace(photoId);
            if (photo == null)
                return ProcessResults.PhotoNotExisting;
            DeleteImage(photo.Image, server);
            foreach (var curPhoto in Data.Photos)
            {
                if (curPhoto.PlaceId == photo.PlaceId && curPhoto.Image == photo.Image)
                    Data.Photos.Remove(curPhoto);
            }
            Data.Photos.Remove(photo);
            Data.SaveChanges();
            return ProcessResults.PhotoWasDeleted;
        }
    }

}