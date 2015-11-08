using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;
using System.Drawing;
using LumberjackRL.Core.Items;
using LumberjackRL.Core.Utilities;
using LumberjackRL.Core.Map;

namespace LumberjackRL.Core.UI
{
    public class DrawManager
    {
        public void drawMonster(Monster monster, Graphics g, int x, int y, GraphicsManager gm)
        {
            switch(monster.monsterCode)
            {
                case MonsterCode.HUMAN:
                switch(monster.role)
                {
                    case MonsterRole.NULL:
                    g.DrawImage(gm.getImage(GraphicsManager.LUMBERJACK, 0), x, y);
                    break;

                    case MonsterRole.CHEF:
                    g.DrawImage(gm.getImage(GraphicsManager.LUMBERJACK, 1), x, y);
                    break;

                    case MonsterRole.HANDYMAN:
                    g.DrawImage(gm.getImage(GraphicsManager.LUMBERJACK, 2), x, y);
                    break;

                    case MonsterRole.BANKER:
                    g.DrawImage(gm.getImage(GraphicsManager.LUMBERJACK, 3), x, y);
                    break;

                    case MonsterRole.BROTHER:
                    g.DrawImage(gm.getImage(GraphicsManager.LUMBERJACK, 0), x, y);
                    break;

                    default:
                    g.DrawImage(gm.getImage(GraphicsManager.LUMBERJACK, 0), x, y);
                    break;


                }
                break;

                case MonsterCode.SPONGE:
                g.DrawImage(gm.getImage(GraphicsManager.SPONGE), x, y);
                break;

                case MonsterCode.GHOST:
                g.DrawImage(gm.getImage(GraphicsManager.GHOST), x, y);
                break;

                case MonsterCode.SLIME:
                g.DrawImage(gm.getImage(GraphicsManager.SLIME, 0), x, y);
                break;

                case MonsterCode.SMALL_SLIME:
                g.DrawImage(gm.getImage(GraphicsManager.SLIME, 1), x, y);
                break;

                case MonsterCode.TINY_SLIME:
                g.DrawImage(gm.getImage(GraphicsManager.SLIME, 2), x, y);
                break;

                case MonsterCode.DEER:
                if(monster.isSleeping())
                {
                    g.DrawImage(gm.getImage(GraphicsManager.DEER, 1), x, y);
                }
                else
                {
                    g.DrawImage(gm.getImage(GraphicsManager.DEER, 0), x, y);
                }
                break;

                case MonsterCode.PIG:
                if(monster.isSleeping())
                {
                    g.DrawImage(gm.getImage(GraphicsManager.PIG, 1), x, y);
                }
                else
                {
                    g.DrawImage(gm.getImage(GraphicsManager.PIG, 0), x, y);
                }
                break;
            }
        }

        public void drawItem(Item item, Graphics g, int x, int y, double scale, GraphicsManager gm)
        {
            drawItem(item,g,x,y,scale,gm,true);
        }

