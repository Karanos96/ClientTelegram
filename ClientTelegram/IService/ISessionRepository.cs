namespace ClientTelegram.IService
{
    public interface ISessionRepository
    {
        Task<SessionDto> AddNewSession(string phonenumber);
        Task<List<SessionDto>> GetAllSessions();

    }
}
