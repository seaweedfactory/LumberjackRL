using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Map;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Generator
{
    public class TownGenerator
    {
        public RegionHeader header;
        public Region region;
        public int wiggle;
        public double radius;
        public double treeDensity;
        public OverworldCell overworldCell;

        private Quinoa quinoa;

        public TownGenerator(int width, int height, String name, int wiggle, double radius, double treeDensity, OverworldCell overworldCell, Quinoa quinoa)
        {
            header = new RegionHeader(name);
            region = new Region(width, height);
            header.setRegion(region);
            header.setName(NameMaker.makeTrivialPlaceName());
            this.wiggle = wiggle;
            this.radius = radius;
            this.treeDensity = treeDensity;
            this.quinoa = quinoa;
            this.overworldCell = overworldCell;
        }

        public void generate()
        {
            int centerX = (int)(region.getWidth() / 2);
            int centerY = (int)(region.getHeight() / 2);
            int distanceCutoff = (int)(centerX * radius);

            //make the town center
            for (int x = 0; x < region.getWidth(); x++)
            {
                for (int y = 0; y < region.getHeight(); y++)
                {
                    region.setTerrain(x, y, new Terrain());
                    int distance = (int)(Math.Sqrt(((x - centerX) * (x - centerX)) + ((y - centerY) * (y - centerY))));
                    if (distance > distanceCutoff + RandomNumber.RandomInteger(wiggle))
                    {
                        region.getTerrain(x, y).setCode(TerrainCode.GRASS);
                    }
                    else
                    {
                        region.getTerrain(x, y).setCode(TerrainCode.DIRT);
                    }
                }
            }

            //make the path
            MapGenerator.makePath(region, overworldCell.nExit, overworldCell.eExit, overworldCell.sExit, overworldCell.wExit);

            //add trees
            MapGenerator.addTrees(region, treeDensity);

            //add grave yard
            MapGenerator.addGraveyard(15, 10, region.getWidth() - 19, region.getHeight() - 14, region);

            //add buildings
            Building bank = new Building();
            bank.setBuildingType(BuildingType.BANK);
            BuildingManager.initialize(bank);
            bank.setPosition((region.getWidth() / 10) * 3, (int)((region.getHeight() / 10) * 2.5));
            region.getBuildings().Add(bank);

            Building inn = new Building();
            inn.setBuildingType(BuildingType.INN);
            BuildingManager.initialize(inn);
            inn.setPosition(3 + ((region.getWidth() / 10) * 5), (int)((region.getHeight() / 10) * 2.5));
            region.getBuildings().Add(inn);

            for (int i = 0; i < 3; i++)
            {
                Building house = new Building();
                if (i == 1)
                {
                    house.setBuildingType(BuildingType.HOUSE);
                }
                else if (i == 0)
                {
                    house.setBuildingType(BuildingType.FOOD_STORE);
                }
                else if (i == 2)
                {
                    house.setBuildingType(BuildingType.TOOL_STORE);
                }
                BuildingManager.initialize(house);
                house.setPosition((region.getWidth() / 10) * (4 + (i * 2)), (int)((region.getHeight() / 10) * 6));
                region.getBuildings().Add(house);
            }

            region.buildBuildings();

            //Add a pond in the corner
            region.getTerrain(7, 7).getParameters().Add(TerrainParameter.HAS_SPRING, TerrainManager.SPRING_RATE + "");
            for (int x = 4; x < 10; x++)
            {
                for (int y = 4; y < 10; y++)
                {
                    region.getTerrain(x, y).setCode(TerrainCode.STREAM_BED);
                }
            }
        }
    }
}
