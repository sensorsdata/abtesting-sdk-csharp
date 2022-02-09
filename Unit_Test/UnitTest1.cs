using System;
using Xunit;
using SensorsData.ABTest;
using SensorsData.ABTest.Bean;
using SensorsData.Analytics;
using System.Collections.Generic;

namespace Unit_Test
{
    //单元测试介绍：https://docs.microsoft.com/zh-cn/dotnet/core/testing/unit-testing-with-dotnet-test
    public class UnitTest1
    {
        //以下试验对象均存在
        [Fact]
        public void Test1()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<string> experiement = sensorsABTest.FastFetchABTest<string>("AB123456", false, "param_cat", "hellokitty");
            Assert.NotNull(experiement);
            Assert.NotNull(experiement.abTestExperimentId);
            Assert.Equal("shiyan2", experiement.result);
            sa.Shutdown();
        }


        [Fact]
        public void Test2()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<bool> experiement = sensorsABTest.FastFetchABTest<bool>("AB123456", false, "param_dog", false);
            Assert.NotNull(experiement);
            Assert.NotNull(experiement.abTestExperimentId);
            Assert.True(experiement.result);
            sa.Shutdown();
        }

        [Fact]
        public void Test3()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<int> experiement = sensorsABTest.FastFetchABTest<int>("AB123456", false, "qqqq", 22);
            Assert.NotNull(experiement);
            Assert.NotNull(experiement.abTestExperimentId);
            Assert.Equal(111, experiement.result);
            sa.Shutdown();
        }

        //以下试验对象均不存在
        [Fact]
        public void Test4()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<string> experiement = sensorsABTest.FastFetchABTest<string>("AB123456", false, "not_found", "not_found_default");
            Assert.NotNull(experiement);
            Assert.Null(experiement.abTestExperimentId);
            Assert.Equal("not_found_default", experiement.result);
            sa.Shutdown();
        }

        [Fact]
        public void Test5()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<bool> experiement = sensorsABTest.FastFetchABTest<bool>("AB123456", false, "param_dog2", false);
            Assert.NotNull(experiement);
            Assert.Null(experiement.abTestExperimentId);
            Assert.False(experiement.result);
            sa.Shutdown();
        }


        [Fact]
        public void Test5_1()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<bool> experiement = sensorsABTest.FastFetchABTest<bool>("AB123456", false, "param_dog2", true);//默认 true
            Assert.NotNull(experiement);
            Assert.Null(experiement.abTestExperimentId);
            Assert.True(experiement.result);
            sa.Shutdown();
        }

        [Fact]
        public void Test6()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<int> experiement = sensorsABTest.FastFetchABTest<int>("AB123456", false, "qqqq2", 22);
            Assert.NotNull(experiement);
            Assert.Null(experiement.abTestExperimentId);
            Assert.Equal(22, experiement.result);
            sa.Shutdown();
        }

        //测试接口的异常情况
        [Fact]
        public void Test7()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig.Builder builder = ABTestConfig.builder();
            //.setSensorsAnalytics(sa)
            //.setApiUrl(apiUrl)
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void Test8()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig.Builder builder = ABTestConfig.builder();
            builder.SetSensorsAnalytics(sa);
            //.setApiUrl(apiUrl)
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }

        [Fact]
        public void Test9()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig.Builder builder = ABTestConfig.builder();
            //builder.setSensorsAnalytics(sa);
            builder.SetApiUrl(apiUrl);
            Assert.Throws<ArgumentNullException>(() => builder.Build());
        }


        //AsyncFetchABTest: 以下试验对象均存在
        [Fact]
        public void Test10()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<string> experiement = sensorsABTest.AsyncFetchABTest<string>("AB123456", false, "param_cat", "hellokitty");
            Assert.NotNull(experiement);
            Assert.NotNull(experiement.abTestExperimentId);
            Assert.Equal("shiyan2", experiement.result);
            sa.Shutdown();
        }


        [Fact]
        public void Test11()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<bool> experiement = sensorsABTest.AsyncFetchABTest<bool>("AB123456", false, "param_dog", false);
            Assert.NotNull(experiement);
            Assert.NotNull(experiement.abTestExperimentId);
            Assert.True(experiement.result);
            sa.Shutdown();
        }

        [Fact]
        public void Test12()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<int> experiement = sensorsABTest.AsyncFetchABTest<int>("AB123456", false, "qqqq", 22);
            Assert.NotNull(experiement);
            Assert.NotNull(experiement.abTestExperimentId);
            Assert.Equal(111, experiement.result);
            sa.Shutdown();
        }

        //以下试验对象均不存在
        [Fact]
        public void Test13()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<string> experiement = sensorsABTest.AsyncFetchABTest<string>("AB123456", false, "not_found", "not_found_default");
            Assert.NotNull(experiement);
            Assert.Null(experiement.abTestExperimentId);
            Assert.Equal("not_found_default", experiement.result);
            sa.Shutdown();
        }

        [Fact]
        public void Test14()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<bool> experiement = sensorsABTest.AsyncFetchABTest<bool>("AB123456", false, "param_dog2", false);
            Assert.NotNull(experiement);
            Assert.Null(experiement.abTestExperimentId);
            Assert.False(experiement.result);
            sa.Shutdown();
        }


        [Fact]
        public void Test14_1()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<bool> experiement = sensorsABTest.AsyncFetchABTest<bool>("AB123456", false, "param_dog2", true);//默认 true
            Assert.NotNull(experiement);
            Assert.Null(experiement.abTestExperimentId);
            Assert.True(experiement.result);
            sa.Shutdown();
        }

        [Fact]
        public void Test15()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<int> experiement = sensorsABTest.AsyncFetchABTest<int>("AB123456", false, "qqqq2", 22);
            Assert.NotNull(experiement);
            Assert.Null(experiement.abTestExperimentId);
            Assert.Equal(22, experiement.result);
            sa.Shutdown();
        }

        //测试从缓存中获取结果
        [Fact]
        public void Test16()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<string> experiement = sensorsABTest.FastFetchABTest<string>("AB123456", false, "param_cat", "hellokitty");
            Assert.NotNull(experiement);
            Assert.NotNull(experiement.abTestExperimentId);
            Assert.Equal("shiyan2", experiement.result);

            //从缓存中获取结果
            long timeStamp = SensorsABTestUtils.GetTimeStamp();
            experiement = sensorsABTest.FastFetchABTest<string>("AB123456", false, "param_cat", "hellokitty");
            timeStamp = SensorsABTestUtils.GetTimeStamp() - timeStamp;
            Assert.InRange(timeStamp, 0, 50);
            Assert.NotNull(experiement);
            Assert.NotNull(experiement.abTestExperimentId);
            Assert.Equal("shiyan2", experiement.result);

            sa.Shutdown();
        }

        //测试自动上报事件
        [Fact]
        public void Test17()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<string> experiement = sensorsABTest.FastFetchABTest<string>("AB123456", false, "param_cat", "hellokitty", enableAutoTrackEvent: false, timeoutMilliseconds:3000);
            Assert.NotNull(experiement);
            Assert.NotNull(experiement.abTestExperimentId);
            Assert.Equal("shiyan2", experiement.result);

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("h11111","s2222222");
            sensorsABTest.TrackABTestTriggerEvent<string>(experiement, properties:properties);

            sa.Shutdown();
        }



        //测试 timeout
        [Fact]
        public void Test18()
        {
            string apiUrl = "http://10.130.6.5:8202/api/v2/abtest/online/results?project-key=438B9364C98D54371751BA82F6484A1A03A5155E";
            IConsumer consumer = new NewClientConsumer("http://newsdktest.datasink.sensorsdata.cn/sa?project=zhangwei&token=5a394d2405c147ca",
                "/Users/zhangwei/consumer/sss.txt", 10, 10 * 1000);
            SensorsAnalytics sa = new SensorsAnalytics(consumer, true);
            ABTestConfig config = ABTestConfig.builder()
                .SetSensorsAnalytics(sa)
                .SetApiUrl(apiUrl)
                .Build();
            SensorsABTest sensorsABTest = new SensorsABTest(config);
            Experiement<string> experiement = sensorsABTest.FastFetchABTest<string>("AB123456", false, "param_cat", "hellokitty", timeoutMilliseconds: 30);
            Assert.NotNull(experiement);
            Assert.Null(experiement.abTestExperimentId);
            Assert.Equal("hellokitty", experiement.result);


            sa.Shutdown();
        }





    }
}
