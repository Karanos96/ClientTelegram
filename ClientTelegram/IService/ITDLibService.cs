namespace ClientTelegram.Service
{
    public interface ITDLibService
    {
        Task SetPhoneNumber(string phoneNumber);
        Task SetAccessCode(string accessCode);
    }
}
