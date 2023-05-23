using System;
using System.Collections.Generic;
using BitFaster.Caching.Lru;
using Newtonsoft.Json;
using SensorsData.ABTest.Bean;

namespace SensorsData.ABTest
{
    public class SensorsABTestManager
    {
        private ABTestConfig config;
        private ExperimentCacheManager experimentCacheManager;
        private TriggerEventCacheManager triggerEventCacheManager;
        private HttpManager httpManager;
        private long lastLibPluginTime = 0;
        private readonly long ONE_DAY_MILLISECONDS = 24 * 60 * 60 * 1000;

        public SensorsABTestManager(ABTestConfig config)
        {
            this.config = config;
            experimentCacheManager = new ExperimentCacheManager(config.ExperimentCacheTime, config.ExperimentCacheSize);
            triggerEventCacheManager = new TriggerEventCacheManager(config.EventCacheTime, config.EventCacheSize);
            httpManager = new HttpManager(config.ApiUrl);
        }

        /// <summary>
        /// 获取 A/B Test 试验结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="distinctId"></param>
        /// <param name="isLoginId"></param>
        /// <param name="experimentVariableName"></param>
        /// <param name="defaultValue"></param>
        /// <param name="enableAutoTrackEvent"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// <param name="enableCache"> 默认为 true，表示先冲缓存中获取</param>
        /// <param name="customProperties">自定义属性，默认值为 null</param>
        /// <returns></returns>
        public Experiement<T> fetchABTest<T>(string distinctId, bool isLoginId, string experimentVariableName,
            T defaultValue, bool enableAutoTrackEvent = true, int timeoutMilliseconds = 3000, bool enableCache = true, Dictionary<string, object> customProperties = null)
        {
            if (timeoutMilliseconds <= 0)
            {
                timeoutMilliseconds = 3000;
            }
            if (isEmpty(distinctId))
            {
                SensorsABTestLogger.info("The distinctId is empty or null, return default value.");
                return new Experiement<T>(distinctId, isLoginId, defaultValue);
            }

            if (isEmpty(experimentVariableName))
            {
                SensorsABTestLogger.info("The experimentVariableName is empty or null, return default value.");
                return new Experiement<T>(distinctId, isLoginId, defaultValue);
            }

            if (!SensorsABTestUtils.AssertDefaultValueType(defaultValue))
            {
                SensorsABTestLogger.info("The type of default value is not Number、String、Boolean or Json String, the the default value will be returned.");
                return new Experiement<T>(distinctId, isLoginId, defaultValue);
            }
            HttpABTestResult httpABTestResult;
            //先从缓存中获取
            if (enableCache)
            {
                httpABTestResult = experimentCacheManager.GetExperimentResultByCache(distinctId, isLoginId, experimentVariableName);
                //如果未命中
                if (httpABTestResult == null)
                {
                    SensorsABTestLogger.info($"Not hit experiment cache, making network request.{distinctId}; {experimentVariableName}.");
                    httpABTestResult = GetABTestByHttp(distinctId, isLoginId, timeoutMilliseconds, experimentVariableName, customProperties);

                    //缓存试验结果，只有 enableCache 时才会缓存试验结果
                    experimentCacheManager.SetExperimentResultCache(distinctId, isLoginId, httpABTestResult);
                }
            }
            else
            {
                httpABTestResult = GetABTestByHttp(distinctId, isLoginId, timeoutMilliseconds, experimentVariableName, customProperties);
            }

            Experiement<T> result = convertExperiment(httpABTestResult, distinctId, isLoginId, experimentVariableName, defaultValue);

            //如果运行缓存
            if (enableAutoTrackEvent)
            {
                try
                {
                    trackABTestTrigger<T>(result, null);
                }
                catch (Exception)
                {
                    SensorsABTestLogger.info($"Failed auto track ABTest event.distinctId:{distinctId};isLoginId:{isLoginId};experimentVariableName:{experimentVariableName}");
                }
            }
            return result;
        }

