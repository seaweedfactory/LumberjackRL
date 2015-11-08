package quinoa.region;

import java.util.ArrayList;
import quinoa.Quinoa;
import quinoa.items.Item;
import quinoa.items.ItemManager;
import quinoa.items.ItemManager.ItemClass;
import quinoa.items.ItemManager.ItemState;
import quinoa.monsters.Monster;
import quinoa.monsters.MonsterActionManager.MonsterCode;

public class TerrainManager
{
    public enum TerrainCode {PATH, DIRT, GRASS, STONE_WALL, STONE_FLOOR, FERTILE_LAND, ROCK, STREAM_BED;}
    public enum TerrainParameter {HAS_TREE, HAS_DOOR, HAS_SIGN, FIRE, GROW_COUNTER, HAS_SPRING, 
                                  HAS_DRAIN, HAS_MINERALS, HAS_MUSHROOM_SPORES, HAS_BED, HAS_GRAVE, 
                                  DAMAGE, HAS_CLOVER, HAS_SEED, HAS_FLOODGATE, HAS_MOSS};
    public enum MushroomSporeCode {DEATH_CAP, PUFFBALL, FLY_AGARIC, MOREL, BUTTON_MUSHROOM, GHOST_FUNGUS, POISON, EDIBLE};
    public enum TreeCode {PINE_TREE, BIRCH_TREE, OAK_TREE, APPLE_TREE};
    public enum GraveCode {NORMAL, SPECIAL, BROKEN};
    public enum SeedType {CORN, PUMPKIN};
    public enum DoorCode {OPEN, CLOSED};

    public static final int DEEP_WATER=25;
    public static final int DRAIN_RATE=200;
    public static final int SPRING_RATE=250;
    public static final double EVAPORATION_RATE= 0.50;
    public static final int MAX_WATER = 2000;
    public static final int WATER_CYCLE_COUNT = 100;
    public static final int BASE_FLAME_RATE=3;
    public static final double SPORE_LIGHT_LEVEL_MAX=0.80;
    public static final double PLANT_LIGHT_LEVEL_MIN=0.70;
    public static final int CLOVER_GROW_COUNT=100;
    public static final int GRASS_GROW_COUNT=10000;
    public static final int CORN_GROW_COUNT=1000;
    public static final int PUMPKIN_GROW_COUNT=400;
    public static final double MUSHROOM_GROW_RATE_WET_GROUND=0.02;
    public static final double MUSHROOM_GROW_RATE_DRY_GROUND=0.001;
    public static final double MUSHROOM_REMOVAL_RATE=0.02;
    public static final double MUSHROOM_SPREAD_RATE=0.0025;
    public static final double CLOVER_SPREAD_RATE=0.0025;
    public static final double CROP_SPREAD_RATE=0.025;
    public static final double PUMPKIN_PRODUCTION_RATE=0.01;
    public static final double TREE_REGROWTH_RATE=0.01;
    public static final double MOSS_GROW_RATE=0.0025;
    public static final double MOSS_DROP_RATE=0.20;
    public static final double GRAVE_ROB_CHANCE=0.50;
    public static final double APPLE_TREE_PRODUCTION_RATE=0.25;
    public static final double APPLE_TREE_FROM_APPLE_CHANCE=0.10;


    public static void removeParameters(Terrain terrain)
    {
        terrain.getParameters().clear();
    }


    public static int getMonsterCount(Region region, MonsterCode monsterCode)
    {
        int total = 0;
        for(Monster tempMonster : region.getMonsters())
        {
            if(tempMonster.getMonsterCode() == monsterCode)
            {
                total = total + 1;
            }
        }
        return total;
    }

    public static TreeCode getRandomTree()
    {
        int rnd = (int)(Math.random() * 1000);
        if(rnd <= 5)
        {
            return TreeCode.APPLE_TREE;
        }
        else if(rnd <= 100)
        {
            return TreeCode.BIRCH_TREE;
        }
        else if(rnd <= 500)
        {
            return TreeCode.OAK_TREE;
        }
        else
        {
            return TreeCode.PINE_TREE;
        }

    }


