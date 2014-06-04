using System;
using System.Collections.Generic;
using System.Linq;

namespace ITourist.Models.DataModels
{
    public partial class Route : Translatable
    {
        public string GetName(Culture culture)
        {
            return GetTranslation(culture, Translation);
        }

        public string GetStatus(Culture culture)
        {
            return RouteStatus.GetStatus(Status).GetName(culture);
        }

        public bool Contains(string s)
        {
            return Contains(Translation, s);
        }

        public bool Contains(string s, Culture culture)
        {
            return Contains(Translation, s, culture);
        }

        public double RelativeRating
        {
            get { return Rating / CheckPoints.Count; }
        }

        public double AverageTime
        {
            get { return CheckPoints.Sum(x => x.AverageTime); }
        }

        public string FormatAverageTime(Culture culture)
        {
            string s = NumberTranslator.TimeToString(TimeSpan.FromMinutes(AverageTime), culture);
            return String.IsNullOrEmpty(s) ? GetTranslation(culture, new Translation("unknown", "неизвестно")) : s;
        }

        public IEnumerable<Place> Places
        {
            get { return CheckPoints.Select(checkPoint => checkPoint.Place); }
        } 
    }

    public class RouteStatus
    {
        public static Status Hidden
        {
            get { return new Status(-1, new Translation("Hidden", "Скрытый")); }
        }

        public static Status Private
        {
            get { return new Status(0, new Translation("Private", "Частный")); }
        }

        public static Status Public
        {
            get { return new Status(1, new Translation("Public", "Публичный")); }
        }

        public static Status GetStatus(short status)
        {
            return status == Hidden.Id ? Hidden
                  : status == Private.Id ? Private
                  : status == Public.Id ? Public
                  : null;
        }

        public static IEnumerable<Status> GetAll(Culture culture)
        {
            return new[]
            {
                Hidden,
                Private,
                Public
            };
        }
    }


    public class TrackingStatus
    {
        public static Status NotStarted
        {
            get { return new Status(0, new Translation("Not started", "Не начат")); }
        }

        public static Status Current
        {
            get { return new Status(1, new Translation("Current", "Текущий")); }
        }

        public static Status Finished
        {
            get { return new Status(2, new Translation("Finished", "Закончен")); }
        }

        public static Status GetStatus(short status)
        {
            return status == NotStarted.Id ? NotStarted
                 : status == Current.Id ? Current
                 : status == Finished.Id ? Finished
                 : null;
        }

        public static IEnumerable<Status> GetAll()
        {
            return new[]
            {
                NotStarted,
                Current,
                Finished
            };
        }
    }
}