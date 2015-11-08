using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;

namespace LumberjackRL.Core.Pathfinding
{
    /// <summary>
    /// A heuristic that uses the tile that is closest to the target as the next best tile.
    /// </summary>
    public class ClosestHeuristic: IHeuristic
    {
        public float getCost(Monster mover, int x, int y, int tx, int ty)
        {
            float dx = tx - x;
            float dy = ty - y;

            float result = (float)(Math.Sqrt((dx * dx) + (dy * dy)));

            return result;
        }
    }
}
