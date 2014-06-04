using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ITourist.Models.DataModels;
using ITourist.Models.LogicModels.Services;

namespace ITourist.Models.LogicModels.Managers
{
    public class RegionManager : Manager
    {
        private int tempMax;

        public RegionManager(ITouristDatabaseEntities entities)
            : base(entities)
        {

        }

        public IEnumerable<Region> GetAll()
        {
            return Data.Regions;
        }

        public IEnumerable<Region> GetRegionsOfTheCountry(int id)
        {
            return Data.Regions.Where(x => x.CountryId == id);
        }

        public IEnumerable<Region> GetRegions(int? countryId, int page, int regionsPerPage, out int? max, out Country country, Culture culture = Culture.En, string search = null, Order order = Order.Default)
        {
            var n = GetRegions(countryId, (page - 1) * regionsPerPage, regionsPerPage, culture, search, order);
            max = tempMax;
            country = countryId.HasValue ? DataManager.Countries.GetCountry(countryId.Value) : null;
            return n;
        }

        public IEnumerable<Region> GetRegions(int? countryId = null, int offset = 0, int count = 20, Culture culture = Culture.En, string search = null, Order order = Order.Default)
        {
            var temp = countryId.HasValue ? Data.Regions.Where(x => x.CountryId == countryId.Value) : Data.Regions;
            var regions = new List<Region>();
            if (!string.IsNullOrWhiteSpace(search))
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (Region region in temp)
                {
                    if (region.Contains(search))
                        regions.Add(region);
                }
            }
            else
                regions = temp.ToList();
            tempMax = (int)Math.Ceiling((decimal)regions.Count() / count);
            IEnumerable<Region> result = regions;
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


        public Region GetRegion(int id)
        {
            return Data.Regions.FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Region> SelectRegionsLike(int countryId, string name)
        {
            return Data.Regions.Where(x => x.CountryId == countryId && x.Contains(name));
        }

        public Region GetRegion(string nameRussian, string nameEnglish,int countryId)
        {
            return Data.Regions.FirstOrDefault(x => x.CountryId==countryId&&(x.Translation.En == nameEnglish || x.Translation.Ru == nameRussian));
        }

        public ProcessResult AddRegion(string nameRussian, string nameEnglish, int countryId, HttpPostedFileBase imageUpload, HttpServerUtilityBase server)
        {
            if (String.IsNullOrWhiteSpace(nameRussian) || String.IsNullOrWhiteSpace(nameEnglish))
                return ProcessResults.TitleCannotBeEmpty;
            var region = GetRegion(nameRussian, nameEnglish,countryId);
            if (region != null)
                return ProcessResults.RegionAlreadyExists;
            var t = new Translation { En = nameEnglish, Ru = nameRussian };
            t = Data.Translations.Add(t);
            Data.SaveChanges();
            region = Data.Regions.Add(new Region { Name = t.Id, CountryId = countryId });
            Data.SaveChanges();
            if (imageUpload != null)
            {
                if (imageUpload.ContentLength <= 0 || !SecurityManager.IsImage(imageUpload))
                    return ProcessResults.InvalidImageFormat;
                region.Image = SaveImage(region.Id, StaticSettings.RegionsUploadFolderPath, imageUpload, server);
                Data.SaveChanges();
            }
            ProcessResult result = ProcessResults.RegionAdded;
            result.AffectedObjectId = region.Id;
            return result;
        }

        public ProcessResult EditRegion(int id, string nameRus, string nameEng, int countryId, HttpPostedFileBase imageUpload, bool deleteImage, HttpServerUtilityBase server)
        {
            Region region = GetRegion(id);
            if (String.IsNullOrEmpty(nameRus) || String.IsNullOrEmpty(nameEng))
                return ProcessResults.TitleCannotBeEmpty;
            if (region == null)
                return ProcessResults.NoSuchRegion;
            Region r = GetRegion(nameRus, nameEng, countryId);
            if (r != null && r.Id != region.Id)
                return ProcessResults.RegionAlreadyExists;
            region.Translation.En = nameEng;
            region.Translation.Ru = nameRus;
            region.CountryId = countryId;
            if (deleteImage && imageUpload == null)
            {
                DeleteImage(region.Image, server);
                region.Image = null;
            }
            else if (imageUpload != null)
            {
                if (imageUpload.ContentLength <= 0 || !SecurityManager.IsImage(imageUpload))
                    return ProcessResults.InvalidImageFormat;
                region.Image = SaveImage(region.Id, StaticSettings.RegionsUploadFolderPath, imageUpload, server);
            }
            Data.SaveChanges();
            ProcessResult result = ProcessResults.RegionEdited;
            result.AffectedObjectId = region.Id;
            return result;
        }


        public ProcessResult DeleteRegion(int id, HttpServerUtilityBase server)
        {
            Region region = GetRegion(id);
            if (region == null)
                return ProcessResults.NoSuchRegion;
            string img = region.Image;
            Data.Regions.Remove(region);
            Data.SaveChanges();
            DeleteImage(img, server);
            return ProcessResults.RegionDeleted;
        }
    }
}