package quinoa.generator;

import quinoa.region.Position;
import java.util.ArrayList;
import java.util.UUID;
import quinoa.Quinoa;
import quinoa.generator.Chamber.ChamberType;
import quinoa.region.OverworldCell.CellType;
import quinoa.items.Item;
import quinoa.items.ItemManager.ItemClass;
import quinoa.monsters.Monster;
import quinoa.monsters.Monster.Direction;
import quinoa.monsters.MonsterActionManager;
import quinoa.monsters.MonsterActionManager.MonsterCode;
import quinoa.monsters.MonsterInventory.ItemSlot;
import quinoa.region.Building;
import quinoa.region.BuildingManager;
import quinoa.region.BuildingManager.BuildingType;
import quinoa.region.OverworldCell;
import quinoa.region.Region;
import quinoa.region.RegionExit;
import quinoa.region.RegionExit.ExitDecorator;
import quinoa.region.RegionHeader;
import quinoa.region.Terrain;
import quinoa.region.TerrainManager;
import quinoa.region.TerrainManager.GraveCode;
import quinoa.region.TerrainManager.MushroomSporeCode;
import quinoa.region.TerrainManager.TerrainCode;
import quinoa.region.TerrainManager.TreeCode;

public class MapGenerator implements MapGeneratorInterface
{
    public ArrayList<String> caveBottomIDs;

    public MapGenerator()
    {
        caveBottomIDs = new ArrayList<String>();
    }

    public void placeBrother(Quinoa quinoa) throws Exception
    {
        String caveID = caveBottomIDs.get((int)(Math.random() * caveBottomIDs.size()));

        RegionHeader header = quinoa.getMap().getRegionHeaderByID(caveID);
        header.recallRegion();

        ArrayList<Position> tempPos = MapGenerator.getTerrainPositions(TerrainCode.STONE_FLOOR, header.getRegion(), false);
        Position pos = tempPos.get((int)(Math.random() * tempPos.size()));

        Monster monster = new Monster();
        monster.setMonsterCode(MonsterCode.HUMAN);
        MonsterActionManager.initialize(monster);
        monster.setRole(MonsterActionManager.MonsterRole.BROTHER);
        monster.setPosition(pos.x, pos.y);

        Item lantern = new Item();
        lantern.setItemClass(ItemClass.LANTERN);
        monster.getInventory().equipItem(lantern, ItemSlot.BELT_1);

        header.getRegion().getMonsters().add(monster);

        header.storeRegion(true);
    }

    public static void makePath(Region region, boolean nExit, boolean eExit, boolean sExit, boolean wExit)
    {
        int pathX = 0;
        int pathY = 0;

        region.getTerrain((int)(region.getWidth() / 2), (int)(region.getHeight() / 2)).setCode(TerrainManager.TerrainCode.PATH);
        region.getTerrain((int)(region.getWidth() / 2)-1, (int)(region.getHeight() / 2)).setCode(TerrainManager.TerrainCode.PATH);
        region.getTerrain((int)(region.getWidth() / 2)+1, (int)(region.getHeight() / 2)).setCode(TerrainManager.TerrainCode.PATH);
        region.getTerrain((int)(region.getWidth() / 2), (int)(region.getHeight() / 2)-1).setCode(TerrainManager.TerrainCode.PATH);
        region.getTerrain((int)(region.getWidth() / 2), (int)(region.getHeight() / 2)+1).setCode(TerrainManager.TerrainCode.PATH);

        if(nExit)
        {
            pathX = (int)(region.getWidth() / 2);
            for(pathY = 0; pathY < region.getHeight() / 2; pathY++)
            {
                region.getTerrain(pathX, pathY).setCode(TerrainManager.TerrainCode.PATH);
                region.getTerrain(pathX-1, pathY).setCode(TerrainManager.TerrainCode.PATH);
                region.getTerrain(pathX+1, pathY).setCode(TerrainManager.TerrainCode.PATH);
            }
         }

        if(eExit)
        {
            pathY = (int)(region.getWidth() / 2);
            for(pathX = 0; pathX < region.getWidth() / 2; pathX++)
            {
                region.getTerrain((region.getWidth()/2) + pathX, pathY).setCode(TerrainManager.TerrainCode.PATH);
                region.getTerrain((region.getWidth()/2) + pathX, pathY-1).setCode(TerrainManager.TerrainCode.PATH);
                region.getTerrain((region.getWidth()/2) + pathX, pathY+1).setCode(TerrainManager.TerrainCode.PATH);
            }
        }

        if(sExit)
        {
            pathX = (int)(region.getWidth() / 2);
            for(pathY = 0; pathY < region.getHeight() / 2; pathY++)
            {
                region.getTerrain(pathX, (region.getHeight()/2) + pathY).setCode(TerrainManager.TerrainCode.PATH);
                region.getTerrain(pathX-1, (region.getHeight()/2) + pathY).setCode(TerrainManager.TerrainCode.PATH);
                region.getTerrain(pathX+1, (region.getHeight()/2) + pathY).setCode(TerrainManager.TerrainCode.PATH);
            }
         }

        if(wExit)
        {
            pathY = (int)(region.getWidth() / 2);
            for(pathX = 0; pathX < region.getWidth() / 2; pathX++)
            {
                region.getTerrain(pathX, pathY).setCode(TerrainManager.TerrainCode.PATH);
                region.getTerrain(pathX, pathY-1).setCode(TerrainManager.TerrainCode.PATH);
                region.getTerrain(pathX, pathY+1).setCode(TerrainManager.TerrainCode.PATH);
            }
        }
    }

