package quinoa;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.util.ArrayList;
import java.util.HashMap;
import quinoa.generator.MapGenerator;
import quinoa.generator.MapGeneratorInterface;
import quinoa.items.Item;
import quinoa.items.ItemManager;
import quinoa.items.ItemManager.*;
import quinoa.monsters.Monster;
import quinoa.monsters.MonsterActionManager;
import quinoa.monsters.MonsterActionManager.MonsterCode;
import quinoa.monsters.MonsterActionManager.MonsterRole;
import quinoa.region.LightMap;
import quinoa.region.LightMap.LightingModel;
import quinoa.region.Region;
import quinoa.region.RegionExit;
import quinoa.region.RegionMap;
import quinoa.region.Terrain;
import quinoa.region.TerrainManager;
import quinoa.region.TerrainManager.DoorCode;
import quinoa.region.TerrainManager.GraveCode;
import quinoa.region.TerrainManager.MushroomSporeCode;
import quinoa.region.TerrainManager.SeedType;
import quinoa.region.TerrainManager.TerrainCode;
import quinoa.region.TerrainManager.TerrainParameter;
import quinoa.region.TerrainManager.TreeCode;

public class QuinoaActions
{
    private Quinoa quinoa;  //reference to main class
    private ArrayList<Monster> monsterQueue; //used to add monsters during the game to avoid concurrent mod
    public static final String SAVED_GAME_FILENAME="save_game";

    public QuinoaActions(Quinoa quinoa)
    {
        this.quinoa = quinoa;
        this.monsterQueue = new ArrayList<Monster>();
    }

    public void addMonster(Monster monster)
    {
        monsterQueue.add(monster);
    }

