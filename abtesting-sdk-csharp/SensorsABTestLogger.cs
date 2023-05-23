using System;
namespace SensorsData.ABTest
{
    public class SensorsABTestLogger
    {
        public static bool DEBUG = true;

        public static void info(String message)
        {
            if (DEBUG)
            {
                Console.WriteLine($"[SensorsABTest  INFO]: {message}");
            }
        }

        public static void error(String message)
        {
            if (DEBUG)
            {
                Console.WriteLine($"[SensorsABTest ERROR]: {message}");
            }
        }
    }
}
