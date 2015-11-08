using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Generator;
using LumberjackRL.Core.Monsters;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Map
{
    public enum BuildingType 
    { 
        NULL,
        BANK, 
        INN, 
        FOOD_STORE, 
        TOOL_STORE, 
        HOUSE 
    }

    public class BuildingManager
    {
        

        public static int[,] bankStruct = {
                                       {1,1,1,1,1,1,1},
                                       {1,0,1,0,0,0,1},
                                       {1,2,1,0,0,0,1},
                                       {1,0,0,0,0,0,1},
                                       {1,0,0,0,0,0,1},
                                       {1,1,1,9,8,1,1},
                                       };

        public static int[,] innStruct = {
                                       {1,1,1,1,1,1,1,1,1,1},
                                       {1,0,0,0,3,0,0,3,0,1},
                                       {1,0,0,0,0,0,0,0,0,1},
                                       {1,0,0,0,3,0,0,3,0,1},
                                       {1,1,9,8,1,1,1,1,1,1},
                                       };

        public static int[,] houseStruct = {
                                       {1,1,1,1,1,1},
                                       {1,0,0,0,0,1},
                                       {1,0,0,0,3,1},
                                       {1,0,0,0,0,1},
                                       {1,1,9,1,1,1}
                                       };

        public static int[,] foodStoreStruct = {
                                       {1,1,1,1,1},
                                       {1,0,0,0,1},
                                       {1,2,0,0,1},
                                       {1,0,0,0,1},
                                       {1,1,9,8,1}
                                       };

        public static int[,] toolStoreStruct = {
                                       {1,1,1,1,1},
                                       {1,0,0,0,1},
                                       {1,0,0,2,1},
                                       {1,0,0,0,1},
                                       {1,1,9,8,1}
                                       };


        public static int getWidth(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.BANK:
                    return 7;

                case BuildingType.INN:
                    return 10;

                case BuildingType.HOUSE:
                    return 6;

                case BuildingType.FOOD_STORE:
                    return 5;

                case BuildingType.TOOL_STORE:
                    return 5;

                default:
                    return 0;
            }
        }

        public static int getHeight(BuildingType type)
        {
            switch (type)
            {
                case BuildingType.BANK:
                    return 6;

                case BuildingType.INN:
                    return 5;

                case BuildingType.HOUSE:
                    return 5;

                case BuildingType.FOOD_STORE:
                    return 5;

                case BuildingType.TOOL_STORE:
                    return 5;

                default:
                    return 0;
            }
        }

        public static void initialize(Building building)
        {
            building.setWidth(getWidth(building.getBuildingType()));
            building.setHeight(getHeight(building.getBuildingType()));
            building.setName(BuildingManager.makeBuildingName(building));
        }


        public static String makeBuildingName(Building building)
        {
            switch (building.getBuildingType())
            {
                case BuildingType.BANK:
                    return NameMaker.makeBankName();

                case BuildingType.FOOD_STORE:
                    return NameMaker.makeFoodName();

            }
            return EnumUtil.EnumName<BuildingType>(building.getBuildingType());
        }


        public static void build(Region region, int x, int y, Building building)
        {
            switch (building.getBuildingType())
            {
                case BuildingType.BANK:
                    buildByArray(region, x, y, building, bankStruct);
                    break;

                case BuildingType.INN:
                    buildByArray(region, x, y, building, innStruct);
                    building.setLit(false);
                    break;

                case BuildingType.HOUSE:
                    buildByArray(region, x, y, building, houseStruct);
                    break;

                case BuildingType.FOOD_STORE:
                    buildByArray(region, x, y, building, foodStoreStruct);
                    break;

                case BuildingType.TOOL_STORE:
                    buildByArray(region, x, y, building, toolStoreStruct);
                    break;
            }
        }

        public static void buildByArray(Region region, int startX, int startY, Building building, int[,] plan)
        {
            for (int x = 0; x < BuildingManager.getWidth(building.getBuildingType()); x++)
            {
                for (int y = 0; y < BuildingManager.getHeight(building.getBuildingType()); y++)
                {
                    if (x + startX < region.getWidth() && x + startX > 0
                    && y + startY < region.getHeight() && y + startY > 0)
                    {
                        switch (plan[y,x])
                        {
                            case 0:
                                region.getTerrain(startX + x, startY + y).setCode(TerrainCode.STONE_FLOOR);
                                break;

                            case 1:
                                region.getTerrain(startX + x, startY + y).setCode(TerrainCode.STONE_WALL);
                                break;

                            case 9:
                                region.getTerrain(startX + x, startY + y).setCode(TerrainCode.STONE_FLOOR);
                                region.getTerrain(startX + x, startY + y).getParameters().Add(TerrainParameter.HAS_DOOR, Enum.GetName(typeof(DoorCode), DoorCode.CLOSED));
                                building.setDoor(startX + x, startY + y);
                                break;

                            case 8:
                                region.getTerrain(startX + x, startY + y).setCode(TerrainCode.STONE_WALL);
                                region.getTerrain(startX + x, startY + y).getParameters().Add(TerrainParameter.HAS_SIGN, building.getName());
                                break;

                            case 2:
                                region.getTerrain(startX + x, startY + y).setCode(TerrainCode.STONE_FLOOR);
                                Monster newMon = new Monster();
                                switch (building.getBuildingType())
                                {
                                    case BuildingType.BANK:
                                        newMon.monsterCode = MonsterCode.HUMAN;
                                        newMon.role = MonsterRole.BANKER;
                                        break;

                                    case BuildingType.INN:
                                        newMon.monsterCode = MonsterCode.HUMAN;
                                        newMon.role = MonsterRole.NULL;
                                        break;

                                    case BuildingType.FOOD_STORE:
                                        newMon.monsterCode = MonsterCode.HUMAN;
                                        newMon.role = MonsterRole.CHEF;
                                        break;

                                    case BuildingType.TOOL_STORE:
                                        newMon.monsterCode = MonsterCode.HUMAN;
                                        newMon.role = MonsterRole.HANDYMAN;
                                        break;
                                }
                                MonsterActionManager.initialize(newMon);
                                newMon.setPosition(startX + x, startY + y);
                                region.getMonsters().Add(newMon);
                                break;

                            case 3:
                                //Add a bed
                                region.getTerrain(startX + x, startY + y).setCode(TerrainCode.STONE_FLOOR);
                                region.getTerrain(startX + x, startY + y).getParameters().Add(TerrainParameter.HAS_BED, "");
                                break;
                        }
                    }
                }
            }
        }
    }
}
