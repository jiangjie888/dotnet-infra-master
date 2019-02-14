using System.ComponentModel.DataAnnotations;

namespace WebApp.WebCore.Models.TokenAuth
{
    public class AuthenticateModel
    {
        [Required]
        [StringLength(30)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(30)]
        public string Password { get; set; }
        
        public bool RememberClient { get; set; }
    }
}
