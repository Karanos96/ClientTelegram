using ClientTelegram.Constant;
using ClientTelegram.IService;
using ClientTelegram.OptionEntity;
using System.Collections.Concurrent;

namespace ClientTelegram.Service
{
    /// <summary>
    /// In this class living all session for all lifetime of the application
    /// </summary>
    public class TelegramOrchestrator : ITelegramOrchestrator
    {
        private readonly ConcurrentDictionary<int, ITelegramSessionService> _liveSession = new ConcurrentDictionary<int, ITelegramSessionService>();
        private readonly TelegramOptions _telegramOptions;
        private readonly LogOptions _logOptions;

        public TelegramOrchestrator(IConfiguration configuration)
        {
            /*In this constructor only one time the configuration was read and they 
             will pass to the TelegramSessionService*/

            _telegramOptions = configuration.GetSection("Telegram").Get<TelegramOptions>()
                ?? throw new InvalidOperationException(ErrorMessage.ERROR_OPTION_TELEGRAM);

            _logOptions = configuration.GetSection("Log").Get<LogOptions>()
                ?? throw new InvalidOperationException(ErrorMessage.ERROR_OPTION_TELEGRAM);
        }

        /// <summary>
        /// This method search an exists session, otherwise create a new session
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public ITelegramSessionService GetOrCreateSession(int sessionId)
        {
            return _liveSession.GetOrAdd(sessionId, id => new TelegramSessionService(
                id,
                _telegramOptions,
                _logOptions
                ));
        }

        /// <summary>
        /// Only get an already session exist for ever session in state Authenticated
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public ITelegramSessionService GetSession(int sessionId)
        {
            if (_liveSession.TryGetValue(sessionId, out var session))
            {
                return session;
            }
            else
            {
                throw new KeyNotFoundException(ErrorMessage.ERROR_SESSION_NOT_FOUND + sessionId);
            }
        }
    }
}
