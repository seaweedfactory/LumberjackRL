using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumberjackRL.Core.Map
{
    public class Light
    {
        public int x;
        public int y;
        public double intensity;

        public Light(int x, int y, double intensity)
        {
            this.x = x;
            this.y = y;
            this.intensity = intensity;
        }
    }
}
