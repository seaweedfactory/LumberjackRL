﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Items;
using LumberjackRL.Core.Monsters;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Map
{

    public class TerrainManager
    {
        public const int DEEP_WATER=25;
        public const int DRAIN_RATE=200;
        public const int SPRING_RATE=250;
        public const double EVAPORATION_RATE= 0.50;
        public const int MAX_WATER = 2000;
        public const int WATER_CYCLE_COUNT = 100;
        public const int BASE_FLAME_RATE=3;
        public const double SPORE_LIGHT_LEVEL_MAX=0.80;
        public const double PLANT_LIGHT_LEVEL_MIN=0.70;
        public const int CLOVER_GROW_COUNT=100;
        public const int GRASS_GROW_COUNT=10000;
        public const int CORN_GROW_COUNT=1000;
        public const int PUMPKIN_GROW_COUNT=400;
        public const double MUSHROOM_GROW_RATE_WET_GROUND=0.02;
        public const double MUSHROOM_GROW_RATE_DRY_GROUND=0.001;
        public const double MUSHROOM_REMOVAL_RATE=0.02;
        public const double MUSHROOM_SPREAD_RATE=0.0025;
        public const double CLOVER_SPREAD_RATE=0.0025;
        public const double CROP_SPREAD_RATE=0.025;
        public const double PUMPKIN_PRODUCTION_RATE=0.01;
        public const double TREE_REGROWTH_RATE=0.01;
        public const double MOSS_GROW_RATE=0.0025;
        public const double MOSS_DROP_RATE=0.20;
        public const double GRAVE_ROB_CHANCE=0.50;
        public const double APPLE_TREE_PRODUCTION_RATE=0.25;
        public const double APPLE_TREE_FROM_APPLE_CHANCE=0.10;


        public static void removeParameters(Terrain terrain)
        {
            terrain.getParameters().Clear();
        }


        public static int getMonsterCount(Region region, MonsterClassType monsterCode)
        {
            int total = 0;
            foreach(Monster tempMonster in region.getMonsters())
            {
                if(tempMonster.monsterCode == monsterCode)
                {
                    total = total + 1;
                }
            }
            return total;
        }

        public static TreeCode getRandomTree()
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            int rnd = rng.RandomInteger(1000);
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
            RandomNumberGenerator rng = new RandomNumberGenerator();
            //moss is destroyed if stepped upon
            if (TerrainManager.hasParameter(region.getTerrain(x, y), TerrainParameter.HAS_MOSS))
            {
                region.getTerrain(x, y).getParameters().Remove(TerrainParameter.HAS_MOSS);

                if(rng.RandomDouble() < MOSS_DROP_RATE)
                {
                    Item moss = new Item();
                    moss.itemClass = ItemClassType.MOSS;
                    moss.setPosition(x, y);
                    moss.itemState = ItemState.GROUND;
                    region.getItems().Add(moss);
                }
            }
        }

        //Return the name of a seedType's vine or stalk
        public static String getStemType(SeedType seedType)
        {
            switch(seedType)
            {
                case SeedType.CORN:
                return "stalks";

                case SeedType.PUMPKIN:
                return "vines";

                default:
                return "stems";
            }
        }


        //Return the default grow count, plus a small variation
        public static int getGrowCount(SeedType seedType)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            if (seedType == SeedType.NULL)
            {
                return GRASS_GROW_COUNT + rng.RandomInteger((GRASS_GROW_COUNT / 10));
            }
            else
            {
                switch(seedType)
                {
                    case SeedType.CORN:
                    return CORN_GROW_COUNT + rng.RandomInteger((CORN_GROW_COUNT / 20));

                    case SeedType.PUMPKIN:
                    return PUMPKIN_GROW_COUNT + rng.RandomInteger((PUMPKIN_GROW_COUNT / 4));

                    default:
                    return 1;
                }
            }
        }

        //Does this crop spread?
        public static bool spreadingCrop(SeedType seedType)
        {
           switch(seedType)
           {
               case SeedType.CORN:
               return false;

               case SeedType.PUMPKIN:
               return true;

               default:
               return false;
           }
        }

        //Grow crops on this tile
        public static void growCrops(Terrain terrain, int x, int y, Quinoa quinoa)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            if (TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED))
            {
                if(quinoa.getLightMap().getCalc(x, y) >= TerrainManager.PLANT_LIGHT_LEVEL_MIN)
                {
                    SeedType seedType = (SeedType)Enum.Parse(typeof(SeedType), TerrainManager.getParameter(terrain, TerrainParameter.HAS_SEED));
                    switch(seedType)
                    {
                        case SeedType.CORN:
                        terrain.getParameters().Remove(TerrainParameter.HAS_SEED);
                        terrain.getParameters().Add(TerrainParameter.GROW_COUNTER, TerrainManager.GRASS_GROW_COUNT+"");

                        if(quinoa.getCurrentRegionHeader().getRegion().getItem(x, y) == null)
                        {
                            if(rng.RandomDouble() < 0.50)
                            {
                                Item cornSeed = new Item();
                                cornSeed.itemClass = ItemClassType.CORN_SEED;
                                cornSeed.stackSize = rng.RandomInteger(9) + 1;
                                cornSeed.setPosition(x, y);
                                quinoa.getCurrentRegionHeader().getRegion().getItems().Add(cornSeed);
                            }

                            Item corn = new Item();
                            corn.itemClass = ItemClassType.CORN;
                            corn.stackSize = rng.RandomInteger(3) + 1;
                            corn.setPosition(x, y);
                            quinoa.getCurrentRegionHeader().getRegion().getItems().Add(corn);
                        }
                        break;

                        case SeedType.PUMPKIN:
                        //reset the counter
                        terrain.getParameters().Add(TerrainParameter.GROW_COUNTER, (TerrainManager.PUMPKIN_GROW_COUNT / 2)+"");

                        //chance to produce a pumpkin
                        if(quinoa.getCurrentRegionHeader().getRegion().getItem(x, y) == null)
                        {
                            if(rng.RandomDouble() < TerrainManager.PUMPKIN_PRODUCTION_RATE)
                            {
                                Item pumpkinSeed = new Item();
                                pumpkinSeed.itemClass = ItemClassType.PUMPKIN_SEED;
                                pumpkinSeed.stackSize = rng.RandomInteger(3) + 1;
                                pumpkinSeed.setPosition(x, y);
                                quinoa.getCurrentRegionHeader().getRegion().getItems().Add(pumpkinSeed);

                                Item pumpkin = new Item();
                                pumpkin.itemClass = ItemClassType.PUMPKIN;
                                pumpkin.stackSize = 1;
                                pumpkin.setPosition(x, y);
                                quinoa.getCurrentRegionHeader().getRegion().getItems().Add(pumpkin);
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                //no seeds, just grow grass
                terrain.getParameters().Remove(TerrainParameter.GROW_COUNTER);
                terrain.setCode(TerrainCode.GRASS);

                if (rng.RandomDouble() < TerrainManager.TREE_REGROWTH_RATE)
                {
                    terrain.getParameters().Add(TerrainParameter.HAS_TREE, TerrainManager.getRandomTree().ToString());
                }
            }
        }

        public static int maxTreeDamage(TreeCode tc)
        {
            switch(tc)
            {
                case TreeCode.PINE_TREE:
                return 3;

                case TreeCode.BIRCH_TREE:
                return 2;

                case TreeCode.OAK_TREE:
                return 5;

                default:
                return 3;
            }
        }
    
        public static ItemClassType mushroomSporeToItemClass(MushroomSporeCode msc)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            List<ItemClassType> choices = new List<ItemClassType>();
            switch(msc)
            {
                case MushroomSporeCode.DEATH_CAP:
                return ItemClassType.DEATH_CAP;

                case MushroomSporeCode.PUFFBALL:
                return ItemClassType.PUFFBALL;

                case MushroomSporeCode.FLY_AGARIC:
                return ItemClassType.FLY_AGARIC;

                case MushroomSporeCode.MOREL:
                return ItemClassType.MOREL;

                case MushroomSporeCode.BUTTON_MUSHROOM:
                return ItemClassType.BUTTON_MUSHROOM;

                case MushroomSporeCode.GHOST_FUNGUS:
                return ItemClassType.GHOST_FUNGUS;

                case MushroomSporeCode.POISON:
                choices.Add(ItemClassType.DEATH_CAP);
                choices.Add(ItemClassType.FLY_AGARIC);
                choices.Add(ItemClassType.GHOST_FUNGUS);
                return choices[rng.RandomInteger(choices.Count)];

                case MushroomSporeCode.EDIBLE:
                default:
                choices.Add(ItemClassType.PUFFBALL);
                choices.Add(ItemClassType.MOREL);
                choices.Add(ItemClassType.BUTTON_MUSHROOM);
                return choices[rng.RandomInteger(choices.Count)];
            }
        }



        public static bool growable(Terrain terrain, int x, int y, LightMap lightMap, TerrainParameter thingToGrow)
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
                case TerrainParameter.HAS_SEED:
                if(terrain.getCode() == TerrainCode.FERTILE_LAND
                && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED)
                && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS))
                {
                    return true;
                }
                return false;

                //mushrooms
                case TerrainParameter.HAS_MUSHROOM_SPORES:
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
                case TerrainParameter.HAS_CLOVER:
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
                case TerrainParameter.HAS_MOSS:
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

        public static Item containsItem(ItemClassType itemClass, int x, int y, Quinoa quinoa)
        {
            foreach(Item tempItem in quinoa.getCurrentRegionHeader().getRegion().getItems())
            {
                if(tempItem.itemClass == itemClass
                && tempItem.x == x && tempItem.y == y)
                {
                    return tempItem;
                }
            }
            return null;
        }

        public static bool hasParameter(Terrain terrain, TerrainParameter parameter)
        {
            if (terrain == null || !terrain.getParameters().ContainsKey(parameter))
            {
                return false;
            }
            return terrain.getParameters()[parameter] != null;
        }

        public static String getParameter(Terrain terrain, TerrainParameter parameter)
        {
            Object returned = terrain.getParameters()[parameter];
            if(returned != null)
            {
                return (String)returned;
            }
            return null;
        }

        public static int fuelsFire(Terrain terrain, int x, int y, Quinoa quinoa)
        {
            if(containsItem(ItemClassType.LOG, x, y, quinoa) != null)
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

        public static bool wetable(Terrain terrain, int x, int y, Quinoa quinoa)
        {
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR))
            {
                DoorCode doorCode = (DoorCode)Enum.Parse(typeof(DoorCode),TerrainManager.getParameter(terrain, TerrainParameter.HAS_DOOR));
                return (doorCode == DoorCode.OPEN);
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_FLOODGATE))
            {
                DoorCode doorCode = (DoorCode)Enum.Parse(typeof(DoorCode), TerrainManager.getParameter(terrain, TerrainParameter.HAS_FLOODGATE));
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
                case TerrainCode.PATH:
                case TerrainCode.DIRT:
                case TerrainCode.GRASS:
                case TerrainCode.STONE_FLOOR:
                case TerrainCode.FERTILE_LAND:
                case TerrainCode.STREAM_BED:
                return true;


                case TerrainCode.STONE_WALL:
                case TerrainCode.ROCK:
                default:
                return false;
            }
        }


        public static bool mineable(Terrain terrain)
        {
            switch(terrain.getCode())
            {
                case TerrainCode.ROCK:
                case TerrainCode.STONE_FLOOR:
                case TerrainCode.STREAM_BED:
                return true;

                default:
                return false;
            }
        }


        public static bool diggable(Terrain terrain)
        {
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE)
            || TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR)
            || TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN))
            {
                return false;
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
            {
                GraveCode gc = (GraveCode)Enum.Parse(typeof(GraveCode), TerrainManager.getParameter(terrain, TerrainParameter.HAS_GRAVE));
                return (gc != GraveCode.BROKEN);
            }

            switch(terrain.getCode())
            {
                case TerrainCode.PATH:
                case TerrainCode.STONE_WALL:
                case TerrainCode.STONE_FLOOR:
                case TerrainCode.FERTILE_LAND:
                case TerrainCode.ROCK:
                default:
                return false;

                case TerrainCode.GRASS:
                case TerrainCode.DIRT:
                case TerrainCode.STREAM_BED:
                return true;
            }
        }


        public static void mine(Terrain terrain, int x, int y, Quinoa quinoa)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            switch (terrain.getCode())
            {
                case TerrainCode.ROCK:
                terrain.setCode(TerrainCode.STONE_FLOOR);

                if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MINERALS))
                {
                    int mineralCode = Int32.Parse(TerrainManager.getParameter(terrain, TerrainParameter.HAS_MINERALS));
                    switch(mineralCode)
                    {
                        case 1:
                        if(rng.RandomDouble() > 0.5)
                        {
                            Item gem = new Item();
                            gem.itemClass = ItemClassType.AMETHYST;
                            gem.setPosition(x, y);
                            quinoa.getCurrentRegionHeader().getRegion().getItems().Add(gem);
                        }
                        else
                        {
                            //do nothing
                        }
                        break;

                        case 2:
                        if(rng.RandomDouble() > 0.5)
                        {
                            Item gem = new Item();
                            gem.itemClass = ItemClassType.SAPPHIRE;
                            gem.setPosition(x, y);
                            quinoa.getCurrentRegionHeader().getRegion().getItems().Add(gem);
                        }
                        else
                        {
                            Item gem = new Item();
                            gem.itemClass = ItemClassType.EMERALD;
                            gem.setPosition(x, y);
                            quinoa.getCurrentRegionHeader().getRegion().getItems().Add(gem);
                        }
                        break;

                        case 3:
                        if(rng.RandomDouble() > 0.33)
                        {
                            Item gem = new Item();
                            gem.itemClass = ItemClassType.RUBY;
                            gem.setPosition(x, y);
                            quinoa.getCurrentRegionHeader().getRegion().getItems().Add(gem);
                        }
                        else
                        {
                            Item gem = new Item();
                            gem.itemClass = ItemClassType.DIAMOND;
                            gem.setPosition(x, y);
                            quinoa.getCurrentRegionHeader().getRegion().getItems().Add(gem);
                        }
                        break;

                    }
                }

                break;


                case TerrainCode.STONE_FLOOR:
                terrain.setCode(TerrainCode.STREAM_BED);
                break;

                case TerrainCode.STREAM_BED:
                if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DRAIN))
                {
                    terrain.getParameters().Remove(TerrainParameter.HAS_DRAIN);
                }
                else
                {
                    terrain.getParameters().Add(TerrainParameter.HAS_DRAIN, TerrainManager.DRAIN_RATE+"");
                }
                break;


                default:
                //do nothing
                break;
            }
        }


        public static void dig(Terrain terrain, int x, int y, Quinoa quinoa)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            //check for graves
            if (TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
            {
                GraveCode gc = (GraveCode)Enum.Parse(typeof(GraveCode), TerrainManager.getParameter(terrain, TerrainParameter.HAS_GRAVE));
                if(gc == GraveCode.NORMAL && rng.RandomDouble() < TerrainManager.GRAVE_ROB_CHANCE)
                {
                    Item reward = ItemManager.getTreasure(rng.RandomDouble());
                    reward.setPosition(x, y);
                    quinoa.getCurrentRegionHeader().getRegion().getItems().Add(reward);
                }

                //open the grave
                terrain.getParameters().Add(TerrainParameter.HAS_GRAVE, GraveCode.BROKEN.ToString());

                //drop bones
                Item bones = new Item();
                bones.itemClass = ItemClassType.BONES;
                bones.setPosition(x, y);
                bones.itemState = ItemState.GROUND;
                quinoa.getCurrentRegionHeader().getRegion().getItems().Add(bones);

                return;
            }

            switch(terrain.getCode())
            {
                case TerrainCode.GRASS:
                terrain.setCode(TerrainCode.DIRT);
                break;

                case TerrainCode.DIRT:
                terrain.setCode(TerrainCode.STONE_FLOOR);
                break;

                case TerrainCode.STREAM_BED:
                if(!TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DRAIN))
                {
                    if(!TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SPRING))
                    {
                        terrain.getParameters().Add(TerrainParameter.HAS_SPRING, TerrainManager.SPRING_RATE+"");
                    }
                    else
                    {
                        terrain.getParameters().Remove(TerrainParameter.HAS_SPRING);
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
            RandomNumberGenerator rng = new RandomNumberGenerator();

            bool dropAsh = false;

            //Look for items on this squares and remove them
            foreach(Item tempItem in quinoa.getCurrentRegionHeader().getRegion().getItems())
            {
                if(tempItem.x == x && tempItem.y == y)
                {
                    switch(tempItem.itemClass)
                    {
                        case ItemClassType.LOG: dropAsh=true; break;
                        default: dropAsh= false; break;
                    }
                    tempItem.RemoveObject();
                }
            }

            //Examine terrain features
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE))
            {
                terrain.getParameters().Remove(TerrainParameter.HAS_TREE);
                if(rng.RandomDouble() < 0.125)
                {
                    dropAsh = true;
                }
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR))
            {
                terrain.getParameters().Remove(TerrainParameter.HAS_DOOR);
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN))
            {
                terrain.getParameters().Remove(TerrainParameter.HAS_SIGN);
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES))
            {
                terrain.getParameters().Remove(TerrainParameter.HAS_MUSHROOM_SPORES);
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_CLOVER))
            {
                terrain.getParameters().Remove(TerrainParameter.HAS_CLOVER);
            }

            switch(terrain.getCode())
            {
                case TerrainCode.GRASS:
                if(rng.RandomDouble() < 0.98)
                {
                    terrain.setCode(TerrainCode.FERTILE_LAND);
                    terrain.getParameters().Add(TerrainParameter.GROW_COUNTER, TerrainManager.GRASS_GROW_COUNT+"");


                    if(rng.RandomDouble() < 0.001)
                    {
                        int cloverCount = (int)(rng.RandomDouble() * (TerrainManager.CLOVER_GROW_COUNT / 4)) + TerrainManager.CLOVER_GROW_COUNT;
                        terrain.getParameters().Add(TerrainParameter.HAS_CLOVER, cloverCount+"");
                    }
                    else
                    {
                        if(rng.RandomDouble() < 0.001)
                        {
                            MushroomSporeCode msc = EnumUtil.RandomEnumValue<MushroomSporeCode>();
                            terrain.getParameters().Add(TerrainParameter.HAS_MUSHROOM_SPORES, msc.ToString());
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
                ash.itemClass = ItemClassType.ASH;
                ash.setPosition(x, y);
                quinoa.getCurrentRegionHeader().getRegion().getItems().Add(ash);
            }
        }

        public static bool flammable(Terrain terrain, int x, int y, Quinoa quinoa)
        {
            //See if the area is wet
            if(terrain.getWater() > 0)
            {
                return false;
            }

            if(containsItem(ItemClassType.LOG, x, y, quinoa) != null)
            {
                return true;
            }

            switch(terrain.getCode())
            {
                case TerrainCode.PATH:
                case TerrainCode.DIRT:
                case TerrainCode.STONE_WALL:
                case TerrainCode.STONE_FLOOR:
                case TerrainCode.FERTILE_LAND:
                case TerrainCode.ROCK:
                case TerrainCode.STREAM_BED:
                default:
                return false;

                case TerrainCode.GRASS:
                return true;
            }
        }

        public static bool transparent(Terrain terrain)
        {
            if(hasParameter(terrain, TerrainParameter.HAS_TREE))
            {
                return false;
            }

            if(hasParameter(terrain, TerrainParameter.HAS_DOOR))
            {
                DoorCode doorCode = (DoorCode)Enum.Parse(typeof(DoorCode), TerrainManager.getParameter(terrain, TerrainParameter.HAS_DOOR));
                return (doorCode == DoorCode.OPEN);
            }

            switch(terrain.getCode())
            {
                case TerrainCode.PATH:
                case TerrainCode.DIRT:
                case TerrainCode.GRASS:
                case TerrainCode.STONE_FLOOR:
                case TerrainCode.FERTILE_LAND:
                case TerrainCode.STREAM_BED:
                default:
                return true;

                case TerrainCode.STONE_WALL:
                case TerrainCode.ROCK:
                return false;
            }
        }


        public static bool allowWaterToFlowBetween(Terrain oldTerrain, Terrain newTerrain)
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
                case TerrainCode.STREAM_BED:
                return false;
            }

            return true;
        }


        public static bool allowsMonsterToPass(Terrain newTerrain, Monster monster)
        {

            //blocking newTerrain parameter check
            if(TerrainManager.hasParameter(newTerrain, TerrainParameter.HAS_TREE)
            || TerrainManager.hasParameter(newTerrain, TerrainParameter.HAS_SIGN))
            {
                switch(monster.monsterCode)
                {
                    case MonsterClassType.HUMAN: return false;
                    case MonsterClassType.SPONGE: return false;
                    case MonsterClassType.GHOST: return true;
                    case MonsterClassType.SLIME: return false;
                    case MonsterClassType.SMALL_SLIME: return false;
                    case MonsterClassType.TINY_SLIME: return false;
                    case MonsterClassType.DEER: return false;
                    case MonsterClassType.PIG: return false;
                    default: return false;
                }
            }

            //door newTerrain check
            if(TerrainManager.hasParameter(newTerrain, TerrainParameter.HAS_DOOR))
            {
                DoorCode doorCode = (DoorCode)Enum.Parse(typeof(DoorCode), TerrainManager.getParameter(newTerrain, TerrainParameter.HAS_DOOR));
                if(doorCode == DoorCode.CLOSED)
                {
                    switch(monster.monsterCode)
                    {
                        case MonsterClassType.HUMAN: return false;
                        case MonsterClassType.SPONGE: return false;
                        case MonsterClassType.GHOST: return true;
                        case MonsterClassType.SLIME: return false;
                        case MonsterClassType.SMALL_SLIME: return false;
                        case MonsterClassType.TINY_SLIME: return false;
                        case MonsterClassType.DEER: return false;
                        case MonsterClassType.PIG: return false;
                        default: return false;
                    }
                }
            }

            //deep water check
            if(newTerrain.getWater() >= TerrainManager.DEEP_WATER)
            {
                switch(monster.monsterCode)
                {
                    case MonsterClassType.GHOST: return false;
                    case MonsterClassType.TINY_SLIME: return false;
                    default: break; //do nothing
                }
            }
        
        
            //empty newTerrain check
            switch(newTerrain.getCode())
            {
                case TerrainCode.PATH:
                switch (monster.monsterCode)
                {
                    case MonsterClassType.HUMAN: return true;
                    case MonsterClassType.SPONGE: return true;
                    case MonsterClassType.GHOST: return true;
                    case MonsterClassType.SLIME: return true;
                    case MonsterClassType.SMALL_SLIME: return true;
                    case MonsterClassType.TINY_SLIME: return true;
                    case MonsterClassType.DEER: return true;
                    case MonsterClassType.PIG: return true;
                    default: return false;
                }

                case TerrainCode.DIRT:
                switch (monster.monsterCode)
                {
                    case MonsterClassType.HUMAN: return true;
                    case MonsterClassType.SPONGE: return true;
                    case MonsterClassType.GHOST: return true;
                    case MonsterClassType.SLIME: return true;
                    case MonsterClassType.SMALL_SLIME: return true;
                    case MonsterClassType.TINY_SLIME: return true;
                    case MonsterClassType.DEER: return true;
                    case MonsterClassType.PIG: return true;
                    default: return false;
                }

                case TerrainCode.ROCK:
                switch (monster.monsterCode)
                {
                    case MonsterClassType.HUMAN: return false;
                    case MonsterClassType.SPONGE: return false;
                    case MonsterClassType.GHOST: return true;
                    case MonsterClassType.SLIME: return false;
                    case MonsterClassType.SMALL_SLIME: return false;
                    case MonsterClassType.TINY_SLIME: return false;
                    case MonsterClassType.DEER: return false;
                    case MonsterClassType.PIG: return false;
                    default: return false;
                }

                case TerrainCode.STREAM_BED:
                switch (monster.monsterCode)
                {
                    case MonsterClassType.HUMAN: return true;
                    case MonsterClassType.SPONGE: return true;
                    case MonsterClassType.GHOST: return true;
                    case MonsterClassType.SLIME: return true;
                    case MonsterClassType.SMALL_SLIME: return false;
                    case MonsterClassType.TINY_SLIME: return false;
                    case MonsterClassType.DEER: return true;
                    case MonsterClassType.PIG: return true;
                    default: return false;
                }

                case TerrainCode.GRASS:
                switch (monster.monsterCode)
                {
                    case MonsterClassType.HUMAN: return true;
                    case MonsterClassType.SPONGE: return true;
                    case MonsterClassType.GHOST: return true;
                    case MonsterClassType.SLIME: return true;
                    case MonsterClassType.SMALL_SLIME: return true;
                    case MonsterClassType.TINY_SLIME: return true;
                    case MonsterClassType.DEER: return true;
                    case MonsterClassType.PIG: return true;
                    default: return false;
                }

                case TerrainCode.STONE_WALL:
                switch (monster.monsterCode)
                {
                    case MonsterClassType.HUMAN: return false;
                    case MonsterClassType.SPONGE: return false;
                    case MonsterClassType.GHOST: return true;
                    case MonsterClassType.SLIME: return false;
                    case MonsterClassType.SMALL_SLIME: return false;
                    case MonsterClassType.TINY_SLIME: return false;
                    case MonsterClassType.DEER: return false;
                    case MonsterClassType.PIG: return false;
                    default: return false;
                }

                case TerrainCode.STONE_FLOOR:
                switch (monster.monsterCode)
                {
                    case MonsterClassType.HUMAN: return true;
                    case MonsterClassType.SPONGE: return true;
                    case MonsterClassType.GHOST: return true;
                    case MonsterClassType.SLIME: return true;
                    case MonsterClassType.SMALL_SLIME: return true;
                    case MonsterClassType.TINY_SLIME: return true;
                    case MonsterClassType.DEER: return true;
                    case MonsterClassType.PIG: return true;
                    default: return false;
                }

                case TerrainCode.FERTILE_LAND:
                switch (monster.monsterCode)
                {
                    case MonsterClassType.HUMAN: return true;
                    case MonsterClassType.SPONGE: return true;
                    case MonsterClassType.GHOST: return true;
                    case MonsterClassType.SLIME: return true;
                    case MonsterClassType.SMALL_SLIME: return true;
                    case MonsterClassType.TINY_SLIME: return true;
                    case MonsterClassType.DEER: return true;
                    case MonsterClassType.PIG: return true;
                    default: return false;
                }
            }

            return false;
        }
    }
}
