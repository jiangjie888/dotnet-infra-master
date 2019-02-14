using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebApp.Core.Extensions
{
    public interface IAbpSessionExtension : IAbpSession
    {
        string MyUserId { get; }
        string Email { get; }
    }
}