    public static void step(Terrain terrain, int x, int y, Region region, Quinoa quinoa)
    {
        //moss is destroyed if stepped upon
        if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainParameter.HAS_MOSS))
        {
            region.getTerrain(x, y).getParameters().remove(TerrainParameter.HAS_MOSS);

            if(Math.random() < MOSS_DROP_RATE)
            {
                Item moss = new Item();
                moss.setItemClass(ItemManager.ItemClass.MOSS);
                moss.setPosition(x, y);
                moss.setItemState(ItemState.GROUND);
                region.getItems().add(moss);
            }
        }
    }

    //Return the name of a seedType's vine or stalk
    public static String getStemType(SeedType seedType)
    {
        switch(seedType)
        {
            case CORN:
            return "stalks";

            case PUMPKIN:
            return "vines";

            default:
            return "stems";
        }
    }


    //Return the default grow count, plus a small variation
    public static int getGrowCount(SeedType seedType)
    {
        if(seedType == null)
        {
            return GRASS_GROW_COUNT + (int)(Math.random() * (GRASS_GROW_COUNT / 10));
        }
        else
        {
            switch(seedType)
            {
                case CORN:
                return CORN_GROW_COUNT + (int)(Math.random() * (CORN_GROW_COUNT / 20));

                case PUMPKIN:
                return PUMPKIN_GROW_COUNT + (int)(Math.random() * (PUMPKIN_GROW_COUNT / 4));

                default:
                return 1;
            }
        }
    }

    //Does this crop spread?
    public static boolean spreadingCrop(SeedType seedType)
    {
       switch(seedType)
       {
           case CORN:
           return false;

           case PUMPKIN:
           return true;

           default:
           return false;
       }
    }

    //Grow crops on this tile
    public static void growCrops(Terrain terrain, int x, int y, Quinoa quinoa)
    {
        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED))
        {
            if(quinoa.getLightMap().getCalc(x, y) >= TerrainManager.PLANT_LIGHT_LEVEL_MIN)
            {
                SeedType seedType = SeedType.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_SEED));
                switch(seedType)
                {
                    case CORN:
                    terrain.getParameters().remove(TerrainManager.TerrainParameter.HAS_SEED);
                    terrain.getParameters().put(TerrainManager.TerrainParameter.GROW_COUNTER, TerrainManager.GRASS_GROW_COUNT+"");

                    if(quinoa.getCurrentRegionHeader().getRegion().getItem(x, y) == null)
                    {
                        if(Math.random() < 0.50)
                        {
                            Item cornSeed = new Item();
                            cornSeed.setItemClass(ItemClass.CORN_SEED);
                            cornSeed.setStackSize((int)(Math.random() * 9) + 1);
                            cornSeed.setPosition(x, y);
                            quinoa.getCurrentRegionHeader().getRegion().getItems().add(cornSeed);
                        }

                        Item corn = new Item();
                        corn.setItemClass(ItemClass.CORN);
                        corn.setStackSize((int)(Math.random() * 3) + 1);
                        corn.setPosition(x, y);
                        quinoa.getCurrentRegionHeader().getRegion().getItems().add(corn);
                    }
                    break;

                    case PUMPKIN:
                    //reset the counter
                    terrain.getParameters().put(TerrainManager.TerrainParameter.GROW_COUNTER, (TerrainManager.PUMPKIN_GROW_COUNT / 2)+"");

                    //chance to produce a pumpkin
                    if(quinoa.getCurrentRegionHeader().getRegion().getItem(x, y) == null)
                    {
                        if(Math.random() < TerrainManager.PUMPKIN_PRODUCTION_RATE)
                        {
                            Item pumpkinSeed = new Item();
                            pumpkinSeed.setItemClass(ItemClass.PUMPKIN_SEED);
                            pumpkinSeed.setStackSize((int)(Math.random() * 3) + 1);
                            pumpkinSeed.setPosition(x, y);
                            quinoa.getCurrentRegionHeader().getRegion().getItems().add(pumpkinSeed);

                            Item pumpkin = new Item();
                            pumpkin.setItemClass(ItemClass.PUMPKIN);
                            pumpkin.setStackSize(1);
                            pumpkin.setPosition(x, y);
                            quinoa.getCurrentRegionHeader().getRegion().getItems().add(pumpkin);
                        }
                    }
                    break;
                }
            }
        }
        else
        {
            //no seeds, just grow grass
            terrain.getParameters().remove(TerrainManager.TerrainParameter.GROW_COUNTER);
            terrain.setCode(TerrainManager.TerrainCode.GRASS);

            if(Math.random() < TerrainManager.TREE_REGROWTH_RATE)
            {
                terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_TREE, TerrainManager.getRandomTree().name());
            }
        }
    }

    public static int maxTreeDamage(TreeCode tc)
    {
        switch(tc)
        {
            case PINE_TREE:
            return 3;

            case BIRCH_TREE:
            return 2;

            case OAK_TREE:
            return 5;

            default:
            return 3;
        }
    }
    
    public static ItemClass mushroomSporeToItemClass(MushroomSporeCode msc)
    {
        ArrayList<ItemClass> choices = new ArrayList<ItemClass>();
        switch(msc)
        {
            case DEATH_CAP:
            return ItemClass.DEATH_CAP;

            case PUFFBALL:
            return ItemClass.PUFFBALL;

            case FLY_AGARIC:
            return ItemClass.FLY_AGARIC;

            case MOREL:
            return ItemClass.MOREL;

            case BUTTON_MUSHROOM:
            return ItemClass.BUTTON_MUSHROOM;

            case GHOST_FUNGUS:
            return ItemClass.GHOST_FUNGUS;

            case POISON:
            choices.add(ItemClass.DEATH_CAP);
            choices.add(ItemClass.FLY_AGARIC);
            choices.add(ItemClass.GHOST_FUNGUS);
            return choices.get((int)(Math.random() * choices.size()));

            case EDIBLE:
            choices.add(ItemClass.PUFFBALL);
            choices.add(ItemClass.MOREL);
            choices.add(ItemClass.BUTTON_MUSHROOM);
            return choices.get((int)(Math.random() * choices.size()));
        }
        return null;
    }



    public static boolean growable(Terrain terrain, int x, int y, LightMap lightMap, TerrainParameter thingToGrow)
    {
        //check for doors, trees, signs, graves
        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE)
        || TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN)
        || TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE)
        || TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR))
        {
            return false;
        }
        
        switch(thingToGrow)
        {
            //crops
            case HAS_SEED:
            if(terrain.getCode() == TerrainManager.TerrainCode.FERTILE_LAND
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS))
            {
                return true;
            }
            return false;

            //mushrooms
            case HAS_MUSHROOM_SPORES:
            if(lightMap.getCalc(x, y) <= TerrainManager.SPORE_LIGHT_LEVEL_MAX
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_CLOVER)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED)
            && terrain.getCode() != TerrainCode.STONE_WALL
            && terrain.getCode() != TerrainCode.ROCK)
            {
                return true;
            }
            return false;

            //clover
            case HAS_CLOVER:
            if(!TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_CLOVER)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS)
            && terrain.getCode() != TerrainCode.STONE_WALL
            && terrain.getCode() != TerrainCode.ROCK
            && terrain.getCode() != TerrainCode.STREAM_BED
            && terrain.getCode() != TerrainCode.PATH)
            {
                return true;
            }
            return false;

            //moss
            case HAS_MOSS:
            if(terrain.getWater() <= 0
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_CLOVER)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS)
            && (terrain.getCode() == TerrainCode.STONE_FLOOR
            || terrain.getCode() == TerrainCode.STONE_WALL
            || terrain.getCode() == TerrainCode.ROCK))
            {
                return true;
            }
            return false;


            default:
            return true;
        }
    }

    public static Item containsItem(ItemClass itemClass, int x, int y, Quinoa quinoa)
    {
        for(Item tempItem : quinoa.getCurrentRegionHeader().getRegion().getItems())
        {
            if(tempItem.getItemClass() == itemClass
            && tempItem.getX() == x && tempItem.getY() == y)
            {
                return tempItem;
            }
        }
        return null;
    }

    public static boolean hasParameter(Terrain terrain, TerrainParameter parameter)
    {
        return terrain.getParameters().get(parameter) != null;
    }

    public static String getParameter(Terrain terrain, TerrainParameter parameter)
    {
        Object returned = terrain.getParameters().get(parameter);
        if(returned != null)
        {
            return (String)returned;
        }
        return null;
    }

    public static int fuelsFire(Terrain terrain, int x, int y, Quinoa quinoa)
    {
        if(containsItem(ItemClass.LOG, x, y, quinoa) != null)
        {
            return 10;
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE))
        {
            return 2;
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR))
        {
            return 1;
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN))
        {
            return 1;
        }

        switch(terrain.getCode())
        {
            default:
            return 0;
        }

    }

    public static boolean wetable(Terrain terrain, int x, int y, Quinoa quinoa)
    {
        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR))
        {
            DoorCode doorCode = DoorCode.valueOf(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.HAS_DOOR));
            if(doorCode == DoorCode.OPEN)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_FLOODGATE))
        {
            DoorCode doorCode = DoorCode.valueOf(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.HAS_FLOODGATE));
            if(doorCode == DoorCode.OPEN)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        switch(terrain.getCode())
        {
            case PATH:
            case DIRT:
            case GRASS:
            case STONE_FLOOR:
            case FERTILE_LAND:
            case STREAM_BED:
            return true;


            case STONE_WALL:
            case ROCK:
            return false;

            default:
            return false;
        }
    }


    public static boolean mineable(Terrain terrain)
    {
        switch(terrain.getCode())
        {
            case ROCK:
            case STONE_FLOOR:
            case STREAM_BED:
            return true;

            default:
            return false;
        }
    }


    public static boolean diggable(Terrain terrain)
    {
        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE)
        || TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR)
        || TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN))
        {
            return false;
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
        {
            GraveCode gc = GraveCode.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_GRAVE));
            if(gc != GraveCode.BROKEN)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        switch(terrain.getCode())
        {
            case PATH:
            case STONE_WALL:
            case STONE_FLOOR:
            case FERTILE_LAND:
            case ROCK:
            return false;

            case GRASS:
            case DIRT:
            case STREAM_BED:
            return true;

            default:
            return false;
        }
    }


    public static void mine(Terrain terrain, int x, int y, Quinoa quinoa)
    {
        switch(terrain.getCode())
        {
            case ROCK:
            terrain.setCode(TerrainCode.STONE_FLOOR);

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MINERALS))
            {
                int mineralCode = Integer.parseInt(TerrainManager.getParameter(terrain, TerrainParameter.HAS_MINERALS));
                switch(mineralCode)
                {
                    case 1:
                    if(Math.random() > 0.5)
                    {
                        Item gem = new Item();
                        gem.setItemClass(ItemClass.AMETHYST);
                        gem.setPosition(x, y);
                        quinoa.getCurrentRegionHeader().getRegion().getItems().add(gem);
                    }
                    else
                    {
                        //do nothing
                    }
                    break;

                    case 2:
                    if(Math.random() > 0.5)
                    {
                        Item gem = new Item();
                        gem.setItemClass(ItemClass.SAPPHIRE);
                        gem.setPosition(x, y);
                        quinoa.getCurrentRegionHeader().getRegion().getItems().add(gem);
                    }
                    else
                    {
                        Item gem = new Item();
                        gem.setItemClass(ItemClass.EMERALD);
                        gem.setPosition(x, y);
                        quinoa.getCurrentRegionHeader().getRegion().getItems().add(gem);
                    }
                    break;

                    case 3:
                    if(Math.random() > 0.33)
                    {
                        Item gem = new Item();
                        gem.setItemClass(ItemClass.RUBY);
                        gem.setPosition(x, y);
                        quinoa.getCurrentRegionHeader().getRegion().getItems().add(gem);
                    }
                    else
                    {
                        Item gem = new Item();
                        gem.setItemClass(ItemClass.DIAMOND);
                        gem.setPosition(x, y);
                        quinoa.getCurrentRegionHeader().getRegion().getItems().add(gem);
                    }
                    break;

                }
            }

            break;


            case STONE_FLOOR:
            terrain.setCode(TerrainCode.STREAM_BED);
            break;

            case STREAM_BED:
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DRAIN))
            {
                terrain.getParameters().remove(TerrainParameter.HAS_DRAIN);
            }
            else
            {
                terrain.getParameters().put(TerrainParameter.HAS_DRAIN, TerrainManager.DRAIN_RATE+"");
            }


            default:
            //do nothing
            break;
        }
    }


    public static void dig(Terrain terrain, int x, int y, Quinoa quinoa)
    {
        //check for graves
        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
        {
            GraveCode gc = GraveCode.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_GRAVE));
            if(gc == GraveCode.NORMAL && Math.random() < TerrainManager.GRAVE_ROB_CHANCE)
            {
                Item reward = ItemManager.getTreasure(Math.random());
                reward.setPosition(x, y);
                quinoa.getCurrentRegionHeader().getRegion().getItems().add(reward);
            }

            //open the grave
            terrain.getParameters().put(TerrainParameter.HAS_GRAVE, GraveCode.BROKEN.name());

            //drop bones
            Item bones = new Item();
            bones.setItemClass(ItemClass.BONES);
            bones.setPosition(x, y);
            bones.setItemState(ItemState.GROUND);
            quinoa.getCurrentRegionHeader().getRegion().getItems().add(bones);

            return;
        }

        switch(terrain.getCode())
        {
            case GRASS:
            terrain.setCode(TerrainCode.DIRT);
            break;

            case DIRT:
            terrain.setCode(TerrainCode.STONE_FLOOR);
            break;

            case STREAM_BED:
            if(!TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DRAIN))
            {
                if(!TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SPRING))
                {
                    terrain.getParameters().put(TerrainParameter.HAS_SPRING, TerrainManager.SPRING_RATE+"");
                }
                else
                {
                    terrain.getParameters().remove(TerrainParameter.HAS_SPRING);
                }
            }
            break;

            default:
            //Do nothing, can not dig
            break;

        }
    }


    public static void burn(Terrain terrain, int x, int y, Quinoa quinoa)
    {
        boolean dropAsh = false;

        //Look for items on this squares and remove them
        for(Item tempItem : quinoa.getCurrentRegionHeader().getRegion().getItems())
        {
            if(tempItem.getX() == x && tempItem.getY() == y)
            {
                switch(tempItem.getItemClass())
                {
                    case LOG: dropAsh=true; break;
                    default: dropAsh= false; break;
                }
                tempItem.remove();
            }
        }

        //Examine terrain features
        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE))
        {
            terrain.getParameters().remove(TerrainParameter.HAS_TREE);
            if(Math.random() < 0.125)
            {
                dropAsh = true;
            }
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR))
        {
            terrain.getParameters().remove(TerrainParameter.HAS_DOOR);
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN))
        {
            terrain.getParameters().remove(TerrainParameter.HAS_SIGN);
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES))
        {
            terrain.getParameters().remove(TerrainParameter.HAS_MUSHROOM_SPORES);
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_CLOVER))
        {
            terrain.getParameters().remove(TerrainParameter.HAS_CLOVER);
        }

        switch(terrain.getCode())
        {
            
            case GRASS:
            if(Math.random() < 0.98)
            {
                terrain.setCode(TerrainCode.FERTILE_LAND);
                terrain.getParameters().put(TerrainParameter.GROW_COUNTER, TerrainManager.GRASS_GROW_COUNT+"");


                if(Math.random() < 0.001)
                {
                    int cloverCount = (int)(Math.random() * (TerrainManager.CLOVER_GROW_COUNT / 4)) + TerrainManager.CLOVER_GROW_COUNT;
                    terrain.getParameters().put(TerrainParameter.HAS_CLOVER, cloverCount+"");
                }
                else
                {
                    if(Math.random() < 0.001)
                    {
                        MushroomSporeCode msc = TerrainManager.MushroomSporeCode.values()[(int)(Math.random() * TerrainManager.MushroomSporeCode.values().length)];
                        terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES, msc.name());
                    }
                }

            }
            else
            {
                terrain.setCode(TerrainCode.DIRT);
            }
            break;

            default:
            //Do nothing, does not burn
            break;

        }

        if(dropAsh)
        {
            Item ash = new Item();
            ash.setItemClass(ItemClass.ASH);
            ash.setPosition(x, y);
            quinoa.getCurrentRegionHeader().getRegion().getItems().add(ash);
        }
    }

    public static boolean flammable(Terrain terrain, int x, int y, Quinoa quinoa)
    {
        //See if the area is wet
        if(terrain.getWater() > 0)
        {
            return false;
        }

        if(containsItem(ItemClass.LOG, x, y, quinoa) != null)
        {
            return true;
        }

        switch(terrain.getCode())
        {
            case PATH:
            case DIRT:
            case STONE_WALL:
            case STONE_FLOOR:
            case FERTILE_LAND:
            case ROCK:
            case STREAM_BED:
            return false;

            case GRASS:
            return true;

            default:
            return false;
        }
    }

    public static boolean transparent(Terrain terrain)
    {
        if(hasParameter(terrain, TerrainParameter.HAS_TREE))
        {
            return false;
        }

        if(hasParameter(terrain, TerrainParameter.HAS_DOOR))
        {
            DoorCode doorCode = DoorCode.valueOf(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.HAS_DOOR));
            if(doorCode == DoorCode.OPEN)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        switch(terrain.getCode())
        {
            case PATH:
            case DIRT:
            case GRASS:
            case STONE_FLOOR:
            case FERTILE_LAND:
            case STREAM_BED:
            return true;

            case STONE_WALL:
            case ROCK:
            return false;
        }

        return true;
    }


    public static boolean allowWaterToFlowBetween(Terrain oldTerrain, Terrain newTerrain)
    {
        int oldWater = oldTerrain.getWater();
        int newWater = newTerrain.getWater();

        if(newWater >= oldWater || newWater >= TerrainManager.MAX_WATER)
        {
            return false;
        }

        //If terrains are the same, then allow it
        if(oldTerrain.getCode() == newTerrain.getCode())
        {
            return true;
        }

        //Handle water flow out
        switch(oldTerrain.getCode())
        {
            case STREAM_BED:
            return false;
        }

        return true;
    }


    public static boolean allowsMonsterToPass(Terrain newTerrain, Monster monster)
    {

        //blocking newTerrain parameter check
        if(TerrainManager.hasParameter(newTerrain, TerrainParameter.HAS_TREE)
        || TerrainManager.hasParameter(newTerrain, TerrainParameter.HAS_SIGN))
        {
            switch(monster.getMonsterCode())
            {
                case HUMAN: return false;
                case SPONGE: return false;
                case GHOST: return true;
                case SLIME: return false;
                case SMALL_SLIME: return false;
                case TINY_SLIME: return false;
                case DEER: return false;
                case PIG: return false;
                default: return false;
            }
        }

        //door newTerrain check
        if(TerrainManager.hasParameter(newTerrain, TerrainParameter.HAS_DOOR))
        {
            DoorCode doorCode = DoorCode.valueOf(TerrainManager.getParameter(newTerrain, TerrainManager.TerrainParameter.HAS_DOOR));
            if(doorCode == doorCode.CLOSED)
            {
                switch(monster.getMonsterCode())
                {
                    case HUMAN: return false;
                    case SPONGE: return false;
                    case GHOST: return true;
                    case SLIME: return false;
                    case SMALL_SLIME: return false;
                    case TINY_SLIME: return false;
                    case DEER: return false;
                    case PIG: return false;
                    default: return false;
                }
            }
        }

        //deep water check
        if(newTerrain.getWater() >= TerrainManager.DEEP_WATER)
        {
            switch(monster.getMonsterCode())
            {
                case GHOST: return false;
                case TINY_SLIME: return false;
                default: break; //do nothing
            }
        }
        
        
        //empty newTerrain check
        switch(newTerrain.getCode())
        {
            case PATH:
            switch(monster.getMonsterCode())
            {
                case HUMAN: return true;
                case SPONGE: return true;
                case GHOST: return true;
                case SLIME: return true;
                case SMALL_SLIME: return true;
                case TINY_SLIME: return true;
                case DEER: return true;
                case PIG: return true;
                default: return false;
            }

            case DIRT:
            switch(monster.getMonsterCode())
            {
                case HUMAN: return true;
                case SPONGE: return true;
                case GHOST: return true;
                case SLIME: return true;
                case SMALL_SLIME: return true;
                case TINY_SLIME: return true;
                case DEER: return true;
                case PIG: return true;
                default: return false;
            }

            case ROCK:
            switch(monster.getMonsterCode())
            {
                case HUMAN: return false;
                case SPONGE: return false;
                case GHOST: return true;
                case SLIME: return false;
                case SMALL_SLIME: return false;
                case TINY_SLIME: return false;
                case DEER: return false;
                case PIG: return false;
                default: return false;
            }

            case STREAM_BED:
            switch(monster.getMonsterCode())
            {
                case HUMAN: return true;
                case SPONGE: return true;
                case GHOST: return true;
                case SLIME: return true;
                case SMALL_SLIME: return false;
                case TINY_SLIME: return false;
                case DEER: return true;
                case PIG: return true;
                default: return false;
            }

            case GRASS:
            switch(monster.getMonsterCode())
            {
                case HUMAN: return true;
                case SPONGE: return true;
                case GHOST: return true;
                case SLIME: return true;
                case SMALL_SLIME: return true;
                case TINY_SLIME: return true;
                case DEER: return true;
                case PIG: return true;
                default: return false;
            }

            case STONE_WALL:
            switch(monster.getMonsterCode())
            {
                case HUMAN: return false;
                case SPONGE: return false;
                case GHOST: return true;
                case SLIME: return false;
                case SMALL_SLIME: return false;
                case TINY_SLIME: return false;
                case DEER: return false;
                case PIG: return false;
                default: return false;
            }

            case STONE_FLOOR:
            switch(monster.getMonsterCode())
            {
                case HUMAN: return true;
                case SPONGE: return true;
                case GHOST: return true;
                case SLIME: return true;
                case SMALL_SLIME: return true;
                case TINY_SLIME: return true;
                case DEER: return true;
                case PIG: return true;
                default: return false;
            }

            case FERTILE_LAND:
            switch(monster.getMonsterCode())
            {
                case HUMAN: return true;
                case SPONGE: return true;
                case GHOST: return true;
                case SLIME: return true;
                case SMALL_SLIME: return true;
                case TINY_SLIME: return true;
                case DEER: return true;
                case PIG: return true;
                default: return false;
            }
        }

        return false;
    }
    
}
