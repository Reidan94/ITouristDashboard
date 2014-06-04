using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ITourist.Models.DataModels
{
    public enum Culture
    {
        En,
        Ru
    }

    public class Translatable
    {
        private bool Contains(Translation translation, Culture culture, params string[] keys)
        {
            return keys.All(key => GetTranslation(culture, translation).ToUpper().Contains(key.ToUpper()));
        }

        protected static string GetTranslation(Culture culture, Translation translation)
        {
            switch (culture)
            {
                case Culture.En:
                    return translation.En;
                case Culture.Ru:
                    return translation.Ru;
                default:
                    return null;
            }
        }

        protected bool Contains(Translation translation, string s, Culture culture)
        {
            string[] keys = s.Split(' ');
            return Contains(translation, culture, keys);
        }

        protected bool Contains(Translation translation, string s)
        {
            return (from Culture culture in Enum.GetValues(typeof(Culture)) select Contains(translation, s, culture)).Any(b => b);
        }

        public static Culture DefineCulture(string culture)
        {
            culture = culture.ToLower();
            switch (culture)
            {
                case "ru":
                    return Culture.Ru;
                default:
                    return Culture.En;
            }
        }

        public static string DefineFullCultureName(Culture culture)
        {
            switch (culture)
            {
                case Culture.Ru:
                    return "ru_RU";
                default:
                    return "en_US" +
                           "" +
                           "" +
                           "";
            }
        }
    }
}