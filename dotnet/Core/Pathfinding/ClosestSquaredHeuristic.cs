using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;

namespace LumberjackRL.Core.Pathfinding
{
    /// <summary>
    /// A heuristic that uses the tile that is closest to the target as the next best tile. 
    /// In this case the sqrt is removed and the distance squared is used instead.
    /// </summary>
    public class ClosestSquaredHeuristic : IHeuristic
    {
        public float getCost(Monster mover, int x, int y, int tx, int ty)
        {
            float dx = tx - x;
            float dy = ty - y;

            return ((dx * dx) + (dy * dy));
        }
    }
}
