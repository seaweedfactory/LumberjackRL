using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;
using LumberjackRL.Core.Map;

namespace LumberjackRL.Core.Pathfinding
{
    /// <summary>
    /// A description of an implementation that can find a path from one location on a tile map to 
    /// another based on information provided by that tile map.
    /// </summary>
    public interface IPathfinder
    {
        /// <summary>
        /// Find a path from the starting location provided (sx,sy) to the target location (tx,ty) 
        /// avoiding blockages and attempting to honour costs provided by the tile map.
        /// </summary>
        /// <param name="region"></param>
        /// <param name="maxSearchDistance"></param>
        /// <param name="mover">
        /// The entity that will be moving along the path. This provides a place to pass context 
        /// information about the game entity doing the moving, e.g. can it fly? can it swim etc
        ///</param>
        /// <param name="sx">The x coordinate of the start location</param>
        /// <param name="sy">The y coordinate of the start location</param>
        /// <param name="tx">The x coordinate of the target location</param>
        /// <param name="ty">The y coordinate of the target location</param>
        /// <returns>The path found from start to end, or null if no path can be found.</returns>
        List<Position> findPath(Region region, int maxSearchDistance, Monster mover, int sx, int sy, int tx, int ty);
    }
}
