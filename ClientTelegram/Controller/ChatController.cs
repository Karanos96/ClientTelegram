using ClientTelegram.Constant;
using ClientTelegram.Entity;
using ClientTelegram.IService;
using ClientTelegram.OptionEntity;
using ClientTelegram.Service;
using ClientTelegram.Utility;
using Microsoft.AspNetCore.Mvc;
using static TdLib.TdApi;

namespace ClientTelegram.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ITelegramOrchestrator _orchestrator;

        public ChatController(ITelegramOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpGet("ChatsInfo/{recordLimit}/{sessionId}")]
        public async Task<ActionResult> GetChatsInfo(int recordLimit, int sessionId)
        {
            List<ChatInfoResponse> result = new List<ChatInfoResponse>();

            try
            {

                if (recordLimit == 0)
                    return BadRequest(ErrorMessage.ERROR_LIMIT_CHATS);

                if (sessionId == 0)
                    return BadRequest(ErrorMessage.ERROR_SESSION_ID_NOT_VALID);

                ITelegramSessionService session = _orchestrator.GetSession(sessionId);
                Chats chats = await session.GetChatList(recordLimit);

                foreach (var chatId in chats.ChatIds)
                {
                    result.Add(await session.GetChatInfoById(chatId));
                }


            }
            catch (Exception ex)
            {
                return BadRequest(ErrorMessage.ERROR_GET_INFO_CHATS);
            }

            return Ok(result);
        }
    }
}