    public void saveGame()
    {
        try
        {
            BufferedWriter out = new BufferedWriter(new FileWriter(SAVED_GAME_FILENAME));
            quinoa.save(out);
            out.close();
            quinoa.getMessageManager().addMessage("Saved game.");
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }

    public void loadGame()
    {
        try
        {
            BufferedReader in = new BufferedReader(new FileReader(SAVED_GAME_FILENAME));
            quinoa.load(in);
            in.close();
            quinoa.getMessageManager().addMessage("Loaded game.");
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }

    public void clearMap()
    {
        quinoa.setMap(new RegionMap());
    }

    public void tickClock()
    {
        quinoa.setTicks(quinoa.getTicks() + 1);
        if(quinoa.getTicks() > Quinoa.TICKS_PER_SECOND * 60 * 60 * 24)
        {
            quinoa.setTicks(0);
        }
    }

    //Does a region wide cycle for events like fire spreading, growing plants, etc
    //Performed when loading a map as well
    public void regionCycle()
    {
        if(quinoa.getCurrentRegionHeader().regionIsLoaded())
        {
            spreadWater();
            moveItemsInWater();
            spreadFire();
            growPlants();
        }
    }

    //Perform actions that occur over a longer period
    public void longCycle()
    {
        dropChanceItems();
    }


    //drop items that occur naturally
    public void dropChanceItems()
    {
        Region region = quinoa.getCurrentRegionHeader().getRegion();

        for(int x=0; x < region.getWidth(); x++)
        {
            for(int y=0; y < region.getHeight(); y++)
            {
                Terrain terrain = region.getTerrain(x,y);

                //handle trees dropping fruit
                if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_TREE))
                {
                    TreeCode tc = TreeCode.valueOf(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.HAS_TREE));
                    if(tc == TreeCode.APPLE_TREE)
                    {
                        if(Math.random() < TerrainManager.APPLE_TREE_PRODUCTION_RATE)
                        {
                            int dx = x + (int)(Math.random() * 5) - 2;
                            int dy = y + (int)(Math.random() * 5) - 2;

                            if(dx == x)
                            {
                                dx = x + 1;
                            }

                            if(dy == y)
                            {
                                dy = y + 1;
                            }

                            if(dx >= 0 && dx < region.getWidth()
                            && dy >= 0 && dy < region.getHeight()
                            && (region.getTerrain(dx, dy).getCode() == TerrainCode.GRASS
                            || region.getTerrain(dx, dy).getCode() == TerrainCode.STREAM_BED)
                            && !TerrainManager.hasParameter(region.getTerrain(dx, dy), TerrainParameter.HAS_TREE))
                            {
                                Item apple = new Item();
                                apple.setItemClass(ItemClass.APPLE);
                                apple.setPosition(dx, dy);
                                apple.setItemState(ItemState.GROUND);
                                apple.setStackSize(1 + (int)(Math.random() * apple.getMaxStackSize() - 1) );
                                region.getItems().add(apple);

                            }
                        }
                    }
                }
            }
        }
    }


    public void moveItemsInWater()
    {
        Region region = quinoa.getCurrentRegionHeader().getRegion();
        for(Item tempItem : region.getItems())
        {
            int x = tempItem.getX();
            int y = tempItem.getY();
            Terrain terrain = region.getTerrain(x, y);
            boolean moved=false;

            if(terrain.getWater() > 0)
            {
                int centerWater = terrain.getWater();

                if(centerWater > 1)
                {

                    if(y > 0 && region.getTerrain(x,y-1).getWater() > 0)
                    {
                        if(region.getTerrain(x,y-1).getWater() <= centerWater)
                        {
                            tempItem.setPosition(x, y-1);
                            moved = true;
                        }
                    }

                    if(!moved && y < region.getHeight()-1 && region.getTerrain(x,y+1).getWater() > 0)
                    {
                        if(region.getTerrain(x,y+1).getWater() <= centerWater)
                        {
                            tempItem.setPosition(x, y+1);
                            moved = true;
                        }
                    }

                    if(!moved && x > 0 && region.getTerrain(x-1,y).getWater() > 0)
                    {
                        
                        if(region.getTerrain(x-1,y).getWater() <= centerWater)
                        {
                            tempItem.setPosition(x-1, y);
                            moved = true;
                        }
                    }

                    if(!moved && x < region.getWidth()-1 && region.getTerrain(x+1,y).getWater() > 0)
                    {
                        if(region.getTerrain(x+1,y).getWater() <= centerWater)
                        {
                            tempItem.setPosition(x+1, y);
                            moved = true;
                        }
                    }
                }
            }
        }
    }

    //Spread water around
    public void spreadWater()
    {
        Region region = quinoa.getCurrentRegionHeader().getRegion();

        //Spread water
        int[][] waterMap = new int[region.getWidth()][region.getHeight()];
        for(int x=0; x < region.getWidth(); x++)
        {
            for(int y=0; y < region.getHeight(); y++)
            {
                Terrain terrain = region.getTerrain(x, y);
                if(TerrainManager.wetable(terrain, x, y, quinoa))
                {
                    waterMap[x][y] = terrain.getWater();

                    //do spring
                    if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_SPRING))
                    {
                        int springValue = Integer.parseInt(TerrainManager.getParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_SPRING));
                        waterMap[x][y] = waterMap[x][y] + springValue;

                        if(waterMap[x][y] > TerrainManager.MAX_WATER)
                        {
                            waterMap[x][y] = TerrainManager.MAX_WATER;
                        }
                    }


                }
                else
                {
                    waterMap[x][y] = -1;
                }


            }
        }

        for(int i=0; i < TerrainManager.WATER_CYCLE_COUNT; i++)
        {
            for(int x=0; x < region.getWidth(); x++)
            {
                for(int y=0; y < region.getHeight(); y++)
                {
                    if(waterMap[x][y] > -1)
                    {
                        int waterValue = waterMap[x][y];

                        //try to pull at least one water from each neightbor
                        if(y > 0 && waterMap[x][y-1] - 1 > waterValue
                        && TerrainManager.allowWaterToFlowBetween(region.getTerrain(x, y-1), region.getTerrain(x, y)))
                        {
                            waterValue = waterValue + 1;
                            waterMap[x][y-1] = waterMap[x][y-1] - 1;
                        }
                        if(y < region.getHeight()-1 && waterMap[x][y+1] - 1 > waterValue
                        && TerrainManager.allowWaterToFlowBetween(region.getTerrain(x, y+1), region.getTerrain(x, y)))
                        {
                            waterValue = waterValue + 1;
                            waterMap[x][y+1] = waterMap[x][y+1] - 1;
                        }
                        if(x < region.getWidth() - 1 && waterMap[x+1][y] - 1 > waterValue
                        && TerrainManager.allowWaterToFlowBetween(region.getTerrain(x+1, y), region.getTerrain(x, y)))
                        {
                            waterValue = waterValue + 1;
                            waterMap[x+1][y] = waterMap[x+1][y] - 1;
                        }
                        if(x > 0 && waterMap[x-1][y] - 1 > waterValue
                        && TerrainManager.allowWaterToFlowBetween(region.getTerrain(x-1, y), region.getTerrain(x, y)))
                        {
                            waterValue = waterValue + 1;
                            waterMap[x-1][y] = waterMap[x-1][y] - 1;
                        }

                        //do drain
                        if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_DRAIN))
                        {
                            int drainValue = Integer.parseInt(TerrainManager.getParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_DRAIN));
                            waterValue = waterValue - drainValue;
                            if(waterValue < 0)
                            {
                                waterValue = 0;
                            }
                        }

                        waterMap[x][y] = waterValue;
                    }
                    else
                    {
                        //Can't have water here
                    }
                }
            }
        }

        
        //Refresh cycle
        boolean doEvaporation = false;
        if(Math.random() < TerrainManager.EVAPORATION_RATE)
        {
            doEvaporation = true;
        }
        for(int x=0; x < region.getWidth(); x++)
        {
            for(int y=0; y < region.getHeight(); y++)
            {
                //Do evaporation
                if(doEvaporation)
                {
                    waterMap[x][y] = waterMap[x][y] - 1;
                    if(waterMap[x][y] < 0)
                    {
                        waterMap[x][y] = 0;
                    }
                }

                //do moss generation
                if(waterMap[x][y] <= 0)
                {
                    if(TerrainManager.growable(region.getTerrain(x, y), x, y, quinoa.getLightMap(), TerrainParameter.HAS_MOSS))
                    {
                        boolean canGrow = false;
                        if(y > 0 && region.getTerrain(x,y-1).getWater() > 0)
                        {
                            canGrow = true;
                        }
                        else if(y < region.getHeight()-1 && region.getTerrain(x,y+1).getWater() > 0)
                        {
                            canGrow = true;
                        }
                        else if(x > 0 && region.getTerrain(x-1,y).getWater() > 0)
                        {
                            canGrow = true;
                        }
                        else if(x < region.getWidth()-1 && region.getTerrain(x+1,y).getWater() > 0)
                        {
                            canGrow = true;
                        }

                        if(canGrow && Math.random() < TerrainManager.MOSS_GROW_RATE)
                        {
                            region.getTerrain(x,y).getParameters().put(TerrainParameter.HAS_MOSS,"");
                        }

                    }
                }
                else
                {
                    if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainParameter.HAS_MOSS))
                    {
                        region.getTerrain(x,y).getParameters().remove(TerrainParameter.HAS_MOSS);
                    }
                }

                //Put out fires
                if(waterMap[x][y] > 0)
                {
                    //Put out fires
                    if(TerrainManager.hasParameter(region.getTerrain(x,y), TerrainManager.TerrainParameter.FIRE))
                    {
                       region.getTerrain(x,y).getParameters().remove(TerrainManager.TerrainParameter.FIRE);
                       waterMap[x][y] = (int)(waterMap[x][y] * 0.75);
                    }
                }

                //update water values
                region.getTerrain(x,y).setWater(waterMap[x][y]);
            }
        }
    }


    //Grow plant life
    public void growPlants()
    {
        Region region = quinoa.getCurrentRegionHeader().getRegion();

        for(int x=0; x < region.getWidth(); x++)
        {
            for(int y=0; y < region.getHeight(); y++)
            {
                Terrain terrain = region.getTerrain(x, y);

                //Grow grass and trees
                if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.GROW_COUNTER))
                {
                    int growCount = Integer.parseInt(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.GROW_COUNTER));
                    growCount = growCount - 1;
                    
                    if(growCount <= 0)
                    {
                        TerrainManager.growCrops(terrain, x, y, quinoa);
                    }
                    else
                    {
                        terrain.getParameters().put(TerrainManager.TerrainParameter.GROW_COUNTER, growCount+"");
                    }
                }

                //Grow mushrooms
                if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES)
                && quinoa.getLightMap().getCalc(x, y) < TerrainManager.SPORE_LIGHT_LEVEL_MAX)
                {
                    //only grow at night
                    MushroomSporeCode msc = TerrainManager.MushroomSporeCode.valueOf(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES));
                    boolean growMushroom=false;
                    
                    //don't grow in water, but encourage growth in damp ground
                    if(terrain.getWater() > 0)
                    {
                        if(terrain.getWater() < TerrainManager.DEEP_WATER)
                        {
                            if(Math.random() < TerrainManager.MUSHROOM_GROW_RATE_WET_GROUND)
                            {
                                growMushroom = true;
                            }
                        }
                    }
                    else
                    {
                        //slight chance to grow on dry ground
                        if(Math.random() < TerrainManager.MUSHROOM_GROW_RATE_DRY_GROUND)
                        {
                            growMushroom = true;
                        }
                    }
                    

                    //Grow the mushroom
                    if(growMushroom)
                    {
                        if(region.getItem(x, y) == null)
                        {

                            Item newMushroom = new Item();
                            newMushroom.setItemClass(TerrainManager.mushroomSporeToItemClass(msc));
                            ItemManager.initialize(newMushroom);
                            newMushroom.setPosition(x, y);
                            region.getItems().add(newMushroom);

                            //slight chance to remove spore
                            if(Math.random() < TerrainManager.MUSHROOM_REMOVAL_RATE)
                            {
                                terrain.getParameters().remove(TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES);
                            }
                        }

                    }

                    //slight chance of spreading spores
                    if(Math.random() < TerrainManager.MUSHROOM_SPREAD_RATE)
                    {
                        
                        this.spreadSpore(x, y, msc);
                        
                        //slight chance to remove spore
                        if(Math.random() < (TerrainManager.MUSHROOM_REMOVAL_RATE * 2.0d))
                        {
                            terrain.getParameters().remove(TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES);
                        }
                        
                    }
                }


                //handle clover
                if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_CLOVER)
                && quinoa.getLightMap().getCalc(x, y) >= TerrainManager.PLANT_LIGHT_LEVEL_MIN)
                {
                    int cloverCount = Integer.parseInt(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.HAS_CLOVER));
                    if(cloverCount > 0)
                    {
                        cloverCount = cloverCount - 1;
                        terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_CLOVER, cloverCount+"");
                    }

                    if(Math.random() < TerrainManager.CLOVER_SPREAD_RATE)
                    {
                        spreadClover(x,y);
                    }
                }

                //handle spreaing seeds
                if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_SEED))
                {
                    if(TerrainManager.spreadingCrop(SeedType.valueOf(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.HAS_SEED))))
                    {
                        if(Math.random() < TerrainManager.CROP_SPREAD_RATE)
                        {
                            spreadCrops(x,y);
                        }
                    }
                }

            }
        }
    }


    public void spreadClover(int x, int y)
    {
        //spread wild plants
        Region region = quinoa.getCurrentRegionHeader().getRegion();
        ArrayList<Terrain> terrains = new ArrayList<Terrain>();
        if(y > 0 && TerrainManager.growable(region.getTerrain(x, y - 1), x, y-1, quinoa.getLightMap(), TerrainParameter.HAS_CLOVER))
        {
            terrains.add(region.getTerrain(x, y - 1));
        }

        if(y < region.getHeight()-1 && TerrainManager.growable(region.getTerrain(x, y + 1), x, y+1, quinoa.getLightMap(), TerrainParameter.HAS_CLOVER))
        {
            terrains.add(region.getTerrain(x, y + 1));
        }

        if(x > 0 && TerrainManager.growable(region.getTerrain(x - 1, y), x - 1, y, quinoa.getLightMap(), TerrainParameter.HAS_CLOVER))
        {
            terrains.add(region.getTerrain(x - 1, y));
        }

        if(x < region.getWidth()-1 && TerrainManager.growable(region.getTerrain(x + 1, y), x + 1, y, quinoa.getLightMap(), TerrainParameter.HAS_CLOVER))
        {
            terrains.add(region.getTerrain(x + 1, y));
        }

        if(!terrains.isEmpty())
        {
            Terrain targetTerrain = terrains.get((int)(Math.random() * terrains.size()));
            int cloverCount = (int)(Math.random() * (TerrainManager.CLOVER_GROW_COUNT / 4)) + TerrainManager.CLOVER_GROW_COUNT;
            targetTerrain.getParameters().put(TerrainManager.TerrainParameter.HAS_CLOVER, cloverCount+"");
        }


    }



    public void spreadCrops(int x, int y)
    {
        Region region = quinoa.getCurrentRegionHeader().getRegion();
        ArrayList<Terrain> terrains = new ArrayList<Terrain>();

        SeedType seedType = SeedType.valueOf(TerrainManager.getParameter(region.getTerrain(x,y), TerrainManager.TerrainParameter.HAS_SEED));

        terrains = new ArrayList<Terrain>();
        if(y > 0 && TerrainManager.growable(region.getTerrain(x, y - 1), x, y-1, quinoa.getLightMap(), TerrainParameter.HAS_SEED))
        {
            terrains.add(region.getTerrain(x, y - 1));
        }

        if(y < region.getHeight()-1 && TerrainManager.growable(region.getTerrain(x, y + 1), x, y+1, quinoa.getLightMap(), TerrainParameter.HAS_SEED))
        {
            terrains.add(region.getTerrain(x, y + 1));
        }

        if(x > 0 && TerrainManager.growable(region.getTerrain(x - 1, y), x - 1, y, quinoa.getLightMap(), TerrainParameter.HAS_SEED))
        {
            terrains.add(region.getTerrain(x - 1, y));
        }

        if(x < region.getWidth()-1 && TerrainManager.growable(region.getTerrain(x + 1, y), x + 1, y, quinoa.getLightMap(), TerrainParameter.HAS_SEED))
        {
            terrains.add(region.getTerrain(x + 1, y));
        }

        if(!terrains.isEmpty())
        {
            Terrain targetTerrain = terrains.get((int)(Math.random() * terrains.size()));

            targetTerrain.getParameters().put(TerrainManager.TerrainParameter.HAS_SEED, seedType.name());
            targetTerrain.getParameters().put(TerrainManager.TerrainParameter.GROW_COUNTER, TerrainManager.getGrowCount(seedType)+"");
        }
    }


    public void spreadSpore(int x, int y, MushroomSporeCode msc)
    {
        Region region = quinoa.getCurrentRegionHeader().getRegion();
        ArrayList<Terrain> terrains = new ArrayList<Terrain>();
        if(y > 0 && TerrainManager.growable(region.getTerrain(x, y - 1), x, y-1, quinoa.getLightMap(), TerrainParameter.HAS_MUSHROOM_SPORES))
        {
            terrains.add(region.getTerrain(x, y - 1));
        }

        if(y < region.getHeight()-1 && TerrainManager.growable(region.getTerrain(x, y + 1), x, y+1, quinoa.getLightMap(), TerrainParameter.HAS_MUSHROOM_SPORES))
        {
            terrains.add(region.getTerrain(x, y + 1));
        }

        if(x > 0 && TerrainManager.growable(region.getTerrain(x - 1, y), x - 1, y, quinoa.getLightMap(), TerrainParameter.HAS_MUSHROOM_SPORES))
        {
            terrains.add(region.getTerrain(x - 1, y));
        }

        if(x < region.getWidth()-1 && TerrainManager.growable(region.getTerrain(x + 1, y), x + 1, y, quinoa.getLightMap(), TerrainParameter.HAS_MUSHROOM_SPORES))
        {
            terrains.add(region.getTerrain(x + 1, y));
        }

        if(!terrains.isEmpty())
        {
            Terrain targetTerrain = terrains.get((int)(Math.random() * terrains.size()));
            targetTerrain.getParameters().put(TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES, msc.name());
        }
    }

    //Spread fire around
    public void spreadFire()
    {
        Region region = quinoa.getCurrentRegionHeader().getRegion();

        //Spread fire
        int[][] fireMap = new int[region.getWidth()][region.getHeight()];
        for(int x=0; x < region.getWidth(); x++)
        {
            for(int y=0; y < region.getHeight(); y++)
            {
                fireMap[x][y] = 0;
            }
        }

        for(int x=0; x < region.getWidth(); x++)
        {
            for(int y=0; y < region.getHeight(); y++)
            {
                Terrain terrain = region.getTerrain(x, y);

                if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.FIRE))
                {
                    int fireValue = Integer.parseInt(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.FIRE));

                    //Allow fire to die if not feed
                    fireValue = fireValue - (int)(Math.random() * 3);

                    fireMap[x][y] = fireValue;

                    if(x > 0 && TerrainManager.flammable(region.getTerrain(x-1, y), x-1, y, quinoa))
                    {
                        fireMap[x-1][y] = fireValue + TerrainManager.fuelsFire(region.getTerrain(x-1, y), x-1, y, quinoa);
                    }

                    if(x < region.getWidth()-1 && TerrainManager.flammable(region.getTerrain(x+1, y), x+1, y, quinoa))
                    {
                        fireMap[x+1][y] = fireValue + TerrainManager.fuelsFire(region.getTerrain(x+1, y), x+1, y, quinoa);
                    }

                    if(y > 0 && TerrainManager.flammable(region.getTerrain(x, y-1), x, y-1, quinoa))
                    {
                        fireMap[x][y-1] = fireValue + TerrainManager.fuelsFire(region.getTerrain(x, y-1), x, y-1, quinoa);
                    }

                    if(y < region.getHeight()-1 && TerrainManager.flammable(region.getTerrain(x, y+1), x, y+1, quinoa))
                    {
                        fireMap[x][y+1] = fireValue+ TerrainManager.fuelsFire(region.getTerrain(x, y+1), x, y+1, quinoa);
                    }
                }
            }
        }

        //Refresh cycle
        for(int x=0; x < region.getWidth(); x++)
        {
            for(int y=0; y < region.getHeight(); y++)
            {
                Terrain terrain = region.getTerrain(x,y);
                if(fireMap[x][y] <= 0 && TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.FIRE))
                {
                    terrain.getParameters().remove(TerrainManager.TerrainParameter.FIRE);
                    TerrainManager.burn(terrain, x, y, quinoa);
                }
                else if(fireMap[x][y] > 0)
                {
                    terrain.getParameters().put(TerrainManager.TerrainParameter.FIRE, fireMap[x][y] + "");
                }
            }
        }
    }

    public void look(int targetX, int targetY)
    {
        String message = "";

        Terrain targetTerrain = quinoa.getCurrentRegionHeader().getRegion().getTerrain(targetX, targetY);

        if(quinoa.getLightMap().getCalc(targetX, targetY) > LightMap.DARKNESS_LIGHT_LEVEL)
        {
            if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_SIGN))
            {
                message = message + "The sign reads: " + TerrainManager.getParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_SIGN);
            }
            else
            {
                message = "";

                //list growing plants
                ArrayList<String> whatGrows = new ArrayList<String>();
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_MOSS))
                {
                    whatGrows.add("MOSS");
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES))
                {
                    whatGrows.add("a MUSHROOM patch");
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_CLOVER))
                {
                    whatGrows.add("CLOVER");
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_TREE))
                {
                    TreeCode tc = TreeCode.valueOf(TerrainManager.getParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_TREE));
                    whatGrows.add(article(tc.name()) + " " + tc.name());
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_SEED))
                {
                    SeedType st = SeedType.valueOf(TerrainManager.getParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_SEED));
                    whatGrows.add("a patch of " + st.name() + " " + TerrainManager.getStemType(st));
                }

                if(!whatGrows.isEmpty())
                {
                    String growString = " There ";
                    if(whatGrows.size() > 1)
                    {
                        growString = growString + "are ";
                    }
                    else
                    {
                        growString = growString + "is ";
                    }
                    int growStringCounter=0;
                    for(String tempString : whatGrows)
                    {
                        if(growStringCounter > 0)
                        {
                            if(growStringCounter == whatGrows.size()-2)
                            {
                                growString = growString + " and " + tempString;
                            }
                            else
                            {
                                growString = growString + "," + tempString;
                            }
                        }
                        else
                        {
                            growString = growString + tempString;
                        }
                        growStringCounter++;
                    }

                    growString = growString + " growing there.";

                    message = message + growString;
                }


                //check for features like graves, doors, drains, springs, bed
                ArrayList<String> whatIs = new ArrayList<String>();
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_BED))
                {
                    whatIs.add("a BED");
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_DOOR))
                {
                    whatIs.add("a DOOR");
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_DRAIN))
                {
                    whatIs.add("a DRAIN");
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_FLOODGATE))
                {
                    whatIs.add("a FLOODGATE");
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_MINERALS))
                {
                    whatIs.add("MINERALS in the rock");
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_SPRING))
                {
                    whatIs.add("a SPRING");
                }
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_GRAVE))
                {
                    GraveCode gc = GraveCode.valueOf(TerrainManager.getParameter(targetTerrain, TerrainManager.TerrainParameter.HAS_GRAVE));
                    if(gc == GraveCode.NORMAL)
                    {
                        whatIs.add("a GRAVE");
                    }
                    else if(gc == GraveCode.SPECIAL)
                    {
                        whatIs.add("a spooky GRAVE");
                    }
                    else if(gc == GraveCode.BROKEN)
                    {
                        whatIs.add("an open GRAVE");
                    }
                }


                if(!whatIs.isEmpty())
                {
                    String isString = " You see ";
                    int isStringCounter=0;
                    for(String tempString : whatIs)
                    {
                        if(isStringCounter > 0)
                        {
                            if(isStringCounter == whatIs.size()-2)
                            {
                                isString = isString + " and " + tempString;
                            }
                            else
                            {
                                isString = isString + "," + tempString;
                            }
                        }
                        else
                        {
                            isString = isString + tempString;
                        }
                        isStringCounter++;
                    }

                    isString = isString + " there.";

                    message = message + isString;
                }


                //ground states, i.e. on fire, wet
                if(TerrainManager.hasParameter(targetTerrain, TerrainManager.TerrainParameter.FIRE))
                {
                    message = message + " The " + targetTerrain.getCode() + " is on fire!";
                }
                else if(targetTerrain.getWater() > 0)
                {
                    if(targetTerrain.getWater() < TerrainManager.DEEP_WATER)
                    {
                        message = message + " The " + targetTerrain.getCode() + " is wet.";
                    }
                    else
                    {
                        message = message + " Water covers the " + targetTerrain.getCode() + ".";
                    }
                }

                if(message.startsWith(" "))
                {
                    message = message.substring(1);
                }


                //Default to terrain desciption
                if(message.equals(""))
                {
                    message = message + "You see the " + targetTerrain.getCode() + " there.";
                }
            }
        }
        else
        {
            message = "You can't see in this light.";
        }

        quinoa.getMessageManager().addMessage(message);
    }

    public void talk(int targetX, int targetY)
    {
        String message = "";
        //Look for a monster at that given point
        Monster tempMon = quinoa.getMonster(targetX, targetY);
        if(tempMon != null)
        {
            if(tempMon == quinoa.getPlayer())
            {
                message = "You talk to yourself.";
            }
            else if(tempMon.getRole() == MonsterRole.BROTHER)
            {
                message = "You found me, brother! Congratulations!";
                quinoa.getUI().getScreen().displayDialog();
            }
            else
            {
                message = "The " + tempMon.getMonsterCode().name() + " doesn't have much to say yet.";
            }
        }
        else
        {
            message = "There's no one to talk to there.";
        }
        quinoa.getMessageManager().addMessage(message);
    }

    public void trade(int targetX, int targetY)
    {
        String message = "";
        //Look for a monster at that given point
        Monster tempMon = quinoa.getMonster(targetX, targetY);
        if(tempMon != null)
        {
            if(tempMon != quinoa.getPlayer())
            {
                if(MonsterActionManager.tradingRole(tempMon.getRole()))
                {
                    quinoa.getUI().getScreen().displayTrade(tempMon);
                }
                else
                {
                    message = "They don't want to trade.";
                    quinoa.getMessageManager().addMessage(message);
                }
            }
            else
            {
                message = "There's no one to trade with there.";
                quinoa.getMessageManager().addMessage(message);
            }
        }
        else
        {
            message = "There's no one to trade with there.";
            quinoa.getMessageManager().addMessage(message);
        }
    }

    public void newGame()
    {
        try
        {
            quinoa.setTicks(((Quinoa.TICKS_PER_SECOND * 60) * 60) * 10);
            quinoa.getMessageManager().clearMessages();
            MapGeneratorInterface mapGen = new MapGenerator();
            mapGen.generate(quinoa);
            quinoa.getMap().changeCurrentRegion("main");
            insertPlayerInRegion(quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }

    public void followExit(RegionExit exit)
    {
        try
        {
            removePlayerFromRegion();
            quinoa.getMap().changeCurrentRegion(exit.getDestinationRegionID());
            spawnMonsters();

            if(quinoa.getCurrentRegionHeader().regionIsLoaded())
            {
                for(Monster tempMon : quinoa.getCurrentRegionHeader().getRegion().getMonsters())
                {
                    MonsterActionManager.updateRole(tempMon, quinoa);
                }
            }

            insertPlayerInRegion(exit.getDx(),exit.getDy());
        }
        catch(Exception e)
        {
            e.printStackTrace();
        }
    }

    public void checkForPlayerOnExits()
    {
        RegionExit exitToUse = null;
        for(RegionExit tempExit: quinoa.getCurrentRegionHeader().getExits())
        {
            if(tempExit.getX() == quinoa.getPlayer().getX()
            && tempExit.getY() == quinoa.getPlayer().getY())
            {
                exitToUse = tempExit;
            }
        }

        if(exitToUse != null)
        {
            quinoa.getActions().followExit(exitToUse);
        }
    }

    public void removePlayerFromRegion()
    {
        if(quinoa.getMap().getCurrentRegionHeader().regionIsLoaded())
        {
            Monster player = quinoa.getMonsterByID(MonsterActionManager.PLAYER_ID);
            quinoa.getMap().getCurrentRegionHeader().getRegion().getMonsters().remove(player);
        }
    }

    public void insertPlayerInRegion(int x, int y)
    {
        if(quinoa.getMap().getCurrentRegionHeader().regionIsLoaded())
        {
            quinoa.getMap().getCurrentRegionHeader().getRegion().getMonsters().add(quinoa.getPlayer());
            quinoa.getPlayer().setPosition(x, y);

            if(regionContainsBrother())
            {
                quinoa.getMessageManager().addMessage("You can here your brother calling!");
            }
        }
    }

    public boolean regionContainsBrother()
    {
        if(quinoa.getMap().getCurrentRegionHeader().regionIsLoaded())
        {
            for(Monster tempMon : quinoa.getMap().getCurrentRegionHeader().getRegion().getMonsters())
            {
                if(tempMon.getRole() == MonsterRole.BROTHER)
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public void showDialog()
    {
        quinoa.getUI().getScreen().displayDialog();
    }

    public void addQueuedMonsters()
    {
        if(!monsterQueue.isEmpty())
        {
            for(Monster tempMon : monsterQueue)
            {
                quinoa.getCurrentRegionHeader().getRegion().getMonsters().add(tempMon);
            }
            monsterQueue.clear();
        }
    }


    /**
     * Checks line of sight from one coordinate to the other. Sees through
     * diagonal corners.
     *
     * This function comes from:
     * http://roguebasin.roguelikedevelopment.org/index.php?title=Simple_Line_of_Sight
     *
     * @param x0p Start X coordinate
     * @param y0p Start Y coordinate
     * @param x1p End X coordinate
     * @param y1p End Y coordinate
     * @return Returns true if there are no obstacles, false if there are obstacles
     */
    public boolean hasLineOfSight(int x0p, int y0p, int x1p, int y1p)
    {
        int t, x, y, ax, ay, sx, sy, dx, dy;

        int mx = x1p;
        int my = y1p;
        int px = x0p;
        int py = y0p;

        Region region = quinoa.getCurrentRegionHeader().getRegion();

        /* Delta x is the players x minus the monsters x    *
        * d is my dungeon structure and px is the players  *
        * x position. mx is the monsters x position passed *
        * to the function.                                 */
        dx = px - mx;

        /* dy is the same as dx using the y coordinates */
        dy = py - my;

        /* ax & ay: these are the absolute values of dx & dy *
        * multiplied by 2 ( shift left 1 position)          */
        ax = Math.abs(dx)<<1;
        ay = Math.abs(dy)<<1;

        /* sx & sy: these are the signs of dx & dy */
        //sx = sign(dx);
        //sy = sign(dy);
        sx = (dx == 0) ? 0 : (dx > 0) ? 1 : -1;
        sy = (dy == 0) ? 0 : (dy > 0) ? 1 : -1;

        /* x & y: these are the monster's x & y coords */
        x = mx;
        y = my;

        /* The following if statement checks to see if the line *
        * is x dominate or y dominate and loops accordingly    */
        if(ax > ay)
        {
            /* X dominate loop */
            /* t = the absolute of y minus the absolute of x divided *
            by 2 (shift right 1 position)                         */
            t = ay - (ax >> 1);
            do
            {
                if(t >= 0)
                {
                /* if t is greater than or equal to zero then *
                * add the sign of dy to y                    *
                * subtract the absolute of dx from t         */
                y += sy;
                t -= ax;
                }

                /* add the sign of dx to x      *
                * add the adsolute of dy to t  */
                x += sx;
                t += ay;

                /* check to see if we are at the player's position */
                if(x == px && y == py)
                {
                    /* return that the monster can see the player */
                    return true;
                }
                /* keep looping until the monster's sight is blocked *
                * by an object at the updated x,y coord             */
        }
        while(TerrainManager.transparent(region.getTerrain(x, y)));

        /* NOTE: sight_blocked is a function that returns true      *
        * if an object at the x,y coord. would block the monster's *
        * sight                                                    */

        /* the loop was exited because the monster's sight was blocked *
        * return FALSE: the monster cannot see the player             */
        return false;
        }
        else
        {
            /* Y dominate loop, this loop is basically the same as the x loop */
            t = ax - (ay >> 1);
            do
            {
                if(t >= 0)
                {
                    x += sx;
                    t -= ay;
                }
                y += sy;
                t += ax;
                if(x == px && y == py)
                {
                    return true;
                }
            }
            while(TerrainManager.transparent(region.getTerrain(x, y)));
            return false;
        }
    }


    public void spawnMonsters()
    {
        //get a list of how many monsters are on the map
        Region region = quinoa.getCurrentRegionHeader().getRegion();
        quinoa.getLightMap().calculate(region);
        HashMap<MonsterCode, Integer> monsterCount = new HashMap<MonsterCode, Integer>();
        for(MonsterCode mc : MonsterCode.values())
        {
            Integer newCount = TerrainManager.getMonsterCount(region, mc);
            monsterCount.put(mc, newCount);
        }

        //look for oppurtunities to spawn monsters
        for(int x=0; x < region.getWidth(); x++)
        {
            for(int y=0; y < region.getHeight(); y++)
            {
                Terrain terrain = region.getTerrain(x, y);

                if(quinoa.getLightMap().getCalc(x, y) < LightMap.DARKNESS_LIGHT_LEVEL)
                {
                    //dark spawning monsters

                    //GHOST spawn
                    if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
                    {
                        GraveCode gc = GraveCode.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_GRAVE));
                        if(gc == GraveCode.BROKEN || (gc == GraveCode.SPECIAL && Math.random() < 0.1))
                        {
                            if(Math.random() < MonsterActionManager.getSpawnRate(MonsterCode.GHOST)
                            && monsterCount.get(MonsterCode.GHOST).intValue() < MonsterActionManager.getMaxMonsterPerRegion(MonsterCode.GHOST))
                            {
                                //spawn a ghost
                                Monster newMon = new Monster();
                                newMon.setMonsterCode(MonsterCode.GHOST);
                                MonsterActionManager.initialize(newMon);
                                newMon.setPosition(x, y);
                                quinoa.getActions().addMonster(newMon);
                                monsterCount.put(MonsterCode.GHOST, new Integer(monsterCount.get(MonsterCode.GHOST).intValue() + 1));
                            }
                        }
                    }

                    //SPONGE spawn
                    if(terrain.getWater() > 0)
                    {
                        if(terrain.getWater() > TerrainManager.DEEP_WATER
                        && Math.random() < MonsterActionManager.getSpawnRate(MonsterCode.SPONGE)
                        && monsterCount.get(MonsterCode.SPONGE).intValue() < MonsterActionManager.getMaxMonsterPerRegion(MonsterCode.SPONGE))
                        {
                            //spawn a sponge
                            Monster newMon = new Monster();
                            newMon.setMonsterCode(MonsterCode.SPONGE);
                            MonsterActionManager.initialize(newMon);
                            newMon.setPosition(x, y);
                            quinoa.getActions().addMonster(newMon);
                            monsterCount.put(MonsterCode.SPONGE, new Integer(monsterCount.get(MonsterCode.SPONGE).intValue() + 1));
                        }
                    }

                    //SLIME spawn
                    if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS))
                    {
                        if(Math.random() < MonsterActionManager.getSpawnRate(MonsterCode.SLIME)
                        && monsterCount.get(MonsterCode.SLIME).intValue() < MonsterActionManager.getMaxMonsterPerRegion(MonsterCode.SLIME))
                        {
                            //spawn a slime
                            Monster newMon = new Monster();
                            newMon.setMonsterCode(MonsterCode.SLIME);
                            MonsterActionManager.initialize(newMon);
                            newMon.setPosition(x, y);
                            quinoa.getActions().addMonster(newMon);
                            monsterCount.put(MonsterCode.SLIME, new Integer(monsterCount.get(MonsterCode.SLIME).intValue() + 1));
                        }
                    }
                    
                }
                else
                {
                    //Light spawning monsters

                    //TINY_SLIME spawn
                    if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS))
                    {
                        if(Math.random() < MonsterActionManager.getSpawnRate(MonsterCode.TINY_SLIME)
                        && monsterCount.get(MonsterCode.TINY_SLIME).intValue() < MonsterActionManager.getMaxMonsterPerRegion(MonsterCode.TINY_SLIME))
                        {
                            //spawn a tiny slime
                            Monster newMon = new Monster();
                            newMon.setMonsterCode(MonsterCode.TINY_SLIME);
                            MonsterActionManager.initialize(newMon);
                            newMon.setPosition(x, y);
                            quinoa.getActions().addMonster(newMon);
                            monsterCount.put(MonsterCode.TINY_SLIME, new Integer(monsterCount.get(MonsterCode.TINY_SLIME).intValue() + 1));
                        }
                    }

                    //PIG spawn
                    if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES) && region.getLightingModel() == LightingModel.ABOVE_GROUND)
                    {
                        if(Math.random() < MonsterActionManager.getSpawnRate(MonsterCode.PIG)
                        && monsterCount.get(MonsterCode.PIG).intValue() < MonsterActionManager.getMaxMonsterPerRegion(MonsterCode.PIG))
                        {
                            //spawn a pig
                            Monster newMon = new Monster();
                            newMon.setMonsterCode(MonsterCode.PIG);
                            MonsterActionManager.initialize(newMon);
                            newMon.setPosition(x, y);
                            quinoa.getActions().addMonster(newMon);
                            monsterCount.put(MonsterCode.PIG, new Integer(monsterCount.get(MonsterCode.PIG).intValue() + 1));
                        }
                    }

                    //DEER spawn
                    if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_CLOVER) && region.getLightingModel() == LightingModel.ABOVE_GROUND)
                    {
                        if(Math.random() < MonsterActionManager.getSpawnRate(MonsterCode.DEER)
                        && monsterCount.get(MonsterCode.DEER).intValue() < MonsterActionManager.getMaxMonsterPerRegion(MonsterCode.DEER))
                        {
                            //spawn a deer
                            Monster newMon = new Monster();
                            newMon.setMonsterCode(MonsterCode.DEER);
                            MonsterActionManager.initialize(newMon);
                            newMon.setPosition(x, y);
                            quinoa.getActions().addMonster(newMon);
                            monsterCount.put(MonsterCode.DEER, new Integer(monsterCount.get(MonsterCode.DEER).intValue() + 1));
                        }
                    }
                }
            }
        }
    }


    //return the article of the string
    public String article(String string)
    {
        if(string.toLowerCase().startsWith("a")
        || string.toLowerCase().startsWith("e")
        || string.toLowerCase().startsWith("i")
        || string.toLowerCase().startsWith("o")
        || string.toLowerCase().startsWith("u"))
        {
            return "an";
        }
        else
        {
            return "a";
        }
    }
    
}