        public void trackABTestTrigger<T>(Experiement<T> experiement, Dictionary<string, object> properties)
        {
            if (experiement == null)
            {
                SensorsABTestLogger.info("The track ABTest event experiment result is null.");
                return;
            }
            //如果在白名单中，就不触发
            if (experiement.isWhiteList == true || experiement.abTestExperimentId == null)
            {
                SensorsABTestLogger.info($"The track ABTest event user not hit experiment or in the whiteList.distinctId:{experiement.distinctId}");
                return;
            }
            //如果缓存中已经存在，就不触发
            if (triggerEventCacheManager.IsEventCacheExist(experiement.distinctId, experiement.isLoginId, experiement.abTestExperimentId))
            {
                SensorsABTestLogger.info($"The event has been triggered.distinctId:{experiement.distinctId}、experimentId:{experiement.abTestExperimentId}.");
                return;
            }
            if (properties == null)
            {
                properties = new Dictionary<string, object>();
            }

            properties.Add(SensorsABTestConstants.EXPERIMENT_ID, experiement.abTestExperimentId);
            properties.Add(SensorsABTestConstants.EXPERIMENT_GROUP_ID, experiement.abTestExperimentGroupId);
            properties.Add("$is_login_id", experiement.isLoginId);

            long currentTime = SensorsABTestUtils.GetTimeStamp();
            //超过 24 小时，就添加记录
            if ((currentTime - lastLibPluginTime) > ONE_DAY_MILLISECONDS)
            {
                List<string> libPlugin = new List<string>();
                libPlugin.Add(SensorsABTestConstants.AB_TEST_EVENT_LIB_VERSION + ":" + SensorsABTestConstants.VERSION);
                properties.Add(SensorsABTestConstants.LIB_PLUGIN_VERSION, libPlugin);
                lastLibPluginTime = currentTime;
            }

            this.config.SensorsAnalytics.Track(experiement.distinctId, SensorsABTestConstants.TRIGGER_EVENT_NAME, properties);
            //缓存事件
            if (this.config.EnableEventCache)
            {
                triggerEventCacheManager.SetEventCache(experiement.distinctId, experiement.isLoginId, experiement.abTestExperimentId);
            }
        }

        private Experiement<T> convertExperiment<T>(HttpABTestResult httpResult, string distinctId, bool isLoginId, string experimentVariableName, T defaultValue)
        {
            if (httpResult == null)
            {
                SensorsABTestLogger.info($"The experiment result is null,return defaultValue：{defaultValue}");
                return new Experiement<T>(distinctId, isLoginId, defaultValue);
            }

            if (!SensorsABTestConstants.SUCCESS.Equals(httpResult.status))
            {
                SensorsABTestLogger.info($"The experiment result is error, error_type is {httpResult.error_type}, error_msg is {httpResult.error}.");
                return new Experiement<T>(distinctId, isLoginId, defaultValue);
            }

            HttpABTestExperimentResult[] experimentResults = httpResult.results;
            if (experimentResults != null && experimentResults.Length != 0)
            {
                foreach (HttpABTestExperimentResult result in experimentResults)
                {
                    HttpABTestVariable[] variables = result.variables;
                    if (variables != null && variables.Length != 0)
                    {
                        foreach (HttpABTestVariable variable in variables)
                        {
                            if (experimentVariableName.Equals(variable.name))
                            {
                                Experiement<T> value = hitExperimentValue(variable, result, distinctId, isLoginId, experimentVariableName, defaultValue);
                                if (value != null)
                                {
                                    return value;
                                }
                            }
                        }
                    }
                }
            }
            return new Experiement<T>(distinctId, isLoginId, defaultValue); ;
        }

