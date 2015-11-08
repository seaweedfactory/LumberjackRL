using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumberjackRL.Core.Pathfinding
{
    /// <summary>
    /// Simple sorted list.
    /// </summary>
    public class SortedNodeList
    {
        /** The list of elements */
        private List<PathNode> list = new List<PathNode>();

        /**
            * Retrieve the first element from the list
            *  
            * @return The first element from the list
            */
        public Object first()
        {
            return list[0];
        }

        /**
            * Empty the list
            */
        public void clear()
        {
            list.Clear();
        }

        /**
            * Add an element to the list - causes sorting
            * 
            * @param o The element to add
            */
        public void add(Object o)
        {
            list.Add((PathNode)o);
            list.Sort();
        }

        /**
            * Remove an element from the list
            * 
            * @param o The element to remove
            */
        public void remove(Object o)
        {
            list.Remove((PathNode)o);
        }

        /**
            * Get the number of elements in the list
            * 
            * @return The number of element in the list
            */
        public int size()
        {
            return list.Count();
        }

        /**
            * Check if an element is in the list
            * 
            * @param o The element to search for
            * @return True if the element is in the list
            */
        public bool contains(Object o)
        {
            return list.Contains((PathNode)o);
        }
    }
}
