using System.ComponentModel;

namespace DJS.Core.CPlatform.Utilities
{
    /// <summary>
    /// 字段验证类型
    /// </summary>
    public enum VerifyType
    {
        [Description("非Null")]
        IsNull = 0,
        [Description("非Null或者空")]
        IsNullOrEmpty = 1,
        [Description("数字有效性")]
        IsInt = 2,
        [Description("身份证有效性")]
        IsIdCard = 3,
        [Description("邮箱有效性")]
        IsEmail = 4,
        [Description("手机号有效性")]
        IsPhone = 5,
        [Description("Url有效性")]
        IsUrl = 6,
        [Description("IP有效性")]
        IsIP = 7,
        [Description("域名有效性")]
        IsDomain = 8,
        [Description("GUID有效性")]
        IsGuid = 9,
        [Description("日期有效性")]
        IsDate = 10,
        [Description("可空域名有效性")]
        IsDomainOrEmpty = 11,
        [Description("域名或者IP")]
        IsDomainOrIP = 12,
        [Description("可空GUID有效性")]
        IsNullOrGuid = 13,
        [Description("通用父级有效性")]
        IsParentId = 14,
        [Description("通用父级含默认值有效性")]
        IsParentIdOrDefault = 15
    }
}
