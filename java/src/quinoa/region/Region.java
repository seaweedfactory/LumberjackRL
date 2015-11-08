package quinoa.region;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.util.ArrayList;
import quinoa.Storable;
import quinoa.items.Item;
import quinoa.monsters.Monster;
import quinoa.monsters.MonsterActionManager;
import quinoa.region.LightMap.LightingModel;

public class Region implements Storable
{
    private int width;                  //Width of region in tiles
    private int height;                 //Height of region in tiles
    private Terrain[][] terrainField;   //Holds persistant terrain
    private ArrayList<Monster> monsters;//Holds monster data
    private ArrayList<Item> items;      //Holds item data
    private ArrayList<Building> buildings; //Holds building data
    private LightingModel lightingModel;    //What kind of lighting to use

    public Region(int width , int height)
    {
        this.width = width;
        this.height = height;
        terrainField = new Terrain[width][height];
        monsters = new ArrayList<Monster>();
        items = new ArrayList<Item>();
        buildings = new ArrayList<Building>();
        lightingModel = LightingModel.ABOVE_GROUND;

        for(int x=0; x < width; x++)
        {
            for(int y=0; y < height; y++)
            {
                terrainField[x][y] = new Terrain();
            }
        }
    }

    public void buildBuildings()
    {
        for(Building tempBuild : buildings)
        {
            BuildingManager.build(this, tempBuild.getX(), tempBuild.getY(), tempBuild);
        }
    }
      
    public void setTerrain(int x, int y, Terrain newTerrain)
    {
        terrainField[x][y] = newTerrain;
    }

    public Terrain getTerrain(int x, int y)
    {
        if(x >= 0 && x < width && y >=0 && y < height)
        {
            return terrainField[x][y];
        }
        else
        {
            return null;
        }
    }

    public Item getItem(int x, int y)
    {
        for(Item tempItem : items)
        {
            if(tempItem.getX() == x && tempItem.getY() == y)
            {
                return tempItem;
            }
        }
        return null;
    }

    public void removeExpiredObjects()
    {
        //remove expired items on monsters
        for(Monster tempMonster : getMonsters())
        {
            tempMonster.getInventory().removedExpiredItems();
        }
        
        //remove items on the ground
        ArrayList<Item> removeItemList = new ArrayList<Item>();
        for(Item tempItem : getItems())
        {
            if(tempItem.shouldBeRemoved())
            {
                tempItem.remove();
                removeItemList.add(tempItem);
            }
        }
        for(Item tempItem : removeItemList)
        {
            getItems().remove(tempItem);
        }

        //remove monsters
        ArrayList<Monster> removeMonsterList = new ArrayList<Monster>();
        for(Monster tempMonster : getMonsters())
        {
            if(tempMonster.shouldBeRemoved() && !tempMonster.getID().equals(MonsterActionManager.PLAYER_ID))
            {
                tempMonster.remove();
                removeMonsterList.add(tempMonster);
            }
        }
        for(Monster tempMonster : removeMonsterList)
        {
            getMonsters().remove(tempMonster);
        }
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getWidth()+""); out.newLine();
        out.write(getHeight()+""); out.newLine();
        for(int x=0; x < getWidth(); x++)
        {
            for(int y=0; y < getHeight(); y++)
            {
                terrainField[x][y].save(out);
            }
        }
        this.removeExpiredObjects();    //Remove any objects which should not be saved or have been copied
        out.write(monsters.size()+""); out.newLine();
        for(Monster tempMonster : getMonsters())
        {
            tempMonster.save(out);
        }
        out.write(getItems().size()+""); out.newLine();
        for(Item tempItem : getItems())
        {
            tempItem.save(out);
        }
        out.write(getBuildings().size()+""); out.newLine();
        for(Building tempBuild : getBuildings())
        {
            tempBuild.save(out);
        }
        out.write(this.getLightingModel().name()); out.newLine();
    }

    public void load(BufferedReader in) throws Exception
    {
        this.width = Integer.parseInt(in.readLine());
        this.height = Integer.parseInt(in.readLine());
        terrainField = new Terrain[getWidth()][getHeight()];
        for(int x=0; x < getWidth(); x++)
        {
            for(int y=0; y < getHeight(); y++)
            {
                Terrain newTerrain = new Terrain();
                newTerrain.load(in);
                terrainField[x][y] = newTerrain;
            }
        }
        getMonsters().clear();
        int monstersSize = Integer.parseInt(in.readLine());
        for(int i=0; i < monstersSize; i++)
        {
            Monster newMonster = new Monster();
            newMonster.load(in);

            //do not load any monste with the player ID
            if(!newMonster.getID().equals(MonsterActionManager.PLAYER_ID))
            {
                getMonsters().add(newMonster);
            }
        }
        getItems().clear();
        int itemsSize = Integer.parseInt(in.readLine());
        for(int i=0; i < itemsSize; i++)
        {
            Item newItem = new Item();
            newItem.load(in);
            getItems().add(newItem);
        }
        getBuildings().clear();
        int buildSize = Integer.parseInt(in.readLine());
        for(int i=0; i < buildSize; i++)
        {
            Building newBuild = new Building();
            newBuild.load(in);
            getBuildings().add(newBuild);
        }
        this.lightingModel = LightingModel.valueOf(in.readLine());
    }

    /**
     * @return the monsters
     */
    public ArrayList<Monster> getMonsters()
    {
        return monsters;
    }

    /**
     * @return the width
     */
    public int getWidth() {
        return width;
    }

    /**
     * @return the height
     */
    public int getHeight() {
        return height;
    }

    /**
     * @return the items
     */
    public ArrayList<Item> getItems() {
        return items;
    }

    /**
     * @return the buildings
     */
    public ArrayList<Building> getBuildings() {
        return buildings;
    }

    /**
     * @return the lightingModel
     */
    public LightingModel getLightingModel() {
        return lightingModel;
    }

    /**
     * @param lightingModel the lightingModel to set
     */
    public void setLightingModel(LightingModel lightingModel) {
        this.lightingModel = lightingModel;
    }

}
