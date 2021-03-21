using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Utilities;
using LumberjackRL.Core.Items;
using LumberjackRL.Core.Monsters;

namespace LumberjackRL.Core.Map
{
    public class LightMap
    {
        public static int MAX_LEVEL=13; //How many transparency levels do we have, including full?
        public static double MAX_LIGHT=1.00; //Maximum light value
        public static double MIN_LIGHT=0.00; //Minimum light value
        public static double LIGHT_DROPOFF_RATE = 0.075; //Very touchy to adjust, can lead to stack overflow if too low
        public static double BUILDING_BASE_LIGHT = 0.85; //Base lighting for lit buildings and doorways
        public static double DARKNESS_LIGHT_LEVEL = 0.40;
        
        private int width;  //Width of the current region
        private int height; //Height of the current region
        private int[,] values; //Rounded values
        private double[,] calc; //Raw light values
        private List<Light> lights; //Point light sources
        private Quinoa quinoa; //Reference to main structure
        private LightingModelType lightingModel; //Current lighting model


        public LightMap(Quinoa quinoa)
        {
            this.quinoa = quinoa;
        }

        public int getValue(int x, int y)
        {
            if(x >= 0 && x < width && y >= 0 && y < height)
            {
                return values[x,y];
            }
            else
            {
                return 0;
            }
        }

        public double getCalc(int x, int y)
        {
            if(x >= 0 && x < this.width && y >=0 && y < this.height)
            {
                return calc[x,y];
            }
            else
            {
                return 0;
            }
        }

        public void calculate(Region region)
        {
            width = region.getWidth();
            height = region.getHeight();
            values = new int[width,height];
            calc = new double[width,height];
            lights = new List<Light>();
            lightingModel = region.getLightingModel();

            //Blank out the entire region with base light
            for(int x=0; x < width; x++)
            {
                for(int y=0; y < height; y++)
                {
                    if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainParameter.FIRE))
                    {
                        calc[x,y] = ((MAX_LIGHT / 2) * RandomNumber.RandomDouble()) + (MAX_LIGHT / 2);
                        lights.Add(new Light(x,y,calc[x,y]));
                    }
                    else
                    {
                        calc[x,y] = baseLight();
                    }
                }
            }

            //Add lights for appropriate monsters
            calculateMonsterLights();

            //Add lights for appropriate building features
            calculateBuildingFeatureLight();

            //Add lights for items
            calculateItemLights();

            //Propogate light from point sources
            foreach(Light tempLight in lights)
            {
                spreadLight(tempLight.x, tempLight.y, tempLight.intensity, baseLight());
            }

            //Cover buildings if roofs prevent lighting
            calculateBuildingCoverLight();

