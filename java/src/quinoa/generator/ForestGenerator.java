package quinoa.generator;

import java.util.ArrayList;
import quinoa.Quinoa;
import quinoa.region.LightMap.LightingModel;
import quinoa.region.Position;
import quinoa.region.Region;
import quinoa.region.RegionHeader;
import quinoa.region.Terrain;
import quinoa.region.TerrainManager;
import quinoa.region.TerrainManager.TerrainCode;
import quinoa.region.TerrainManager.TerrainParameter;

public class ForestGenerator
{
    public RegionHeader header;
    public Region region;
    public double treeDensity;
    private Quinoa quinoa;

    public ForestGenerator(int width, int height, String name, double treeDensity, Quinoa quinoa)
    {
        header = new RegionHeader(name);
        region = new Region(width, height);
        header.setRegion(region);
        region.setLightingModel(LightingModel.ABOVE_GROUND);
        this.quinoa = quinoa;
        this.treeDensity = treeDensity;
        
        //fill the forest with grass
        for(int x=0; x < width; x++)
        {
            for(int y=0; y < height; y++)
            {
                region.setTerrain(x, y, new Terrain());
                region.getTerrain(x, y).setCode(TerrainManager.TerrainCode.GRASS);
            }
        }
    }


    public void addWater(double waterDensity, int smoothness)
    {
        ArrayList<Position> grassTiles = MapGenerator.getTerrainPositions(TerrainCode.GRASS, region, false, false);
        int waterCount = (int)(grassTiles.size() * waterDensity);

        if(waterCount <= 0)
        {
            return;
        }

        for(int j=0; j < waterCount; j++)
        {
            Position pos = grassTiles.get((int)(Math.random() * grassTiles.size()));
            region.getTerrain(pos.x, pos.y).setCode(TerrainManager.TerrainCode.STREAM_BED);
            grassTiles.remove(pos);
        }

        //iterative smoothing
        for(int i=0; i < smoothness; i++)
        {
            for(int x=1; x < region.getWidth()-1; x++)
            {
                for(int y=1; y < region.getHeight()-1; y++)
                {
                    int wallCount=0;
                    if(region.getTerrain(x-1, y-1).getCode() == TerrainCode.GRASS)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x, y-1).getCode() == TerrainCode.GRASS)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x+1, y-1).getCode() == TerrainCode.GRASS)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x-1, y).getCode() == TerrainCode.GRASS)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x+1, y).getCode() == TerrainCode.GRASS)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x-1, y+1).getCode() == TerrainCode.GRASS)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x, y+1).getCode() == TerrainCode.GRASS)
                    {
                        wallCount++;
                    }
                    if(region.getTerrain(x+1, y+1).getCode() == TerrainCode.GRASS)
                    {
                        wallCount++;
                    }

                    if((region.getTerrain(x, y).getCode() == TerrainCode.GRASS && wallCount >= 4)
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
        ArrayList<Position> positions = MapGenerator.getTerrainPositions(TerrainCode.STREAM_BED, region, true);
        int springCount = positions.size() / 100;
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

    public void generate(boolean nExit, boolean eExit, boolean sExit, boolean wExit)
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
