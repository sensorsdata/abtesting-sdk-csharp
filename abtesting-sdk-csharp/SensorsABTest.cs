using System;
using System.Collections.Generic;
using SensorsData.ABTest.Bean;

namespace SensorsData.ABTest
{
    public interface ISensorsABTest
    {
        /// <summary>
        /// 立即从服务端请求试验结果，忽略内存配置
        /// </summary>
        /// <typeparam name="T">支持的泛型包括：number｜bool｜string｜json string</typeparam>
        /// <param name="distinctId">匿名ID/用户业务ID</param>
        /// <param name="isLoginId">是否为登录 ID</param>
        /// <param name="experimentVariableName">试验变量名</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="enableAutoTrackEvent">是否自动上报 $ABTestTrigger 事件，默认值为 true</param>
        /// <param name="timeoutMilliseconds">网络请求超时，单位：ms，默认值为 3000</param>
        /// <param name="properties">自定义属性，默认值为 null</param>
        /// <returns>试验结果</returns>
        Experiement<T> AsyncFetchABTest<T>(string distinctId, bool isLoginId, string experimentVariableName, T defaultValue,
            bool enableAutoTrackEvent = true, int timeoutMilliseconds = 3000, Dictionary<string, object> properties = null);

        /// <summary>
        /// 优先读取内存缓存，缓存不存在时从再服务端获取试验数据
        /// </summary>
        /// <typeparam name="T">支持的泛型包括：number｜bool｜string｜json string</typeparam>
        /// <param name="distinctId">匿名ID/用户业务ID</param>
        /// <param name="isLoginId">是否为登录 ID</param>
        /// <param name="experimentVariableName">试验变量名</param>
        /// <param name="defaultValue">默认值，默认值为 null</param>
        /// <param name="enableAutoTrackEvent">是否自动上报 $ABTestTrigger 事件，默认值为 true</param>
        /// <param name="timeoutMilliseconds">网络请求超时，单位：ms，默认值为 3000</param>
        /// <param name="properties">自定义属性，默认值为 null</param>
        /// <returns>试验结果</returns>
        Experiement<T> FastFetchABTest<T>(string distinctId, bool isLoginId, string experimentVariableName, T defaultValue,
            bool enableAutoTrackEvent = true, int timeoutMilliseconds = 3000, Dictionary<string, object> properties = null);


        /// <summary>
        /// 手动上报 $ABTestTrigger 事件
        /// </summary>
        /// <typeparam name="T"> 支持数据类型：number｜bool｜string｜json string</typeparam>
        /// <param name="experiement">试验结果</param>
        /// <param name="properties">额外属性</param>
        void TrackABTestTriggerEvent<T>(Experiement<T> experiement, Dictionary<string, object> properties = null);

    }

    public class SensorsABTest : ISensorsABTest
    {

        private readonly SensorsABTestManager sensorsABTestManager;
        public SensorsABTest(ABTestConfig config)
        {
            sensorsABTestManager = new SensorsABTestManager(config);
        }

        public Experiement<T> AsyncFetchABTest<T>(string distinctId, bool isLoginId, string experimentVariableName, T defaultValue, bool enableAutoTrackEvent = true, int timeoutMilliseconds = 3000, Dictionary<string, object> properties = null)
        {
            return sensorsABTestManager.fetchABTest<T>(distinctId, isLoginId, experimentVariableName, defaultValue, enableAutoTrackEvent, timeoutMilliseconds, false, properties);
        }

        public Experiement<T> FastFetchABTest<T>(string distinctId, bool isLoginId, string experimentVariableName, T defaultValue, bool enableAutoTrackEvent = true, int timeoutMilliseconds = 3000, Dictionary<string, object> properties = null)
        {
            return sensorsABTestManager.fetchABTest<T>(distinctId, isLoginId, experimentVariableName, defaultValue, enableAutoTrackEvent, timeoutMilliseconds, true, properties);
        }

        public void TrackABTestTriggerEvent<T>(Experiement<T> experiement, Dictionary<string, object> properties = null)
        {
            sensorsABTestManager.trackABTestTrigger(experiement, properties);
        }
    }
}