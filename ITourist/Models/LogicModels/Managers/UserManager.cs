using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ITourist.Models.DataModels;
using ITourist.Models.LogicModels.Services;
using ITourist.Models.ViewModels;

namespace ITourist.Models.LogicModels.Managers
{
    public class UserManager : Manager
    {

        public UserManager(ITouristDatabaseEntities entities)
            : base(entities)
        {

        }

        public IEnumerable<User> GetAll()
        {
            return Data.Users;
        }  

        public User Define(HttpContextBase context)
        {
            var cookieUser = context.Request.Cookies["UserId"];
            var cookieKey = context.Request.Cookies["Key"];
            if (cookieUser != null && cookieKey != null)
            {
                int id = Convert.ToInt32(cookieUser.Value);
                User user = Find(id);
                if (user == null) return null;
                return (cookieKey.Value == user.Password) ? user : null;
            }
            return null;
        }

        public User Find(int userId, bool confirmedOnly = true)
        {
            User user = Data.Users.FirstOrDefault(x => x.Id == userId);
            if (confirmedOnly)
                return user != null && user.HasUserAccess ? user : null;
            return user;
        }

        public User Find(string email, bool confirmedOnly = true)
        {
            User user = Data.Users.FirstOrDefault(x => x.Email == email);
            if (confirmedOnly)
                return user != null && user.HasUserAccess ? user : null;
            return user;
        }

        public User Find(AuthenticationProvider provider, string id)
        {
            switch (provider)
            {
                case AuthenticationProvider.Facebook:
                    return Data.Users.FirstOrDefault(x => x.FacebookLogin == id);
                case AuthenticationProvider.Google:
                    return Data.Users.FirstOrDefault(x => x.GoogleLogin == id);
                case AuthenticationProvider.Vk:
                    return Data.Users.FirstOrDefault(x => x.VkLogin == id);
                default:
                    return null;
            }
        }

        public ProcessResult LogInUser(string email, string password, out User user)
        {
            user = Find(email);
            if (user == null) return ProcessResults.InvalidEmail;
            if (user.Password == SecurityManager.GetHashString(password))
            {
                ProcessResult result = ProcessResults.LoginSuccessful;
                result.AffectedObjectId = user.Id;
                return result;
            }
            return ProcessResults.InvalidPassword;
        }

        public User Authenticate(AuthenticationProvider provider, string id, string name, string lastName, string image)
        {
            User user;
            bool isNewUser = false;
            switch (provider)
            {
                case AuthenticationProvider.Facebook:
                    user = Data.Users.FirstOrDefault(x => x.FacebookLogin == id);
                    if (user == null)
                    {
                        isNewUser = true;
                        user = new User();
                    }
                    user.FacebookLogin = id;
                    break;
                case AuthenticationProvider.Google:
                    user = Data.Users.FirstOrDefault(x => x.GoogleLogin == id);
                    if (user == null)
                    {
                        isNewUser = true;
                        user = new User();
                    }
                    user.GoogleLogin = id;
                    break;
                case AuthenticationProvider.Vk:
                    user = Data.Users.FirstOrDefault(x => x.VkLogin == id);
                    if (user == null)
                    {
                        isNewUser = true;
                        user = new User();
                    }
                    user.VkLogin = id;
                    break;
                default:
                    return null;
            }
            user.Name = name;
            user.LastName = lastName;
            user.Image = image;
            if (isNewUser)
                Data.Users.Add(user);
            Data.SaveChanges();
            return user;
        }


        public ProcessResult RegistrateUser(HttpContextBase context, RegistrationModel registrationModel, HttpServerUtilityBase server, HttpPostedFileBase imageUpload,Culture culture =Culture.En)
        {
            User existingUser = Find(registrationModel.Email,false);
            if (existingUser != null)
                return ProcessResults.UserWithSuchEmailExists;

            var user = new User
            {
                Name = registrationModel.Name,
                LastName = registrationModel.LastName,
                Email = registrationModel.Email,
                Password = SecurityManager.GetHashString(registrationModel.Password),
                RegistrationDate = DateTime.Now.Date,
                Status = (short)UserStatus.Unconfirmed.Id
            };


            try
            {
                Data.Users.Add(user);
                Data.SaveChanges();
                if (imageUpload != null)
                {
                    if (imageUpload.ContentLength <= 0 || !SecurityManager.IsImage(imageUpload))
                    {
                        return ProcessResults.InvalidImageFormat;
                    }
                    user.Image = SaveImage(user.Id, StaticSettings.AvatarsUploadFolderPath, imageUpload, server);
                    Data.SaveChanges();
                }
                SendConfirmationMail(context, user,culture);
            }
            
            catch(Exception)
            {
                Data.Users.Remove(user);
                return ProcessResults.RegistrationError;
            }

            return ProcessResults.UserRegistered;
        }

        private void SendConfirmationMail(HttpContextBase context, User user, Culture culture = Culture.En)
        {
            var confirmationMessageSender = new ConfirmationMailSender();
            string token = SecurityManager.GetHashString(user.Email + user.Password);
            if (context.Request.Url != null)
            {
                string path = context.Request.Url.GetLeftPart(UriPartial.Authority) + "/User/Confirm?hash=" + token;
                string message = String.Format(StaticSettings.ConfirmationMessage(culture) + "{0}", path);
                confirmationMessageSender.Send(StaticSettings.ConfirmationTitle(culture), message, user.Email);
            }
        }


        public bool ConfirmRegistration(string hash)
        {
            User user = Enumerable.FirstOrDefault(Data.Users, x => (hash == SecurityManager.GetHashString(x.Email + x.Password)) && x.HasNoAccess);
            if (user != null)
            {
                user.Status = (short)UserStatus.User.Id;
                Data.SaveChanges();
                return true;
            }
            return false;
        }

        public ProcessResult SetUserStatus(int id, int type)
        {
            var user = Find(id);
            if (user == null)
                return ProcessResults.UserNotFound;
            user.Status = (short)type;
            Data.SaveChanges();
            return ProcessResults.UserEdited;
        }
        
    }
}