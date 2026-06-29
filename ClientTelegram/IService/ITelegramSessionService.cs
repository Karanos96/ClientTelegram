using ClientTelegram.Entity;
using TdLib;
using static TdLib.TdApi;

namespace ClientTelegram.Service
{
    public interface ITelegramSessionService
    {
        Task SetPhoneNumber(string phoneNumber);
        Task SetAccessCode(string accessCode);

        Task<Chats> GetChatList(int limitGetChat);
        Task<ChatInfoResponse> GetChatInfoById(long chatId);
        Task HandleNewMessage(TdApi.Message message);
    }
}
