using System.ComponentModel.DataAnnotations;

namespace ClientTelegram.Entity
{
    public class AuthRequest
    {
        public int SessionId { get; set; }
        public string Phonenumber {  get; set; }
        public string AccessCode { get; set; }
    }
}
