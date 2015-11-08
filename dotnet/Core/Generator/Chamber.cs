using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumberjackRL.Core.Generator
{
    public enum ChamberType 
    { 
        NULL,
        OPEN, 
        FLOODED, 
        MUSHROOM 
    }

    public class Chamber
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public ChamberType type;
        

        public Chamber(int x, int y, int width, int height, ChamberType type)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.type = type;
        }
    }
}
