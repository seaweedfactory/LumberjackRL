using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;

namespace LumberjackRL.Core.Pathfinding
{
    /// <summary>
    /// The description of a class providing a cost for a given tile based on a target location 
    /// and entity being moved. This heuristic controls what priority is placed on different 
    /// tiles during the search for a path.
    /// </summary>
    public interface IHeuristic
    {
        /// <summary>
        /// Get the additional heuristic cost of the given tile. 
        /// This controls the order in which tiles are searched while attempting to find a 
        /// path to the target location. The lower the cost the more likely the tile will be searched.
        /// </summary>
        /// <param name="mover">The entity that is moving along the path</param>
        /// <param name="x">The x coordinate of the tile being evaluated</param>
        /// <param name="y">The y coordinate of the tile being evaluated</param>
        /// <param name="tx">The x coordinate of the target location</param>
        /// <param name="ty">The y coordinate of the target location</param>
        /// <returns>The cost associated with the given tile</returns>
        float getCost(Monster mover, int x, int y, int tx, int ty);
    }
}
