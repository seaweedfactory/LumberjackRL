package pathfinding;

import quinoa.monsters.Monster;


/**
 * A heuristic that drives the search based on the Manhattan distance
 * between the current location and the target
 * 
 * @author Kevin Glass
 */
public class ManhattanHeuristic implements HeuristicInterface {
	/** The minimum movement cost from any one square to the next */
	private int minimumCost;
	
	/**
	 * Create a new heuristic 
	 * 
	 * @param minimumCost The minimum movement cost from any one square to the next
	 */
	public ManhattanHeuristic(int minimumCost) {
		this.minimumCost = minimumCost;
	}
	
	/**
	 * @see HeuristicInterface#getCost(TileBasedMap, Mover, int, int, int, int)
	 */
	public float getCost(Monster mover, int x, int y, int tx,
			int ty) {
		return minimumCost * (Math.abs(x-tx) + Math.abs(y-ty));
	}

}
