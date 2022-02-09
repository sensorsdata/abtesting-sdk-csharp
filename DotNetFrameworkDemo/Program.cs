using System;
using SensorsData.ABTest;
using SensorsData.ABTest.Bean;
using SensorsData.Analytics;

namespace DotNetFrameworkDemo
{
    class Program
    {
        static void Main(string[] args)
        {


            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca", "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<string> result = sensorsABTest.FastFetchABTest<string>("AB123456", false, "param_cat", "hellokitty");
            Console.WriteLine("final result===" + result);
            //Experiement<bool> result2 = sensorsABTest.FastFetchABTest<bool>("AB123456", false, "param_dog", false);
            //Experiement<int> result2 = sensorsABTest.FastFetchABTest<int>("AB123456", false, "qqqq", 22);
            //Console.WriteLine("final result2===" + result2);
            sa.Shutdown();


        }

    }
}
