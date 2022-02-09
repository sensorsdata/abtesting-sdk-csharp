using System;
using Newtonsoft.Json;
using SensorsData.Analytics;

namespace SensorsData.ABTest.Bean
{
    /// <summary>
    /// 网络请求时，返回的试验结果
    /// </summary>
    public class HttpABTestResult {
        public string status;
        public string error_type;
        public string error;
        public HttpABTestExperimentResult[] results;
    }

    /// <summary>
    /// 网络请求时，试验 item
    /// </summary>
    public class HttpABTestExperimentResult {
        public string abtest_experiment_id;
        public string abtest_experiment_group_id;
        public bool is_control_group;
        public bool is_white_list;
        public HttpABTestVariable[] variables;
    }

    /// <summary>
    /// 试验 item，对应的变量
    /// </summary>
    public class HttpABTestVariable {
        public string name;
        public string value;
        public string type;
    }

    public class HttpABTestRequest
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string abtest_lib_version { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string anonymous_id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string login_id { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string param_name { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string platform { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public HttpABTestRequestProperties properties { get; set; }
    }

    public class HttpABTestRequestProperties
    {

    }

    /// <summary>
    /// 试验结果
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Experiement<T>
    {
        /// <summary>
        /// 匿名 ID 或登录 ID
        /// </summary>
        public string distinctId;

        /// <summary>
        /// 是否为登录ID
        /// </summary>
        public Boolean isLoginId;

        /// <summary>
        /// 试验 ID
        /// </summary>
        public string abTestExperimentId;

        /// <summary>
        /// 试验分组 ID
        /// </summary>
        public string abTestExperimentGroupId;

        /// <summary>
        /// 是否是对照组
        /// </summary>
        public Boolean? isControlGroup;

        /// <summary>
        /// 是否在白名单中，在白名单中的用户不触发 $ABTestTrigger 事件
        /// </summary>
        public Boolean? isWhiteList;

        /// <summary>
        /// 命中的结果
        /// </summary>
        public T result;


        public Experiement(string distinctId, Boolean isLoginId, T value)
        {
            this.distinctId = distinctId;
            this.isLoginId = isLoginId;
            this.result = value;
        }

        public Experiement()
        {
        }

        public override string ToString()
        {
            return $"distinctId:{distinctId}, isLoginId:{isLoginId}, abTestExperimentId:{abTestExperimentId}," +
                $" abTestExperimentGroupId:{abTestExperimentGroupId}, isControlGroup:{isControlGroup}," +
                $" isWhiteList:{isWhiteList}, result:{result}";
        }
    }

    public class ABTestConfig {

        /// <summary>
        /// $ABTestTrigger 事件单用户缓存时间，范围是：0 ~ 1440，默认值 1440，单位分钟
        /// </summary>
        private int eventCacheTime = 1440;

        /// <summary>
        /// $ABTestTrigger 事件总缓存用户量限制，默认值 4096，不能小于 0
        /// </summary>
        private int eventCacheSize = 4096;

        /// <summary>
        /// 试验总缓存用户量限制，默认值 4096，不能小于 0
        /// </summary>
        private int experimentCacheSize = 4096;

        /// <summary>
        /// 试验单用户缓存时间限制，范围是：0 ~ 1440，默认值 1440，单位分钟
        /// </summary>
        private int experimentCacheTime = 1440;

        /// <summary>
        /// 是否默认自动上报 $ABTestTrigger 事件
        /// </summary>
        private bool enableEventCache = true;

        /// <summary>
        /// 分流 api
        /// </summary>
        private string apiUrl;

        /// <summary>
        /// 埋点 SDK 
        /// </summary>
        private SensorsAnalytics sensorsAnalytics;

        //只提供 get 方法供外部使用

        public int EventCacheTime
        {
            get { return eventCacheTime; }
        }

        public int EventCacheSize
        {
            get { return eventCacheSize; }
        }

        public int ExperimentCacheSize
        {
            get { return experimentCacheSize; }
        }

        public int ExperimentCacheTime
        {
            get { return experimentCacheTime; }
        }

        public bool EnableEventCache
        {
            get { return enableEventCache; }
        }

        public string ApiUrl
        {
            get { return apiUrl; }
        }

        public SensorsAnalytics SensorsAnalytics
        {
            get { return sensorsAnalytics; }
        }

        public static Builder builder()
        {
            return new Builder();
        }

        //builder 
        public class Builder {

            private int eventCacheTime = 1440;
            private int eventCacheSize = 4096;
            private int experimentCacheSize = 4096;
            private int experimentCacheTime = 1440;
            private bool enableEventCache = true;
            private string apiUrl;
            private SensorsAnalytics sensorsAnalytics;

            /// <summary>
            /// $ABTestTrigger 事件单用户缓存时间，范围是：0 ~ 1440，默认值 1440，单位分钟
            /// </summary>
            /// <param name="eventCacheTime"></param>
            /// <returns></returns>
            public ABTestConfig.Builder SetEventCacheTime(int eventCacheTime) {
                if (eventCacheTime > 0 && eventCacheTime <= 1440) {
                    this.eventCacheTime = eventCacheTime;
                }
                return this;
            }

            /// <summary>
            /// $ABTestTrigger 事件总缓存用户量限制，默认值 4096，最小值不能小于 3
            /// </summary>
            /// <param name="eventCacheSize"></param>
            /// <returns></returns>
            public ABTestConfig.Builder SetEventCacheSize(int eventCacheSize) {
                if (eventCacheSize >= 0 && eventCacheSize < 3) {
                    this.eventCacheSize = 3;
                    Console.WriteLine($"The cache size must greater than or equal to 3, " +
                        $"current value is {experimentCacheSize}, so eventCacheSize value will be set to 3. ");
                }
                else if (eventCacheSize >= 3)
                {
                    this.eventCacheSize = eventCacheSize;
                } 
                return this;
            }

            /// <summary>
            /// 试验总缓存用户量限制，默认值 4096，最小值不能小于 3
            /// </summary>
            /// <param name="experimentCacheSize"></param>
            /// <returns></returns>
            public ABTestConfig.Builder SetExperimentCacheSize(int experimentCacheSize)
            {
                if (experimentCacheSize >= 0 && experimentCacheSize < 3)
                {
                    this.experimentCacheSize = 3;
                    Console.WriteLine($"The cache size must greater than or equal to 3, " +
                        $"current value is {experimentCacheSize}, so experimentCacheSize value will be set to 3. ");
                }
                else if (experimentCacheSize >= 3)
                {
                    this.experimentCacheSize = experimentCacheSize;
                }
                return this;
            }

            /// <summary>
            /// 试验单用户缓存时间限制，范围是：0 ~ 1440，默认值 1440，单位分钟
            /// </summary>
            /// <param name="experimentCacheTime"></param>
            /// <returns></returns>
            public ABTestConfig.Builder SetExperimentCacheTime(int experimentCacheTime)
            {
                if (experimentCacheTime > 0 && experimentCacheTime <= 1440)
                {
                    this.experimentCacheTime = experimentCacheTime;
                }
                return this;
            }

            /// <summary>
            /// 是否默认上报 $ABTestTrigger 事件
            /// </summary>
            /// <param name="enableEventCache"></param>
            /// <returns></returns>
            public ABTestConfig.Builder EnableEventCache(bool enableEventCache) {
                this.enableEventCache = enableEventCache;
                return this;
            }

            /// <summary>
            /// 埋点 SDK 
            /// </summary>
            /// <param name="sensorsAnalytics"></param>
            /// <returns></returns>
            public ABTestConfig.Builder SetSensorsAnalytics(SensorsAnalytics sensorsAnalytics) {
                this.sensorsAnalytics = sensorsAnalytics;
                return this;
            }

            public ABTestConfig.Builder SetApiUrl(string apiUrl)
            {
                this.apiUrl = apiUrl;
                return this;
            }

            public ABTestConfig Build() {

                ABTestConfig config = new ABTestConfig();
                if (this.sensorsAnalytics == null) {
                    throw new ArgumentNullException("The SensorsAnalytcis SDK instance is empty.");
                }
                if (apiUrl == null || apiUrl.Length == 0) {
                    throw new ArgumentNullException("The apiUrl is empty.");
                }
                config.apiUrl = apiUrl;
                config.enableEventCache = enableEventCache;
                config.eventCacheSize = eventCacheSize;
                config.eventCacheTime = eventCacheTime;
                config.experimentCacheSize = experimentCacheSize;
                config.experimentCacheTime = experimentCacheTime;
                config.sensorsAnalytics = sensorsAnalytics;
                return config;
            }
        }
    }
}
