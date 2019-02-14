using Abp.Configuration.Startup;
using Abp.MultiTenancy;
using Abp.Runtime;
using Abp.Runtime.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace WebApp.Core.Extensions
{
    public class AbpSessionExtension : ClaimsAbpSession, IAbpSessionExtension
    {
        public AbpSessionExtension(IPrincipalAccessor principalAccessor,
            IMultiTenancyConfig multiTenancy,
            ITenantResolver tenantResolver,
            IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider) : base(principalAccessor,multiTenancy, tenantResolver, sessionOverrideScopeProvider)
        {

        }

        public string MyUserId => GetClaimValue(ClaimTypes.NameIdentifier);
        public string MyUserAccout => GetClaimValue(ClaimTypes.Sid);

        public string Email => GetClaimValue(ClaimTypes.Email);

        private string GetClaimValue(string claimType)
        {
            var claimsPrincipal = PrincipalAccessor.Principal;

            var claim = claimsPrincipal?.Claims.FirstOrDefault(c => c.Type == claimType);
            if (string.IsNullOrEmpty(claim?.Value))
                return null;

            return claim.Value;
        }
    }
}
