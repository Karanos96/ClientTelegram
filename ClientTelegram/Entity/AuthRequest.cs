using System.ComponentModel.DataAnnotations;

namespace ClientTelegram.Entity
{
    public class AuthRequest
    {
        public string Phonenumber {  get; set; }
        public string AccessCode { get; set; }
    }
}
