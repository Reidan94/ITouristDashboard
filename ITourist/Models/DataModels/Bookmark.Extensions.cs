using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ITourist.Models.DataModels
{
    public partial class Bookmark
    {
        public string GetStatus(Culture culture)
        {
            return TrackingStatus.GetStatus(Status).GetName(culture);
        }
    }
}