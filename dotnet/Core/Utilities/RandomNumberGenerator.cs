using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumberjackRL.Core.Utilities
{
    public class RandomNumberGenerator
    {
        private Random tmpRandom = new Random();

        public RandomNumberGenerator()
        {

        }

        public int RandomInteger(int maxValue)
        {
            return (maxValue > 0) ? tmpRandom.Next(maxValue) : 0;
        }

        public double RandomDouble()
        {
            return tmpRandom.NextDouble();
        }

        public Guid RandomUUID()
        {
            return Guid.NewGuid();
        }
    }
}
