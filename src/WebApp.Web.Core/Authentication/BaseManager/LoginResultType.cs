using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.WebCore.Authentication.BaseManager
{
    public enum LoginResultType : byte
    {
        Success = 1,

        InvalidUserNameOrEmailAddress,

        InvalidPassword,

        UserIsNotActive,

        UserEmailIsNotConfirmed,

        UnknownExternalLogin,

        LockedOut,

        UserPhoneNumberIsNotConfirmed
    }
}
