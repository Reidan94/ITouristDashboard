namespace ITourist.Models.DataModels.Serializable
{
    public class SBookmark
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public SRoute Route { get; set; }

        public SBookmark(Bookmark bookmark, SRoute route, Culture culture)
        {
            Id = bookmark.Id;
            Status = bookmark.GetStatus(culture);
            Route = route;
        }
    }
}