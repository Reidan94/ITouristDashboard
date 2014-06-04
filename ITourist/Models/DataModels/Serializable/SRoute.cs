using System.Collections.Generic;

namespace ITourist.Models.DataModels.Serializable
{

    public class SRoute
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Rating { get; set; }
        public int AuthorId { get; set; }
        public string Status { get; set; }
        public IEnumerable<SCheckPoint> CheckPoints { get; set; }

        public SRoute(Route route, IEnumerable<SCheckPoint> checkPoints, Culture culture)
        {
            Id = route.Id;
            Name = route.GetName(culture);
            Rating = route.RelativeRating;
            AuthorId = route.Author;
            Status = RouteStatus.GetStatus(route.Status).GetName(culture);
            CheckPoints = checkPoints;
        }
    }

    public class SRouteExtended : SRoute
    {
        public double AverageTime { get; private set; }
        public string AverageTimeFormatted { get; private set; }
        public int NumberOfCheckPoints { get; set; }
        public string AuthorName { get; set; }

        public SRouteExtended(Route route, IEnumerable<SCheckPoint> checkPoints, Culture culture)
            : base(route, checkPoints, culture)
        {
            NumberOfCheckPoints = route.CheckPoints.Count;
            AverageTime = route.AverageTime;
            AverageTimeFormatted = route.FormatAverageTime(culture);
            AuthorName = route.User.FullName;
        }
    }
}