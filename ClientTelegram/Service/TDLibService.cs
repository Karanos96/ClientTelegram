using ClientTelegram.OptionEntity;
using TdLib;

namespace ClientTelegram.Service
{
    public class TDLibService : ITDLibService
    {
        private readonly TdClient _client;
        private TdApi.AuthorizationState? _currentState;

        public TDLibService(IConfiguration configuration)
        {
            var telegramOptions = configuration.GetSection("Telegram")
                .Get<TelegramOptions>();
        }

        public void UpdateAuthorizationState()
        {
            throw new NotImplementedException();
        }
    }
}
