using ClientTelegram.Constant;
using ClientTelegram.Entity;
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

            if (telegramOptions == null || logOptions == null)
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
                _utility.Log("INFO", _currentState.ToString());

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
                            DatabaseDirectory = Path.Combine(basePath, "ClientTelegram", telegramOptions.DatabaseDirectory),
                            FilesDirectory = Path.Combine(basePath, "ClientTelegram", telegramOptions.FilesDirectory),
                        });
                        break;
                }
            };
        }

        public async Task SetPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentException(ErrorMessage.ERROR_PHONENUMBER);
            }

            await _client.ExecuteAsync(new TdApi.SetAuthenticationPhoneNumber
            {
                PhoneNumber = phoneNumber
            });
        }

        public async Task SetAccessCode(string accessCode)
        {
            if (string.IsNullOrEmpty(accessCode))
            {
                throw new ArgumentException(ErrorMessage.ERROR_ACCESS_CODE);
            }

            await _client.ExecuteAsync(new TdApi.CheckAuthenticationCode
            {
                Code = accessCode
            });
        }

        public async Task<Chats> GetChatList(int limitGetChat)
        {
            Chats chatsList = await _client.ExecuteAsync(new TdApi.GetChats
            {
                ChatList = new TdApi.ChatList.ChatListMain(),
                Limit = limitGetChat
            });

            return chatsList;
        }

        public async Task<ChatInfoResponse> GetChatInfoById(long chatId)
        {
            ChatInfoResponse chatInfoResponse = null;
            var chat = await _client.ExecuteAsync(new TdApi.GetChat
            {
                ChatId = chatId
            });

            if (chat != null)
            {
                chatInfoResponse = new ChatInfoResponse()
                {
                    ChatId = chat.Id,
                    ChatTitle = chat.Title,
                };

            }
            return chatInfoResponse;

        }
    }
}
