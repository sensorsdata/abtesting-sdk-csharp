using System;
using System.Collections.Generic;
using System.Threading;
using SensorsData.ABTest;
using SensorsData.ABTest.Bean;
using SensorsData.Analytics;


namespace DotNetCoreDemo
{


    class Program
    {
        static void Main(string[] args)
        {

            SensorsABTestLogger.info("hello");
            SensorsABTestLogger.error("world");

            string apiUrl = "http://10.129.29.10:8202/api/v2/abtest/online/results?project-key=130EB9E0EE57A09D91AC167C6CE63F7723CE0B22";
            IConsumer consumer = new NewClientConsumer("http://10.129.28.106:8106/sa?project=default", "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Dictionary<string, object> properties = new Dictionary<string, object>();
            //properties.Add("key_str", "sss");
            //properties.Add("key_str_name", "sss");
            //properties.Add("key_bool", true);
            //properties.Add("key_number", 111);
            //properties.Add("key_time", DateTime.Now);
            //List<string> list = new List<string>();
            //list.Add("item1");
            //list.Add("item2");
            //properties.Add("key_list", list);
            properties.Add("$time", DateTime.Now);

            Experiement<string> experiement =
                sensorsABTest.FastFetchABTest<string>("AB123456222", true, "cqs_color",
                "hellokitty", timeoutMilliseconds: 30000, properties: properties);
            Console.Error.WriteLine("result  ==" + experiement.result);

            //properties.Add("is_hk_user", true);
            //Experiement<string> experiement2 = sensorsABTest.AsyncFetchABTest<string>("AB123456", false, "cqs_color2",
            //    "hellokitty", timeoutMilliseconds: 30000, properties: properties);

            //Console.Error.WriteLine("result  =="+ experiement2.result);

            sa.Shutdown();

        }

    }
}