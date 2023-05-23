using System;
namespace SensorsData.ABTest
{
    public class SensorsABTestConstants
    {
        ///网络请求需要的属性值
        public static readonly string PLATFORM = "platform";
        public static readonly string DOTNET = "DotNET";
        public static readonly string VERSION = "0.0.2";
        public static readonly string VERSION_KEY = "abtest_lib_version";

        //上报事件使用的key
        public static readonly string EXPERIMENT_ID = "$abtest_experiment_id";
        public static readonly string EXPERIMENT_GROUP_ID = "$abtest_experiment_group_id";
        public static readonly string EVENT_TYPE = "$ABTestTrigger";
        public static readonly string LIB_PLUGIN_VERSION = "$lib_plugin_version";
        public static readonly string AB_TEST_EVENT_LIB_VERSION = "csharp_abtesting";

        public static readonly string SUCCESS = "SUCCESS";

        public static readonly string TRIGGER_EVENT_NAME = "$ABTestTrigger";
    }
}
