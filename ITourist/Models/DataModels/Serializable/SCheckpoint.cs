namespace ITourist.Models.DataModels.Serializable
{
    public class SCheckPoint
    {
        public int Id { get; set; }
        public SPlace Place { get; set; }

        public SCheckPoint(CheckPoint checkPoint, SPlace place)
        {
            Id = checkPoint.Id;
            Place = place;
        }
    }

    public class SCheckPointExtended : SCheckPoint
    {
        public double AverageTime { get; private set; }
        public string AverageTimeFormatted { get; private set; }

        public SCheckPointExtended(CheckPoint checkPoint, SPlace place,Culture culture)
            : base(checkPoint,place)
        {
            AverageTime = checkPoint.AverageTime;
            AverageTimeFormatted = checkPoint.FormatAverageTime(culture);
        }
    }
}