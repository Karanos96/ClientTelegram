using ClientTelegram.Constant;
using ClientTelegram.Entity;
using ClientTelegram.IService;
using ClientTelegram.OptionEntity;
using ClientTelegram.Service;
using ClientTelegram.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ClientTelegram.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITelegramOrchestrator _orchestrator;

        public AuthController(ITelegramOrchestrator orchestrator)
        {
            _orchestrator = orchestrator;
        }

        [HttpPost("Phonenumber")]
        public async Task<ActionResult> SetPhonenumber([FromBody] AuthRequest request)
        {
            try
            {
                ITelegramSessionService session = _orchestrator.GetOrCreateSession(request.SessionId);
                await session.SetPhoneNumber(request.Phonenumber);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorMessage.ERROR_AUTHENTICATION_PHONENUMBER);
            }

            return Ok(SuccessMessage.SEND_SUCCESSFULLY_PHONENUMBER);
        }

        [HttpPost("AccessCode")]
        public async Task<ActionResult> SetAccessCode([FromBody] AuthRequest request)
        {
            try
            {
                ITelegramSessionService session = _orchestrator.GetSession(request.SessionId);
                await session.SetAccessCode(request.AccessCode);
            }
            catch (Exception ex)
            {
                return BadRequest(ErrorMessage.ERROR_AUTHENTICATION_ACCESS_CODE);
            }

            return Ok(SuccessMessage.SUCCESSFULLY_CONNECTION);
        }


    }
}
