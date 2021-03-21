using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Utilities;
using LumberjackRL.Core.Map;
using LumberjackRL.Core.Monsters;
using LumberjackRL.Core.Items;

namespace LumberjackRL.Core.Generator
{
    public class MapGenerator : IMapGenerator
    {
        public List<String> caveBottomIDs;
        private RandomNumberGenerator rng = new RandomNumberGenerator();

        public MapGenerator()
        {
            caveBottomIDs = new List<String>();
        }

        public void placeBrother(Quinoa quinoa)
        {
            String caveID = caveBottomIDs[rng.RandomInteger(caveBottomIDs.Count)];

            RegionHeader header = quinoa.getMap().getRegionHeaderByID(caveID);
            header.recallRegion();

            Position pos = new Position();
            List<Position> tempPos = MapGenerator.getTerrainPositions(TerrainCode.STONE_FLOOR, header.getRegion(), false);
            if (tempPos.Count > 0)
            {
                pos = tempPos[rng.RandomInteger(tempPos.Count)];
            }

            Monster monster = new Monster();
            monster.monsterCode = MonsterClassType.HUMAN;
            MonsterActionManager.initialize(monster);
            monster.role = MonsterRoleType.BROTHER;
            monster.setPosition(pos.x, pos.y);

            Item lantern = new Item();
            lantern.itemClass = ItemClassType.LANTERN;
            monster.inventory.equipItem(lantern, MonsterItemSlotType.BELT_1);

            header.getRegion().getMonsters().Add(monster);

            header.storeRegion(true);
        }

        public static void makePath(Region region, bool nExit, bool eExit, bool sExit, bool wExit)
        {
            int pathX = 0;
            int pathY = 0;

            region.getTerrain((int)(region.getWidth() / 2), (int)(region.getHeight() / 2)).setCode(TerrainCode.PATH);
            region.getTerrain((int)(region.getWidth() / 2)-1, (int)(region.getHeight() / 2)).setCode(TerrainCode.PATH);
            region.getTerrain((int)(region.getWidth() / 2)+1, (int)(region.getHeight() / 2)).setCode(TerrainCode.PATH);
            region.getTerrain((int)(region.getWidth() / 2), (int)(region.getHeight() / 2)-1).setCode(TerrainCode.PATH);
            region.getTerrain((int)(region.getWidth() / 2), (int)(region.getHeight() / 2)+1).setCode(TerrainCode.PATH);

            if(nExit)
            {
                pathX = (int)(region.getWidth() / 2);
                for(pathY = 0; pathY < region.getHeight() / 2; pathY++)
                {
                    region.getTerrain(pathX, pathY).setCode(TerrainCode.PATH);
                    region.getTerrain(pathX-1, pathY).setCode(TerrainCode.PATH);
                    region.getTerrain(pathX+1, pathY).setCode(TerrainCode.PATH);
                }
             }

            if(eExit)
            {
                pathY = (int)(region.getWidth() / 2);
                for(pathX = 0; pathX < region.getWidth() / 2; pathX++)
                {
                    region.getTerrain((region.getWidth()/2) + pathX, pathY).setCode(TerrainCode.PATH);
                    region.getTerrain((region.getWidth()/2) + pathX, pathY-1).setCode(TerrainCode.PATH);
                    region.getTerrain((region.getWidth()/2) + pathX, pathY+1).setCode(TerrainCode.PATH);
                }
            }

            if(sExit)
            {
                pathX = (int)(region.getWidth() / 2);
                for(pathY = 0; pathY < region.getHeight() / 2; pathY++)
                {
                    region.getTerrain(pathX, (region.getHeight()/2) + pathY).setCode(TerrainCode.PATH);
                    region.getTerrain(pathX-1, (region.getHeight()/2) + pathY).setCode(TerrainCode.PATH);
                    region.getTerrain(pathX+1, (region.getHeight()/2) + pathY).setCode(TerrainCode.PATH);
                }
             }

            if(wExit)
            {
                pathY = (int)(region.getWidth() / 2);
                for(pathX = 0; pathX < region.getWidth() / 2; pathX++)
                {
                    region.getTerrain(pathX, pathY).setCode(TerrainCode.PATH);
                    region.getTerrain(pathX, pathY-1).setCode(TerrainCode.PATH);
                    region.getTerrain(pathX, pathY+1).setCode(TerrainCode.PATH);
                }
            }
        }

