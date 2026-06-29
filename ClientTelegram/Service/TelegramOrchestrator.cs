using ClientTelegram.Constant;
using ClientTelegram.IService;
using ClientTelegram.OptionEntity;
using ClientTelegram.Security;
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
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMessageCryptoService _messageCryptoService;


        public TelegramOrchestrator(IConfiguration configuration , 
                                    IServiceScopeFactory scopeFactory,
                                    IMessageCryptoService messageCryptoService)
        {
            /*In this constructor only one time the configuration was read and they 
             will pass to the TelegramSessionService*/
            _scopeFactory = scopeFactory;
            _messageCryptoService = messageCryptoService;

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
                _logOptions,
                _scopeFactory,
                _messageCryptoService
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

        /// <summary>
        /// This method read from db the session was register 
        /// and foreach session build the client 
        /// </summary>
        /// <returns></returns>
        public async Task InitializeAsync()
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<ISessionRepository>();
            var sessions = await repo.GetAllSessions();

            foreach(var session in sessions)
            {
                GetOrCreateSession(session.Id);
            }
        }
    }
}
