using DJS.Core.Common.Validate.Validate;
using DJS.Core.CPlatform.Utilities;
using System;
using System.ComponentModel;
using System.Reflection;

namespace DJS.Core.CPlatform.DB.Extensions
{
    public class VerifyExtension
    {

        public static void Verity<T>(T entity)
        {
            var type = typeof(T);
            PropertyInfo[] props = type.GetProperties();
            foreach (PropertyInfo prop in props)
            {
                Object[] attrs = prop.GetCustomAttributes(true);
                string strDesc = string.Empty;
                DescriptionAttribute descAttr = Attribute.GetCustomAttribute(prop, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (descAttr != null)
                {
                    strDesc = descAttr.Description;
                }
                foreach (Object verify in attrs)
                {
                    if (verify is VerifyAttribute)
                    {
                        VerifyType[] Verifys = (verify as VerifyAttribute).Verify;
                        if (Verifys != null && Verifys.Length > 0)
                        {
                            foreach (var Verify in Verifys)
                            {
                                object val = entity.GetType().GetProperty(prop.Name).GetValue(entity, null);
                                VerityEntity(Verify, val, strDesc);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 当个字段合法性校验
        /// </summary>
        /// <param name="Verifys"></param>
        /// <param name="val"></param>
        /// <param name="desc"></param>
        private static void VerityEntity(VerifyType Verifys, object val, string desc)
        {
            switch (Verifys)
            {
                case VerifyType.IsNull:
                    if (val == null)
                    {
                        throw new Exception("字段 '" + desc + "'不能为空！");
                    }
                    break;
                case VerifyType.IsNullOrEmpty:
                    if (val == null || string.IsNullOrEmpty(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'不能为空！");
                    }
                    break;
                case VerifyType.IsInt:
                    if (val != null && !Validate.IsNumber(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'只能为数字！");
                    }
                    break;
                case VerifyType.IsIdCard:
                    if (val != null && !Validate.IsIdCard(val.ToString()))
                    {
                        throw new Exception("身份证格式不正确！");
                    }
                    break;
                case VerifyType.IsEmail:
                    if (val != null && !Validate.IsEmail(val.ToString()))
                    {
                        throw new Exception("邮箱格式不正确！");
                    }
                    break;
                case VerifyType.IsPhone:
                    if (val != null && !Validate.IsValidPhoneAndMobile(val.ToString()))
                    {
                        throw new Exception("手机号格式不正确！");
                    }
                    break;
                case VerifyType.IsUrl:
                    if (val != null && !Validate.IsValidURL(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
                case VerifyType.IsIP:
                    if (val != null && !Validate.IsValidIP(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
                case VerifyType.IsDomain:
                    if (val != null && !Validate.IsValidDomain(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
                case VerifyType.IsDomainOrEmpty:
                    if (val != null && !string.IsNullOrEmpty(val.ToString()) && !Validate.IsValidDomain(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
                case VerifyType.IsGuid:
                    if (!Validate.IsGuid(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
                case VerifyType.IsDate:
                    if (val != null && !string.IsNullOrEmpty(val.ToString()) && !Validate.IsDate(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
                case VerifyType.IsDomainOrIP:
                    if (val != null && !string.IsNullOrEmpty(val.ToString()) && !(Validate.IsValidDomain(val.ToString()) || !Validate.IsValidIP(val.ToString())))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
                case VerifyType.IsNullOrGuid:
                    if (val != null && !string.IsNullOrEmpty(val.ToString()) && !Validate.IsGuid(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
                case VerifyType.IsParentId:
                    if (!Validate.IsGuid(val.ToString()))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
                case VerifyType.IsParentIdOrDefault:
                    if (val != null && val.ToString() != "0" && (!string.IsNullOrEmpty(val.ToString()) && !Validate.IsGuid(val.ToString())))
                    {
                        throw new Exception("字段 '" + desc + "'格式不正确！");
                    }
                    break;
            }
        }
    }
}
