package quinoa.generator;

import quinoa.region.Position;
import java.util.ArrayList;
import quinoa.Quinoa;
import quinoa.generator.Chamber.ChamberType;
import quinoa.items.Item;
import quinoa.items.ItemManager;
import quinoa.items.ItemManager.ItemCategory;
import quinoa.items.ItemManager.ItemClass;
import quinoa.items.ItemManager.ItemState;
import quinoa.region.LightMap.LightingModel;
import quinoa.region.Region;
import quinoa.region.RegionExit;
import quinoa.region.RegionExit.ExitDecorator;
import quinoa.region.RegionHeader;
import quinoa.region.Terrain;
import quinoa.region.TerrainManager;
import quinoa.region.TerrainManager.MushroomSporeCode;
import quinoa.region.TerrainManager.TerrainCode;
import quinoa.region.TerrainManager.TerrainParameter;

public class CaveGenerator
{
    public RegionHeader header;
    public Region region;
    public RegionExit entrance;
    public RegionExit exit;
    public double fillPercentage;
    public int smoothness;
    public ArrayList<Chamber> chambers;
    private Quinoa quinoa;


    public CaveGenerator(int width, int height, String name, double fillPercentage, int smoothness, Quinoa quinoa)
    {
        header = new RegionHeader(name);
        region = new Region(width, height);
        header.setRegion(region);
        entrance = new RegionExit(width / 2, height / 2, 0, 0, "", ExitDecorator.UP_STAIR);
        exit = new RegionExit(width / 2, (height / 2) + 1, 0, 0, "", ExitDecorator.DOWN_STAIR);
        header.getExits().add(entrance);
        header.getExits().add(exit);
        region.setLightingModel(LightingModel.CAVE);
        this.quinoa = quinoa;
        this.fillPercentage = fillPercentage;
        this.smoothness = smoothness;

        chambers = new ArrayList<Chamber>();

        //fill with solid rock
        for(int x=0; x < width; x++)
        {
            for(int y=0; y < height; y++)
            {
                region.setTerrain(x, y, new Terrain());
                region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.ROCK);
            }
        }
    }

    public void addTreasure()
    {
        ArrayList<Position> waterSide = this.getAdjacentTerrainPositions(1, 1, region.getWidth()-2, region.getHeight()-2, TerrainCode.STONE_FLOOR, TerrainCode.STREAM_BED);
        waterSide.addAll(waterSide);
        for(int i=0; i < 25; i++)
        {
            if(!waterSide.isEmpty())
            {
                Position pos = waterSide.get((int)(Math.random() * (waterSide.size()-1)));

                Item tempItem = ItemManager.getRandomItem(ItemCategory.MONEY, false);
                tempItem.setStackSize((int)(tempItem.getMaxStackSize() * ((Math.random() * 0.70) + 0.20)));
                tempItem.setItemState(ItemState.GROUND);
                tempItem.setPosition(pos.x, pos.y);
                region.getItems().add(tempItem);

                waterSide.remove(pos);
            }
        }
        for(int i=0; i < 10; i++)
        {
            if(!waterSide.isEmpty())
            {
                Position pos = waterSide.get((int)(Math.random() * (waterSide.size()-1)));

                Item tempItem = ItemManager.getRandomItem(ItemCategory.MATERIAL, false);
                tempItem.setItemState(ItemState.GROUND);
                tempItem.setPosition(pos.x, pos.y);
                region.getItems().add(tempItem);

                waterSide.remove(pos);
            }
        }
    }

    public RegionExit getEntrance()
    {
        return entrance;
    }

    
    public void addChamber(Chamber chamber)
    {
        chambers.add(chamber);

        //initial random fill
        for(int x=chamber.x; x < chamber.x + chamber.width; x++)
        {
            for(int y=chamber.y; y < chamber.y + chamber.height; y++)
            {
                if(chamber.type == ChamberType.OPEN)
                {
                    if(Math.random() < fillPercentage)
                    {
                        region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.STONE_FLOOR);
                    }
                    else
                    {
                        region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.ROCK);
                    }
                }
                else if(chamber.type == ChamberType.MUSHROOM)
                {
                    if(Math.random() < (fillPercentage * 0.80))
                    {
                        region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.STONE_FLOOR);
                    }
                    else
                    {
                        region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.ROCK);
                    }
                }
                else if(chamber.type == ChamberType.FLOODED)
                {
                    if(Math.random() < fillPercentage)
                    {
                        region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.STREAM_BED);
                    }
                    else
                    {
                        region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.ROCK);
                    }
                }
            }
        }

        //iterative smoothing
        for(int i=0; i < (4 + smoothness); i++)
        {
            for(int x=chamber.x; x < chamber.x + chamber.width; x++)
            {
                for(int y=chamber.y; y < chamber.y + chamber.height; y++)
                {
                    int wallCount=0;
                    if(region.getTerrain(x-1, y-1).getCode() == TerrainCode.ROCK)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x, y-1).getCode() == TerrainCode.ROCK)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x+1, y-1).getCode() == TerrainCode.ROCK)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x-1, y).getCode() == TerrainCode.ROCK)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x+1, y).getCode() == TerrainCode.ROCK)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x-1, y+1).getCode() == TerrainCode.ROCK)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x, y+1).getCode() == TerrainCode.ROCK)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x+1, y+1).getCode() == TerrainCode.ROCK)
                    {
                        wallCount++;
                    }

                    if((region.getTerrain(x, y).getCode() == TerrainCode.ROCK && wallCount >= 4)
                    || (region.getTerrain(x, y).getCode() != TerrainCode.ROCK && wallCount >= 5))
                    {
                        region.getTerrain(x, y).setCode(TerrainCode.ROCK);
                    }
                    else
                    {
                        if(chamber.type == ChamberType.OPEN || chamber.type == ChamberType.MUSHROOM)
                        {
                            region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.STONE_FLOOR);
                        }
                        else if(chamber.type == ChamberType.FLOODED)
                        {
                            region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.STREAM_BED);
                        }
                    }
                }
            }
        }



        //do final features
        if(chamber.type == ChamberType.FLOODED)
        {
            ArrayList<Position> positions = this.getTerrainPositions(chamber.x, chamber.y, chamber.x + chamber.width - 1, chamber.y + chamber.height - 1, TerrainCode.STREAM_BED);
            int springCount = (int)(positions.size() / 100) + 1;
            if(!positions.isEmpty())
            {
                for(int i=0; i < springCount; i++)
                {
                    Position pos = positions.get((int)(Math.random() * (positions.size() - 1)));
                    region.getTerrain(pos.x, pos.y).getParameters().put(TerrainManager.TerrainParameter.HAS_SPRING, TerrainManager.SPRING_RATE + "");
                    positions.remove(pos);
                }
                for(Position tempPos: positions)
                {
                    region.getTerrain(tempPos.x, tempPos.y).setWater(TerrainManager.DEEP_WATER);
                }
            }

        }
        else if(chamber.type == ChamberType.MUSHROOM)
        {
            ArrayList<Position> positions = this.getTerrainPositions(chamber.x, chamber.y, chamber.x + chamber.width - 1, chamber.y + chamber.height - 1, TerrainCode.STONE_FLOOR);
            if(!positions.isEmpty())
            {
                int mushCount = (int)(positions.size() / 10) + 1;
                for(int i=0; i < mushCount; i++)
                {
                    Position pos = positions.get((int)(Math.random() * (positions.size() - 1)));
                    MushroomSporeCode msc = TerrainManager.MushroomSporeCode.values()[(int)(Math.random() * TerrainManager.MushroomSporeCode.values().length)];
                    region.getTerrain(pos.x, pos.y).getParameters().put(TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES, msc.name());
                    positions.remove(pos);
                }
            }
        }
    }


    public void addTorches()
    {
        ArrayList<Position> rockFloor = this.getAdjacentTerrainPositions(1, 1, region.getWidth()-2, region.getHeight()-2, TerrainCode.STONE_FLOOR, TerrainCode.ROCK);
        ArrayList<Position> waterSide = this.getAdjacentTerrainPositions(1, 1, region.getWidth()-2, region.getHeight()-2, TerrainCode.STONE_FLOOR, TerrainCode.STREAM_BED);
        rockFloor.addAll(waterSide);
        for(int i=0; i < 5; i++)
        {
            if(!rockFloor.isEmpty())
            {
                Position pos = rockFloor.get((int)(Math.random() * (rockFloor.size()-1)));

                Item tempItem = new Item();
                tempItem.setItemClass(ItemClass.TORCH);
                tempItem.setItemState(ItemState.GROUND);
                tempItem.setPosition(pos.x, pos.y);
                region.getItems().add(tempItem);

                rockFloor.remove(pos);
            }
        }       
    }


    public void addMinerals(int level)
    {
        ArrayList<Position> waterSideRocks = this.getAdjacentTerrainPositions(1, 1, region.getWidth()-2, region.getHeight()-2, TerrainCode.ROCK, TerrainCode.STREAM_BED);
        for(int i=0; i < 10 * level; i++)
        {
            if(!waterSideRocks.isEmpty())
            {
                Position pos = waterSideRocks.get((int)(Math.random() * (waterSideRocks.size()-1)));

                region.getTerrain(pos.x, pos.y).getParameters().put(TerrainParameter.HAS_MINERALS, ((int)(Math.random() * 4))+"");

                waterSideRocks.remove(pos);
            }
        }
    }

    public void smoothWater()
    {
        //iterative smoothing
        for(int i=0; i < 6; i++)
        {
            for(int x=1; x < region.getWidth()-1; x++)
            {
                for(int y=1; y < region.getHeight()-1; y++)
                {
                    int wallCount=0;
                    if(region.getTerrain(x-1, y-1).getCode() == TerrainCode.STREAM_BED)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x, y-1).getCode() == TerrainCode.STREAM_BED)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x+1, y-1).getCode() == TerrainCode.STREAM_BED)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x-1, y).getCode() == TerrainCode.STREAM_BED)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x+1, y).getCode() == TerrainCode.STREAM_BED)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x-1, y+1).getCode() == TerrainCode.STREAM_BED)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x, y+1).getCode() == TerrainCode.STREAM_BED)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x+1, y+1).getCode() == TerrainCode.STREAM_BED)
                    {
                        wallCount++;
                    }

                    if(region.getTerrain(x, y).getCode() != TerrainCode.ROCK)
                    {
                        if((region.getTerrain(x, y).getCode() == TerrainCode.STREAM_BED && wallCount >= 4)
                        || (region.getTerrain(x, y).getCode() != TerrainCode.STONE_FLOOR && wallCount >= 5))
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.STREAM_BED);
                        }
                        else
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.STONE_FLOOR);
                            region.getTerrain(x, y).getParameters().remove(TerrainParameter.HAS_SPRING);
                        }
                    }
                    else
                    {
                        if((region.getTerrain(x, y).getCode() == TerrainCode.STREAM_BED && wallCount >= 4)
                        || (region.getTerrain(x, y).getCode() != TerrainCode.STONE_FLOOR && wallCount >= 5))
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.STREAM_BED);
                        }
                        else
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.ROCK);
                            region.getTerrain(x, y).getParameters().remove(TerrainParameter.HAS_SPRING);
                        }
                    }
                }
            }
        }
    }

    

    public void placeExits()
    {
        ArrayList<Position> pos = getTerrainPositions(5,5,region.getWidth()-10, region.getHeight()-10,TerrainCode.STONE_FLOOR);

        //entrance
        Position here = pos.get((int)(Math.random() * pos.size() - 1));
        entrance.setX(here.x);
        entrance.setY(here.y);

        region.getTerrain(here.x-1, here.y-1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x-1, here.y-1));
        region.getTerrain(here.x, here.y-1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x, here.y-1));
        region.getTerrain(here.x+1, here.y-1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x+1, here.y-1));

        region.getTerrain(here.x-1, here.y).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x-1, here.y));
        region.getTerrain(here.x+1, here.y).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x+1, here.y));

        region.getTerrain(here.x-1, here.y+1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x-1, here.y+1));
        region.getTerrain(here.x, here.y+1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x, here.y+1));
        region.getTerrain(here.x+1, here.y+1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x+1, here.y+1));
        
        pos.remove(here);

        //exit
        here = pos.get((int)(Math.random() * pos.size() - 1));
        exit.setX(here.x);
        exit.setY(here.y);

        region.getTerrain(here.x-1, here.y-1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x-1, here.y-1));
        region.getTerrain(here.x, here.y-1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x, here.y-1));
        region.getTerrain(here.x+1, here.y-1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x+1, here.y-1));

        region.getTerrain(here.x-1, here.y).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x-1, here.y));
        region.getTerrain(here.x+1, here.y).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x+1, here.y));

        region.getTerrain(here.x-1, here.y+1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x-1, here.y+1));
        region.getTerrain(here.x, here.y+1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x, here.y+1));
        region.getTerrain(here.x+1, here.y+1).setCode(TerrainCode.STONE_FLOOR);
        TerrainManager.removeParameters(region.getTerrain(here.x+1, here.y+1));
    }


    public ArrayList<Position> getTerrainPositions(int x1, int y1, int x2, int y2, TerrainCode code)
    {
        ArrayList<Position> positions = new ArrayList<Position>();
        for(int x=x1; x < x2; x++)
        {
            for(int y=y1; y < y2; y++)
            {
                if(region.getTerrain(x, y).getCode() == code)
                {
                    positions.add(new Position(x,y));
                }
            }
        }

        return positions;
    }

    public ArrayList<Position> getAdjacentTerrainPositions(int x1, int y1, int x2, int y2, TerrainCode code, TerrainCode adjacent)
    {
        ArrayList<Position> positions = new ArrayList<Position>();
        for(int x=x1; x < x2; x++)
        {
            for(int y=y1; y < y2; y++)
            {
                if(region.getTerrain(x, y).getCode() == code)
                {
                    positions.add(new Position(x,y));
                }
            }
        }

        ArrayList<Position> returnList = new ArrayList<Position>();
        for(Position tempPos : positions)
        {
            if(region.getTerrain(tempPos.x+1, tempPos.y-1).getCode() == adjacent
            || region.getTerrain(tempPos.x, tempPos.y-1).getCode() == adjacent
            || region.getTerrain(tempPos.x-1, tempPos.y-1).getCode() == adjacent
            || region.getTerrain(tempPos.x+1, tempPos.y).getCode() == adjacent
            || region.getTerrain(tempPos.x-1, tempPos.y).getCode() == adjacent
            || region.getTerrain(tempPos.x+1, tempPos.y+1).getCode() == adjacent
            || region.getTerrain(tempPos.x, tempPos.y+1).getCode() == adjacent
            || region.getTerrain(tempPos.x-1, tempPos.y+1).getCode() == adjacent)
            {
                returnList.add(tempPos);
            }
        }

        return returnList;
    }
}
