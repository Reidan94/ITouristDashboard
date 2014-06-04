namespace ITourist.Models.LogicModels.Services
{
    public interface ISender
    {
        void Send(string topic, string text, string userMail);
    }
}