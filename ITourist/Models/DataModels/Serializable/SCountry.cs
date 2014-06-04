using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITourist.Models.DataModels.Serializable
{
    public class SCountry : Serializable
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double Rating { get; private set; }
        public string Image { get; private set; }

        private SCountry(HttpContextBase context, Country country, Culture culture = Culture.En)
        {
            Id = country.Id;
            Name = country.GetName(culture);
            Image = DefineImagePath(context, country.Image);
            Rating = country.Rating;
        }

        public static SCountry Convert(HttpContextBase context, Country country, Culture culture = Culture.En)
        {
            if (country == null) return null;
            return new SCountry(context, country, culture);
        }

        public static IEnumerable<SCountry> Convert(HttpContextBase context, IEnumerable<Country> countries,  Culture culture = Culture.En)
        {
            var enumerable = countries as List<Country> ?? countries.ToList();
            if (!enumerable.Any()) return null;
            return enumerable.Select(country => new SCountry(context, country, culture));
        }
    }
}