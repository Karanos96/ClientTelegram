using ClientTelegram.Service;

namespace ClientTelegram.IService
{
    public interface ITelegramOrchestrator
    {
        ITelegramSessionService GetOrCreateSession(int sessionId);
        ITelegramSessionService GetSession(int sessionId);
    }
}
