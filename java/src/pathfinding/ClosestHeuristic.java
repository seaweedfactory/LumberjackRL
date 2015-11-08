package pathfinding;

import quinoa.monsters.Monster;

/**
 * A heuristic that uses the tile that is closest to the target
 * as the next best tile.
 * 
 * @author Kevin Glass
 */
public class ClosestHeuristic implements HeuristicInterface {
	/**
	 * @see HeuristicInterface#getCost(TileBasedMap, Mover, int, int, int, int)
	 */
	public float getCost(Monster mover, int x, int y, int tx, int ty) {
		float dx = tx - x;
		float dy = ty - y;
		
		float result = (float) (Math.sqrt((dx*dx)+(dy*dy)));
		
		return result;
	}

}
