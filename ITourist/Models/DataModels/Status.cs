namespace ITourist.Models.DataModels
{
    public class Status:Translatable
    {
        public int Id { get; private set; }
        public Translation Translation { get; private set; }

        public Status(int id, Translation translation)
        {
            Id = id;
            Translation = translation;
        }

        public string GetName(Culture culture)
        {
            return GetTranslation(culture, Translation);
        }
    }
}