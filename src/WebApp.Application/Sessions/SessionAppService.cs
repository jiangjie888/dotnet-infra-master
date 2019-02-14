﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Auditing;
using WebApp.Application.Sessions.Dto;

namespace WebApp.Application.Sessions
{
    public class SessionAppService : WebAppAppServiceBase, ISessionAppService
    {
        [DisableAuditing]
        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput
            {
                //Application = new ApplicationInfoDto
                //{
                //    Version = AppVersionHelper.Version,
                //    ReleaseDate = AppVersionHelper.ReleaseDate,
                //    Features = new Dictionary<string, bool>
                //    {
                //        { "SignalR", SignalRFeature.IsAvailable },
                //        { "SignalR.AspNetCore", SignalRFeature.IsAspNetCore }
                //    }
                //}
            };

            if (AbpSession.UserId.HasValue)
            {
                output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
            }

            return output;
        }
    }
}
