namespace ITourist.Models.DataModels.Serializable
{
    public class SProcessResult : Serializable
    {
        public bool Succeeded { get; private set; }
        public string Message { get; private set; }
        public int? AffectedObjectId { get; private set; }

        private SProcessResult(ProcessResult result, Culture culture = Culture.En)
        {
            Succeeded = result.Succeeded;
            Message = result.GetMessage(culture);
            AffectedObjectId = result.AffectedObjectId;
        }

        public static SProcessResult Convert(ProcessResult result, Culture culture = Culture.En)
        {
            if (result == null) return null;
            return new SProcessResult(result, culture);
        }
    }
}