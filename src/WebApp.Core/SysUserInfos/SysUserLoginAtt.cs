using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApp.Core.SysUserInfos
{

    [Table("Sys_UserLoginAtt")]
    public class SysUserLoginAtt : FullAuditedEntity<long>
    {

        public const int MaxUserNameOrEmailAddressLength = 30;

        /// <summary>
        /// Maximum length of <see cref="ClientIpAddress"/> property.
        /// </summary>
        public const int MaxClientIpAddressLength = 64;

        /// <summary>
        /// Maximum length of <see cref="ClientName"/> property.
        /// </summary>
        public const int MaxClientNameLength = 128;

        /// <summary>
        /// Maximum length of <see cref="BrowserInfo"/> property.
        /// </summary>
        public const int MaxBrowserInfoLength = 512;


        /// <summary>
        /// User's Id, if <see cref="UserNameOrEmailAddress"/> was a valid username or email address.
        /// </summary>
        public virtual long? UserId { get; set; }

        /// <summary>
        /// User name or email address
        /// </summary>
        [MaxLength(MaxUserNameOrEmailAddressLength)]
        public virtual string UserNameOrEmailAddress { get; set; }

        /// <summary>
        /// IP address of the client.
        /// </summary>
        [MaxLength(MaxClientIpAddressLength)]
        public virtual string ClientIpAddress { get; set; }

        /// <summary>
        /// Name (generally computer name) of the client.
        /// </summary>
        [MaxLength(MaxClientNameLength)]
        public virtual string ClientName { get; set; }

        /// <summary>
        /// Browser information if this method is called in a web request.
        /// </summary>
        [MaxLength(MaxBrowserInfoLength)]
        public virtual string BrowserInfo { get; set; }

        /// <summary>
        /// Login attempt result.
        /// </summary>
        public virtual Int16 Result { get; set; }

    }
}
