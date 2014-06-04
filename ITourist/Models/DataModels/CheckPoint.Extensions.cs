using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITourist.Models.DataModels
{
    public partial class CheckPoint
    {
        public void SetNewAverageTime(double timeDifference)
        {
            int count = CheckIns.Count(x => x.Time != null);
            if (count == 0)
                AverageTime = timeDifference;
            else
                AverageTime = (AverageTime*count + timeDifference)/(count + 1);
        }

        public string FormatAverageTime(Culture culture)
        {
            return NumberTranslator.TimeToString(TimeSpan.FromMinutes(AverageTime), culture);
        }
    }
}