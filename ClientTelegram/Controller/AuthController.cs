using ClientTelegram.Constant;
using ClientTelegram.Entity;
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
        private readonly ITDLibService _service;
        private readonly MethodUtility _utility;

        public AuthController(ITDLibService service, IConfiguration configuration)
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

        [HttpPost("Phonenumber")]
        public async Task<ActionResult> SetPhonenumber([FromBody] AuthRequest request)
        {
            try
            {
                await _service.SetPhoneNumber(request.Phonenumber);
            }
            catch (Exception ex)
            {
                _utility.Log("ERROR-MESSAGE", ex.Message);
                _utility.Log("ERROR-STACKTRACE", ex.StackTrace);

                return BadRequest(ErrorMessage.ERROR_AUTHENTICATION_PHONENUMBER);
            }

            return Ok(SuccessMessage.SEND_SUCCESSFULLY_PHONENUMBER);
        }

        [HttpPost("AccessCode")]
        public async Task<ActionResult> SetAccessCode([FromBody] AuthRequest request)
        {
            try
            {
                await _service.SetAccessCode(request.AccessCode);
            }
            catch (Exception ex)
            {
                _utility.Log("ERROR-MESSAGE", ex.Message);
                _utility.Log("ERROR-STACKTRACE", ex.StackTrace);

                return BadRequest(ErrorMessage.ERROR_AUTHENTICATION_ACCESS_CODE);
            }

            return Ok(SuccessMessage.SUCCESSFULLY_CONNECTION);
        }
    }
}