        public static void addGraveyard(int width, int height, int stx, int sty, Region region)
        {
            int treesPlaced = 0;
            RandomNumberGenerator grng = new RandomNumberGenerator();


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
                            if(grng.RandomDouble() < 0.75)
                            {
                                if(grng.RandomDouble() < 0.01)
                                {
                                    region.getTerrain(x, y).getParameters().Add(TerrainParameter.HAS_GRAVE, GraveCode.SPECIAL.ToString());
                                }
                                else
                                {
                                    region.getTerrain(x, y).getParameters().Add(TerrainParameter.HAS_GRAVE, GraveCode.NORMAL.ToString());
                                }
                            }
                            else
                            {
                                if(region.getTerrain(x, y).getCode() == TerrainCode.GRASS)
                                {
                                    if(grng.RandomDouble() < 0.25 && treesPlaced < 2)
                                    {
                                        region.getTerrain(x, y).getParameters().Add(TerrainParameter.HAS_TREE, TreeCode.APPLE_TREE.ToString());
                                        treesPlaced++;
                                    }

                                }
                                else if(region.getTerrain(x, y).getCode() == TerrainCode.STONE_FLOOR)
                                {
                                    if(grng.RandomDouble() < 0.25 && treesPlaced < 5)
                                    {
                                        region.getTerrain(x, y).getParameters().Add(TerrainParameter.HAS_MOSS, "");
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
            RandomNumberGenerator grng = new RandomNumberGenerator();
            List<Position> grassTiles = MapGenerator.getTerrainPositions(TerrainCode.GRASS, region, false);
            int treeCount = (int)((double)grassTiles.Count * treeDensity);
            for(int i=0; i < treeCount; i++)
            {
                if(grassTiles.Count > 0)
                {
                    Position pos = grassTiles[grng.RandomInteger(grassTiles.Count)];
                    region.getTerrain(pos.x, pos.y).getParameters().Add(TerrainParameter.HAS_TREE, TerrainManager.getRandomTree().ToString());
                    region.getTerrain(pos.x, pos.y).getParameters().Add(TerrainParameter.DAMAGE, "0");
                    grassTiles.Remove(pos);
                }
            }
        }


        public static void addMushroomSpores(Region region, int sporeCount)
        {
            RandomNumberGenerator grng = new RandomNumberGenerator();
            List<Position> grassTiles = MapGenerator.getTerrainPositions(TerrainCode.GRASS, region, false);
            for(int i=0; i < sporeCount; i++)
            {
                if(grassTiles.Count > 0)
                {
                    Position pos = grassTiles[grng.RandomInteger(grassTiles.Count)];
                    Terrain terrain = region.getTerrain(pos.x, pos.y);
                    MushroomSporeCode msc = EnumUtil.RandomEnumValue<MushroomSporeCode>();
                    if(!terrain.getParameters().ContainsKey(TerrainParameter.HAS_MUSHROOM_SPORES))
                    {
                        terrain.getParameters().Add(TerrainParameter.HAS_MUSHROOM_SPORES, msc.ToString());
                    }
                }
            }
        }


        public static void addClover(Region region, int cloverCount)
        {
            RandomNumberGenerator grng = new RandomNumberGenerator();
            List<Position> grassTiles = MapGenerator.getTerrainPositions(TerrainCode.GRASS, region, false);
            for(int i=0; i < cloverCount; i++)
            {
                if(grassTiles.Count > 0)
                {
                    Position pos = grassTiles[grng.RandomInteger(grassTiles.Count)];
                    Terrain terrain = region.getTerrain(pos.x, pos.y);
                    int cloverGrowCount = (int)(grng.RandomDouble() * (TerrainManager.CLOVER_GROW_COUNT / 4)) + TerrainManager.CLOVER_GROW_COUNT;
                    if (!terrain.getParameters().ContainsKey(TerrainParameter.HAS_CLOVER))
                    {
                        terrain.getParameters().Add(TerrainParameter.HAS_CLOVER, cloverGrowCount.ToString());
                    }
                }
            }
        }


        public static List<Position> getTerrainPositions(TerrainCode code, Region region, bool allowParameters)
        {
            return MapGenerator.getTerrainPositions(code, region, allowParameters, true);
        }


        public static List<Position> getTerrainPositions(TerrainCode code, Region region, bool allowParameters, bool includeEdges)
        {
            int edgeAdjust=0;
            if(!includeEdges)
            {
                edgeAdjust=1;
            }
            List<Position> positions = new List<Position>();
            for(int x=0+edgeAdjust; x < region.getWidth() - (edgeAdjust * 2); x++)
            {
                for(int y=0+edgeAdjust; y < region.getHeight() - (edgeAdjust * 2); y++)
                {
                    if(region.getTerrain(x, y).getCode() == code
                    && ((!allowParameters && region.getTerrain(x, y).getParameters().Count == 0) || allowParameters))
                    {
                        positions.Add(new Position(x,y));
                    }
                }
            }
        
            return positions;
        }


        public void initializePlayer(Quinoa quinoa)
        {
            quinoa.setPlayer(new Monster());
            quinoa.getPlayer().monsterCode = MonsterClassType.HUMAN;
            quinoa.getPlayer().ID = MonsterActionManager.PLAYER_ID;
            quinoa.getPlayer().setPosition(50, 50);

            Item boots = new Item();
            boots.itemClass = ItemClassType.BOOTS;
            quinoa.getPlayer().inventory.addItem(boots);

            Item jacket = new Item();
            jacket.itemClass = ItemClassType.JACKET;
            quinoa.getPlayer().inventory.addItem(jacket);

            Item axe = new Item();
            axe.itemClass = ItemClassType.AXE;
            quinoa.getPlayer().inventory.addItem(axe);

            Item pickaxe = new Item();
            pickaxe.itemClass = ItemClassType.PICKAXE;
            quinoa.getPlayer().inventory.addItem(pickaxe);

            Item flapjacks = new Item();
            flapjacks.itemClass = ItemClassType.FLAPJACKS;
            flapjacks.stackSize = 4;
            quinoa.getPlayer().inventory.addItem(flapjacks);

            Item lantern = new Item();
            lantern.itemClass = ItemClassType.LANTERN;
            quinoa.getPlayer().inventory.addItem(lantern);

            Item hat = new Item();
            hat.itemClass = ItemClassType.HAT;
            quinoa.getPlayer().inventory.addItem(hat);

            Item torch = new Item();
            torch.itemClass = ItemClassType.TORCH;
            torch.stackSize = 25;
            quinoa.getPlayer().inventory.addItem(torch);

            Item bucket = new Item();
            bucket.itemClass = ItemClassType.BUCKET;
            quinoa.getPlayer().inventory.addItem(bucket);

            Item shovel = new Item();
            shovel.itemClass = ItemClassType.SHOVEL;
            quinoa.getPlayer().inventory.addItem(shovel);

            Item tent = new Item();
            tent.itemClass = ItemClassType.TENT;
            quinoa.getPlayer().inventory.addItem(tent);

            Item ash = new Item();
            ash.itemClass = ItemClassType.ASH;
            ash.stackSize = 35;
            quinoa.getPlayer().inventory.addItem(ash);

            Item corn = new Item();
            corn.itemClass = ItemClassType.CORN;
            corn.stackSize = corn.maxStackSize;
            quinoa.getPlayer().inventory.addItem(corn);

            Item cornSeed = new Item();
            cornSeed.itemClass = ItemClassType.CORN_SEED;
            cornSeed.stackSize = 10;
            quinoa.getPlayer().inventory.addItem(cornSeed);

            Item pumpkin = new Item();
            pumpkin.itemClass = ItemClassType.PUMPKIN;
            pumpkin.stackSize = 1;
            quinoa.getPlayer().inventory.addItem(pumpkin);

            Item pumpkinSeed = new Item();
            pumpkinSeed.itemClass = ItemClassType.PUMPKIN_SEED;
            pumpkinSeed.stackSize = 10;
            quinoa.getPlayer().inventory.addItem(pumpkinSeed);

            Item floodgate = new Item();
            floodgate.itemClass = ItemClassType.FLOODGATE;
            floodgate.stackSize = floodgate.maxStackSize;
            quinoa.getPlayer().inventory.addItem(floodgate);

            Item mop = new Item();
            mop.itemClass = ItemClassType.MOP;
            mop.stackSize = mop.maxStackSize;
            quinoa.getPlayer().inventory.addItem(mop);  
        }

        public void connectExits(OverworldCell owc1, OverworldCell owc2, Direction exitDirection)
        {
            int exitX = 0;
            int exitY = 0;
            int dX = 0;
            int dY = 0;

            owc1.header.recallRegion();
            owc2.header.recallRegion();
                
            switch(exitDirection)
            {
                case Direction.N:
                exitX = (int)(owc1.header.getRegion().getWidth() / 2);    
                exitY = 0;
                dX = (int)(owc2.header.getRegion().getWidth() / 2);
                dY = owc2.header.getRegion().getHeight() - 2;
                owc1.header.getExits().Add(new RegionExit(exitX, exitY,dX,dY,owc2.header.getId(), RegionExitDecoratorType.NONE));
                break;
            
                case Direction.E:
                exitX = owc1.header.getRegion().getWidth() - 1;
                exitY = (owc1.header.getRegion().getHeight() / 2);
                dX = 1;
                dY = (owc2.header.getRegion().getHeight() / 2);
                owc1.header.getExits().Add(new RegionExit(exitX, exitY,dX,dY,owc2.header.getId(), RegionExitDecoratorType.NONE));
                break;
            
                case Direction.S:
                exitX = (int)(owc1.header.getRegion().getWidth() / 2);    
                exitY = owc1.header.getRegion().getHeight() - 1;
                dX = (int)(owc2.header.getRegion().getWidth() / 2);
                dY = 1;
                owc1.header.getExits().Add(new RegionExit(exitX, exitY,dX,dY,owc2.header.getId(), RegionExitDecoratorType.NONE));
                break;
            
                case Direction.W:
                exitX = 0;    
                exitY = (owc1.header.getRegion().getHeight() / 2);
                dX = owc2.header.getRegion().getWidth() - 2;
                dY = (owc2.header.getRegion().getHeight() / 2);
                owc1.header.getExits().Add(new RegionExit(exitX, exitY,dX,dY,owc2.header.getId(), RegionExitDecoratorType.NONE));
                break;
            }

            owc1.header.storeRegion(true);
            owc2.header.storeRegion(true);
        }


        public override void generate(Quinoa quinoa)
        {
            //Fill the overworld cell array
            int owWidth = quinoa.getMap().getOverworldWidth();
            int owHeight = quinoa.getMap().getOverworldHeight();
            OverworldCell[,] owc = quinoa.getMap().getOverworldCells();
            for(int x=0; x < owWidth; x++)
            {
                for(int y=0; y < owHeight; y++)
                {
                    owc[x,y] = new OverworldCell();
                }
            }

            //choose a random spot to be town, set as cross
            int townX = owWidth / 2 + ((int)(rng.RandomDouble() * 3) - 1);
            int townY = owHeight / 2 + ((int)(rng.RandomDouble() * 3) - 1);
            owc[townX,townY].cellType = OverworldCellType.MAIN_TOWN;
            owc[townX,townY].nExit = true;
            owc[townX,townY].eExit = true;
            owc[townX,townY].sExit = true;
            owc[townX,townY].wExit = true;

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
                    if(owc[x,y].cellType == OverworldCellType.MAIN_TOWN)
                    {
                        TownGenerator townGen = new TownGenerator(100, 100, "main", 3, 0.75, 0.01, owc[x,y], quinoa);
                        townGen.generate();
                        owc[x,y].header = townGen.header;
                        townGen.header.storeRegion(true);
                        this.addCaveBranch(townGen.header, 14, 14, 3 + (int)(rng.RandomDouble() * 3), quinoa);
                        quinoa.getMap().addRegionHeader(owc[x,y].header);
                    }
                    else if(owc[x,y].cellType == OverworldCellType.TOWN)
                    {
                        TownGenerator townGen = new TownGenerator(100, 100, "town" + rng.RandomUUID().ToString(), 3, 0.75, 0.01, owc[x,y], quinoa);
                        townGen.generate();
                        owc[x,y].header = townGen.header;
                        townGen.header.storeRegion(true);
                        quinoa.getMap().addRegionHeader(owc[x,y].header);
                    }
                    else if(owc[x,y].cellType == OverworldCellType.FOREST)
                    {
                        String forestName = "forest" + rng.RandomUUID().ToString();

                        int depthCount = 0;
                        int depthTotal = 0;
                        if(owc[x,y].nExit)
                        {
                            depthCount++;
                            depthTotal = depthTotal + owc[x,y-1].depth;
                        }
                        if(owc[x,y].eExit)
                        {
                            depthCount++;
                            depthTotal = depthTotal + owc[x + 1,y].depth;
                        }
                        if(owc[x,y].sExit)
                        {
                            depthCount++;
                            depthTotal = depthTotal + owc[x,y+1].depth;
                        }
                        if(owc[x,y].wExit)
                        {
                            depthCount++;
                            depthTotal = depthTotal + owc[x-1,y].depth;
                        }

                        depthTotal = depthTotal + owc[x,y].depth;
                        depthCount = depthCount + 1;

                        double treeDensity = 0.01 + ((depthTotal / depthCount) * 0.02);
                        if(treeDensity > 0.55)
                        {
                            treeDensity = 0.55;
                        }
                        ForestGenerator forGen = new ForestGenerator(100, 100, forestName, treeDensity, quinoa);
                        double waterDensity = 0.15 + (rng.RandomDouble() * 0.35);
                        if(waterDensity > 0.65)
                        {
                            waterDensity = 0.65;
                        }
                        forGen.addWater(waterDensity, 4);
                        forGen.generate(owc[x,y].nExit, owc[x,y].eExit, owc[x,y].sExit, owc[x,y].wExit);
                        forGen.addSprings();
                        forGen.header.setName("Forest");
                        owc[x,y].header = forGen.header;

                        //add cave branches if only one exit is present
                        int exitCount = 0;
                        if(owc[x,y].nExit) {exitCount++;}
                        if(owc[x,y].eExit) {exitCount++;}
                        if(owc[x,y].sExit) {exitCount++;}
                        if(owc[x,y].wExit) {exitCount++;}
                        if(exitCount == 1 && rng.RandomDouble() < 0.50)
                        {
                            this.addCaveBranch(owc[x,y].header, owc[x,y].header.getRegion().getWidth() / 2, owc[x,y].header.getRegion().getHeight() / 2, 2 + (int)(rng.RandomDouble() * (depthTotal / depthCount)), quinoa);
                        }

                        forGen.header.storeRegion(true);
                        quinoa.getMap().addRegionHeader(owc[x,y].header);
                    }
                }
            }

            //connect exits
            for(int x=0; x < owWidth; x++)
            {
                for(int y=0; y < owHeight; y++)
                {
                    if(owc[x,y].header != null)
                    {
                        if(owc[x,y].nExit && y > 0)
                        {
                            this.connectExits(owc[x,y], owc[x,y-1], Direction.N);
                        }
                        if(owc[x,y].sExit && y < owHeight-1)
                        {
                            this.connectExits(owc[x,y], owc[x,y+1], Direction.S);
                        }
                        if(owc[x,y].eExit && x < owWidth-1)
                        {
                            this.connectExits(owc[x,y], owc[x+1,y], Direction.E);
                        }
                        if(owc[x,y].wExit && x > 0)
                        {
                            this.connectExits(owc[x,y], owc[x-1,y], Direction.W);
                        }
                    }
                }
            }

            //place the brother
            this.placeBrother(quinoa);
                
            this.initializePlayer(quinoa);
        }

        public void addPathBranch(int x, int y, int width, int height, OverworldCell[,] owc, int depth, int branchCountdown, int townCounter)
        {
            int townCountdown = townCounter;

            if(owc[x,y].cellType != OverworldCellType.NULL)
            {
                return;
            }
            else
            {
                //figure out vital connections
                owc[x,y].cellType = OverworldCellType.FOREST;
                owc[x,y].depth = depth;
                if(y > 0 && owc[x,y-1].sExit)
                {
                    owc[x,y].nExit = true;
                }

                if(y < height-1 && owc[x,y+1].nExit)
                {
                    owc[x,y].sExit = true;
                }

                if(x > 0 && owc[x-1,y].eExit)
                {
                    owc[x,y].wExit = true;
                }

                if(x < width-1 && owc[x+1,y].wExit)
                {
                    owc[x,y].eExit = true;
                }


                //randomly try different directions
                //try to lower amount of branching by only allowing up to two exit follows
                int exitCount = 0;
                if(owc[x,y].nExit)
                {
                    exitCount++;
                }
                if(owc[x,y].eExit)
                {
                    exitCount++;
                }
                if(owc[x,y].sExit)
                {
                    exitCount++;
                }
                if(owc[x,y].wExit)
                {
                    exitCount++;
                }

                if(townCountdown == 0)
                {
                    owc[x,y].cellType = OverworldCellType.TOWN;
                    townCountdown = 6 + (int)(rng.RandomDouble() * 3);
                }

                branchCountdown = branchCountdown - 1;
                int branchMax = 2 + (int)(rng.RandomDouble() * 4);
                if(branchCountdown > 0)
                {
                    if(owc[x,y].nExit && y < height - 1)
                    {
                        owc[x,y].sExit = true;
                        owc[x,y+1].nExit = true;
                        this.addPathBranch(x, y+1, width, height, owc, depth + 1, branchCountdown - 1, townCountdown - 1);
                    }
                    else if(owc[x,y].sExit && y > 0)
                    {
                        owc[x,y].nExit = true;
                        owc[x,y-1].sExit = true;
                        this.addPathBranch(x, y-1, width, height, owc, depth + 1, branchCountdown - 1, townCountdown - 1);
                    }

                    if(owc[x,y].eExit && x > 0)
                    {
                        owc[x,y].wExit = true;
                        owc[x-1,y].eExit = true;
                        this.addPathBranch(x-1, y, width, height, owc, depth + 1, branchCountdown - 1, townCountdown - 1);
                    }
                    else if(owc[x,y].wExit && x < width - 1)
                    {
                        owc[x,y].eExit = true;
                        owc[x+1,y].wExit = true;
                        this.addPathBranch(x+1, y, width, height, owc, depth + 1, branchCountdown - 1, townCountdown - 1);
                    }

                }
                else
                {
                    //branch off in any direction
                    int exitCutoff = 2;
                    double branchChance = 0.35;
                
                    if(exitCount < exitCutoff && y > 0 && owc[x,y].nExit == false && rng.RandomDouble() < branchChance)
                    {
                        owc[x,y].nExit = true;
                        owc[x,y-1].sExit = true;
                        this.addPathBranch(x, y-1, width, height, owc, depth + 1, branchMax, townCountdown - 1);
                    }

                    if(exitCount < exitCutoff && y < height-1 && owc[x,y].sExit == false && rng.RandomDouble() < branchChance)
                    {
                        owc[x,y].sExit = true;
                        owc[x,y+1].nExit = true;
                        this.addPathBranch(x, y+1, width, height, owc, depth + 1, branchMax, townCountdown - 1);
                    }

                    if(exitCount < exitCutoff && x > 0 && owc[x,y].wExit == false && rng.RandomDouble() < branchChance)
                    {
                        owc[x,y].wExit = true;
                        owc[x - 1,y].eExit = true;
                        this.addPathBranch(x - 1, y, width, height, owc, depth + 1, branchMax, townCountdown - 1);
                    }

                    if(exitCount < exitCutoff && x < width-1 && owc[x,y].eExit == false && rng.RandomDouble() < branchChance)
                    {
                        owc[x,y].eExit = true;
                        owc[x + 1,y].wExit = true;
                        this.addPathBranch(x + 1, y, width, height, owc, depth + 1, branchMax, townCountdown - 1);
                    }
                }
            }
        }


        public void addCaveBranch(RegionHeader topLevel, int topX, int topY, int depth, Quinoa quinoa)
        {
            CaveGenerator lastGen = null;

            for(int deep=0; deep < depth; deep++)
            {
                int width = 80 + (int)(rng.RandomDouble() * 40);
                int height = 80 + (int)(rng.RandomDouble() * 40);
                int chamberCount = 3 + (int)(rng.RandomDouble() * 4);
                int smoothness = (int)(rng.RandomDouble() * 2);
                double fillPercentage = 0.45 + (rng.RandomDouble() * 0.20);

                CaveGenerator caveGen = new CaveGenerator(width, height, topLevel.getId() + "-cave" + deep, fillPercentage, smoothness, quinoa);
                caveGen.addChamber(new Chamber(5,5,caveGen.region.getWidth() - 10,caveGen.region.getHeight() - 10, ChamberType.OPEN));
                for(int i=0; i < chamberCount; i++)
                {
                    int caveX = 5 + (int)(rng.RandomDouble() * (caveGen.region.getWidth() - 41));
                    int caveY = 5 + (int)(rng.RandomDouble() * (caveGen.region.getHeight() - 41));

                    ChamberType type = ChamberType.NULL;
                    if(rng.RandomDouble() < 0.20)
                    {
                        type = ChamberType.MUSHROOM;
                    }
                    else
                    {
                        type = ChamberType.FLOODED;
                    }
                    caveGen.addChamber(new Chamber(caveX,caveY,(int)(rng.RandomDouble() * 20) + 10,(int)(rng.RandomDouble() * 20) + 10, type));
                }
                caveGen.smoothWater();
                caveGen.placeExits();
                caveGen.addTorches();
                caveGen.addMinerals(deep+1);

                if(deep == depth - 1)
                {
                    caveGen.addTreasure();
                    this.caveBottomIDs.Add(caveGen.header.getId());
                }

                caveGen.header.setName("Cave (level " + (deep + 1) + ")");
                caveGen.header.setRegion(caveGen.region);
                caveGen.header.storeRegion(true);


                if(deep == 0)
                {
                    caveGen.getEntrance().setDx(topX);
                    caveGen.getEntrance().setDy(topY+1);
                    caveGen.getEntrance().setDestinationRegionID(topLevel.getId());

                    topLevel.getExits().Add(new RegionExit(topX,topY,caveGen.getEntrance().getX(),caveGen.getEntrance().getY()+1,caveGen.header.getId(), RegionExitDecoratorType.CAVE));
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
                        caveGen.header.getExits().Remove(caveGen.exit);
                    }
                }

            

                quinoa.getMap().addRegionHeader(caveGen.header);

                lastGen = caveGen;
            }
        }
    }
}
