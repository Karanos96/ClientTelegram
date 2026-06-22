using ClientTelegram.Entity;
using ClientTelegram.IService;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace ClientTelegram.Controller
{

    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        private readonly ISessionRepository _sessionRepository;

        public SessionController(ISessionRepository sessionRepository)
        {
            this._sessionRepository = sessionRepository;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register([FromBody] RegisterSessionRequest request)
        {
            try
            {
                SessionDto session = await _sessionRepository.AddNewSession(request.Phonenumber);
                return Ok(session);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
