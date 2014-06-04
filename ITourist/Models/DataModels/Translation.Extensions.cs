using System;

namespace ITourist.Models.DataModels
{
    public partial class Translation
    {
        public Translation(string en,string ru)
        {
            En = en;
            Ru = ru;
        }

        public static bool IsInValid(Translation translation)
        {
            return translation == null
                   || String.IsNullOrWhiteSpace(translation.En)
                   || String.IsNullOrWhiteSpace(translation.Ru);
        }

        public void Copy(Translation translation)
        {
            En = translation.En;
            Ru = translation.Ru;
        }
    }
}