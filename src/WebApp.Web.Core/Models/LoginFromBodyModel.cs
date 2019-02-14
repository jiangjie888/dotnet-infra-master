using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebApp.WebCore.Models.Account;
using WebApp.WebCore.Models.TokenAuth;

namespace WebApp.WebCore.Models
{
    public class LoginFromBodyModel
    {
        //public AuthenticateModel AuthModel { set; get; }

        //public LoginClientModel ClientModel { set; get; }

        [Required]
        [StringLength(30)]
        public string UserNameOrEmailAddress { get; set; }

        [Required]
        [StringLength(18)]
        public string Password { get; set; }

        public bool RememberClient { get; set; }

        public string ClientState { get; set; }
        public string ClientId { get; set; }
        public string ReturnUrl { get; set; }

    }
}
