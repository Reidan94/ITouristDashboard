using System.Web;

namespace ITourist.Models.DataModels.Serializable
{
    public class SPlace : Serializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string Image { get; set; }

        public SPlace(HttpContextBase context, Place place, Culture culture = Culture.En)
        {
            Id = place.Id;
            Name = place.GetName(culture);
            Rating = place.Rating;
            X = place.X;
            Y = place.Y;
            Image = DefineImagePath(context, place.Image);
        }
    }
    public class SPlaceExtended : SPlace
    {
        public string Type { get; set; }
        public string Description { get; set; }
        public int RegionId { get; set; }
        public string RegionName { get; set; }

        public SPlaceExtended(HttpContextBase context, Place place, Culture culture = Culture.En)
            : base(context, place, culture)
        {
            Type = PlaceTypes.GetPlaceType(place.TypeId).GetName(culture);
            Description = place.GetDescription(culture);
            RegionId = place.RegionId;
            RegionName = place.Region.GetName(culture);
        }
    }
}