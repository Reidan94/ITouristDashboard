using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using ITourist.Models.LogicModels.Services;

namespace ITourist.Models.DataModels
{
    public partial class User : Translatable
    {
        public bool HasAdminAccess
        {
            get
            {
                return Status == UserStatus.Admin.Id;
            }
        }

        public bool HasModeratorAccess
        {
            get
            {
                return Status == UserStatus.Moderator.Id || HasAdminAccess;
            }
        }

        public bool HasUserAccess
        {
            get
            {
                return Status == UserStatus.User.Id || HasModeratorAccess;
            }
        }

        public bool HasNoAccess
        {
            get
            {
                return Status == UserStatus.Unconfirmed.Id;
            }
        }

        public string GetStatus(Culture culture)
        {
            return UserStatus.GetStatus(Status).GetName(culture);
        }

        public string FullName
        {
            get { return Name + " " + LastName; }
        }

        public bool HasRouteInBookmarks(int routeId)
        {
            return Bookmarks.Any(x => x.RouteId == routeId);
        }

        public bool HasLowExperience
        {
            get { return Experience < StaticSettings.MinUserExperince; }
        }
    }

    public class UserStatus
    {
        public static Status Unconfirmed
        {
            get { return new Status(-1, new Translation("Unconfirmed", "Не подтвержен")); }
        }

        public static Status User
        {
            get { return new Status(0, new Translation("User", "Пользователь")); }
        }


        public static Status Moderator
        {
            get { return new Status(1, new Translation("Moderator", "Модератор")); }
        }

        public static Status Admin
        {
            get { return new Status(2, new Translation("Administrator", "Администратор")); }
        }

        public static Status GetStatus(short status)
        {
            return  status == Unconfirmed.Id ? Unconfirmed
                  : status == User.Id ? User
                  : status == Moderator.Id ? Moderator
                  : status == Admin.Id ? Admin 
                  : null;
        }

        public static IEnumerable<Status> GetAll()
        {
            return new[]
            {
                User,
                Moderator,
                Admin
            };
        }
    }
}