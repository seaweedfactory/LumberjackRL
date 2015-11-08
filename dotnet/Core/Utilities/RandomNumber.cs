using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumberjackRL.Core.Utilities
{
    public class RandomNumber
    {
        public static int RandomInteger(int maxValue)
        {
            Random tmpRandom = new Random();
            return (maxValue > 0) ? tmpRandom.Next(maxValue) : 0;
        }

        public static double RandomDouble()
        {
            Random tmpRandom = new Random();
            return tmpRandom.NextDouble();
        }

        public static Guid RandomUUID()
        {
            return Guid.NewGuid();
        }
    }
}