        private Experiement<T> hitExperimentValue<T>(HttpABTestVariable variable, HttpABTestExperimentResult result,
            string distinctId, bool isLoginId, string experimentVariableName, T defaultValue)
        {
            string value = variable.value;
            Experiement<T> experiement = new Experiement<T>();
            bool isFound = false;
            switch (variable.type)
            {
                case "STRING":
                    if (defaultValue is string)
                    {
                        if (typeof(T) == typeof(string))
                        {
                            experiement.result = (T)(object)value;
                            isFound = true;
                        }
                    }
                    break;
                case "INTEGER":
                    if (defaultValue is int)
                    {
                        if (typeof(T) == typeof(int))
                        {
                            experiement.result = (T)(object)(int.Parse(value));
                            isFound = true;
                        }
                    }
                    break;
                case "JSON":
                    if (defaultValue is string)
                    {
                        if (typeof(T) == typeof(string))
                        {
                            experiement.result = (T)(object)value;
                            isFound = true;
                        }
                    }
                    break;
                case "BOOLEAN":
                    if (defaultValue is bool)
                    {
                        if (typeof(T) == typeof(bool))
                        {
                            experiement.result = (T)(object)(bool.Parse(value));
                            isFound = true;
                        }
                    }
                    break;
                //未命中类型
                default:
                    break;
            }
            if (experiement.result != null && isFound)
            {
                experiement.distinctId = distinctId;
                experiement.isLoginId = isLoginId;
                experiement.abTestExperimentGroupId = result.abtest_experiment_group_id;
                experiement.abTestExperimentId = result.abtest_experiment_id;
                experiement.isControlGroup = result.is_control_group;
                experiement.isWhiteList = result.is_white_list;
                return experiement;
            }
            return null;
        }

        /// <summary>
        /// 发送网络请求
        /// </summary>
        /// <param name="distinctId"></param>
        /// <param name="isLoginId"></param>
        /// <param name="timeoutMilliseconds"></param>
        /// <param name="experimentVariableName"></param>
        /// <param name="customProperties"></param>
        /// <returns></returns>
        private HttpABTestResult GetABTestByHttp(string distinctId, bool isLoginId, int timeoutMilliseconds, string experimentVariableName, Dictionary<string, object> customProperties)
        {
            try
            {
                HttpABTestRequest httpABTestRequest = new HttpABTestRequest();
                if (isLoginId)
                {
                    httpABTestRequest.login_id = distinctId;
                }
                else
                {
                    httpABTestRequest.anonymous_id = distinctId;
                }

                httpABTestRequest.platform = SensorsABTestConstants.DOTNET;
                httpABTestRequest.abtest_lib_version = SensorsABTestConstants.VERSION;
                httpABTestRequest.properties = new HttpABTestRequestProperties();
                Dictionary<string, object> newCustomProperties = SensorsABTestUtils.customPropertiesHandler(customProperties);
                if (newCustomProperties != null && newCustomProperties.Count > 0)
                {
                    httpABTestRequest.custom_properties = newCustomProperties;
                    httpABTestRequest.param_name = experimentVariableName;
                }
                string content = JsonConvert.SerializeObject(httpABTestRequest);
                string result = httpManager.SendToServer(content, timeoutMilliseconds);
                //Console.WriteLine($"Http request result: {result}");
                return JsonConvert.DeserializeObject<HttpABTestResult>(result);
            }
            catch (ArgumentException e)
            {
                SensorsABTestLogger.error($"Invalid custom properties, distinctid:{distinctId}, isLoginId:{isLoginId}, experimentName:{experimentVariableName}. Error messages:" + e.Message);
            }
            catch (Exception e)
            {
                SensorsABTestLogger.error("http request error:" + e.Message);
            }
            return null;
        }

        private bool isEmpty(string str)
        {
            return str == null || str.Length == 0;
        }

    }

    /// <summary>
    /// 试验结果缓存管理
    /// </summary>
    public class ExperimentCacheManager
    {
        /// <summary>
        /// 关于 Cache 的相关资料
        /// https://www.cjavapy.com/article/496/
        /// https://github.com/planetarium/LruCacheNet
        /// https://github.com/bitfaster/BitFaster.Caching
        /// https://github.com/JKirk865/LRUCache/blob/master/LRUCacheTests/SimpleLRUCacheTests_lockfree.cs
        /// </summary>
        private ConcurrentTLru<string, HttpABTestResult> lruCache;

