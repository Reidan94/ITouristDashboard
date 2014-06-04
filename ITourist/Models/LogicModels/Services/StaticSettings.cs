using ITourist.Models.DataModels;

namespace ITourist.Models.LogicModels.Services
{
    public class StaticSettings:Translatable
    {
        public static string UploadFolderPath
        {
            get { return "Images"; }
        }

        public static string CountriesUploadFolderPath
        {
            get { return UploadFolderPath + "/Countries/"; }
        }

        public static string RegionsUploadFolderPath
        {
            get { return UploadFolderPath + "/Regions/"; }
        }

        public static string PlacesUploadFolderPath
        {
            get { return UploadFolderPath + "/Places/"; }
        }

        public static string AvatarsUploadFolderPath
        {
            get { return UploadFolderPath + "/Avatars/"; }
        }

        public static int MinUserExperince
        {
            get { return 10; }
        }

        public static string ConfirmationMessage(Culture culture)
        {
              return GetTranslation(culture,new Translation("To confirm registration, open this link : ","Для подтверждения регистрации перейдите по ссылке : ")); 
        }

        public static string ConfirmationTitle(Culture culture)
        {
            return GetTranslation(culture, new Translation("Confirm registration", "Подтвердите регистрации"));
        }
    }
}