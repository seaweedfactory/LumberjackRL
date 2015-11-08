package quinoa.region;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import quinoa.Storable;

public class RegionExit implements Storable
{

    /**
     * @return the exitDecorator
     */
    public ExitDecorator getExitDecorator() {
        return exitDecorator;
    }

    /**
     * @param exitDecorator the exitDecorator to set
     */
    public void setExitDecorator(ExitDecorator exitDecorator) {
        this.exitDecorator = exitDecorator;
    }
    public static enum ExitDecorator {NONE, DOWN_STAIR, UP_STAIR, CAVE};
    private int x, y;                   //origin of the exit
    private int dx, dy;                 //destination of the exit
    private String destinationRegionID; //what region does the exit lead to?
    private ExitDecorator exitDecorator; //what does the exit look like?

    public RegionExit()
    {
        x = 0;
        y = 0;
        dx = 0;
        dy = 0;
        destinationRegionID = "";
        exitDecorator = ExitDecorator.NONE;
    }

    public RegionExit(int x, int y, int dx, int dy, String destinationRegionID, ExitDecorator exitDecorator)
    {
        this.x = x;
        this.y = y;
        this.dx = dx;
        this.dy = dy;
        this.destinationRegionID = destinationRegionID;
        this.exitDecorator = exitDecorator;
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(x+""); out.newLine();
        out.write(y+""); out.newLine();
        out.write(getDx()+""); out.newLine();
        out.write(getDy()+""); out.newLine();
        out.write(getDestinationRegionID()); out.newLine();
        out.write(exitDecorator.name()); out.newLine();
    }

    public void load(BufferedReader in) throws Exception
    {
        setX(Integer.parseInt(in.readLine()));
        setY(Integer.parseInt(in.readLine()));
        setDx(Integer.parseInt(in.readLine()));
        setDy(Integer.parseInt(in.readLine()));
        setDestinationRegionID(in.readLine());
        setExitDecorator(ExitDecorator.valueOf(in.readLine()));
    }

    /**
     * @return the x
     */
    public int getX() {
        return x;
    }

    /**
     * @return the y
     */
    public int getY() {
        return y;
    }

    /**
     * @return the destinationRegionID
     */
    public String getDestinationRegionID() {
        return destinationRegionID;
    }

    /**
     * @return the dy
     */
    public int getDy() {
        return dy;
    }

    /**
     * @param dy the dy to set
     */
    public void setDy(int dy) {
        this.dy = dy;
    }

    /**
     * @return the dx
     */
    public int getDx() {
        return dx;
    }

    /**
     * @param dx the dx to set
     */
    public void setDx(int dx) {
        this.dx = dx;
    }

    /**
     * @param destinationRegionID the destinationRegionID to set
     */
    public void setDestinationRegionID(String destinationRegionID) {
        this.destinationRegionID = destinationRegionID;
    }

    /**
     * @param x the x to set
     */
    public void setX(int x) {
        this.x = x;
    }

    /**
     * @param y the y to set
     */
    public void setY(int y) {
        this.y = y;
    }
}
