package quinoa.ui;

import java.awt.Color;
import java.awt.Graphics;
import java.awt.image.BufferedImage;
import quinoa.Quinoa;
import quinoa.items.Item;
import quinoa.monsters.Monster;
import quinoa.region.Terrain;
import quinoa.region.TerrainManager;
import quinoa.region.TerrainManager.DoorCode;
import quinoa.region.TerrainManager.GraveCode;
import quinoa.region.TerrainManager.SeedType;
import quinoa.region.TerrainManager.TerrainParameter;
import quinoa.region.TerrainManager.TreeCode;

public class DrawManager
{

    public void drawMonster(Monster monster, Graphics g, int x, int y, GraphicsManager gm)
    {
        switch(monster.getMonsterCode())
        {
            case HUMAN:
            switch(monster.getRole())
            {
                case NULL:
                g.drawImage(gm.getImage(GraphicsManager.LUMBERJACK, 0), x, y, null);
                break;

                case CHEF:
                g.drawImage(gm.getImage(GraphicsManager.LUMBERJACK, 1), x, y, null);
                break;

                case HANDYMAN:
                g.drawImage(gm.getImage(GraphicsManager.LUMBERJACK, 2), x, y, null);
                break;

                case BANKER:
                g.drawImage(gm.getImage(GraphicsManager.LUMBERJACK, 3), x, y, null);
                break;

                case BROTHER:
                g.drawImage(gm.getImage(GraphicsManager.LUMBERJACK, 0), x, y, null);
                break;

                default:
                g.drawImage(gm.getImage(GraphicsManager.LUMBERJACK, 0), x, y, null);
                break;


            }
            break;

            case SPONGE:
            g.drawImage(gm.getImage(GraphicsManager.SPONGE), x, y, null);
            break;

            case GHOST:
            g.drawImage(gm.getImage(GraphicsManager.GHOST), x, y, null);
            break;

            case SLIME:
            g.drawImage(gm.getImage(GraphicsManager.SLIME, 0), x, y, null);
            break;

            case SMALL_SLIME:
            g.drawImage(gm.getImage(GraphicsManager.SLIME, 1), x, y, null);
            break;

            case TINY_SLIME:
            g.drawImage(gm.getImage(GraphicsManager.SLIME, 2), x, y, null);
            break;

            case DEER:
            if(monster.isSleeping())
            {
                g.drawImage(gm.getImage(GraphicsManager.DEER, 1), x, y, null);
            }
            else
            {
                g.drawImage(gm.getImage(GraphicsManager.DEER, 0), x, y, null);
            }
            break;

            case PIG:
            if(monster.isSleeping())
            {
                g.drawImage(gm.getImage(GraphicsManager.PIG, 1), x, y, null);
            }
            else
            {
                g.drawImage(gm.getImage(GraphicsManager.PIG, 0), x, y, null);
            }
            break;
        }
    }

    public void drawItem(Item item, Graphics g, int x, int y, double scale, GraphicsManager gm)
    {
        drawItem(item,g,x,y,scale,gm,true);
    }

