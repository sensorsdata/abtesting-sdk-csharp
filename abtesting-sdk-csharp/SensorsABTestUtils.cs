using System;
using Newtonsoft.Json.Linq;

namespace SensorsData.ABTest
{
    /// <summary>
    /// 工具类
    /// </summary>
    public class SensorsABTestUtils
    {
        public SensorsABTestUtils()
        {
        }

        public static bool AssertDefaultValueType<T>( T value)
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

        public static bool IsJsonStr(object value) {
            //首先判断是否是字符串
            if(!(value is string))
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
    }
}
