using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SensorsData.ABTest
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class SensorsABTestUtils
    {
        private static readonly Regex KEY_PATTERN = new Regex("^((?!^distinct_id$|^original_id$|^time$|^properties$|^id$|^first_id$|^second_id$|^users$|^events$|^event$|^user_id$|^date$|^datetime|^user_group|^user_tag)[a-zA-Z_$][a-zA-Z\\d_$]{0,99})$", RegexOptions.IgnoreCase);

        public SensorsABTestUtils()
        {
        }

        public static bool AssertDefaultValueType<T>(T value)
        {
            return IsNumber(value) || value is string || value is bool;
        }

        /// <summary>
        /// 获取时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds);
        }

        /// <summary>
        /// 判断是否是数字
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumber(object value)
        {
            return (value is sbyte) || (value is short) || (value is int) || (value is long) || (value is byte)
                    || (value is ushort) || (value is uint) || (value is ulong) || (value is decimal) || (value is Single)
                    || (value is float) || (value is double);
        }

        public static bool IsJsonStr(object value)
        {
            //首先判断是否是字符串
            if (!(value is string))
            {
                return false;
            }
            try
            {
                JObject jObject = JObject.Parse((string)value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static Dictionary<string, object> customPropertiesHandler(Dictionary<string, object> customProperties)
        {
            if (customProperties == null)
            {
                return null;
            }
            Dictionary<string, object> newProperties = new Dictionary<string, object>();
            foreach (KeyValuePair<string, object> kvp in customProperties)
            {
                string key = kvp.Key;
                object value = kvp.Value;
                //1.判断 key 的格式和类型
                if (key == null || key.Length == 0)
                {
                    throw new ArgumentException("The property's name is empty.");
                }
                if (key.Length > 100)
                {
                    throw new ArgumentException($"The property's name [{key}] is too long, max length is 100.");
                }
                if (!KEY_PATTERN.IsMatch(key))
                {
                    throw new ArgumentException($"The property's name [{key}] is invalid format.");
                }
                //2.判断 value 的格式和类型alue
                if (value == null)
                {
                    throw new ArgumentException($"The key [{key}]'s property value is null.");
                }
                if (!IsNumber(value) && !(value is string) && !(value is DateTime) && !(value is bool) && !(value is List<string>))
                {
                    throw new ArgumentException("The property value should be a basic type: Number, String, Date, Boolean, List<String>.");
                }
                // String 类型的属性值，长度不能超过 8192
                if ((value is string) && value != null && ((string)value).Length > 8191)
                {
                    throw new ArgumentException($"The property's value's length of [{key}] is too long. Max length is 8192.");
                }
                if (value is DateTime)
                {
                    value = ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.fff");
                }

                if (value.GetType().IsGenericType && value is List<string>)
                {
                    value = JsonConvert.SerializeObject(value);
                }
                newProperties[key] = value;
            }
            return newProperties;
        }
    }
}