        public void drawItem(Item item, Graphics g, int x, int y, double scale, GraphicsManager gm, bool drawCount)
        {
            switch(item.itemClass)
            {
                case ItemClass.BONES:
                g.DrawImage(gm.getImage(GraphicsManager.BONES), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.MOP:
                g.DrawImage(gm.getImage(GraphicsManager.MOP), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.MOSS:
                g.DrawImage(gm.getImage(GraphicsManager.MOSS, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.FLOODGATE:
                g.DrawImage(gm.getImage(GraphicsManager.FLOODGATE, 0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.DEATH_CAP:
                g.DrawImage(gm.getImage(GraphicsManager.MUSHROOM, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.PUFFBALL:
                g.DrawImage(gm.getImage(GraphicsManager.MUSHROOM, 2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.FLY_AGARIC:
                g.DrawImage(gm.getImage(GraphicsManager.MUSHROOM, 3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.MOREL:
                g.DrawImage(gm.getImage(GraphicsManager.MUSHROOM, 4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.BUTTON_MUSHROOM:
                g.DrawImage(gm.getImage(GraphicsManager.MUSHROOM, 5), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.GHOST_FUNGUS:
                g.DrawImage(gm.getImage(GraphicsManager.MUSHROOM, 6), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.APPLE:
                g.DrawImage(gm.getImage(GraphicsManager.APPLE), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.CORN:
                g.DrawImage(gm.getImage(GraphicsManager.CORN), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.CORN_SEED:
                g.DrawImage(gm.getImage(GraphicsManager.CORN, 4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.PUMPKIN:
                g.DrawImage(gm.getImage(GraphicsManager.PUMPKIN), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.PUMPKIN_SEED:
                g.DrawImage(gm.getImage(GraphicsManager.PUMPKIN, 4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.BACON:
                if(item.stackSize == 1)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.BACON,0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize == 2)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.BACON,1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize == 3)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.BACON,2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize >= 4)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.BACON,3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                break;

                case ItemClass.FLAPJACKS:
                if(item.stackSize == 1)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.FLAPJACKS,0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize == 2)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.FLAPJACKS,1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize == 3)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.FLAPJACKS,2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize >= 4)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.FLAPJACKS,3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                break;

                case ItemClass.KEY:
                g.DrawImage(gm.getImage(GraphicsManager.KEY), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.AXE:
                g.DrawImage(gm.getImage(GraphicsManager.AXE), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.PICKAXE:
                g.DrawImage(gm.getImage(GraphicsManager.PICKAXE), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.COINS:
                if(item.stackSize == 1)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.COINS,0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize == 2)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.COINS,1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize == 3)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.COINS,2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize > 3 && item.stackSize < 50)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.COINS,3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(item.stackSize >= 50)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.COINS,4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                break;

                case ItemClass.JACKET:
                g.DrawImage(gm.getImage(GraphicsManager.JACKET), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.HAT:
                g.DrawImage(gm.getImage(GraphicsManager.HAT), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.BOOTS:
                g.DrawImage(gm.getImage(GraphicsManager.BOOTS), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.LOG:
                g.DrawImage(gm.getImage(GraphicsManager.LOG), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.ASH:
                g.DrawImage(gm.getImage(GraphicsManager.ASH), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.LANTERN:
                g.DrawImage(gm.getImage(GraphicsManager.LANTERN), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.BUCKET:
                g.DrawImage(gm.getImage(GraphicsManager.BUCKET, 0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.WATER_BUCKET:
                g.DrawImage(gm.getImage(GraphicsManager.BUCKET, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.SHOVEL:
                g.DrawImage(gm.getImage(GraphicsManager.SHOVEL), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.TORCH:
                if(RandomNumber.RandomDouble() < 0.33)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.TORCH, 0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else if(RandomNumber.RandomDouble() < 0.66)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.TORCH, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                else
                {
                    g.DrawImage(gm.getImage(GraphicsManager.TORCH, 2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                }
                break;

                case ItemClass.SAPPHIRE:
                g.DrawImage(gm.getImage(GraphicsManager.GEM, 0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.EMERALD:
                g.DrawImage(gm.getImage(GraphicsManager.GEM, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.RUBY:
                g.DrawImage(gm.getImage(GraphicsManager.GEM, 2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.AMETHYST:
                g.DrawImage(gm.getImage(GraphicsManager.GEM, 3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.DIAMOND:
                g.DrawImage(gm.getImage(GraphicsManager.GEM, 4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                case ItemClass.TENT:
                g.DrawImage(gm.getImage(GraphicsManager.BED, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                break;

                default:
                    //Use log as a default image
                    g.DrawImage(gm.getImage(GraphicsManager.LOG), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale));
                    break;
            }

            if(item.stackSize > 1 && drawCount)
            {
                int countXOffset = 16;
                if((item.stackSize + "").Length == 2)
                {
                    countXOffset = 11;
                }
                else if((item.stackSize + "").Length > 2)
                {
                    countXOffset = 6;
                }
                this.drawString(item.stackSize + "", 1.0, g, countXOffset + x, 16 + y, gm);
            }
        }

        public void drawTerrain(Terrain terrain, Graphics g, int x, int y, GraphicsManager gm)
        {

            //for null terrain
            if(terrain == null)
            {
                //Don't draw null terrain
                //g.setColor(new Color(10,10,10));
                //g.fillRect(x, y, 16, 16);
                return;
            }

            switch(terrain.getCode())
            {
                case TerrainCode.FERTILE_LAND:
                if(TerrainManager.hasParameter(terrain, TerrainParameter.GROW_COUNTER))
                {
                    int growCount = Int32.Parse(TerrainManager.getParameter(terrain, TerrainParameter.GROW_COUNTER));

                    if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED))
                    {
                        SeedType seedType = (SeedType)Enum.Parse(typeof(SeedType), TerrainManager.getParameter(terrain, TerrainParameter.HAS_SEED));
                        g.DrawImage(gm.getImage(GraphicsManager.DIRT, 2), x, y);

                        switch(seedType)
                        {
                            case SeedType.CORN:
                            if(growCount > (TerrainManager.CORN_GROW_COUNT * 0.66))
                            {
                                g.DrawImage(gm.getImage(GraphicsManager.CORN, 1), x, y);
                            }
                            else if(growCount > (TerrainManager.CORN_GROW_COUNT * 0.33))
                            {
                                g.DrawImage(gm.getImage(GraphicsManager.CORN, 2), x, y);
                            }
                            else if(growCount <= (TerrainManager.CORN_GROW_COUNT * 0.33))
                            {
                                g.DrawImage(gm.getImage(GraphicsManager.CORN, 3), x, y);
                            }
                            break;

                            case SeedType.PUMPKIN:
                            if(growCount > (TerrainManager.PUMPKIN_GROW_COUNT * 0.66))
                            {
                                g.DrawImage(gm.getImage(GraphicsManager.PUMPKIN, 1), x, y);
                            }
                            else if(growCount > (TerrainManager.PUMPKIN_GROW_COUNT * 0.33))
                            {
                                g.DrawImage(gm.getImage(GraphicsManager.PUMPKIN, 2), x, y);
                            }
                            else if(growCount <= (TerrainManager.PUMPKIN_GROW_COUNT * 0.33))
                            {
                                g.DrawImage(gm.getImage(GraphicsManager.PUMPKIN, 3), x, y);
                            }
                            break;
                        }
                    }
                    else
                    {
                        //default growing series
                        if(growCount > (TerrainManager.GRASS_GROW_COUNT * 0.80))
                        {
                            g.DrawImage(gm.getImage(GraphicsManager.DIRT, 2), x, y);
                        }
                        else if(growCount > (TerrainManager.GRASS_GROW_COUNT * 0.60))
                        {
                            g.DrawImage(gm.getImage(GraphicsManager.DIRT, 3), x, y);
                        }
                        else if(growCount > (TerrainManager.GRASS_GROW_COUNT * 0.40))
                        {
                            g.DrawImage(gm.getImage(GraphicsManager.DIRT, 4), x, y);
                        }
                        else if(growCount > (TerrainManager.GRASS_GROW_COUNT * 0.20))
                        {
                            g.DrawImage(gm.getImage(GraphicsManager.DIRT, 5), x, y);
                        }
                        else if(growCount <= (TerrainManager.GRASS_GROW_COUNT * 0.20))
                        {
                            g.DrawImage(gm.getImage(GraphicsManager.DIRT, 6), x, y);
                        }
                    }
                }
                else
                {
                    g.DrawImage(gm.getImage(GraphicsManager.DIRT, 2), x, y);
                }
                break;

                case TerrainCode.PATH:
                g.DrawImage(gm.getImage(GraphicsManager.DIRT, 1), x, y);
                break;

                case TerrainCode.DIRT:
                g.DrawImage(gm.getImage(GraphicsManager.DIRT), x, y);
                break;

                case TerrainCode.GRASS:
                g.DrawImage(gm.getImage(GraphicsManager.GRASS), x, y);
                break;

                case TerrainCode.STONE_WALL:
                g.DrawImage(gm.getImage(GraphicsManager.STONE_WALL), x, y);
                break;

                case TerrainCode.STONE_FLOOR:
                g.DrawImage(gm.getImage(GraphicsManager.STONE_FLOOR), x, y);
                break;

                case TerrainCode.ROCK:
                if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MINERALS))
                {
                    int mineralIndex = Int32.Parse(TerrainManager.getParameter(terrain, TerrainParameter.HAS_MINERALS));
                    g.DrawImage(gm.getImage(GraphicsManager.ROCKFACE, mineralIndex), x, y);
                }
                else
                {
                    g.DrawImage(gm.getImage(GraphicsManager.ROCKFACE, 0), x, y);
                }
                break;

                case TerrainCode.STREAM_BED:
                g.DrawImage(gm.getImage(GraphicsManager.STONE_FLOOR, 1), x, y);
                break;

            }

        
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR))
            {
                DoorCode doorCode = (DoorCode)Enum.Parse(typeof(DoorCode),TerrainManager.getParameter(terrain, TerrainParameter.HAS_DOOR));
                switch(doorCode)
                {
                    case DoorCode.OPEN:
                    default:
                    g.DrawImage(gm.getImage(GraphicsManager.DOOR, 0), x, y);
                    break;

                    case DoorCode.CLOSED:
                    g.DrawImage(gm.getImage(GraphicsManager.DOOR, 1), x, y);
                    break;
                }
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN))
            {
                switch(terrain.getCode())
                {
                    case TerrainCode.STONE_WALL:
                    case TerrainCode.ROCK:
                    g.DrawImage(gm.getImage(GraphicsManager.SIGN, 0), x, y);
                    break;

                    default:
                    g.DrawImage(gm.getImage(GraphicsManager.SIGN, 1), x, y);
                    break;
                }
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.FIRE))
            {
                g.DrawImage(gm.getImage(GraphicsManager.FIRE, RandomNumber.RandomInteger(3)), x, y);
            }

        
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_BED))
            {
                g.DrawImage(gm.getImage(GraphicsManager.BED), x, y);
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS))
            {
                g.DrawImage(gm.getImage(GraphicsManager.MOSS), x, y);
            }

            if(terrain.getWater() > 0)
            {
                int waterValue = terrain.getWater();
                if(waterValue > 0 && waterValue < (int)(TerrainManager.DEEP_WATER * 0.10))
                {
                    g.DrawImage(gm.getImage(GraphicsManager.WATER, 0), x, y);
                }
                else if(waterValue < (int)(TerrainManager.DEEP_WATER * 0.10))
                {
                    g.DrawImage(gm.getImage(GraphicsManager.WATER, 1), x, y);
                }
                else if(waterValue < (int)(TerrainManager.DEEP_WATER * 0.50))
                {
                    g.DrawImage(gm.getImage(GraphicsManager.WATER, 2), x, y);
                }
                else if(waterValue < TerrainManager.DEEP_WATER)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.WATER, 3), x, y);
                }
                else if(waterValue >= TerrainManager.DEEP_WATER)
                {
                    if(RandomNumber.RandomDouble() > 0.5)
                    {
                        g.DrawImage(gm.getImage(GraphicsManager.WATER, 4), x, y);
                    }
                    else
                    {
                        g.DrawImage(gm.getImage(GraphicsManager.WATER, 5), x, y);
                    }
                }        
            }


            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SPRING))
            {
                int springFrame = RandomNumber.RandomInteger(3);
                g.DrawImage(gm.getImage(GraphicsManager.WATER, 6 + springFrame), x, y);
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DRAIN))
            {
                g.DrawImage(gm.getImage(GraphicsManager.WATER, 9), x, y);
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_FLOODGATE))
            {
                DoorCode doorCode = (DoorCode)Enum.Parse(typeof(DoorCode), TerrainManager.getParameter(terrain, TerrainParameter.HAS_FLOODGATE));
                switch(doorCode)
                {
                    case DoorCode.OPEN:
                    default:
                    g.DrawImage(gm.getImage(GraphicsManager.FLOODGATE, 1), x, y);
                    break;

                    case DoorCode.CLOSED:
                    g.DrawImage(gm.getImage(GraphicsManager.FLOODGATE, 0), x, y);
                    break;
                }
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
            {
                GraveCode gc = (GraveCode)Enum.Parse(typeof(GraveCode),TerrainManager.getParameter(terrain, TerrainParameter.HAS_GRAVE));
                switch(gc)
                {
                    case GraveCode.NORMAL:
                    g.DrawImage(gm.getImage(GraphicsManager.GRAVE, 0), x, y);
                    break;

                    case GraveCode.SPECIAL:
                    g.DrawImage(gm.getImage(GraphicsManager.GRAVE, 1), x, y);
                    break;

                    case GraveCode.BROKEN:
                    g.DrawImage(gm.getImage(GraphicsManager.GRAVE, 2), x, y);
                    break;
                }
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE))
            {
                TreeCode tc = (TreeCode)Enum.Parse(typeof(TreeCode), TerrainManager.getParameter(terrain, TerrainParameter.HAS_TREE));
                switch(tc)
                {
                    case TreeCode.PINE_TREE:
                    g.DrawImage(gm.getImage(GraphicsManager.TREE, 0), x, y);
                    break;

                    case TreeCode.BIRCH_TREE:
                    g.DrawImage(gm.getImage(GraphicsManager.TREE, 2), x, y);
                    g.DrawImage(gm.getImage(GraphicsManager.TREE, 3), x, y);
                    break;

                    case TreeCode.OAK_TREE:
                    g.DrawImage(gm.getImage(GraphicsManager.TREE, 1), x, y);
                    g.DrawImage(gm.getImage(GraphicsManager.TREE, 4), x, y);
                    break;

                    case TreeCode.APPLE_TREE:
                    g.DrawImage(gm.getImage(GraphicsManager.TREE, 5), x, y);
                    g.DrawImage(gm.getImage(GraphicsManager.TREE, 6), x, y);
                    break;
                
                    default:
                    g.DrawImage(gm.getImage(GraphicsManager.TREE), x, y);
                    break;
                }
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES))
            {
                g.DrawImage(gm.getImage(GraphicsManager.MUSHROOM, 0), x, y);
            }

            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_CLOVER))
            {
                g.DrawImage(gm.getImage(GraphicsManager.CLOVER, 0), x, y);

                int cloverCounter = Int32.Parse(TerrainManager.getParameter(terrain, TerrainParameter.HAS_CLOVER));
                if(cloverCounter == 0)
                {
                    g.DrawImage(gm.getImage(GraphicsManager.CLOVER, 1), x, y);
                }
            }
        }

        public void drawString(String tmpStr, double scale, Graphics g, int x, int y, GraphicsManager gm)
        {
            for (int i = 0; i < tmpStr.Length; i++)
            {
                Image letter = gm.getLetter(tmpStr.ToCharArray()[i]);
                if(letter != null)
                {
                    g.DrawImage(letter, x + ((int)(gm.getLetterWidth() * gm.getLetterSpacingPercent() * scale) * i), y, (int)(gm.getLetterWidth() * scale), (int)(gm.getLetterHeight() * scale));
                }
            }
         
        }
    }
}
