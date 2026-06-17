using ClientTelegram.Constant;
using ClientTelegram.OptionEntity;
using ClientTelegram.Utility;
using TdLib;
using static TdLib.TdApi;

namespace ClientTelegram.Service
{
    public class TDLibService : ITDLibService
    {
        private readonly TdClient _client;
        private TdApi.AuthorizationState? _currentState;
        private MethodUtility _utility;
        public TDLibService(IConfiguration configuration)
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _client = new TdClient();
           

            TelegramOptions? telegramOptions = configuration.GetSection("Telegram")
                .Get<TelegramOptions>();

            LogOptions? logOptions = configuration.GetSection("Log")
                .Get<LogOptions>();

            if(telegramOptions == null || logOptions == null)
            {
                throw new InvalidOperationException(ErrorMessage.ERROR_OPTION_TELEGRAM);
            }

            string logFilePath = Path.Combine(basePath, "ClientTelegram", logOptions.PathLog, "app.log");
            _utility = new MethodUtility(logFilePath);

            _client.UpdateReceived += async (sender, update) =>
            {
                if (update is not TdApi.Update.UpdateAuthorizationState authUpdate)
                    return;

                _currentState = authUpdate.AuthorizationState;
                _utility.Log("INFO" , _currentState.ToString());

                switch (authUpdate.AuthorizationState)
                {
                    case TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters:
                        await _client.ExecuteAsync(new TdApi.SetTdlibParameters
                        {
                            ApiId = telegramOptions.ApiId,
                            ApiHash = telegramOptions.ApiHash,
                            UseMessageDatabase = true,
                            UseSecretChats = false,
                            SystemLanguageCode = "it",
                            DeviceModel = "Desktop",
                            ApplicationVersion = "1.0",
                            DatabaseDirectory = Path.Combine(basePath, "ClientTelegram" , telegramOptions.DatabaseDirectory),
                            FilesDirectory = Path.Combine(basePath, "ClientTelegram", telegramOptions.FilesDirectory),
                        });
                        break;
                }
            };
        }

        public void UpdateAuthorizationState()
        {
            throw new NotImplementedException();
        }
    }
}
