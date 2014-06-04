using System;
using System.Globalization;
using System.Web;
using System.Web.Mvc;
using ITourist.App_Resources;
using ITourist.Models.DataModels;
using ITourist.Models.LogicModels.Managers;
using ITourist.Models.LogicModels.Services;
using Newtonsoft.Json;

namespace ITourist.Controllers
{
    public abstract class BaseController : Controller
    {
        protected Culture DefineLanguage(HttpContextBase context)
        {
            var cookie = context.Request.Cookies["UserLang"];
            if (cookie != null)
            {
                Language.Culture = new CultureInfo(cookie.Value);
            }
            else
            {
                Language.Culture = new CultureInfo("en");
                cookie = new HttpCookie("UserLang")
                {
                    Value = "en",
                    Expires = DateTime.MaxValue
                };
                context.Response.Cookies.Remove("UserLang");
                context.Response.SetCookie(cookie);
            }
            return Translatable.DefineCulture(Language.Culture.TwoLetterISOLanguageName);
        }

        public User DefineUser(HttpContextBase context)
        {
            var cookieUser = context.Request.Cookies["UserId"];
            var cookieKey = context.Request.Cookies["Key"];
            if (cookieUser != null && cookieKey != null)
            {
                int id = Convert.ToInt32(cookieUser.Value);
                User user = DataManager.Users.Find(id);
                if (user == null) return null;
                return KeyMatch(user, cookieKey.Value) ? user : null;
            }
            return null;
        }


        private bool KeyMatch(User user, string key)
        {
            if (user == null)
                return false;
            return user.Password == key ||
                SecurityManager.GetHashString(user.FacebookLogin) == key ||
                SecurityManager.GetHashString(user.GoogleLogin) == key ||
                SecurityManager.GetHashString(user.VkLogin) == key;
        }

        protected string ToJson(Object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}