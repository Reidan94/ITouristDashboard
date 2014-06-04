using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITourist.Models.DataModels.Serializable
{
    public class SRegion : Serializable
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double Rating { get; private set; }
        public string Image { get; private set; }

        public SRegion(HttpContextBase context, Region region, Culture culture = Culture.En)
        {
            Id = region.Id;
            Name = region.GetName(culture);
            Rating = region.Rating;
            Image = DefineImagePath(context, region.Image);
        }
    }

    public class SRegionExtended : SRegion
    {
        public int CountryId { get; private set; }
        public string CountryName { get; private set; }

        public SRegionExtended(HttpContextBase context, Region region, Culture culture = Culture.En)
            : base(context, region, culture)
        {
            CountryId = region.CountryId;
            CountryName = region.Country.GetName(culture);
        }
    }

}