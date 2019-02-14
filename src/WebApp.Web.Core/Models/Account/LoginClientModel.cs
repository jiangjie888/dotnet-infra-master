using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.WebCore.Models.Account
{
    public class LoginClientModel : PageModel
    {
        public string ClientState { get; set; }
        public string ClientId { get; set; }
        public string ReturnUrl { get; set; }
    }
}
