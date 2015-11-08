using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;
using LumberjackRL.Core.Map;

namespace LumberjackRL.Core.Pathfinding
{
    /// <summary>
    /// A path finder implementation that uses the AStar heuristic based algorithm to determine a path.
    /// </summary>
    public class PathFinder : IPathfinder
    {
        /** The set of nodes that have been searched through */
	    private List<PathNode> closed = new List<PathNode>();
	    /** The set of nodes that we do not yet consider fully searched */
        private SortedNodeList open = new SortedNodeList();
	
	    /** The map being searched */
	    private Quinoa quinoa;
        private Region region;
	        	
	    /** The complete set of nodes across the map */
	    private PathNode[,] nodes;
	
	    /** The heuristic we're applying to determine which nodes to search first */
	    private IHeuristic heuristic;
	
	    /**
	     * Create a path finder 
	     * 
	     * @param heuristic The heuristic used to determine the search order of the map
	     * @param map The map to be searched
	     * @param maxSearchDistance The maximum depth we'll search before giving up
	     * @param allowDiagMovement True if the search should try diagonal movement
	     */
	    public PathFinder(Quinoa quinoa, IHeuristic heuristic)
            {
		    this.heuristic = heuristic;
		    this.quinoa = quinoa;				
	    }
	
	
	    public List<Position> findPath(Region region, int maxSearchDistance, Monster mover, int sx, int sy, int tx, int ty)
            {
                //Initialize
                this.region = region;
                nodes = new PathNode[region.getWidth(),region.getHeight()];
                for (int x=0;x<region.getWidth();x++) {
                        for (int y=0;y<region.getHeight();y++) {
                                nodes[x,y] = new PathNode(x,y);
                        }
                }

                // easy first check, if the destination is blocked, we can't get there
                if(!TerrainManager.allowsMonsterToPass(region.getTerrain(tx, ty), mover))
                {
                    return null;
                }
		
		
                // initial state for A*. The closed group is empty. Only the starting
                // tile is in the open list and it's cost is zero, i.e. we're already there
                nodes[sx,sy].cost = 0;
                nodes[sx,sy].depth = 0;
                closed.Clear();
                open.clear();
                open.add(nodes[sx,sy]);

                nodes[tx,ty].parent = null;
                		
                // while we haven't found the goal and haven't exceeded our max search depth
                int maxDepth = 0;
                while ((maxDepth < maxSearchDistance) && (open.size() != 0))
                {
                    // pull out the first node in our open list, this is determined to
                    // be the most likely to be the next step based on our heuristic
                    PathNode current = getFirstInOpen();
                    if (current == nodes[tx,ty]) 
                    {
                            break;
                    }

                    removeFromOpen(current);
                    addToClosed(current);
			
                    // search through all the neighbours of the current node evaluating
                    // them as next steps
                    for (int x=-1;x<2;x++)
                    {

                        for (int y=-1;y<2;y++)
                        {
                            // not a neighbour, its the current tile
                            if ((x == 0) && (y == 0))
                            {
                                    continue;
                            }
					
                            // if we're not allowing diaganol movement then only
                            // one of x or y can be set
                            if ((x != 0) && (y != 0))
                            {
                                    continue;
                            }
					
					
                            // determine the location of the neighbour and evaluate it
                            int xp = x + current.x;
                            int yp = y + current.y;

                            if (isValidLocation(mover,sx,sy,xp,yp))
                            {
                                    // the cost to get to this node is cost the current plus the movement
                                    // cost to reach this node. Note that the heursitic value is only used
                                    // in the sorted open list
                                    float nextStepCost = current.cost + getMovementCost(mover, current.x, current.y, xp, yp);
                                    PathNode neighbour = nodes[xp,yp];

                                    // if the new cost we've determined for this node is lower than
                                    // it has been previously makes sure the node hasn't been discarded. We've
                                    // determined that there might have been a better path to get to
                                    // this node so it needs to be re-evaluated
                                    if (nextStepCost < neighbour.cost)
                                    {
                                            if (inOpenList(neighbour))
                                            {
                                                    removeFromOpen(neighbour);
                                            }
                                            if (inClosedList(neighbour))
                                            {
                                                    removeFromClosed(neighbour);
                                            }
                                    }

                                    // if the node hasn't already been processed and discarded then
                                    // reset it's cost to our current cost and add it as a next possible
                                    // step (i.e. to the open list)
                                    if (!inOpenList(neighbour) && !(inClosedList(neighbour)))
                                    {
                                            neighbour.cost = nextStepCost;
                                            neighbour.heuristic = getHeuristicCost(mover, xp, yp, tx, ty);
                                            maxDepth = Math.Max(maxDepth, neighbour.setParent(current));
                                            addToOpen(neighbour);
                                    }
                                }
                            }
                        }
		    }

		    // since we've got an empty open list or we've run out of search 
		    // there was no path. Just return null
		    if (nodes[tx,ty].parent == null) 
            {
                return null;
		    }
		
		    // At this point we've definitely found a path so we can uses the parent
		    // references of the nodes to find out way from the target location back
		    // to the start recording the nodes on the way.
		    List<Position> path = new List<Position>();
		    PathNode target = nodes[tx,ty];
		    while (target != nodes[sx,sy]) 
            {
                Position pos = new Position(target.x, target.y);
                path.Insert(0, pos);
                target = target.parent;
		    }
            Position pos2 = new Position(sx, sy);
		    path.Insert(0, pos2);
		
		    // thats it, we have our path
            return path;
	    }

