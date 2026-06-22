using ClientTelegram.Constant;
using ClientTelegram.Context;
using ClientTelegram.Entity;
using ClientTelegram.IService;
using Microsoft.EntityFrameworkCore;

namespace ClientTelegram.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly ApplicationDbContext _context;

        public SessionRepository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<SessionDto> AddNewSession(string phonenumber)
        {
            if (string.IsNullOrWhiteSpace(phonenumber))
                throw new ArgumentNullException(ErrorMessage.ERROR_PHONENUMBER);

            bool phoneNumberExists = await _context.Sessions
                .AsNoTracking()
                .Where(x => x.Phonenumber == phonenumber)
                .AnyAsync();

            if (phoneNumberExists)
                throw new ArgumentException(ErrorMessage.ERROR_PHONENUMBER_ALREADY_REGISTER);

            SessionEntity newSession = new SessionEntity()
            {
                Phonenumber = phonenumber,
            };

            _context.Sessions.Add(newSession);
            await _context.SaveChangesAsync();

            return new SessionDto()
            {
                Id = newSession.Id,
                Phonenumber = newSession.Phonenumber,
            };
        }
    }
}
