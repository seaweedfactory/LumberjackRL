package quinoa.region;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import quinoa.Copyable;
import quinoa.Storable;
import quinoa.region.BuildingManager.BuildingType;

public class Building implements Storable, Copyable
{
    public static final int WOOD_ROOF=0;
    public static final int BEAM_ROOF=1;

    private String name;                //What is the building called?
    private int x, y;                   //position
    private int width, height;          //dimensions
    private BuildingType buildingType;  //type of building
    private Position door;              //where is front door?
    private boolean lit;                //is the building self-lighted?
    private int roofType;               //what does the roof look like?


    public Building()
    {
        name = "";
        x = 0;
        y = 0;
        width = 0;
        height = 0;
        buildingType = null;
        door = new Position();
        lit = true;
        roofType = WOOD_ROOF;
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getName()); out.newLine();
        out.write(x + ""); out.newLine();
        out.write(y + ""); out.newLine();
        out.write(width + ""); out.newLine();
        out.write(height + ""); out.newLine();
        out.write(door.x + ""); out.newLine();
        out.write(door.y + ""); out.newLine();
        out.write(isLit() + ""); out.newLine();
        out.write(getRoofType()+""); out.newLine();
        out.write(buildingType.name()); out.newLine();
    }

    public void load(BufferedReader in) throws Exception
    {
        setName(in.readLine());
        x = Integer.parseInt(in.readLine());
        y = Integer.parseInt(in.readLine());
        width = Integer.parseInt(in.readLine());
        height = Integer.parseInt(in.readLine());
        door.x = Integer.parseInt(in.readLine());
        door.y = Integer.parseInt(in.readLine());
        setLit(Boolean.parseBoolean(in.readLine()));
        setRoofType(Integer.parseInt(in.readLine()));
        buildingType = buildingType.valueOf(in.readLine());
    }

    public Object copy()
    {
        Building newBuild = new Building();
        newBuild.setName(this.getName());
        newBuild.setX(this.x);
        newBuild.setY(this.y);
        newBuild.setWidth(this.width);
        newBuild.setHeight(this.height);
        newBuild.setBuildingType(this.buildingType);
        newBuild.setDoor(this.door.x, this.door.y);
        newBuild.setLit(this.isLit());
        newBuild.setRoofType(this.roofType);

        return newBuild;
    }

    public void setPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    /**
     * @return the x
     */
    public int getX() {
        return x;
    }

    /**
     * @param x the x to set
     */
    public void setX(int x) {
        this.x = x;
    }

    /**
     * @return the y
     */
    public int getY() {
        return y;
    }

    /**
     * @param y the y to set
     */
    public void setY(int y) {
        this.y = y;
    }

    /**
     * @return the width
     */
    public int getWidth() {
        return width;
    }

    /**
     * @param width the width to set
     */
    public void setWidth(int width) {
        this.width = width;
    }

    /**
     * @return the height
     */
    public int getHeight() {
        return height;
    }

    /**
     * @param height the height to set
     */
    public void setHeight(int height) {
        this.height = height;
    }

    /**
     * @return the buildingType
     */
    public BuildingType getBuildingType() {
        return buildingType;
    }

    /**
     * @param buildingType the buildingType to set
     */
    public void setBuildingType(BuildingType buildingType) {
        this.buildingType = buildingType;
    }

    /**
     * @return the door
     */
    public Position getDoor() {
        return door;
    }

    public void setDoor(int x, int y)
    {
        door.x = x;
        door.y = y;
    }

    /**
     * @return the lit
     */
    public boolean isLit() {
        return lit;
    }

    /**
     * @param lit the lit to set
     */
    public void setLit(boolean lit) {
        this.lit = lit;
    }

    /**
     * @return the roofType
     */
    public int getRoofType() {
        return roofType;
    }

    /**
     * @param roofType the roofType to set
     */
    public void setRoofType(int roofType) {
        this.roofType = roofType;
    }

    /**
     * @return the name
     */
    public String getName() {
        return name;
    }

    /**
     * @param name the name to set
     */
    public void setName(String name) {
        this.name = name;
    }


}
