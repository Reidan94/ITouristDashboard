using System.Collections.Generic;
using System.Linq;

namespace ITourist.Models.DataModels
{
    public partial class Region : Translatable
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
            get
            {
                return Places.Sum(place => place.Rating);
            }
        }
        
        public List<Place> GetPlaces()
        {
            return Places.Where(x => x.Confirmed).ToList();
        } 
    }
}