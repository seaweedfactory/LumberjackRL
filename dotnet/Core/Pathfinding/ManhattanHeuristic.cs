using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;

namespace LumberjackRL.Core.Pathfinding
{
    /// <summary>
    /// A heuristic that drives the search based on the Manhattan distance between the current 
    /// location and the target.
    /// </summary>
    public class ManhattanHeuristic : IHeuristic
    {
        private int minimumCost; //The minimum movement cost from any one square to the next
        
        /// <summary>
        /// Create new heuristic.
        /// </summary>
        /// <param name="minimumCost">The minimum movement cost from any one square to the next</param>
        public ManhattanHeuristic(int minimumCost)
        {
            this.minimumCost = minimumCost;
        }

        public float getCost(Monster mover, int x, int y, int tx, int ty)
        {
            return minimumCost * (Math.Abs(x - tx) + Math.Abs(y - ty));
        }
    }
}
