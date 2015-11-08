using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumberjackRL.Core.Pathfinding
{
    public class PathNode : IComparable
    {
		    /** The x coordinate of the node */
		    public int x;
		    /** The y coordinate of the node */
		    public int y;
		    /** The path cost for this node */
            public float cost;
		    /** The parent of this node, how we reached it in the search */
            public PathNode parent;
		    /** The heuristic cost of this node */
            public float heuristic;
		    /** The search depth of this node */
            public int depth;
		
		    /**
		     * Create a new node
		     * 
		     * @param x The x coordinate of the node
		     * @param y The y coordinate of the node
		     */
		    public PathNode(int x, int y) 
            {
			    this.x = x;
			    this.y = y;
		    }
		
		    /**
		     * Set the parent of this node
		     * 
		     * @param parent The parent node which lead us to this node
		     * @return The depth we have no reached in searching
		     */
		    public int setParent(PathNode parent) 
            {
			    depth = parent.depth + 1;
			    this.parent = parent;
			
			    return depth;
		    }
		
		    /**
		     * @see Comparable#compareTo(Object)
		     */
            public int CompareTo(object obj)
            {
                PathNode o = (PathNode)obj;

                float f = heuristic + cost;
                float of = o.heuristic + o.cost;

                if (f < of)
                {
                    return -1;
                }
                else if (f > of)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
    }
}
