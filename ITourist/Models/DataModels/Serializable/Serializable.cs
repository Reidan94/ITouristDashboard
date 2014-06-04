using System;
using System.Web;

namespace ITourist.Models.DataModels.Serializable
{
    public abstract class Serializable
    {
        protected string DefineImagePath(HttpContextBase context, string path)
        {
            if (path == null || context == null || context.Request == null || context.Request.Url == null) return "";
            if (path[0] == 'h') return path;
            if (path[0] == '~') path = path.Substring(1);
            string slash = path[0] == '/' ? "" : "/";
            return context.Request.Url.GetLeftPart(UriPartial.Authority) + slash +  path;
        }
    }
}