    public static void addGraveyard(int width, int height, int stx, int sty, Region region)
    {
        int treesPlaced = 0;

        region.getTerrain(stx, sty).setCode(TerrainCode.ROCK);
        TerrainManager.removeParameters(region.getTerrain(stx, sty));
        region.getTerrain(stx + width, sty).setCode(TerrainCode.ROCK);
        TerrainManager.removeParameters(region.getTerrain(stx + width, sty));
        region.getTerrain(stx, sty + height).setCode(TerrainCode.ROCK);
        TerrainManager.removeParameters(region.getTerrain(stx, sty + height));
        region.getTerrain(stx + width, sty + height).setCode(TerrainCode.ROCK);
        TerrainManager.removeParameters(region.getTerrain(stx + width, sty + height));

        for(int x=stx; x < stx + width; x++)
        {
            for(int y=sty; y < sty + height; y++)
            {
                if(x >= 0 && x < region.getWidth()
                && y >= 0 && y < region.getHeight())
                {
                    if(x % 2 == 0 && y % 3 == 0)
                    {
                        if(Math.random() < 0.75)
                        {
                            if(Math.random() < 0.01)
                            {
                                region.getTerrain(x, y).getParameters().put(TerrainManager.TerrainParameter.HAS_GRAVE, GraveCode.SPECIAL.name());
                            }
                            else
                            {
                                region.getTerrain(x, y).getParameters().put(TerrainManager.TerrainParameter.HAS_GRAVE, GraveCode.NORMAL.name());
                            }
                        }
                        else
                        {
                            if(region.getTerrain(x, y).getCode() == TerrainCode.GRASS)
                            {
                                if(Math.random() < 0.25 && treesPlaced < 2)
                                {
                                    region.getTerrain(x, y).getParameters().put(TerrainManager.TerrainParameter.HAS_TREE, TreeCode.APPLE_TREE.name());
                                    treesPlaced++;
                                }

                            }
                            else if(region.getTerrain(x, y).getCode() == TerrainCode.STONE_FLOOR)
                            {
                                if(Math.random() < 0.25 && treesPlaced < 5)
                                {
                                    region.getTerrain(x, y).getParameters().put(TerrainManager.TerrainParameter.HAS_MOSS, "");
                                    treesPlaced++;
                                }
                            }
                        }
                    }
                    else
                    {
                        //do nothing
                    }
                }
            }
        }
    }


    public static void addTrees(Region region, double treeDensity)
    {
        ArrayList<Position> grassTiles = MapGenerator.getTerrainPositions(TerrainCode.GRASS, region, false);
        int treeCount = (int)(grassTiles.size() * treeDensity);
        for(int i=0; i < treeCount; i++)
        {
            if(!grassTiles.isEmpty())
            {
                Position pos = grassTiles.get((int)(Math.random() * grassTiles.size()));
                region.getTerrain(pos.x, pos.y).getParameters().put(TerrainManager.TerrainParameter.HAS_TREE, TerrainManager.getRandomTree().name());
                region.getTerrain(pos.x, pos.y).getParameters().put(TerrainManager.TerrainParameter.DAMAGE, "0");
                grassTiles.remove(pos);
            }
        }
    }


