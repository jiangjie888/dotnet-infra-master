using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using WebApp.Core.SysUserInfos;

namespace WebApp.WebCore.Authentication.BaseManager
{
    public class LoginResult
    {
        public LoginResultType Result { get; private set; }

        public SysUserInfo User { get; private set; }

        public ClaimsIdentity Identity { get; private set; }

        public LoginResult(LoginResultType result, SysUserInfo user = null)
        {
            Result = result;
            User = user;
        }

        public LoginResult(SysUserInfo user, ClaimsIdentity identity) : this(LoginResultType.Success)
        {
            User = user;
            Identity = identity;
        }
    }
}