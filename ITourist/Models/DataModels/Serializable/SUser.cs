using System;
using System.Web;

namespace ITourist.Models.DataModels.Serializable
{
    public class SUser : Serializable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string FacebookLogin { get; set; }
        public string GoogleLogin { get; set; }
        public string VkLogin { get; set; }
        public string Image { get; set; }
        public int Experience { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public DateTime? RegistrationDate { get; set; }

        private SUser(User user, HttpContextBase context, Culture culture = Culture.En)
        {
            Id = user.Id;
            Name = user.Name;
            LastName = user.LastName;
            FacebookLogin = user.FacebookLogin;
            GoogleLogin = user.GoogleLogin;
            VkLogin = user.VkLogin;
            Image = DefineImagePath(context, user.Image);
            Experience = user.Experience;
            Email = user.Email;
            Password = user.Password;
            Status = UserStatus.GetStatus(user.Status).GetName(culture);
            if (FacebookLogin != null || GoogleLogin != null || VkLogin != null)
                RegistrationDate = null;
            else
                RegistrationDate = user.RegistrationDate;
        }

        public static SUser Convert(User user, HttpContextBase context, Culture culture = Culture.En)
        {
            if (user == null) return null;
            return new SUser(user, context, culture);
        }
    }
}