        public ExperimentCacheManager(int cacheTime, int cacheSize)
        {
            lruCache = new ConcurrentTLru<string, HttpABTestResult>(cacheSize, TimeSpan.FromMinutes(cacheTime));
        }

        /// <summary>
        /// 生成缓存 key 值
        /// </summary>
        /// <param name="distinctId"></param>
        /// <param name="isLoginId"></param>
        /// <returns></returns>
        private string generateKey(string distinctId, bool isLoginId)
        {
            return distinctId + "_" + isLoginId;
        }

        /// <summary>
        /// 设置试验缓存
        /// </summary>
        /// <param name="distinctId"></param>
        /// <param name="isLoginId"></param>
        /// <param name="cacheData"></param>
        public void SetExperimentResultCache(string distinctId, bool isLoginId, HttpABTestResult cacheData)
        {
            //JsonConvert.SerializeObject();
            //JsonConvert.DeserializeObject
            if (cacheData != null)
            {
                lruCache.AddOrUpdate(generateKey(distinctId, isLoginId), cacheData);
            }
        }

        /// <summary>
        /// 从缓存中获取试验
        /// </summary>
        /// <param name="distinctId"></param>
        /// <param name="isLoginId"></param>
        /// <param name="experimentName"></param>
        /// <returns></returns>
        public HttpABTestResult GetExperimentResultByCache(string distinctId, bool isLoginId, string experimentName)
        {
            if (experimentName == null || distinctId == null)
            {
                SensorsABTestLogger.info($"distinctId: {distinctId}、isLoginId: {isLoginId} or experimentName: {experimentName} can not be null.");
                return null;
            }
            HttpABTestResult cacheData;
            bool isGet = lruCache.TryGet(generateKey(distinctId, isLoginId), out cacheData);
            if (isGet)
            {
                HttpABTestExperimentResult[] experimentResults = cacheData.results;
                if (experimentResults != null && experimentResults.Length != 0)
                {
                    foreach (HttpABTestExperimentResult result in experimentResults)
                    {
                        HttpABTestVariable[] variables = result.variables;
                        if (variables != null && variables.Length != 0)
                        {
                            foreach (HttpABTestVariable variable in variables)
                            {
                                if (variable != null && variable.name != null && variable.name.Equals(experimentName))
                                {
                                    return cacheData;
                                }
                            }
                        }
                    }
                }
            }
            SensorsABTestLogger.info($"distinctId: {distinctId}、isLoginId: {isLoginId} or experimentName: {experimentName} not hit cache result.");
            return null;
        }
    }

    /// <summary>
    /// $ABTestTrigger 缓存管理
    /// </summary>
    public class TriggerEventCacheManager
    {
        private ConcurrentTLru<string, bool> lruCache;

        public TriggerEventCacheManager(int cacheTime, int cacheSize)
        {
            lruCache = new ConcurrentTLru<string, bool>(cacheSize, TimeSpan.FromMinutes(cacheTime));
        }

        /// <summary>
        /// 判断事件是否在缓存中
        /// </summary>
        /// <param name="distinctId"></param>
        /// <param name="isLoginId"></param>
        /// <param name="experimentId"></param>
        /// <returns></returns>
        public bool IsEventCacheExist(string distinctId, bool isLoginId, string experimentId)
        {
            return lruCache.TryGet(generateKey(distinctId, isLoginId, experimentId), out _);
        }

        /// <summary>
        /// 将事件保存在缓存中
        /// </summary>
        /// <param name="distinctId"></param>
        /// <param name="isLoginId"></param>
        /// <param name="experimentId"></param>
        public void SetEventCache(string distinctId, bool isLoginId, string experimentId)
        {
            lruCache.AddOrUpdate(generateKey(distinctId, isLoginId, experimentId), true);
        }

        /// <summary>
        ///  生成缓存 key
        /// </summary>
        /// <param name="distinctId"></param>
        /// <param name="isLoginId"></param>
        /// <param name="experimentId"></param>
        /// <returns></returns>
        private string generateKey(string distinctId, bool isLoginId, string experimentId)
        {
            return distinctId + "_" + isLoginId + "_" + experimentId;
        }
    }
}
