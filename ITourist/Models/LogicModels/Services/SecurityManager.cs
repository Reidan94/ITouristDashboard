using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ITourist.Models.LogicModels.Services
{
    public class SecurityManager
    {
        public static string GetHashString(string s)
        {
            try
            {
                byte[] bytes = Encoding.Unicode.GetBytes(s);
                var csp = new MD5CryptoServiceProvider();
                byte[] byteHash = csp.ComputeHash(bytes);
                return byteHash.Aggregate(string.Empty, (current, b) => current + string.Format("{0:x2}", b));
            }
            catch
            {
                return "";
            }
        }

        public static bool IsImage(HttpPostedFileBase file)
        {
            if (file.ContentType.Contains("image"))
                return true;
            var formats = new[] { ".jpg", ".png", ".gif", ".jpeg" };
            return formats.Any(item => file.FileName.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }
    }
}