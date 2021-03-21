using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Map;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Generator
{
    public class ForestGenerator
    {
        public RegionHeader header;
        public Region region;
        public double treeDensity;
        private Quinoa quinoa;
        private RandomNumberGenerator rng = new RandomNumberGenerator();

        public ForestGenerator(int width, int height, String name, double treeDensity, Quinoa quinoa)
        {
            header = new RegionHeader(name);
            region = new Region(width, height);
            header.setRegion(region);
            region.setLightingModel(LightingModelType.ABOVE_GROUND);
            this.quinoa = quinoa;
            this.treeDensity = treeDensity;

            //fill the forest with grass
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    region.setTerrain(x, y, new Terrain());
                    region.getTerrain(x, y).setCode(TerrainCode.GRASS);
                }
            }
        }


        public void addWater(double waterDensity, int smoothness)
        {
            List<Position> grassTiles = MapGenerator.getTerrainPositions(TerrainCode.GRASS, region, false, false);
            int waterCount = (int)((double)grassTiles.Count * waterDensity);

            if (waterCount <= 0)
            {
                return;
            }

            for (int j = 0; j < waterCount; j++)
            {
                Position pos = grassTiles[rng.RandomInteger(grassTiles.Count)];
                region.getTerrain(pos.x, pos.y).setCode(TerrainCode.STREAM_BED);
                grassTiles.Remove(pos);
            }

            //iterative smoothing
            for (int i = 0; i < smoothness; i++)
            {
                for (int x = 1; x < region.getWidth() - 1; x++)
                {
                    for (int y = 1; y < region.getHeight() - 1; y++)
                    {
                        int wallCount = 0;
                        if (region.getTerrain(x - 1, y - 1).getCode() == TerrainCode.GRASS)
                        {
                            wallCount++;
                        }
                        if (region.getTerrain(x, y - 1).getCode() == TerrainCode.GRASS)
                        {
                            wallCount++;
                        }
                        if (region.getTerrain(x + 1, y - 1).getCode() == TerrainCode.GRASS)
                        {
                            wallCount++;
                        }
                        if (region.getTerrain(x - 1, y).getCode() == TerrainCode.GRASS)
                        {
                            wallCount++;
                        }
                        if (region.getTerrain(x + 1, y).getCode() == TerrainCode.GRASS)
                        {
                            wallCount++;
                        }
                        if (region.getTerrain(x - 1, y + 1).getCode() == TerrainCode.GRASS)
                        {
                            wallCount++;
                        }
                        if (region.getTerrain(x, y + 1).getCode() == TerrainCode.GRASS)
                        {
                            wallCount++;
                        }
                        if (region.getTerrain(x + 1, y + 1).getCode() == TerrainCode.GRASS)
                        {
                            wallCount++;
                        }

                        if ((region.getTerrain(x, y).getCode() == TerrainCode.GRASS && wallCount >= 4)
                        || (region.getTerrain(x, y).getCode() != TerrainCode.GRASS && wallCount >= 5))
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.GRASS);
                        }
                        else
                        {
                            region.getTerrain(x, y).setCode(TerrainCode.STREAM_BED);
                        }
                    }
                }
            }
        }


        public void addSprings()
        {
            //add springs
            List<Position> positions = MapGenerator.getTerrainPositions(TerrainCode.STREAM_BED, region, true);
            int springCount = positions.Count / 100;
            if(positions.Count > 0)
            {
                for(int i=0; i < springCount; i++)
                {
                    Position pos = positions[rng.RandomInteger(positions.Count - 1)];
                    region.getTerrain(pos.x, pos.y).getParameters().Add(TerrainParameter.HAS_SPRING, ((int)TerrainManager.SPRING_RATE).ToString());
                    positions.Remove(pos);
                }

                foreach(Position tempPos in positions)
                {
                    region.getTerrain(tempPos.x, tempPos.y).setWater(TerrainManager.DEEP_WATER);
                }
            }
        }

        public void generate(bool nExit, bool eExit, bool sExit, bool wExit)
        {
            //make the path
            MapGenerator.makePath(region, nExit, eExit, sExit, wExit);

            //Add trees
            MapGenerator.addTrees(region, treeDensity);

            //Add mushroom spores
            MapGenerator.addMushroomSpores(region, 30);

            //Add clover
            MapGenerator.addClover(region, 20);
        }
    }
}
