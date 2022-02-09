using System;
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
            Console.WriteLine("11111");

            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .SetEventCacheSize(-1)
                .SetExperimentCacheSize(-1)
                .EnableEventCache(false)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);

            Experiement<string> experiement = sensorsABTest.AsyncFetchABTest<string>("AB123456", false, "param_cat", "hello",
                timeoutMilliseconds: -1000, enableAutoTrackEvent: false);
            Console.WriteLine("final result===: " + experiement);

            sensorsABTest.TrackABTestTriggerEvent<string>(experiement);

            Thread.Sleep(5000);

            sa.Shutdown();


        }

    }
}