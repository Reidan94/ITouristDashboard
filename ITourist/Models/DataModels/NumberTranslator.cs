using System;

namespace ITourist.Models.DataModels
{
    public class NumberTranslator
    {
        private static string ValueToString(int value, string en1, string en2)
        {
            if (value <= 0)
                return "";
            if (value == 1)
                return value + " " + en1;
            return value + " " + en2;
        }

        private static string ValueToString(int value, string ru1, string ru2, string ru3)
        {
            if (value <= 0)
                return "";
            int lastDigit = value%10;
            if ((lastDigit >= 5 && lastDigit <= 9) || lastDigit == 0 || (value >= 11 && value <= 19))
                return value + " " + ru1;
            if (lastDigit == 1)
                return value + " " + ru2;
            return value + " " + ru3;
        }


        public static string TimeToString(TimeSpan timeSpan, Culture culture)
        {
            string s = "";
            switch (culture)
            {
                case Culture.Ru:
                    s += ValueToString(timeSpan.Days, "дней", "день", "дня");
                    s += " " + ValueToString(timeSpan.Hours, "часов", "час", "часа");
                    s = s.Trim();
                    s += " " + ValueToString(timeSpan.Minutes, "минут", "минута", "минуты");
                    s = s.Trim();
                    s += " " + ValueToString(timeSpan.Seconds, "секунд", "секунда", "секунды");
                    s = s.Trim();
                    break;
                default:
                    s += ValueToString(timeSpan.Days, "day", "days");
                    s += " " + ValueToString(timeSpan.Hours, "hour", "hours");
                    s = s.Trim();
                    s += " " + ValueToString(timeSpan.Minutes, "minute", "minutes");
                    s = s.Trim();
                    s += " " + ValueToString(timeSpan.Seconds, "second", "seconds");
                    s = s.Trim();
                    break;
            }
            return s;
        }

        public static string RegionsToString(int regionsCount, Culture culture)
        {
            switch (culture)
            {
                case Culture.Ru:
                    return ValueToString(regionsCount, "регионов", "регион", "региона");
                default:
                    return ValueToString(regionsCount, "region", "regions");
            }
        }

        public static string PlacesToString(int placesCount, Culture culture)
        {
            switch (culture)
            {
                case Culture.Ru:
                    return ValueToString(placesCount, "мест", "место", "места");
                default:
                    return ValueToString(placesCount, "place", "places");
            }
        }

        public static string CommentsToString(int commentsCount, Culture culture)
        {
            switch (culture)
            {
                case Culture.Ru:
                    return ValueToString(commentsCount, "комментиариев", "комментарий", "комментария");
                default:
                    return ValueToString(commentsCount, "comment", "comments");
            }
        }
    }
}