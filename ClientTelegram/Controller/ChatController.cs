using ClientTelegram.Constant;
using ClientTelegram.Entity;
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
        private readonly ITDLibService _service;
        private readonly MethodUtility _utility;

        public ChatController(ITDLibService service, IConfiguration configuration)
        {
            _service = service;

            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            LogOptions? logOptions = configuration.GetSection("Log")
                .Get<LogOptions>();

            if (logOptions == null)
            {
                throw new InvalidOperationException(ErrorMessage.ERROR_OPTION_TELEGRAM);
            }

            string logFilePath = Path.Combine(basePath, "ClientTelegram", logOptions.PathLog, "app.log");
            _utility = new MethodUtility(logFilePath);
        }

        [HttpGet("ChatsInfo/{recordLimit}")]
        public async Task<ActionResult> GetChatsInfo(int recordLimit)
        {
            List<ChatInfoResponse> result = new List<ChatInfoResponse>();

            try
            {

                if (recordLimit != null && recordLimit > 0)
                {
                    Chats chats = await _service.GetChatList(recordLimit);

                    foreach (var chatId in chats.ChatIds)
                    {
                        result.Add(await _service.GetChatInfoById(chatId));
                    }

                }
                else
                {
                    return BadRequest(ErrorMessage.ERROR_LIMIT_CHATS);
                }
            }
            catch (Exception ex)
            {
                _utility.Log("ERROR-MESSAGE", ex.Message);
                _utility.Log("ERROR-STACKTRACE", ex.StackTrace);

                return BadRequest(ErrorMessage.ERROR_AUTHENTICATION_ACCESS_CODE);
            }

            return Ok(result);
        }
    }
}
