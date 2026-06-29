using ClientTelegram.Constant;
using ClientTelegram.Entity;
using ClientTelegram.OptionEntity;
using ClientTelegram.Repository;
using ClientTelegram.Security;
using ClientTelegram.Utility;
using TdLib;
using static TdLib.TdApi;

namespace ClientTelegram.Service
{
    public class TelegramSessionService : ITelegramSessionService
    {
        private readonly TdClient _client;
        private TdApi.AuthorizationState? _currentState;
        private MethodUtility _utility;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMessageCryptoService _messageCryptoService;
        public int SessionId { get;}
        public TelegramSessionService(  int sessionId , 
                                        TelegramOptions telegramOptions,
                                        LogOptions logOptions,
                                        IServiceScopeFactory scopeFactory,
                                        IMessageCryptoService cryptoMessageService)
        {
            SessionId = sessionId;
            _serviceScopeFactory = scopeFactory;
            _messageCryptoService = cryptoMessageService;  

            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            _client = new TdClient();


            if (telegramOptions == null || logOptions == null)
            {
                throw new InvalidOperationException(ErrorMessage.ERROR_OPTION_TELEGRAM);
            }

            string logFilePath = Path.Combine(basePath, "ClientTelegram", logOptions.PathLog, $"session_{sessionId}.log");
            _utility = new MethodUtility(logFilePath);

            _client.UpdateReceived += async (sender, update) =>
            {
                switch (update)
                {
                    case TdApi.Update.UpdateAuthorizationState authUpdate:
                        _currentState = authUpdate.AuthorizationState;
                        _utility.Log("INFO", $"[Session {SessionId}] - {_currentState.ToString()}");

                        if (authUpdate.AuthorizationState is TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters)
                        {
                            await _client.ExecuteAsync(new TdApi.SetTdlibParameters
                            {
                                ApiId = telegramOptions.ApiId,
                                ApiHash = telegramOptions.ApiHash,
                                UseMessageDatabase = true,
                                UseSecretChats = false,
                                SystemLanguageCode = "it",
                                DeviceModel = "Desktop",
                                ApplicationVersion = "1.0",
                                DatabaseDirectory = Path.Combine(basePath, "ClientTelegram", telegramOptions.DatabaseDirectory, sessionId.ToString()),
                                FilesDirectory = Path.Combine(basePath, "ClientTelegram", telegramOptions.FilesDirectory, sessionId.ToString()),
                            });
                        }
                        break;

                    case TdApi.Update.UpdateNewMessage newMessage:
                        await HandleNewMessage(newMessage.Message);
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

        public async Task HandleNewMessage(Message message)
        {
            try
            {
                string description = message.Content switch
                {
                    TdApi.MessageContent.MessageText text
                        => text.Text.Text,

                    TdApi.MessageContent.MessagePhoto photo
                        => $"[PHOTO] {photo.Caption.Text}",

                    TdApi.MessageContent.MessageVideo video
                        => $"[VIDEO] {video.Caption.Text}",

                    TdApi.MessageContent.MessageDocument doc
                        => $"[DOCUMENT: {doc.Document.FileName}] {doc.Caption.Text}",

                    TdApi.MessageContent.MessageAudio audio
                        => $"[AUDIO: {audio.Audio.FileName}]",

                    TdApi.MessageContent.MessageVoiceNote voice
                        => "[VOCAL MESSAGE]",

                    TdApi.MessageContent.MessageSticker sticker
                        => $"[STICKER: {sticker.Sticker.Emoji}]",

                    TdApi.MessageContent.MessageAnimation
                        => "[GIF]",

                    _ => $"[{message.Content.GetType().Name}]"
                };

                EncryptedPayload encryptedPayload = _messageCryptoService.Encrypt(description);

                MessageEntity messageEntity = new MessageEntity
                {
                    SessionId = SessionId,
                    ChatId = message.ChatId,
                    MessageId = message.Id,
                    Type = message.Content.GetType().Name,
                    IsOutgoing = message.IsOutgoing,
                    Date = DateTimeOffset.FromUnixTimeSeconds(message.Date).UtcDateTime,
                    Ciphertext = encryptedPayload.Ciphertext,
                    Nonce = encryptedPayload.Nonce,
                    Tag = encryptedPayload.Tag,
                    KeyId = encryptedPayload.KeyId
                };

                using var scope = _serviceScopeFactory.CreateScope();
                var messageRepository = scope.ServiceProvider.GetRequiredService<IMessageRepository>();

                await messageRepository.AddAsync(messageEntity);

                _utility.Log("SYSTEM", $"Saved new message: sessionId:{SessionId}, Chat: {message.ChatId}, Message: {message.Id}");
            }
            catch (Exception ex) 
            {
                _utility.Log("ERROR", $"Error while saving message: sessionId:{SessionId}, Message: {message.Id}, Error: {ex.Message}, InnerException: {ex.InnerException}");
            }

        }
    }
}