            //Converate raw values to graded values
            calcToValue();
        }


        private void calculateItemLights()
        {
            foreach(Item tempItem in quinoa.getCurrentRegionHeader().getRegion().getItems())
            {
                double radiance=0.0;
                foreach(ItemAttribute attribute in tempItem.attributes)
                {
                    if(attribute.type == ItemAttributeType.MAKES_LIGHT)
                    {
                        radiance = Double.Parse(attribute.parameter);
                    }
                }

                if(radiance > 0)
                {
                    calc[tempItem.x,tempItem.y] = Math.Max(radiance, calc[tempItem.x,tempItem.y]) ;
                    if(calc[tempItem.x,tempItem.y] > MAX_LIGHT)
                    {
                        calc[tempItem.x,tempItem.y] = MAX_LIGHT;
                    }
                    lights.Add(new Light(tempItem.x,tempItem.y, calc[tempItem.x,tempItem.y]));
                }
            }
        }

        private void calculateMonsterLights()
        {
            //Check for monster lights
            foreach(Monster tempMon in quinoa.getCurrentRegionHeader().getRegion().getMonsters())
            {
                double monsterRadiance = tempMon.stats.getRadiance();
                double itemRadiance = 0.0;

                //Check for items the monster is carrying that make light
                foreach(MonsterItemSlotType tempSlot in Enum.GetValues(typeof(MonsterItemSlotType)))
                {
                    Item tempItem = tempMon.inventory.getItem(tempSlot);
                    if(tempItem != null)
                    {
                        foreach(ItemAttribute attribute in tempItem.attributes)
                        {
                            if(attribute.type == ItemAttributeType.MAKES_LIGHT)
                            {
                                if(Double.Parse(attribute.parameter) > itemRadiance)
                                {
                                    itemRadiance = Double.Parse(attribute.parameter);
                                }
                            }
                        }
                    }
                }

                //Add lights if appropriate
                if(monsterRadiance + itemRadiance > MIN_LIGHT)
                {
                    calc[tempMon.x,tempMon.y] = Math.Max(monsterRadiance + itemRadiance, calc[tempMon.x,tempMon.y]) ;
                    if(calc[tempMon.x,tempMon.y] > MAX_LIGHT)
                    {
                        calc[tempMon.x,tempMon.y] = MAX_LIGHT;
                    }
                    lights.Add(new Light(tempMon.x,tempMon.y, calc[tempMon.x,tempMon.y]));
                }
            }
        }
    
        private void calculateBuildingFeatureLight()
        {
            //Add open door lights
            foreach(Building tempBuild in quinoa.getCurrentRegionHeader().getRegion().getBuildings())
            {
                if(tempBuild.isLit())
                {
                    Terrain doorTerrain = quinoa.getCurrentRegionHeader().getRegion().getTerrain(tempBuild.getDoor().x, tempBuild.getDoor().y);
                    if(TerrainManager.hasParameter(doorTerrain, TerrainParameter.HAS_DOOR))
                    {
                        if(TerrainManager.getParameter(doorTerrain, TerrainParameter.HAS_DOOR).Equals(DoorCode.OPEN.ToString()))
                        {
                            calc[tempBuild.getDoor().x,tempBuild.getDoor().y] = BUILDING_BASE_LIGHT;
                            lights.Add(new Light(tempBuild.getDoor().x, tempBuild.getDoor().y, BUILDING_BASE_LIGHT));
                        }
                    }
                }
            }
        }

        private void calculateBuildingCoverLight()
        {
            //light up buildings that the player is inside; darken roofs
            bool coverLight=true;
            foreach(Building tempBuild in quinoa.getCurrentRegionHeader().getRegion().getBuildings())
            {
                int px = quinoa.getPlayer().x;
                int py = quinoa.getPlayer().y;
                double lightValue = BUILDING_BASE_LIGHT;

                if(px >= tempBuild.getX() && px <= tempBuild.getX() + tempBuild.getWidth() - 1
                && py >= tempBuild.getY() && py <= tempBuild.getY() + tempBuild.getHeight() - 1)
                {
                    if(tempBuild.isLit())
                    {
                        lightValue = BUILDING_BASE_LIGHT;
                        for(int x=0; x < tempBuild.getWidth(); x++)
                        {
                            calc[tempBuild.getX() + x, tempBuild.getY() + tempBuild.getHeight()-1] = BUILDING_BASE_LIGHT;
                        }
                    }
                    else
                    {
                        coverLight = false;
                    }
                }
                else
                {
                    lightValue = baseLight();
                }

                if(coverLight)
                {
                    for(int x=0; x < tempBuild.getWidth(); x++)
                    {
                        for(int y=0; y < tempBuild.getHeight()-1; y++)
                        {
                            calc[tempBuild.getX() + x, tempBuild.getY() + y] = lightValue;
                        }
                    }
                }
            }
        }

        private void spreadLight(int x, int y, double intensity, double cutoff)
        {
            double newLight = intensity  - LIGHT_DROPOFF_RATE;
            if(newLight > cutoff)
            {
                if(x < width-1
                && calc[x+1,y] < newLight )
                {
                    if(TerrainManager.transparent(quinoa.getCurrentRegionHeader().getRegion().getTerrain(x+1, y)))
                    {
                        spreadLight(x+1,y,newLight,cutoff);
                    }
                    else
                    {
                        if(calc[x+1,y] < newLight)
                        {
                            calc[x,y] = newLight;
                        }
                    }
                }

                if(y > 0
                && calc[x,y-1] < newLight)
                {
                    if(TerrainManager.transparent(quinoa.getCurrentRegionHeader().getRegion().getTerrain(x, y-1)))
                    {
                        spreadLight(x,y-1,newLight,cutoff);
                    }
                    else
                    {
                        if(calc[x,y-1] < newLight)
                        {
                            calc[x,y-1] = newLight;
                        }
                    }
                }

                if(x > 0
                && calc[x-1,y] < newLight)
                {
                    if(TerrainManager.transparent(quinoa.getCurrentRegionHeader().getRegion().getTerrain(x-1, y)))
                    {
                        spreadLight(x-1,y,newLight,cutoff);
                    }
                    else
                    {
                        if(calc[x-1,y] < newLight)
                        {
                            calc[x-1,y] = newLight;
                        }
                    }
                }

                if(y < height-1
                && calc[x,y+1] < newLight)
                {
                    if(TerrainManager.transparent(quinoa.getCurrentRegionHeader().getRegion().getTerrain(x, y+1)))
                    {
                        spreadLight(x,y+1,newLight,cutoff);
                    }
                    else
                    {
                        if(calc[x,y+1] < newLight)
                        {
                            calc[x,y+1] = newLight;
                        }
                    }
                }

                if(newLight > calc[x,y])
                {
                    calc[x,y] = newLight;
                }
            }
            else
            {
                //end of recursion
            }
        }

        private void calcToValue()
        {
            double maxLevelDbl = Double.Parse(MAX_LEVEL+"");
            for(int x=0; x < width; x++)
            {
                for(int y=0; y < height; y++)
                {
                    values[x,y] = (int)(Math.Ceiling(calc[x,y] * maxLevelDbl));
                    if(values[x,y] > MAX_LEVEL)
                    {
                        values[x,y] = MAX_LEVEL;
                    }
                }
            }
        }

        private double baseLight()
        {
            switch(lightingModel)
            {
                case LightingModelType.ABOVE_GROUND:
                int hour = quinoa.getHour();
                double minute = Double.Parse(quinoa.getMinute()+"");

                switch(hour)
                {
                    case 19:
                    case 20:
                    case 21:
                    case 22:
                    case 23:
                    case 0:
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    return MIN_LIGHT + 0.1;

                    case 5:
                    return MIN_LIGHT + 0.1 + (((MAX_LIGHT - (MIN_LIGHT + 0.1))  / 60.0) * minute);

                    case 6:
                    case 7:
                    case 8:
                    case 9:
                    case 10:
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                    return MAX_LIGHT;

                    case 18:
                    return MIN_LIGHT + 0.1 + ((MAX_LIGHT - (MIN_LIGHT + 0.1)) / 60.0) * (60.0 - minute);


                    default:
                    return MIN_LIGHT + 0.1;
                }

                case LightingModelType.CAVE:
                return MIN_LIGHT + 0.01;

                default:
                return MIN_LIGHT;
            }
        }
    }
}