    public static void addMushroomSpores(Region region, int sporeCount)
    {
        ArrayList<Position> grassTiles = MapGenerator.getTerrainPositions(TerrainCode.GRASS, region, false);
        for(int i=0; i < sporeCount; i++)
        {
            if(!grassTiles.isEmpty())
            {
                Position pos = grassTiles.get((int)(Math.random() * grassTiles.size()));
                Terrain terrain = region.getTerrain(pos.x, pos.y);
                MushroomSporeCode msc = TerrainManager.MushroomSporeCode.values()[(int)(Math.random() * TerrainManager.MushroomSporeCode.values().length)];
                terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES, msc.name());
            }
        }
    }


    public static void addClover(Region region, int cloverCount)
    {
        ArrayList<Position> grassTiles = MapGenerator.getTerrainPositions(TerrainCode.GRASS, region, false);
        for(int i=0; i < cloverCount; i++)
        {
            if(!grassTiles.isEmpty())
            {
                Position pos = grassTiles.get((int)(Math.random() * grassTiles.size()));
                Terrain terrain = region.getTerrain(pos.x, pos.y);
                int cloverGrowCount = (int)(Math.random() * (TerrainManager.CLOVER_GROW_COUNT / 4)) + TerrainManager.CLOVER_GROW_COUNT;
                terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_CLOVER, cloverGrowCount+"");
            }
        }
    }

    public static ArrayList<Position> getTerrainPositions(TerrainCode code, Region region, boolean allowParameters)
    {
        return MapGenerator.getTerrainPositions(code, region, allowParameters, true);
    }


    public static ArrayList<Position> getTerrainPositions(TerrainCode code, Region region, boolean allowParameters, boolean includeEdges)
    {
        int edgeAdjust=0;
        if(!includeEdges)
        {
            edgeAdjust=1;
        }
        ArrayList<Position> positions = new ArrayList<Position>();
        for(int x=0+edgeAdjust; x < region.getWidth() - (edgeAdjust * 2); x++)
        {
            for(int y=0+edgeAdjust; y < region.getHeight() - (edgeAdjust * 2); y++)
            {
                if(region.getTerrain(x, y).getCode() == code
                && ((!allowParameters && region.getTerrain(x, y).getParameters().isEmpty()) || allowParameters))
                {
                    positions.add(new Position(x,y));
                }
            }
        }
        
        return positions;
    }


    public void initializePlayer(Quinoa quinoa)
    {
        quinoa.setPlayer(new Monster());
        quinoa.getPlayer().setMonsterCode(MonsterCode.HUMAN);
        quinoa.getPlayer().setID(MonsterActionManager.PLAYER_ID);
        quinoa.getPlayer().setPosition(50, 50);

        Item boots = new Item();
        boots.setItemClass(ItemClass.BOOTS);
        quinoa.getPlayer().getInventory().addItem(boots);

        Item jacket = new Item();
        jacket.setItemClass(ItemClass.JACKET);
        quinoa.getPlayer().getInventory().addItem(jacket);

        Item axe = new Item();
        axe.setItemClass(ItemClass.AXE);
        quinoa.getPlayer().getInventory().addItem(axe);

        Item pickaxe = new Item();
        pickaxe.setItemClass(ItemClass.PICKAXE);
        quinoa.getPlayer().getInventory().addItem(pickaxe);

        Item flapjacks = new Item();
        flapjacks.setItemClass(ItemClass.FLAPJACKS);
        flapjacks.setStackSize(4);
        quinoa.getPlayer().getInventory().addItem(flapjacks);

        Item lantern = new Item();
        lantern.setItemClass(ItemClass.LANTERN);
        quinoa.getPlayer().getInventory().addItem(lantern);

        if(Quinoa.DEBUG_MODE)
        {
            Item hat = new Item();
            hat.setItemClass(ItemClass.HAT);
            quinoa.getPlayer().getInventory().addItem(hat);
        }

        Item torch = new Item();
        torch.setItemClass(ItemClass.TORCH);
        torch.setStackSize(25);
        quinoa.getPlayer().getInventory().addItem(torch);

        Item bucket = new Item();
        bucket.setItemClass(ItemClass.BUCKET);
        quinoa.getPlayer().getInventory().addItem(bucket);

        Item shovel = new Item();
        shovel.setItemClass(ItemClass.SHOVEL);
        quinoa.getPlayer().getInventory().addItem(shovel);

        Item tent = new Item();
        tent.setItemClass(ItemClass.TENT);
        quinoa.getPlayer().getInventory().addItem(tent);

        Item ash = new Item();
        ash.setItemClass(ItemClass.ASH);
        ash.setStackSize(35);
        quinoa.getPlayer().getInventory().addItem(ash);


        if(Quinoa.DEBUG_MODE)
        {
            Item corn = new Item();
            corn.setItemClass(ItemClass.CORN);
            corn.setStackSize(corn.getMaxStackSize());
            quinoa.getPlayer().getInventory().addItem(corn);
        }

        Item cornSeed = new Item();
        cornSeed.setItemClass(ItemClass.CORN_SEED);
        cornSeed.setStackSize(10);
        quinoa.getPlayer().getInventory().addItem(cornSeed);

        Item pumpkin = new Item();
        pumpkin.setItemClass(ItemClass.PUMPKIN);
        pumpkin.setStackSize(1);
        quinoa.getPlayer().getInventory().addItem(pumpkin);

        Item pumpkinSeed = new Item();
        pumpkinSeed.setItemClass(ItemClass.PUMPKIN_SEED);
        pumpkinSeed.setStackSize(10);
        quinoa.getPlayer().getInventory().addItem(pumpkinSeed);

        if(Quinoa.DEBUG_MODE)
        {
            Item floodgate = new Item();
            floodgate.setItemClass(ItemClass.FLOODGATE);
            floodgate.setStackSize(floodgate.getMaxStackSize());
            quinoa.getPlayer().getInventory().addItem(floodgate);
        }

        if(Quinoa.DEBUG_MODE)
        {
            Item mop = new Item();
            mop.setItemClass(ItemClass.MOP);
            mop.setStackSize(mop.getMaxStackSize());
            quinoa.getPlayer().getInventory().addItem(mop);
        }
    }


    public void connectExits(OverworldCell owc1, OverworldCell owc2, Direction exitDirection) throws Exception
    {
        int exitX = 0;
        int exitY = 0;
        int dX = 0;
        int dY = 0;

        owc1.header.recallRegion();
        owc2.header.recallRegion();
                
        switch(exitDirection)
        {
            case N:
            exitX = (int)(owc1.header.getRegion().getWidth() / 2);    
            exitY = 0;
            dX = (int)(owc2.header.getRegion().getWidth() / 2);
            dY = owc2.header.getRegion().getHeight() - 2;
            owc1.header.getExits().add(new RegionExit(exitX, exitY,dX,dY,owc2.header.getId(), ExitDecorator.NONE));
            break;
            
            case E:
            exitX = owc1.header.getRegion().getWidth() - 1;
            exitY = (owc1.header.getRegion().getHeight() / 2);
            dX = 1;
            dY = (owc2.header.getRegion().getHeight() / 2);
            owc1.header.getExits().add(new RegionExit(exitX, exitY,dX,dY,owc2.header.getId(), ExitDecorator.NONE));
            break;
            
            case S:
            exitX = (int)(owc1.header.getRegion().getWidth() / 2);    
            exitY = owc1.header.getRegion().getHeight() - 1;
            dX = (int)(owc2.header.getRegion().getWidth() / 2);
            dY = 1;
            owc1.header.getExits().add(new RegionExit(exitX, exitY,dX,dY,owc2.header.getId(), ExitDecorator.NONE));
            break;
            
            case W:
            exitX = 0;    
            exitY = (owc1.header.getRegion().getHeight() / 2);
            dX = owc2.header.getRegion().getWidth() - 2;
            dY = (owc2.header.getRegion().getHeight() / 2);
            owc1.header.getExits().add(new RegionExit(exitX, exitY,dX,dY,owc2.header.getId(), ExitDecorator.NONE));
            break;
        }

        owc1.header.storeRegion(true);
        owc2.header.storeRegion(true);
    }


    public void generate(Quinoa quinoa) throws Exception
    {
        //Fill the overworld cell array
        int owWidth = quinoa.getMap().getOverworldWidth();
        int owHeight = quinoa.getMap().getOverworldHeight();
        OverworldCell[][] owc = quinoa.getMap().getOverworldCells();
        for(int x=0; x < owWidth; x++)
        {
            for(int y=0; y < owHeight; y++)
            {
                owc[x][y] = new OverworldCell();
            }
        }

        //choose a random spot to be town, set as cross
        int townX = owWidth / 2 + ((int)(Math.random() * 3) - 1);
        int townY = owHeight / 2 + ((int)(Math.random() * 3) - 1);
        owc[townX][townY].cellType = CellType.MAIN_TOWN;
        owc[townX][townY].nExit = true;
        owc[townX][townY].eExit = true;
        owc[townX][townY].sExit = true;
        owc[townX][townY].wExit = true;

        //generate paths recursively
        this.addPathBranch(townX-1, townY, owWidth, owHeight, owc, 0, 2, 7);
        this.addPathBranch(townX+1, townY, owWidth, owHeight, owc, 0, 2, 7);
        this.addPathBranch(townX, townY-1, owWidth, owHeight, owc, 0, 2, 7);
        this.addPathBranch(townX, townY+1, owWidth, owHeight, owc, 0, 2, 7);

        //generate regions for each overworld cell
        for(int x=0; x < owWidth; x++)
        {
            for(int y=0; y < owHeight; y++)
            {
                if(owc[x][y].cellType == CellType.MAIN_TOWN)
                {
                    TownGenerator townGen = new TownGenerator(100, 100, "main", 3, 0.75, 0.01, owc[x][y], quinoa);
                    townGen.generate();
                    owc[x][y].header = townGen.header;
                    townGen.header.storeRegion(true);
                    this.addCaveBranch(townGen.header, 14, 14, 3 + (int)(Math.random() * 3), quinoa);
                    quinoa.getMap().addRegionHeader(owc[x][y].header);
                }
                else if(owc[x][y].cellType == CellType.TOWN)
                {
                    TownGenerator townGen = new TownGenerator(100, 100, "town" + UUID.randomUUID().toString(), 3, 0.75, 0.01, owc[x][y], quinoa);
                    townGen.generate();
                    owc[x][y].header = townGen.header;
                    townGen.header.storeRegion(true);
                    quinoa.getMap().addRegionHeader(owc[x][y].header);
                }
                else if(owc[x][y].cellType == CellType.FOREST)
                {
                    String forestName = "forest" + UUID.randomUUID().toString();

                    int depthCount = 0;
                    int depthTotal = 0;
                    if(owc[x][y].nExit)
                    {
                        depthCount++;
                        depthTotal = depthTotal + owc[x][y-1].depth;
                    }
                    if(owc[x][y].eExit)
                    {
                        depthCount++;
                        depthTotal = depthTotal + owc[x + 1][y].depth;
                    }
                    if(owc[x][y].sExit)
                    {
                        depthCount++;
                        depthTotal = depthTotal + owc[x][y+1].depth;
                    }
                    if(owc[x][y].wExit)
                    {
                        depthCount++;
                        depthTotal = depthTotal + owc[x-1][y].depth;
                    }

                    depthTotal = depthTotal + owc[x][y].depth;
                    depthCount = depthCount + 1;

                    double treeDensity = 0.01 + ((depthTotal / depthCount) * 0.02);
                    if(treeDensity > 0.55)
                    {
                        treeDensity = 0.55;
                    }
                    ForestGenerator forGen = new ForestGenerator(100, 100, forestName, treeDensity, quinoa);
                    double waterDensity = 0.15 + (Math.random() * 0.35);
                    if(waterDensity > 0.65)
                    {
                        waterDensity = 0.65;
                    }
                    forGen.addWater(waterDensity, 4);
                    forGen.generate(owc[x][y].nExit, owc[x][y].eExit, owc[x][y].sExit, owc[x][y].wExit);
                    forGen.addSprings();
                    forGen.header.setName("Forest");
                    owc[x][y].header = forGen.header;

                    //add cave branches if only one exit is present
                    int exitCount = 0;
                    if(owc[x][y].nExit) {exitCount++;}
                    if(owc[x][y].eExit) {exitCount++;}
                    if(owc[x][y].sExit) {exitCount++;}
                    if(owc[x][y].wExit) {exitCount++;}
                    if(exitCount == 1 && Math.random() < 0.50)
                    {
                        this.addCaveBranch(owc[x][y].header, owc[x][y].header.getRegion().getWidth() / 2, owc[x][y].header.getRegion().getHeight() / 2, 2 + (int)(Math.random() * (depthTotal / depthCount)), quinoa);
                    }

                    forGen.header.storeRegion(true);
                    quinoa.getMap().addRegionHeader(owc[x][y].header);
                }
            }
        }

        //connect exits
        for(int x=0; x < owWidth; x++)
        {
            for(int y=0; y < owHeight; y++)
            {
                if(owc[x][y].header != null)
                {
                    if(owc[x][y].nExit && y > 0)
                    {
                        this.connectExits(owc[x][y], owc[x][y-1], Direction.N);
                    }
                    if(owc[x][y].sExit && y < owHeight-1)
                    {
                        this.connectExits(owc[x][y], owc[x][y+1], Direction.S);
                    }
                    if(owc[x][y].eExit && x < owWidth-1)
                    {
                        this.connectExits(owc[x][y], owc[x+1][y], Direction.E);
                    }
                    if(owc[x][y].wExit && x > 0)
                    {
                        this.connectExits(owc[x][y], owc[x-1][y], Direction.W);
                    }
                }
            }
        }

        //place the brother
        this.placeBrother(quinoa);
                
        this.initializePlayer(quinoa);
    }

    public void addPathBranch(int x, int y, int width, int height, OverworldCell[][] owc, int depth, int branchCountdown, int townCounter)
    {
        int townCountdown = townCounter;

        if(owc[x][y].cellType != CellType.NULL)
        {
            return;
        }
        else
        {
            //figure out vital connections
            owc[x][y].cellType = CellType.FOREST;
            owc[x][y].depth = depth;
            if(y > 0 && owc[x][y-1].sExit)
            {
                owc[x][y].nExit = true;
            }

            if(y < height-1 && owc[x][y+1].nExit)
            {
                owc[x][y].sExit = true;
            }

            if(x > 0 && owc[x-1][y].eExit)
            {
                owc[x][y].wExit = true;
            }

            if(x < width-1 && owc[x+1][y].wExit)
            {
                owc[x][y].eExit = true;
            }


            //randomly try different directions
            //try to lower amount of branching by only allowing up to two exit follows
            int exitCount = 0;
            if(owc[x][y].nExit)
            {
                exitCount++;
            }
            if(owc[x][y].eExit)
            {
                exitCount++;
            }
            if(owc[x][y].sExit)
            {
                exitCount++;
            }
            if(owc[x][y].wExit)
            {
                exitCount++;
            }

            if(townCountdown == 0)
            {
                owc[x][y].cellType = CellType.TOWN;
                townCountdown = 6 + (int)(Math.random() * 3);
            }

            branchCountdown = branchCountdown - 1;
            int branchMax = 2 + (int)(Math.random() * 4);
            if(branchCountdown > 0)
            {
                if(owc[x][y].nExit && y < height - 1)
                {
                    owc[x][y].sExit = true;
                    owc[x][y+1].nExit = true;
                    this.addPathBranch(x, y+1, width, height, owc, depth + 1, branchCountdown - 1, townCountdown - 1);
                }
                else if(owc[x][y].sExit && y > 0)
                {
                    owc[x][y].nExit = true;
                    owc[x][y-1].sExit = true;
                    this.addPathBranch(x, y-1, width, height, owc, depth + 1, branchCountdown - 1, townCountdown - 1);
                }

                if(owc[x][y].eExit && x > 0)
                {
                    owc[x][y].wExit = true;
                    owc[x-1][y].eExit = true;
                    this.addPathBranch(x-1, y, width, height, owc, depth + 1, branchCountdown - 1, townCountdown - 1);
                }
                else if(owc[x][y].wExit && x < width - 1)
                {
                    owc[x][y].eExit = true;
                    owc[x+1][y].wExit = true;
                    this.addPathBranch(x+1, y, width, height, owc, depth + 1, branchCountdown - 1, townCountdown - 1);
                }

            }
            else
            {
                //branch off in any direction
                int exitCutoff = 2;
                double branchChance = 0.35;
                
                if(exitCount < exitCutoff && y > 0 && owc[x][y].nExit == false && Math.random() < branchChance)
                {
                    owc[x][y].nExit = true;
                    owc[x][y-1].sExit = true;
                    this.addPathBranch(x, y-1, width, height, owc, depth + 1, branchMax, townCountdown - 1);
                }

                if(exitCount < exitCutoff && y < height-1 && owc[x][y].sExit == false && Math.random() < branchChance)
                {
                    owc[x][y].sExit = true;
                    owc[x][y+1].nExit = true;
                    this.addPathBranch(x, y+1, width, height, owc, depth + 1, branchMax, townCountdown - 1);
                }

                if(exitCount < exitCutoff && x > 0 && owc[x][y].wExit == false && Math.random() < branchChance)
                {
                    owc[x][y].wExit = true;
                    owc[x - 1][y].eExit = true;
                    this.addPathBranch(x - 1, y, width, height, owc, depth + 1, branchMax, townCountdown - 1);
                }

                if(exitCount < exitCutoff && x < width-1 && owc[x][y].eExit == false && Math.random() < branchChance)
                {
                    owc[x][y].eExit = true;
                    owc[x + 1][y].wExit = true;
                    this.addPathBranch(x + 1, y, width, height, owc, depth + 1, branchMax, townCountdown - 1);
                }
            }
        }
    }


    public void addCaveBranch(RegionHeader topLevel, int topX, int topY, int depth, Quinoa quinoa) throws Exception
    {
        CaveGenerator lastGen = null;

        for(int deep=0; deep < depth; deep++)
        {
            int width = 80 + (int)(Math.random() * 40);
            int height = 80 + (int)(Math.random() * 40);
            int chamberCount = 3 + (int)(Math.random() * 4);
            int smoothness = (int)(Math.random() * 2);
            double fillPercentage = 0.45 + (Math.random() * 0.20);

            CaveGenerator caveGen = new CaveGenerator(width, height, topLevel.getId() + "-cave" + deep, fillPercentage, smoothness, quinoa);
            caveGen.addChamber(new Chamber(5,5,caveGen.region.getWidth() - 10,caveGen.region.getHeight() - 10, ChamberType.OPEN));
            for(int i=0; i < chamberCount; i++)
            {
                int caveX = 5 + (int)(Math.random() * (caveGen.region.getWidth() - 41));
                int caveY = 5 + (int)(Math.random() * (caveGen.region.getHeight() - 41));

                ChamberType type = null;
                if(Math.random() < 0.20)
                {
                    type = ChamberType.MUSHROOM;
                }
                else
                {
                    type = ChamberType.FLOODED;
                }
                caveGen.addChamber(new Chamber(caveX,caveY,(int)(Math.random() * 20) + 10,(int)(Math.random() * 20) + 10, type));
            }
            caveGen.smoothWater();
            caveGen.placeExits();
            caveGen.addTorches();
            caveGen.addMinerals(deep+1);

            if(deep == depth - 1)
            {
                caveGen.addTreasure();
                this.caveBottomIDs.add(caveGen.header.getId());
            }

            caveGen.header.setName("Cave (level " + (deep + 1) + ")");
            caveGen.header.setRegion(caveGen.region);
            caveGen.header.storeRegion(true);


            if(deep == 0)
            {
                caveGen.getEntrance().setDx(topX);
                caveGen.getEntrance().setDy(topY+1);
                caveGen.getEntrance().setDestinationRegionID(topLevel.getId());

                topLevel.getExits().add(new RegionExit(topX,topY,caveGen.getEntrance().getX(),caveGen.getEntrance().getY()+1,caveGen.header.getId(), ExitDecorator.CAVE));
            }
            else
            {
                caveGen.getEntrance().setDx(lastGen.exit.getX());
                caveGen.getEntrance().setDy(lastGen.exit.getY()+1);
                caveGen.getEntrance().setDestinationRegionID(lastGen.header.getId());

                lastGen.exit.setDx(caveGen.entrance.getX());
                lastGen.exit.setDy(caveGen.entrance.getY()+1);
                lastGen.exit.setDestinationRegionID(caveGen.header.getId());

                

                if(deep == depth-1)
                {
                    caveGen.header.setName("Cave (bottom level)");
                    caveGen.header.getExits().remove(caveGen.exit);
                }
            }

            

            quinoa.getMap().addRegionHeader(caveGen.header);

            lastGen = caveGen;
        }
    }
}
