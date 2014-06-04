using System;
using System.Globalization;
using System.Linq;

namespace ITourist.Models.DataModels
{
    public partial class Place : Translatable
    {
        public string GetName(Culture culture)
        {
            return GetTranslation(culture, Translation);
        }

        public string GetDescription(Culture culture)
        {
            return GetTranslation(culture, Translation1);
        }

        public bool Contains(string s)
        {
            return Contains(Translation, s);
        }

        public bool Contains(string s, Culture culture)
        {
            return Contains(Translation, s, culture);
        }

        public string Latitude
        {
            get { return X.ToString("0.000000", CultureInfo.InvariantCulture); }
            set { X = double.Parse(value, CultureInfo.InvariantCulture); }
        }

        public string Longitude
        {
            get { return Y.ToString("0.000000", CultureInfo.InvariantCulture); }
            set { Y = double.Parse(value, CultureInfo.InvariantCulture); }
        }

        public string Coordinates
        {
            get { return Latitude + "," + Longitude; }
        }

        public string CommentsCount(Culture culture)
        {
            return NumberTranslator.CommentsToString(Comments.Count, culture);
        }
    }
}