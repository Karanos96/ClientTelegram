using ClientTelegram.Entity;
using static TdLib.TdApi;

namespace ClientTelegram.Service
{
    public interface ITDLibService
    {
        Task SetPhoneNumber(string phoneNumber);
        Task SetAccessCode(string accessCode);

        Task<Chats> GetChatList(int limitGetChat);
        Task<ChatInfoResponse> GetChatInfoById(long chatId);
    }
}
