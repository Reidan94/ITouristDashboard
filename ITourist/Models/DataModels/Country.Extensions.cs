using System.Collections.Generic;
using System.Linq;

namespace ITourist.Models.DataModels
{
    public partial class Country : Translatable
    {
        public string GetName(Culture culture)
        {
            return GetTranslation(culture, Translation);
        }

        public bool Contains(string s)
        {
            return Contains(Translation, s);
        }

        public bool Contains(string s, Culture culture)
        {
            return Contains(Translation, s, culture);
        }

        public double Rating
        {
            get { return Regions.Sum(x => x.Rating); }
        }

        public static IEnumerable<Country> OrderAscending(IEnumerable<Country> countries, Culture culture)
        {
            switch (culture)
            {
                case Culture.Ru:
                    return countries.OrderBy(x => x.Translation.Ru);
                default:
                    return countries.OrderBy(x => x.Translation.En);
            }
        }

        public static IEnumerable<Country> OrderAscending(IEnumerable<Country> countries, string culture)
        {
            return OrderAscending(countries, DefineCulture(culture));
        }
    }
}