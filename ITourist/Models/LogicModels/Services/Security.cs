using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ITourist.Models.LogicModels.Services
{
    public class Security
    {
        public static string GetHashString(string s)
        {
            try
            {
                //переводим строку в байт-массив 
                byte[] bytes = Encoding.Unicode.GetBytes(s);

                //создаем объект для получения средст шифрования  
                var csp = new MD5CryptoServiceProvider();

                //вычисляем хеш-представление в байтах  
                byte[] byteHash = csp.ComputeHash(bytes);

                //формируем одну цельную строку из массива  

                return byteHash.Aggregate(string.Empty, (current, b) => current + string.Format("{0:x2}", b));
            }
            catch
            {
                return "";
            }
        }
    }
}