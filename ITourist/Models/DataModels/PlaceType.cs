using System.Collections.Generic;
using System.Linq;

namespace ITourist.Models.DataModels
{
    public class PlaceType : Translatable
    {
        public short Id { get; private set; }

        public Translation Translation { get; private set; }

        public PlaceType(short id, Translation translation)
        {
            Id = id;
            Translation = translation;
        }

        public string GetName(Culture culture)
        {
            return GetTranslation(culture, Translation);
        }
    }

    public class PlaceTypes
    {
        private static readonly List<PlaceType> Types = new List<PlaceType>(
            new[]
            {
                new PlaceType(0, new Translation("Theatres", "Театры")),
                new PlaceType(1, new Translation("Museums","Музеи")),
                new PlaceType(2, new Translation("Parks and gardens","Парки и сады")),
                new PlaceType(3, new Translation("Architectural sights","Архитектурные достопримечательности")), 
                new PlaceType(4, new Translation("Natural sights","Природные достопримечательности")), 
                new PlaceType(5, new Translation("Entertaning sights","Развлекательные достопримечательности")), 
                new PlaceType(6, new Translation("Religious sights","Религиозные достопричательности")),
                 new PlaceType(7, new Translation("Institutions","Учебные заведения")),
            });

        public static PlaceType GetPlaceType(int placeType)
        {
            return Types.FirstOrDefault(type => type.Id == placeType);
        }

        public static IEnumerable<PlaceType> GetAllPlaceTypes()
        {
            return Types;
        }
    }
}