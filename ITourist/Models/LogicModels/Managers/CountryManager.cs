using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ITourist.Models.DataModels;
using ITourist.Models.LogicModels.Services;

namespace ITourist.Models.LogicModels.Managers
{
    public class CountryManager : Manager
    {
        private int tempMax;

        public CountryManager(ITouristDatabaseEntities entities)
            : base(entities)
        {

        }

        public IEnumerable<Country> GetAllCountries(bool withRegionsOnly = false)
        {
            return withRegionsOnly ? Data.Countries.Where(x => x.Regions.Any()) : Data.Countries;
        }

        public IEnumerable<Country> GetAllCountries(Culture culture, bool withRegionsOnly = false)
        {
            return GetAllCountries(withRegionsOnly).ToList().OrderBy(x => x.GetName(culture));
        }

        public IEnumerable<Country> GetCountries(int page, int countriesPerPage, out int? max, Culture culture = Culture.En, string search = null, Order order = Order.Default, bool withRegionsOnly = false)
        {
            var n = GetCountries((page - 1) * countriesPerPage, countriesPerPage, culture, search, order,withRegionsOnly);
            max = tempMax;
            return n;
        }

        public List<Country> GetCountries(int offset, int count, Culture culture = Culture.En, string search = null, Order order = Order.Default, bool withRegionsOnly = false)
        {

            var countries = new List<Country>();
            if (!string.IsNullOrWhiteSpace(search))
            {
                // ReSharper disable once LoopCanBeConvertedToQuery
                foreach (Country country in GetAllCountries(culture, withRegionsOnly))
                {
                    if (country.Contains(search))
                        countries.Add(country);
                }
            }
            else
                countries = GetAllCountries(culture, withRegionsOnly).ToList();
            tempMax = (int)Math.Ceiling((decimal)countries.Count() / count);
            IEnumerable<Country> result = countries;
            switch (order)
            {
                case Order.Name:
                    result = result.OrderBy(x => x.GetName(culture));
                    break;
                case Order.Popularity:
                    result = result.OrderByDescending(x => x.Rating);
                    break;
            }
            return result.Skip(offset).Take(count).ToList();
        }

        public Country GetCountry(int id)
        {
            return Data.Countries.FirstOrDefault(x => x.Id == id);
        }
        public Country GetCountry(string nameRussian, string nameEnglish)
        {
            return Data.Countries.FirstOrDefault(x => x.Translation.En == nameEnglish || x.Translation.Ru == nameRussian);
        }

        public ProcessResult AddCountry(string nameRussian, string nameEnglish, HttpPostedFileBase imageUpload, HttpServerUtilityBase server)
        {
            if (String.IsNullOrWhiteSpace(nameRussian) || String.IsNullOrWhiteSpace(nameEnglish))
                return ProcessResults.TitleCannotBeEmpty;
            var country = GetCountry(nameRussian,nameEnglish);
            if (country != null)
                return ProcessResults.CountryAlreadyExists;
            var t = new Translation { En = nameEnglish, Ru = nameRussian };
            t = Data.Translations.Add(t);
            Data.SaveChanges();
            country = Data.Countries.Add(new Country { Name = t.Id });
            Data.SaveChanges();
            if (imageUpload != null)
            {
                if (imageUpload.ContentLength <= 0 || !SecurityManager.IsImage(imageUpload))
                    return ProcessResults.InvalidImageFormat;
                country.Image = SaveImage(country.Id, StaticSettings.CountriesUploadFolderPath, imageUpload, server);
                Data.SaveChanges();
            }
            ProcessResult result = ProcessResults.CountryAdded;
            result.AffectedObjectId = country.Id;
            return result;
        }

        public ProcessResult EditCountry(int id, string nameRus, string nameEng, HttpPostedFileBase imageUpload, bool deleteImage, HttpServerUtilityBase server)
        {
            Country country = GetCountry(id);
            if (String.IsNullOrEmpty(nameRus) || String.IsNullOrEmpty(nameEng))
                return ProcessResults.TitleCannotBeEmpty;
            if (country == null)
                return ProcessResults.NoSuchCountry;
            country.Translation.En = nameEng;
            country.Translation.Ru = nameRus;
            if (deleteImage && imageUpload == null)
            {
                DeleteImage(country.Image, server);
                country.Image = null;
            }
            else if(imageUpload!=null)
            {
                if (imageUpload.ContentLength <= 0 || !SecurityManager.IsImage(imageUpload))
                    return ProcessResults.InvalidImageFormat;
                country.Image = SaveImage(country.Id, StaticSettings.CountriesUploadFolderPath, imageUpload, server);
            }
            Data.SaveChanges();
            ProcessResult result = ProcessResults.CountryEdited;
            result.AffectedObjectId = country.Id;
            return result;
        }


        public ProcessResult DeleteCoutnry(int id, HttpServerUtilityBase server)
        {
            Country country = GetCountry(id);
            if (country == null)
                return ProcessResults.NoSuchCountry;
            string img = country.Image;
            Data.Countries.Remove(country);
            Data.SaveChanges();
            DeleteImage(img, server);
            return ProcessResults.CountryDeleted;
        }
    }
}