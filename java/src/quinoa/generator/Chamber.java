/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */

package quinoa.generator;

public class Chamber
{
    public int x;
    public int y;
    public int width;
    public int height;
    public ChamberType type;
    public static enum ChamberType{OPEN, FLOODED, MUSHROOM};

    public Chamber(int x, int y, int width, int height, ChamberType type)
    {
        this.x = x;
        this.y = y;
        this.width = width;
        this.height = height;
        this.type = type;
    }
}
