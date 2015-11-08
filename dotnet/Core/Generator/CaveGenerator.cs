using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Map;
using LumberjackRL.Core.Items;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Generator
{
    public class CaveGenerator
    {
        public RegionHeader header;
        public Region region;
        public RegionExit entrance;
        public RegionExit exit;
        public double fillPercentage;
        public int smoothness;
        public List<Chamber> chambers;
        private Quinoa quinoa;


        public CaveGenerator(int width, int height, String name, double fillPercentage, int smoothness, Quinoa quinoa)
        {
            header = new RegionHeader(name);
            region = new Region(width, height);
            header.setRegion(region);
            entrance = new RegionExit(width / 2, height / 2, 0, 0, "", ExitDecorator.UP_STAIR);
            exit = new RegionExit(width / 2, (height / 2) + 1, 0, 0, "", ExitDecorator.DOWN_STAIR);
            header.getExits().Add(entrance);
            header.getExits().Add(exit);
            region.setLightingModel(LightingModel.CAVE);
            this.quinoa = quinoa;
            this.fillPercentage = fillPercentage;
            this.smoothness = smoothness;

            chambers = new List<Chamber>();

            //fill with solid rock
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    region.setTerrain(x, y, new Terrain());
                    region.getTerrain(x, y).setCode(TerrainCode.ROCK);
                }
            }
        }

        public void addTreasure()
        {
            List<Position> waterSide = this.getAdjacentTerrainPositions(1, 1, region.getWidth() - 2, region.getHeight() - 2, TerrainCode.STONE_FLOOR, TerrainCode.STREAM_BED);
            waterSide.AddRange(waterSide);
            for (int i = 0; i < 25; i++)
            {
                if (waterSide.Count > 0)
                {
                    Position pos = waterSide[RandomNumber.RandomInteger(waterSide.Count - 1)];

                    Item tempItem = ItemManager.getRandomItem(ItemCategory.MONEY, false);
                    tempItem.stackSize = ((int)(tempItem.maxStackSize * ((RandomNumber.RandomDouble() * 0.70) + 0.20)));
                    tempItem.itemState = ItemState.GROUND;
                    tempItem.setPosition(pos.x, pos.y);
                    region.getItems().Add(tempItem);

                    waterSide.Remove(pos);
                }
            }
            for (int i = 0; i < 10; i++)
            {
                if (waterSide.Count > 0)
                {
                    Position pos = waterSide[RandomNumber.RandomInteger(waterSide.Count - 1)];

                    Item tempItem = ItemManager.getRandomItem(ItemCategory.MATERIAL, false);
                    tempItem.itemState = ItemState.GROUND;
                    tempItem.setPosition(pos.x, pos.y);
                    region.getItems().Add(tempItem);

                    waterSide.Remove(pos);
                }
            }
        }

        public RegionExit getEntrance()
        {
            return entrance;
        }


        public void addChamber(Chamber chamber)
        {
            chambers.Add(chamber);

            //initial random fill
            for(int x=chamber.x; x < chamber.x + chamber.width; x++)
            {
                for(int y=chamber.y; y < chamber.y + chamber.height; y++)
                {
                    if(chamber.type == ChamberType.OPEN)
                    {
                        if(RandomNumber.RandomDouble() < fillPercentage)
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.STONE_FLOOR);
                        }
                        else
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.ROCK);
                        }
                    }
                    else if(chamber.type == ChamberType.MUSHROOM)
                    {
                        if(RandomNumber.RandomDouble() < (fillPercentage * 0.80))
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.STONE_FLOOR);
                        }
                        else
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.ROCK);
                        }
                    }
                    else if(chamber.type == ChamberType.FLOODED)
                    {
                        if(RandomNumber.RandomDouble() < fillPercentage)
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.STREAM_BED);
                        }
                        else
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.ROCK);
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
                                region.getTerrain(x, y).setCode(TerrainCode.STONE_FLOOR);
                            }
                            else if(chamber.type == ChamberType.FLOODED)
                            {
                                region.getTerrain(x, y).setCode(TerrainCode.STREAM_BED);
                            }
                        }
                    }
                }
            }



            //do final features
            if(chamber.type == ChamberType.FLOODED)
            {
                List<Position> positions = this.getTerrainPositions(chamber.x, chamber.y, chamber.x + chamber.width - 1, chamber.y + chamber.height - 1, TerrainCode.STREAM_BED);
                int springCount = (int)(positions.Count / 100) + 1;
                if(positions.Count > 0)
                {
                    for(int i=0; i < springCount; i++)
                    {
                        Position pos = positions[(int)(RandomNumber.RandomDouble() * (positions.Count - 1))];
                        if (!region.getTerrain(pos.x, pos.y).getParameters().ContainsKey(TerrainParameter.HAS_SPRING))
                        {
                            region.getTerrain(pos.x, pos.y).getParameters().Add(TerrainParameter.HAS_SPRING, TerrainManager.SPRING_RATE + "");
                        }
                        positions.Remove(pos);
                    }
                    foreach(Position tempPos in positions)
                    {
                        region.getTerrain(tempPos.x, tempPos.y).setWater(TerrainManager.DEEP_WATER);
                    }
                }

            }
            else if(chamber.type == ChamberType.MUSHROOM)
            {
                List<Position> positions = this.getTerrainPositions(chamber.x, chamber.y, chamber.x + chamber.width - 1, chamber.y + chamber.height - 1, TerrainCode.STONE_FLOOR);
                if(positions.Count > 0)
                {
                    int mushCount = (int)(positions.Count / 10) + 1;
                    for(int i=0; i < mushCount; i++)
                    {
                        Position pos = positions[(int)(RandomNumber.RandomDouble() * (positions.Count- 1))];
                        MushroomSporeCode msc = EnumUtil.RandomEnumValue<MushroomSporeCode>();
                        if (!region.getTerrain(pos.x, pos.y).getParameters().ContainsKey(TerrainParameter.HAS_MUSHROOM_SPORES))
                        {
                            region.getTerrain(pos.x, pos.y).getParameters().Add(TerrainParameter.HAS_MUSHROOM_SPORES, EnumUtil.EnumName<MushroomSporeCode>(msc));
                        }
                        positions.Remove(pos);
                    }
                }
            }
        }


            public void addTorches()
            {
                List<Position> rockFloor = this.getAdjacentTerrainPositions(1, 1, region.getWidth() - 2, region.getHeight() - 2, TerrainCode.STONE_FLOOR, TerrainCode.ROCK);
                List<Position> waterSide = this.getAdjacentTerrainPositions(1, 1, region.getWidth() - 2, region.getHeight() - 2, TerrainCode.STONE_FLOOR, TerrainCode.STREAM_BED);
                rockFloor.AddRange(waterSide);
                for (int i = 0; i < 5; i++)
                {
                    if (rockFloor.Count > 0)
                    {
                        Position pos = rockFloor[RandomNumber.RandomInteger(rockFloor.Count - 1)];

                        Item tempItem = new Item();
                        tempItem.itemClass = ItemClass.TORCH;
                        tempItem.itemState = ItemState.GROUND;
                        tempItem.setPosition(pos.x, pos.y);
                        region.getItems().Add(tempItem);

                        rockFloor.Remove(pos);
                    }
                }
            }


            public void addMinerals(int level)
            {
                List<Position> waterSideRocks = this.getAdjacentTerrainPositions(1, 1, region.getWidth() - 2, region.getHeight() - 2, TerrainCode.ROCK, TerrainCode.STREAM_BED);
                for (int i = 0; i < 10 * level; i++)
                {
                    if (waterSideRocks.Count > 0)
                    {
                        Position pos = waterSideRocks[RandomNumber.RandomInteger(waterSideRocks.Count - 1)];

                        region.getTerrain(pos.x, pos.y).getParameters().Add(TerrainParameter.HAS_MINERALS, RandomNumber.RandomInteger(4).ToString());

                        waterSideRocks.Remove(pos);
                    }
                }
            }

            public void smoothWater()
            {
                //iterative smoothing
                for (int i = 0; i < 6; i++)
                {
                    for (int x = 1; x < region.getWidth() - 1; x++)
                    {
                        for (int y = 1; y < region.getHeight() - 1; y++)
                        {
                            int wallCount = 0;
                            if (region.getTerrain(x - 1, y - 1).getCode() == TerrainCode.STREAM_BED)
                            {
                                wallCount++;
                            }
                            if (region.getTerrain(x, y - 1).getCode() == TerrainCode.STREAM_BED)
                            {
                                wallCount++;
                            }
                            if (region.getTerrain(x + 1, y - 1).getCode() == TerrainCode.STREAM_BED)
                            {
                                wallCount++;
                            }
                            if (region.getTerrain(x - 1, y).getCode() == TerrainCode.STREAM_BED)
                            {
                                wallCount++;
                            }
                            if (region.getTerrain(x + 1, y).getCode() == TerrainCode.STREAM_BED)
                            {
                                wallCount++;
                            }
                            if (region.getTerrain(x - 1, y + 1).getCode() == TerrainCode.STREAM_BED)
                            {
                                wallCount++;
                            }
                            if (region.getTerrain(x, y + 1).getCode() == TerrainCode.STREAM_BED)
                            {
                                wallCount++;
                            }
                            if (region.getTerrain(x + 1, y + 1).getCode() == TerrainCode.STREAM_BED)
                            {
                                wallCount++;
                            }

                            if (region.getTerrain(x, y).getCode() != TerrainCode.ROCK)
                            {
                                if ((region.getTerrain(x, y).getCode() == TerrainCode.STREAM_BED && wallCount >= 4)
                                || (region.getTerrain(x, y).getCode() != TerrainCode.STONE_FLOOR && wallCount >= 5))
                                {
                                    region.getTerrain(x, y).setCode(TerrainCode.STREAM_BED);
                                }
                                else
                                {
                                    region.getTerrain(x, y).setCode(TerrainCode.STONE_FLOOR);
                                    region.getTerrain(x, y).getParameters().Remove(TerrainParameter.HAS_SPRING);
                                }
                            }
                            else
                            {
                                if ((region.getTerrain(x, y).getCode() == TerrainCode.STREAM_BED && wallCount >= 4)
                                || (region.getTerrain(x, y).getCode() != TerrainCode.STONE_FLOOR && wallCount >= 5))
                                {
                                    region.getTerrain(x, y).setCode(TerrainCode.STREAM_BED);
                                }
                                else
                                {
                                    region.getTerrain(x, y).setCode(TerrainCode.ROCK);
                                    region.getTerrain(x, y).getParameters().Remove(TerrainParameter.HAS_SPRING);
                                }
                            }
                        }
                    }
                }
            }



            public void placeExits()
            {
                List<Position> pos = getTerrainPositions(5, 5, region.getWidth() - 10, region.getHeight() - 10, TerrainCode.STONE_FLOOR);

                //entrance
                Position here = pos[RandomNumber.RandomInteger(pos.Count - 1)];
                entrance.setX(here.x);
                entrance.setY(here.y);

                region.getTerrain(here.x - 1, here.y - 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x - 1, here.y - 1));
                region.getTerrain(here.x, here.y - 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x, here.y - 1));
                region.getTerrain(here.x + 1, here.y - 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x + 1, here.y - 1));

                region.getTerrain(here.x - 1, here.y).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x - 1, here.y));
                region.getTerrain(here.x + 1, here.y).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x + 1, here.y));

                region.getTerrain(here.x - 1, here.y + 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x - 1, here.y + 1));
                region.getTerrain(here.x, here.y + 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x, here.y + 1));
                region.getTerrain(here.x + 1, here.y + 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x + 1, here.y + 1));

                pos.Remove(here);

                //exit
                here = pos[RandomNumber.RandomInteger(pos.Count - 1)];
                exit.setX(here.x);
                exit.setY(here.y);

                region.getTerrain(here.x - 1, here.y - 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x - 1, here.y - 1));
                region.getTerrain(here.x, here.y - 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x, here.y - 1));
                region.getTerrain(here.x + 1, here.y - 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x + 1, here.y - 1));

                region.getTerrain(here.x - 1, here.y).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x - 1, here.y));
                region.getTerrain(here.x + 1, here.y).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x + 1, here.y));

                region.getTerrain(here.x - 1, here.y + 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x - 1, here.y + 1));
                region.getTerrain(here.x, here.y + 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x, here.y + 1));
                region.getTerrain(here.x + 1, here.y + 1).setCode(TerrainCode.STONE_FLOOR);
                TerrainManager.removeParameters(region.getTerrain(here.x + 1, here.y + 1));
            }


            public List<Position> getTerrainPositions(int x1, int y1, int x2, int y2, TerrainCode code)
            {
                List<Position> positions = new List<Position>();
                for (int x = x1; x < x2; x++)
                {
                    for (int y = y1; y < y2; y++)
                    {
                        if (region.getTerrain(x, y).getCode() == code)
                        {
                            positions.Add(new Position(x, y));
                        }
                    }
                }

                return positions;
            }

        public List<Position> getAdjacentTerrainPositions(int x1, int y1, int x2, int y2, TerrainCode code, TerrainCode adjacent)
        {
            List<Position> positions = new List<Position>();
            for(int x=x1; x < x2; x++)
            {
                for(int y=y1; y < y2; y++)
                {
                    if(region.getTerrain(x, y).getCode() == code)
                    {
                        positions.Add(new Position(x,y));
                    }
                }
            }

            List<Position> returnList = new List<Position>();
            foreach(Position tempPos in positions)
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
                    returnList.Add(tempPos);
                }
            }

            return returnList;
        }
    }
}
