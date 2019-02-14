namespace Stbis.Common
{
    using System.Collections.Generic;
    /// <summary>
    /// 对应前台 ligerFilter 的检索规则数据
    /// </summary>
    public class FilterGroup
    {
        public IList<FilterRule> rules { get; set; }
        public string op { get; set; }
        public IList<FilterGroup> groups { get; set; }
    }
}