	    /**
	     * Get the first element from the open list. This is the next
	     * one to be searched.
	     * 
	     * @return The first element in the open list
	     */
	    protected PathNode getFirstInOpen() 
        {
		    return (PathNode) open.first();
	    }
	
	    /**
	     * Add a node to the open list
	     * 
	     * @param node The node to be added to the open list
	     */
	    protected void addToOpen(PathNode node) 
        {
		    open.add(node);
	    }
	
	    /**
	     * Check if a node is in the open list
	     * 
	     * @param node The node to check for
	     * @return True if the node given is in the open list
	     */
	    protected bool inOpenList(PathNode node) 
        {
		    return open.contains(node);
	    }
	
	    /**
	     * Remove a node from the open list
	     * 
	     * @param node The node to remove from the open list
	     */
	    protected void removeFromOpen(PathNode node) 
        {
		    open.remove(node);
	    }
	
	    /**
	     * Add a node to the closed list
	     * 
	     * @param node The node to add to the closed list
	     */
	    protected void addToClosed(PathNode node) 
        {
		    closed.Add(node);
	    }
	
	    /**
	     * Check if the node supplied is in the closed list
	     * 
	     * @param node The node to search for
	     * @return True if the node specified is in the closed list
	     */
	    protected bool inClosedList(PathNode node) 
        {
		    return closed.Contains(node);
	    }
	
	    /**
	     * Remove a node from the closed list
	     * 
	     * @param node The node to remove from the closed list
	     */
	    protected void removeFromClosed(PathNode node) 
        {
		    closed.Remove(node);
	    }
	
	    /**
	     * Check if a given location is valid for the supplied mover
	     * 
	     * @param mover The mover that would hold a given location
	     * @param sx The starting x coordinate
	     * @param sy The starting y coordinate
	     * @param x The x coordinate of the location to check
	     * @param y The y coordinate of the location to check
	     * @return True if the location is valid for the given mover
	     */
	    protected bool isValidLocation(Monster mover, int sx, int sy, int x, int y)
            {
                bool invalid = (x < 0) || (y < 0) || (x >= region.getWidth()) || (y >= region.getHeight());

                if ((!invalid) && ((sx != x) || (sy != y)))
                {
                    invalid = !TerrainManager.allowsMonsterToPass(region.getTerrain(x,y), mover);
                }

                return !invalid;
	    }
	
	    /**
	     * Get the cost to move through a given location
	     * 
	     * @param mover The entity that is being moved
	     * @param sx The x coordinate of the tile whose cost is being determined
	     * @param sy The y coordinate of the tile whose cost is being determined
	     * @param tx The x coordinate of the target location
	     * @param ty The y coordinate of the target location
	     * @return The cost of movement through the given tile
	     */
	    public float getMovementCost(Monster mover, int sx, int sy, int tx, int ty)
            {
		    //return map.getCost(mover, sx, sy, tx, ty);
                    return 1;
	    }

	    /**
	     * Get the heuristic cost for the given location. This determines in which 
	     * order the locations are processed.
	     * 
	     * @param mover The entity that is being moved
	     * @param x The x coordinate of the tile whose cost is being determined
	     * @param y The y coordinate of the tile whose cost is being determined
	     * @param tx The x coordinate of the target location
	     * @param ty The y coordinate of the target location
	     * @return The heuristic cost assigned to the tile
	     */
	    public float getHeuristicCost(Monster mover, int x, int y, int tx, int ty) {
		    return heuristic.getCost(mover, x, y, tx, ty);
	    }
    }
}
