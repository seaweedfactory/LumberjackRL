package quinoa.region;

import quinoa.generator.NameMaker;
import quinoa.monsters.Monster;
import quinoa.monsters.MonsterActionManager;
import quinoa.monsters.MonsterActionManager.MonsterCode;
import quinoa.region.TerrainManager.DoorCode;

public class BuildingManager
{
    public static enum BuildingType{BANK, INN, FOOD_STORE, TOOL_STORE, HOUSE};

    public static int[][] bankStruct = {
                                       {1,1,1,1,1,1,1},
                                       {1,0,1,0,0,0,1},
                                       {1,2,1,0,0,0,1},
                                       {1,0,0,0,0,0,1},
                                       {1,0,0,0,0,0,1},
                                       {1,1,1,9,8,1,1},
                                       };

    public static int[][] innStruct = {
                                       {1,1,1,1,1,1,1,1,1,1},
                                       {1,0,0,0,3,0,0,3,0,1},
                                       {1,0,0,0,0,0,0,0,0,1},
                                       {1,0,0,0,3,0,0,3,0,1},
                                       {1,1,9,8,1,1,1,1,1,1},
                                       };

    public static int[][] houseStruct = {
                                       {1,1,1,1,1,1},
                                       {1,0,0,0,0,1},
                                       {1,0,0,0,3,1},
                                       {1,0,0,0,0,1},
                                       {1,1,9,1,1,1}
                                       };

    public static int[][] foodStoreStruct = {
                                       {1,1,1,1,1},
                                       {1,0,0,0,1},
                                       {1,2,0,0,1},
                                       {1,0,0,0,1},
                                       {1,1,9,8,1}
                                       };

    public static int[][] toolStoreStruct = {
                                       {1,1,1,1,1},
                                       {1,0,0,0,1},
                                       {1,0,0,2,1},
                                       {1,0,0,0,1},
                                       {1,1,9,8,1}
                                       };


    public static int getWidth(BuildingType type)
    {
        switch(type)
        {
            case BANK:
            return 7;

            case INN:
            return 10;

            case HOUSE:
            return 6;

            case FOOD_STORE:
            return 5;

            case TOOL_STORE:
            return 5;

            default:
            return 0;
        }
    }

    public static int getHeight(BuildingType type)
    {
        switch(type)
        {
            case BANK:
            return 6;

            case INN:
            return 5;

            case HOUSE:
            return 5;

            case FOOD_STORE:
            return 5;

            case TOOL_STORE:
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
        switch(building.getBuildingType())
        {
            case BANK:
            return NameMaker.makeBankName();

            case FOOD_STORE:
            return NameMaker.makeFoodName();
            
        }
        return building.getBuildingType().name();
    }


    public static void build(Region region, int x, int y, Building building)
    {
        switch(building.getBuildingType())
        {
            case BANK:
            buildByArray(region,x,y,building,bankStruct);
            break;

            case INN:
            buildByArray(region,x,y,building,innStruct);
            building.setLit(false);
            break;

            case HOUSE:
            buildByArray(region,x,y,building,houseStruct);
            break;

            case FOOD_STORE:
            buildByArray(region,x,y,building,foodStoreStruct);
            break;

            case TOOL_STORE:
            buildByArray(region,x,y,building,toolStoreStruct);
            break;
        }
    }

    public static void buildByArray(Region region, int startX, int startY, Building building, int[][] plan)
    {
        for(int x=0; x < BuildingManager.getWidth(building.getBuildingType()); x++)
        {
            for(int y=0; y < BuildingManager.getHeight(building.getBuildingType()); y++)
            {
                if(x + startX < region.getWidth() && x + startX > 0
                && y + startY < region.getHeight() && y + startY > 0)
                {
                    switch(plan[y][x])
                    {
                        case 0:
                        region.getTerrain(startX + x, startY + y).setCode(TerrainManager.TerrainCode.STONE_FLOOR);
                        break;

                        case 1:
                        region.getTerrain(startX + x, startY + y).setCode(TerrainManager.TerrainCode.STONE_WALL);
                        break;

                        case 9:
                        region.getTerrain(startX + x, startY + y).setCode(TerrainManager.TerrainCode.STONE_FLOOR);
                        region.getTerrain(startX + x, startY + y).getParameters().put(TerrainManager.TerrainParameter.HAS_DOOR, DoorCode.CLOSED.name());
                        building.setDoor(startX + x, startY + y);
                        break;

                        case 8:
                        region.getTerrain(startX + x, startY + y).setCode(TerrainManager.TerrainCode.STONE_WALL);
                        region.getTerrain(startX + x, startY + y).getParameters().put(TerrainManager.TerrainParameter.HAS_SIGN, building.getName());
                        break;

                        case 2:
                        region.getTerrain(startX + x, startY + y).setCode(TerrainManager.TerrainCode.STONE_FLOOR);
                        Monster newMon = new Monster();
                        switch(building.getBuildingType())
                        {
                            case BANK:
                            newMon.setMonsterCode(MonsterCode.HUMAN);
                            newMon.setRole(MonsterActionManager.MonsterRole.BANKER);
                            break;

                            case INN:
                            newMon.setMonsterCode(MonsterCode.HUMAN);
                            newMon.setRole(MonsterActionManager.MonsterRole.NULL);
                            break;

                            case FOOD_STORE:
                            newMon.setMonsterCode(MonsterCode.HUMAN);
                            newMon.setRole(MonsterActionManager.MonsterRole.CHEF);
                            break;

                            case TOOL_STORE:
                            newMon.setMonsterCode(MonsterCode.HUMAN);
                            newMon.setRole(MonsterActionManager.MonsterRole.HANDYMAN);
                            break;
                        }
                        MonsterActionManager.initialize(newMon);
                        newMon.setPosition(startX + x, startY + y);
                        region.getMonsters().add(newMon);
                        break;

                        case 3:
                        //Add a bed
                        region.getTerrain(startX + x, startY + y).setCode(TerrainManager.TerrainCode.STONE_FLOOR);
                        region.getTerrain(startX + x, startY + y).getParameters().put(TerrainManager.TerrainParameter.HAS_BED, "");
                        break;
                    }
                }
            }
        }
    }
}