    public void drawItem(Item item, Graphics g, int x, int y, double scale, GraphicsManager gm, boolean drawCount)
    {
        switch(item.getItemClass())
        {
            case BONES:
            g.drawImage(gm.getImage(GraphicsManager.BONES), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case MOP:
            g.drawImage(gm.getImage(GraphicsManager.MOP), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case MOSS:
            g.drawImage(gm.getImage(GraphicsManager.MOSS, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case FLOODGATE:
            g.drawImage(gm.getImage(GraphicsManager.FLOODGATE, 0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case DEATH_CAP:
            g.drawImage(gm.getImage(GraphicsManager.MUSHROOM, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case PUFFBALL:
            g.drawImage(gm.getImage(GraphicsManager.MUSHROOM, 2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case FLY_AGARIC:
            g.drawImage(gm.getImage(GraphicsManager.MUSHROOM, 3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case MOREL:
            g.drawImage(gm.getImage(GraphicsManager.MUSHROOM, 4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case BUTTON_MUSHROOM:
            g.drawImage(gm.getImage(GraphicsManager.MUSHROOM, 5), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case GHOST_FUNGUS:
            g.drawImage(gm.getImage(GraphicsManager.MUSHROOM, 6), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case APPLE:
            g.drawImage(gm.getImage(GraphicsManager.APPLE), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case CORN:
            g.drawImage(gm.getImage(GraphicsManager.CORN), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case CORN_SEED:
            g.drawImage(gm.getImage(GraphicsManager.CORN, 4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case PUMPKIN:
            g.drawImage(gm.getImage(GraphicsManager.PUMPKIN), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case PUMPKIN_SEED:
            g.drawImage(gm.getImage(GraphicsManager.PUMPKIN, 4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case BACON:
            if(item.getStackSize() == 1)
            {
                g.drawImage(gm.getImage(GraphicsManager.BACON,0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() == 2)
            {
                g.drawImage(gm.getImage(GraphicsManager.BACON,1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() == 3)
            {
                g.drawImage(gm.getImage(GraphicsManager.BACON,2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() >= 4)
            {
                g.drawImage(gm.getImage(GraphicsManager.BACON,3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            break;

            case FLAPJACKS:
            if(item.getStackSize() == 1)
            {
                g.drawImage(gm.getImage(GraphicsManager.FLAPJACKS,0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() == 2)
            {
                g.drawImage(gm.getImage(GraphicsManager.FLAPJACKS,1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() == 3)
            {
                g.drawImage(gm.getImage(GraphicsManager.FLAPJACKS,2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() >= 4)
            {
                g.drawImage(gm.getImage(GraphicsManager.FLAPJACKS,3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            break;

            case KEY:
            g.drawImage(gm.getImage(GraphicsManager.KEY), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case AXE:
            g.drawImage(gm.getImage(GraphicsManager.AXE), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case PICKAXE:
            g.drawImage(gm.getImage(GraphicsManager.PICKAXE), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case COINS:
            if(item.getStackSize() == 1)
            {
                g.drawImage(gm.getImage(GraphicsManager.COINS,0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() == 2)
            {
                g.drawImage(gm.getImage(GraphicsManager.COINS,1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() == 3)
            {
                g.drawImage(gm.getImage(GraphicsManager.COINS,2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() > 3 && item.getStackSize() < 50)
            {
                g.drawImage(gm.getImage(GraphicsManager.COINS,3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(item.getStackSize() >= 50)
            {
                g.drawImage(gm.getImage(GraphicsManager.COINS,4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            break;

            case JACKET:
            g.drawImage(gm.getImage(GraphicsManager.JACKET), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case HAT:
            g.drawImage(gm.getImage(GraphicsManager.HAT), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case BOOTS:
            g.drawImage(gm.getImage(GraphicsManager.BOOTS), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case LOG:
            g.drawImage(gm.getImage(GraphicsManager.LOG), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case ASH:
            g.drawImage(gm.getImage(GraphicsManager.ASH), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case LANTERN:
            g.drawImage(gm.getImage(GraphicsManager.LANTERN), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case BUCKET:
            g.drawImage(gm.getImage(GraphicsManager.BUCKET, 0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case WATER_BUCKET:
            g.drawImage(gm.getImage(GraphicsManager.BUCKET, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case SHOVEL:
            g.drawImage(gm.getImage(GraphicsManager.SHOVEL), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case TORCH:
            if(Math.random() < 0.33)
            {
                g.drawImage(gm.getImage(GraphicsManager.TORCH, 0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else if(Math.random() < 0.66)
            {
                g.drawImage(gm.getImage(GraphicsManager.TORCH, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            else
            {
                g.drawImage(gm.getImage(GraphicsManager.TORCH, 2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            }
            break;

            case SAPPHIRE:
            g.drawImage(gm.getImage(GraphicsManager.GEM, 0), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case EMERALD:
            g.drawImage(gm.getImage(GraphicsManager.GEM, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case RUBY:
            g.drawImage(gm.getImage(GraphicsManager.GEM, 2), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case AMETHYST:
            g.drawImage(gm.getImage(GraphicsManager.GEM, 3), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case DIAMOND:
            g.drawImage(gm.getImage(GraphicsManager.GEM, 4), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            case TENT:
            g.drawImage(gm.getImage(GraphicsManager.BED, 1), x, y, (int)(gm.getTileSize() * scale), (int)(gm.getTileSize() * scale), null);
            break;

            default:
            g.fillOval(x, y, gm.getTileSize(), gm.getTileSize());
            break;
        }

        if(item.getStackSize() > 1 && drawCount)
        {
            int countXOffset = 16;
            if((item.getStackSize() + "").length() == 2)
            {
                countXOffset = 11;
            }
            else if((item.getStackSize() + "").length() > 2)
            {
                countXOffset = 6;
            }
            this.drawString(item.getStackSize() + "", 1.0, g, countXOffset + x, 16 + y, gm);
        }
    }

    public void drawTerrain(Terrain terrain, Graphics g, int x, int y, GraphicsManager gm)
    {

        //for null terrain
        if(terrain == null)
        {
            g.setColor(new Color(10,10,10));
            g.fillRect(x, y, 16, 16);
            return;
        }

        switch(terrain.getCode())
        {
            case FERTILE_LAND:
            if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.GROW_COUNTER))
            {
                int growCount = Integer.parseInt(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.GROW_COUNTER));

                if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_SEED))
                {
                    SeedType seedType = SeedType.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_SEED));
                    g.drawImage(gm.getImage(GraphicsManager.DIRT, 2), x, y, null);

                    switch(seedType)
                    {
                        case CORN:
                        if(growCount > (TerrainManager.CORN_GROW_COUNT * 0.66))
                        {
                            g.drawImage(gm.getImage(GraphicsManager.CORN, 1), x, y, null);
                        }
                        else if(growCount > (TerrainManager.CORN_GROW_COUNT * 0.33))
                        {
                            g.drawImage(gm.getImage(GraphicsManager.CORN, 2), x, y, null);
                        }
                        else if(growCount <= (TerrainManager.CORN_GROW_COUNT * 0.33))
                        {
                            g.drawImage(gm.getImage(GraphicsManager.CORN, 3), x, y, null);
                        }
                        break;

                        case PUMPKIN:
                        if(growCount > (TerrainManager.PUMPKIN_GROW_COUNT * 0.66))
                        {
                            g.drawImage(gm.getImage(GraphicsManager.PUMPKIN, 1), x, y, null);
                        }
                        else if(growCount > (TerrainManager.PUMPKIN_GROW_COUNT * 0.33))
                        {
                            g.drawImage(gm.getImage(GraphicsManager.PUMPKIN, 2), x, y, null);
                        }
                        else if(growCount <= (TerrainManager.PUMPKIN_GROW_COUNT * 0.33))
                        {
                            g.drawImage(gm.getImage(GraphicsManager.PUMPKIN, 3), x, y, null);
                        }
                        break;
                    }
                }
                else
                {
                    //default growing series
                    if(growCount > (TerrainManager.GRASS_GROW_COUNT * 0.80))
                    {
                        g.drawImage(gm.getImage(GraphicsManager.DIRT, 2), x, y, null);
                    }
                    else if(growCount > (TerrainManager.GRASS_GROW_COUNT * 0.60))
                    {
                        g.drawImage(gm.getImage(GraphicsManager.DIRT, 3), x, y, null);
                    }
                    else if(growCount > (TerrainManager.GRASS_GROW_COUNT * 0.40))
                    {
                        g.drawImage(gm.getImage(GraphicsManager.DIRT, 4), x, y, null);
                    }
                    else if(growCount > (TerrainManager.GRASS_GROW_COUNT * 0.20))
                    {
                        g.drawImage(gm.getImage(GraphicsManager.DIRT, 5), x, y, null);
                    }
                    else if(growCount <= (TerrainManager.GRASS_GROW_COUNT * 0.20))
                    {
                        g.drawImage(gm.getImage(GraphicsManager.DIRT, 6), x, y, null);
                    }
                }
            }
            else
            {
                g.drawImage(gm.getImage(GraphicsManager.DIRT, 2), x, y, null);
            }
            break;

            case PATH:
            g.drawImage(gm.getImage(GraphicsManager.DIRT, 1), x, y, null);
            break;

            case DIRT:
            g.drawImage(gm.getImage(GraphicsManager.DIRT), x, y, null);
            break;

            case GRASS:
            g.drawImage(gm.getImage(GraphicsManager.GRASS), x, y, null);
            break;

            case STONE_WALL:
            g.drawImage(gm.getImage(GraphicsManager.STONE_WALL), x, y, null);
            break;

            case STONE_FLOOR:
            g.drawImage(gm.getImage(GraphicsManager.STONE_FLOOR), x, y, null);
            break;

            case ROCK:
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MINERALS))
            {
                int mineralIndex = Integer.parseInt(TerrainManager.getParameter(terrain, TerrainParameter.HAS_MINERALS));
                g.drawImage(gm.getImage(GraphicsManager.ROCKFACE, mineralIndex), x, y, null);
            }
            else
            {
                g.drawImage(gm.getImage(GraphicsManager.ROCKFACE, 0), x, y, null);
            }
            break;

            case STREAM_BED:
            g.drawImage(gm.getImage(GraphicsManager.STONE_FLOOR, 1), x, y, null);
            break;

        }

        
        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR))
        {
            DoorCode doorCode = DoorCode.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_DOOR));
            switch(doorCode)
            {
                case OPEN:
                g.drawImage(gm.getImage(GraphicsManager.DOOR, 0), x, y, null);
                break;

                case CLOSED:
                g.drawImage(gm.getImage(GraphicsManager.DOOR, 1), x, y, null);
                break;

                default:
                g.drawImage(gm.getImage(GraphicsManager.DOOR, 0), x, y, null);
                break;
            }
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN))
        {
            switch(terrain.getCode())
            {
                case STONE_WALL:
                case ROCK:
                g.drawImage(gm.getImage(GraphicsManager.SIGN, 0), x, y, null);
                break;

                default:
                g.drawImage(gm.getImage(GraphicsManager.SIGN, 1), x, y, null);
                break;
            }
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.FIRE))
        {
            g.drawImage(gm.getImage(GraphicsManager.FIRE, (int)(Math.random() * 3)), x, y, null);
        }

        
        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_BED))
        {
            g.drawImage(gm.getImage(GraphicsManager.BED), x, y, null);
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS))
        {
            g.drawImage(gm.getImage(GraphicsManager.MOSS), x, y, null);
        }

        if(terrain.getWater() > 0)
        {
            int waterValue = terrain.getWater();
            if(waterValue > 0 && waterValue < (int)(TerrainManager.DEEP_WATER * 0.10))
            {
                g.drawImage(gm.getImage(GraphicsManager.WATER, 0), x, y, null);
            }
            else if(waterValue < (int)(TerrainManager.DEEP_WATER * 0.10))
            {
                g.drawImage(gm.getImage(GraphicsManager.WATER, 1), x, y, null);
            }
            else if(waterValue < (int)(TerrainManager.DEEP_WATER * 0.50))
            {
                g.drawImage(gm.getImage(GraphicsManager.WATER, 2), x, y, null);
            }
            else if(waterValue < TerrainManager.DEEP_WATER)
            {
                g.drawImage(gm.getImage(GraphicsManager.WATER, 3), x, y, null);
            }
            else if(waterValue >= TerrainManager.DEEP_WATER)
            {
                if(Math.random() > 0.5)
                {
                    g.drawImage(gm.getImage(GraphicsManager.WATER, 4), x, y, null);
                }
                else
                {
                    g.drawImage(gm.getImage(GraphicsManager.WATER, 5), x, y, null);
                }
            }

            if(Quinoa.DEBUG_MODE)
            {
                drawString(waterValue +"",1.0, g, x, y, gm);
            }
        
        }


        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SPRING))
        {
            int springFrame = (int)(Math.random() * 3);
            g.drawImage(gm.getImage(GraphicsManager.WATER, 6 + springFrame), x, y, null);
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DRAIN))
        {
            g.drawImage(gm.getImage(GraphicsManager.WATER, 9), x, y, null);
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_FLOODGATE))
        {
            DoorCode doorCode = DoorCode.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_FLOODGATE));
            switch(doorCode)
            {
                case OPEN:
                g.drawImage(gm.getImage(GraphicsManager.FLOODGATE, 1), x, y, null);
                break;

                case CLOSED:
                g.drawImage(gm.getImage(GraphicsManager.FLOODGATE, 0), x, y, null);
                break;

                default:
                g.drawImage(gm.getImage(GraphicsManager.FLOODGATE, 1), x, y, null);
                break;
            }
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
        {
            GraveCode gc = GraveCode.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_GRAVE));
            switch(gc)
            {
                case NORMAL:
                g.drawImage(gm.getImage(GraphicsManager.GRAVE, 0), x, y, null);
                break;
                
                case SPECIAL:
                g.drawImage(gm.getImage(GraphicsManager.GRAVE, 1), x, y, null);
                break;
                
                case BROKEN:
                g.drawImage(gm.getImage(GraphicsManager.GRAVE, 2), x, y, null);
                break;
            }
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE))
        {
            TreeCode tc = TreeCode.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_TREE));
            switch(tc)
            {
                case PINE_TREE:
                g.drawImage(gm.getImage(GraphicsManager.TREE, 0), x, y, null);
                break;

                case BIRCH_TREE:
                g.drawImage(gm.getImage(GraphicsManager.TREE, 2), x, y, null);
                g.drawImage(gm.getImage(GraphicsManager.TREE, 3), x, y, null);
                break;

                case OAK_TREE:
                g.drawImage(gm.getImage(GraphicsManager.TREE, 1), x, y, null);
                g.drawImage(gm.getImage(GraphicsManager.TREE, 4), x, y, null);
                break;

                case APPLE_TREE:
                g.drawImage(gm.getImage(GraphicsManager.TREE, 5), x, y, null);
                g.drawImage(gm.getImage(GraphicsManager.TREE, 6), x, y, null);
                break;
                
                default:
                g.drawImage(gm.getImage(GraphicsManager.TREE), x, y, null);
                break;
            }
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES))
        {
            g.drawImage(gm.getImage(GraphicsManager.MUSHROOM, 0), x, y, null);
        }

        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_CLOVER))
        {
            g.drawImage(gm.getImage(GraphicsManager.CLOVER, 0), x, y, null);

            int cloverCounter = Integer.parseInt(TerrainManager.getParameter(terrain, TerrainParameter.HAS_CLOVER));
            if(cloverCounter == 0)
            {
                g.drawImage(gm.getImage(GraphicsManager.CLOVER, 1), x, y, null);
            }
        }
         

    }

    public void drawString(String string, double scale, Graphics g, int x, int y, GraphicsManager gm)
    {

        for(int i=0; i < string.length(); i++)
        {
            BufferedImage letter = gm.getLetter(string.charAt(i));
            if(letter != null)
            {
                g.drawImage(letter, x + ((int)(gm.getLetterWidth() * gm.getLetterSpacingPercent() * scale) * i), y, (int)(gm.getLetterWidth() * scale), (int)(gm.getLetterHeight() * scale), null);
            }
        }
         
    }

}
