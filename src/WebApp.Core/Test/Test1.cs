using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace WebApp.Core.Test
{
        [Table("Sys_Test1")]
        public class Test1 : FullAuditedEntity<Guid>
        {
        public string code { set; get; }
        }
}
