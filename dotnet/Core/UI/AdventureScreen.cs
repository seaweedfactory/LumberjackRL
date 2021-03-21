using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Items;
using LumberjackRL.Core.Monsters;
using LumberjackRL.Core.Map;
using LumberjackRL.Core.Utilities;
using System.Drawing;
using System.Windows.Forms;

namespace LumberjackRL.Core.UI
{

    public class AdventureScreen : IScreen
    {
        public Form Parent { get; set; }

        public const int MAP_PIXEL_X_OFFSET = 12;
        public const int MAP_PIXEL_Y_OFFSET = 46;
        public const int MAP_WINDOW_TILE_SIZE_X = 30;
        public const int MAP_WINDOW_TILE_SIZE_Y = 15;
        public const int MESSAGES_PIXEL_Y_OFFSET = 583;
        public const int TIME_DISPLAY_OFFSET = 465;
        public const int MESSAGES_LINE_NUMBER = 3;
        public const int ATTRIBUTE_WINDOW_SIZE = 200;

        public const char UP_KEY = 'w';
        public const char DOWN_KEY = 's';
        public const char RIGHT_KEY = 'd';
        public const char LEFT_KEY = 'a';
        public const char ENTER_KEY = ' ';
        public const char EXIT_KEY = 'x';
        public const char INVENTORY_KEY = 'i';
        public const char CHARACTER_KEY = 'c';
        public const char MESSAGES_KEY = 'm';
        public const char EQUIP_KEY = 'e';
        public const char PICKUP_KEY = 'p';
        public const char ACTION_KEY = 'z';
        public const char BELT_1_KEY = '1';
        public const char BELT_2_KEY = '2';
        public const char BELT_3_KEY = '3';
        public const char BELT_4_KEY = '4';
        public const char SAVE_KEY = 'v';
        public const char LOAD_KEY = 'l';
        public const char HELP_KEY = 'h';

        private DrawManager dtm;
        private Quinoa quinoa;
        private int tileOffsetX, tileOffsetY;
        private int targetX, targetY;
        private int targetMaxDistance;
        private int inventoryTargetX, inventoryTargetY;
        private int tradeTargetX, tradeTargetY;
        private int characterIndex;
        private Item verbItem;
        private ItemVerbType verb;
        private int verbIndex;
        private String lastHungerStatus;
        private Monster tradeMonster;
        private bool tradePageIsPlayer;
        private AdvenureScreenMapSelectAction mapSelectAction;
        private AdventureScreenModeType mode;
        private int regionCycleCounter;
    
        public AdventureScreen(Quinoa quinoa, Form parent)
        {
            Parent = parent;
            dtm = new DrawManager();
            this.quinoa = quinoa;
            this.tileOffsetX = 0;
            this.tileOffsetY = 0;
            this.targetX = 0;
            this.targetY = 0;
            this.targetMaxDistance = 1;
            this.inventoryTargetX = 0;
            this.inventoryTargetY = 0;
            this.tradeTargetX = 0;
            this.tradeTargetY = 0;
            this.characterIndex = 0;
            this.targetY = 0;
            this.mapSelectAction = AdvenureScreenMapSelectAction.VERB;
            this.mode = AdventureScreenModeType.MAP;
            this.lastHungerStatus = "";
            this.verb = ItemVerbType.NULL;
            this.verbItem = null;
            this.verbIndex = 0;
            this.tradeMonster = null;
            this.tradePageIsPlayer = true;
            this.regionCycleCounter = Quinoa.REGION_CYCLE_FREQUENCY;
        }

        public void setMode(AdventureScreenModeType newMode)
        {
            switch(newMode)
            {
                case AdventureScreenModeType.MAP:
                    this.mode = AdventureScreenModeType.MAP;
                    quinoa.getUI().refresh();
                    break;

                case AdventureScreenModeType.HELP:
                    this.mode = AdventureScreenModeType.HELP;
                    quinoa.getUI().refresh();
                    break;

                case AdventureScreenModeType.DIALOG:
                    this.mode = AdventureScreenModeType.DIALOG;
                    quinoa.getUI().refresh();
                    break;

                case AdventureScreenModeType.MAP_SELECT:
                    targetX = quinoa.getPlayer().x;
                    targetY = quinoa.getPlayer().y;

                    switch(quinoa.getPlayer().facing)
                    {
                        case Direction.N:
                        if(targetY > 0)
                        {
                            targetY--;
                        }
                        break;

                        case Direction.S:
                        if(targetY < quinoa.getCurrentRegionHeader().getRegion().getHeight() - 2)
                        {
                            targetY++;
                        }
                        break;

                        case Direction.W:
                        if(targetX > 0)
                        {
                            targetX--;
                        }
                        break;

                        case Direction.E:
                        if(targetX < quinoa.getCurrentRegionHeader().getRegion().getWidth() - 2)
                        {
                            targetX++;
                        }
                        break;
                    }

                    this.mode = AdventureScreenModeType.MAP_SELECT;
                    quinoa.getMessageManager().addMessage("Press " + charToStr(ENTER_KEY) + " to select an area.");
                    quinoa.getUI().refresh();
                    break;

                case AdventureScreenModeType.INVENTORY:
                    this.mode = AdventureScreenModeType.INVENTORY;
                    this.inventoryTargetX = 0;
                    this.inventoryTargetY = 0;
                    quinoa.getUI().refresh();
                    break;

                case AdventureScreenModeType.CHARACTER:
                    this.mode = AdventureScreenModeType.CHARACTER;
                    quinoa.getUI().refresh();
                    break;

                case AdventureScreenModeType.VERB_PICK:
                    this.mode = AdventureScreenModeType.VERB_PICK;
                    quinoa.getMessageManager().addMessage("Press " + charToStr(ENTER_KEY) + " select an action.");
                    this.verb = ItemVerbType.NULL;
                    this.verbIndex = 0;
                    quinoa.getUI().refresh();
                    break;

                case AdventureScreenModeType.TRADE:
                    this.tradeTargetX = 0;
                    this.tradeTargetY = 0;
                    this.tradePageIsPlayer = true;
                    quinoa.getMessageManager().addMessage("Press " + charToStr(ENTER_KEY) + " to sell or buy the selected item.");
                    quinoa.getMessageManager().addMessage("Press " + charToStr(EXIT_KEY) + " to stop trading.");
                    this.mode = AdventureScreenModeType.TRADE;
                    quinoa.getUI().refresh();
                    break;
            }
        }

        public void setTradeMonster(Monster monster)
        {
            this.tradeMonster = monster;
        }
    
        public void setMapSelectAction(AdvenureScreenMapSelectAction msa)
        {
            this.mapSelectAction = msa;
        }

        public void setMapSelectMaxDistance(int distance)
        {
            this.targetMaxDistance = distance;
        }

        public void centerOnPoint(int x, int y)
        {
            tileOffsetX = x - (MAP_WINDOW_TILE_SIZE_X / 2);
            tileOffsetY = y - (MAP_WINDOW_TILE_SIZE_Y / 2);

            if(tileOffsetX < 0)
            {
                tileOffsetX = 0;
            }
            else if(tileOffsetX > quinoa.getCurrentRegionHeader().getRegion().getWidth() - MAP_WINDOW_TILE_SIZE_X)
            {
                tileOffsetX = quinoa.getCurrentRegionHeader().getRegion().getWidth() - MAP_WINDOW_TILE_SIZE_X;
            }

            if(tileOffsetY < 0)
            {
                tileOffsetY = 0;
            }
            else if(tileOffsetY > quinoa.getCurrentRegionHeader().getRegion().getHeight() - MAP_WINDOW_TILE_SIZE_Y)
            {
                tileOffsetY = quinoa.getCurrentRegionHeader().getRegion().getHeight() - MAP_WINDOW_TILE_SIZE_Y;
            }

        }

        public void draw(Graphics g)
        {
            //clear the screen
            g.FillRectangle(Brushes.Black, 0, 0, Parent.Size.Width, Parent.Size.Height); 
                
            switch (mode)
            {
                case AdventureScreenModeType.MAP:
                    this.centerOnPoint(quinoa.getPlayer().x, quinoa.getPlayer().y);
                    if (quinoa.getPlayer() != null && !quinoa.getPlayer().isSleeping())
                    {
                        drawMap(g);
                    }
                    drawMapSideBar(g);
                    drawOverworldMap(g);
                    drawMessages(g);
                    break;

                case AdventureScreenModeType.HELP:
                    this.centerOnPoint(quinoa.getPlayer().x, quinoa.getPlayer().y);
                    if (quinoa.getPlayer() != null && !quinoa.getPlayer().isSleeping())
                    {
                        drawMap(g);
                    }
                    drawMapSideBar(g);
                    drawMessages(g);
                    drawHelpSideBar(g);
                    break;

                case AdventureScreenModeType.INVENTORY:
                    this.centerOnPoint(quinoa.getPlayer().x, quinoa.getPlayer().y);
                    drawMap(g);
                    drawInventory(g);
                    drawMessages(g);
                    break;

                case AdventureScreenModeType.DIALOG:
                    this.centerOnPoint(quinoa.getPlayer().x, quinoa.getPlayer().y);
                    drawMap(g);
                    drawMapSideBar(g);
                    drawOverworldMap(g);
                    drawDialog(g);
                    break;

                case AdventureScreenModeType.MAP_SELECT:
                    this.centerOnPoint(targetX, targetY);
                    drawMap(g);
                    drawMapSideBar(g);
                    drawMapSelect(g);
                    drawOverworldMap(g);
                    drawMessages(g);
                    break;

                case AdventureScreenModeType.CHARACTER:
                    this.centerOnPoint(quinoa.getPlayer().x, quinoa.getPlayer().y);
                    drawMap(g);
                    drawCharacter(g);
                    drawMessages(g);
                    break;

                case AdventureScreenModeType.VERB_PICK:
                    this.centerOnPoint(quinoa.getPlayer().x, quinoa.getPlayer().y);
                    drawMap(g);
                    drawMapSideBar(g);
                    drawOverworldMap(g);
                    drawMessages(g);
                    drawPickVerb(g);
                    break;

                case AdventureScreenModeType.TRADE:
                    this.centerOnPoint(quinoa.getPlayer().x, quinoa.getPlayer().y);
                    drawMap(g);
                    drawTrade(g);
                    drawMessages(g);
                    break;

            }
        }

        public void drawMap(Graphics g)
        {
            LumberjackRL.Core.Map.Region region = quinoa.getCurrentRegionHeader().getRegion();

            //Recalculate the lightmap, if applicable
            quinoa.getLightMap().calculate(region);

            //Draw the map, monsters, and items
            for(int x=0; x < MAP_WINDOW_TILE_SIZE_X; x++)
            {
                for(int y=0; y < MAP_WINDOW_TILE_SIZE_Y; y++)
                {
                    dtm.drawTerrain(
                        region.getTerrain(tileOffsetX + x, tileOffsetY + y), 
                        g, 
                        MAP_PIXEL_X_OFFSET + x*quinoa.getUI().getGraphicsManager().getTileSize(), 
                        MAP_PIXEL_Y_OFFSET + y*quinoa.getUI().getGraphicsManager().getTileSize(), 
                        quinoa.getUI().getGraphicsManager());
                }
            }

            foreach(Item tempItem in region.getItems())
            {
                if(tempItem.x - tileOffsetX < MAP_WINDOW_TILE_SIZE_X
                && tempItem.y - tileOffsetY < MAP_WINDOW_TILE_SIZE_Y)
                {
                    int itemX = tempItem.x - tileOffsetX;
                    int itemY = tempItem.y - tileOffsetY;

                    if(itemX >= 0 && itemX < MAP_WINDOW_TILE_SIZE_X
                    && itemY >= 0 && itemY < MAP_WINDOW_TILE_SIZE_Y)
                    {
                        int drawX = MAP_PIXEL_X_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * itemX);
                        int drawY = MAP_PIXEL_Y_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * itemY);
                        dtm.drawItem(tempItem, g, drawX, drawY, 1.0, quinoa.getUI().getGraphicsManager(), false);
                    }
                }
            }

            foreach(Monster tempMon in region.getMonsters())
            {
                if(tempMon.x - tileOffsetX < MAP_WINDOW_TILE_SIZE_X && tempMon.y - tileOffsetY < MAP_WINDOW_TILE_SIZE_Y)
                {
                    int monX = tempMon.x - tileOffsetX;
                    int monY = tempMon.y - tileOffsetY;

                    if(monX >= 0 && monX < MAP_WINDOW_TILE_SIZE_X
                    && monY >= 0 && monY < MAP_WINDOW_TILE_SIZE_Y)
                    {
                        int drawX = MAP_PIXEL_X_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * monX);
                        int drawY = MAP_PIXEL_Y_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * monY);
                        dtm.drawMonster(tempMon, g, drawX, drawY, quinoa.getUI().getGraphicsManager());
                    }
                }
            }



            //draw roofs
            Building playerInside = null;
            foreach(Building tempBuild in quinoa.getCurrentRegionHeader().getRegion().getBuildings())
            {
                int px = quinoa.getPlayer().x;
                int py = quinoa.getPlayer().y;
                if(px >= tempBuild.getX() && px <= tempBuild.getX() + tempBuild.getWidth() - 1
                && py >= tempBuild.getY() && py <= tempBuild.getY() + tempBuild.getHeight() - 1)
                {
                    playerInside = tempBuild;
                }
            }

            foreach(Building tempBuild in quinoa.getCurrentRegionHeader().getRegion().getBuildings())
            {
                if(playerInside == null || (playerInside != null && tempBuild != playerInside))
                {
                    for(int x=0; x < tempBuild.getWidth(); x++)
                    {
                        for(int y=0; y < tempBuild.getHeight() - 1; y++)
                        {
                            int bX = tempBuild.getX() + x;
                            int bY = tempBuild.getY() + y;
                            if(bX - tileOffsetX < MAP_WINDOW_TILE_SIZE_X && bY - tileOffsetY < MAP_WINDOW_TILE_SIZE_Y)
                            {
                                int roofX = bX - tileOffsetX;
                                int roofY = bY - tileOffsetY;

                                if(roofX >= 0 && roofX < MAP_WINDOW_TILE_SIZE_X
                                && roofY >= 0 && roofY < MAP_WINDOW_TILE_SIZE_Y)
                                {
                                    int drawX = MAP_PIXEL_X_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * roofX);
                                    int drawY = MAP_PIXEL_Y_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * roofY);
                                    g.DrawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.ROOF, tempBuild.getRoofType()), drawX, drawY);
                                }
                            }
                        }
                    }
                }
                else
                {
                    //player is inside this building
                }
            }

            //Draw the lightMap overlay
            for(int x=0; x < MAP_WINDOW_TILE_SIZE_X; x++)
            {
                for(int y=0; y < MAP_WINDOW_TILE_SIZE_Y; y++)
                {
                    int drawX = MAP_PIXEL_X_OFFSET + x*quinoa.getUI().getGraphicsManager().getTileSize();
                    int drawY = MAP_PIXEL_Y_OFFSET + y*quinoa.getUI().getGraphicsManager().getTileSize();
                    int index = quinoa.getLightMap().getValue(tileOffsetX + x, tileOffsetY + y);

                    Image tileImage = quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.TRANSPARENT, index);
                    if (tileImage != null)
                    {
                        g.DrawImage(tileImage, drawX, drawY);
                    }
                }
            }

            //draw exits
            foreach(RegionExit tempExit in quinoa.getCurrentRegionHeader().getExits())
            {
                if(tempExit.getX() - tileOffsetX < MAP_WINDOW_TILE_SIZE_X && tempExit.getY() - tileOffsetY < MAP_WINDOW_TILE_SIZE_Y)
                {
                    int exitX = tempExit.getX() - tileOffsetX;
                    int exitY = tempExit.getY() - tileOffsetY;

                    if(exitX >= 0 && exitX < MAP_WINDOW_TILE_SIZE_X
                    && exitY >= 0 && exitY < MAP_WINDOW_TILE_SIZE_Y)
                    {
                        int drawX = MAP_PIXEL_X_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * exitX);
                        int drawY = MAP_PIXEL_Y_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * exitY);
                        switch(tempExit.getExitDecorator())
                        {
                            case RegionExitDecoratorType.UP_STAIR:
                            g.DrawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.EXIT, 0), drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize());
                            break;

                            case RegionExitDecoratorType.DOWN_STAIR:
                            g.DrawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.EXIT, 1), drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize());
                            break;

                            case RegionExitDecoratorType.CAVE:
                            g.DrawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.EXIT, 2), drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize());
                            break;

                            case RegionExitDecoratorType.NONE:
                            g.DrawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.SPARKLE, RandomNumber.RandomInteger(3)), drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize());
                            break;
                        }
                    }
                }
            }


            //outline map
            g.DrawRectangle(Pens.DarkGray, MAP_PIXEL_X_OFFSET - 1, MAP_PIXEL_Y_OFFSET - 1, (MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) + 1, (MAP_WINDOW_TILE_SIZE_Y * quinoa.getUI().getGraphicsManager().getTileSize()) + 1);
            
            //Draw map region name and time
            dtm.drawString(quinoa.getCurrentRegionHeader().getName(), 2, g, MAP_PIXEL_X_OFFSET, MAP_PIXEL_Y_OFFSET - quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager());
            dtm.drawString(quinoa.getTime(), 2, g, MAP_PIXEL_X_OFFSET + TIME_DISPLAY_OFFSET, MAP_PIXEL_Y_OFFSET - quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager());
        
        }

        public String charToStr(char input)
        {
            if(input == ' ')
            {
                return "SPACE";
            }
            else
            {
                return input+"";
            }
        }

        public void drawHelpSideBar(Graphics g)
        {
            g.FillRectangle(Brushes.Black, Parent.Size.Width / 2 - 225, 30, 450, Parent.Size.Height - 50);
            g.DrawRectangle(Pens.LightGray, Parent.Size.Width / 2 - 225, 30, 450, Parent.Size.Height - 50);
        
            int XOffset = Parent.Size.Width / 2 - 225 + 10;
            int YOffset = 35;
            dtm.drawString("KEYS", 2, g, XOffset + 180, (0 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
        
            dtm.drawString("Move Up = " + charToStr(UP_KEY), 2, g, XOffset, (2 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Move Down = " + charToStr(DOWN_KEY), 2, g, XOffset, (3 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Move Left = " + charToStr(LEFT_KEY), 2, g, XOffset, (4 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Move Right = " + charToStr(RIGHT_KEY), 2, g, XOffset, (5 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Enter/Select = " + charToStr(ENTER_KEY), 2, g, XOffset, (6 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Cancel/Exit Screen = " + charToStr(EXIT_KEY), 2, g, XOffset, (7 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
        
            dtm.drawString("Inventory Screen = " + charToStr(INVENTORY_KEY), 2, g, XOffset, (9 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Character Screen = " + charToStr(CHARACTER_KEY), 2, g, XOffset, (10 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Messages Screen = " + charToStr(MESSAGES_KEY), 2, g, XOffset, (11 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Help Screen = " + charToStr(HELP_KEY), 2, g, XOffset, (12 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
        
            dtm.drawString("Equip/Unequip Item = " + charToStr(EQUIP_KEY), 2, g, XOffset, (14 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Pick up/Drop Item = " + charToStr(PICKUP_KEY), 2, g, XOffset, (15 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Use Item 1 = " + charToStr(BELT_1_KEY), 2, g, XOffset, (16 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Use Item 2 = " + charToStr(BELT_2_KEY), 2, g, XOffset, (17 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Use Item 3 = " + charToStr(BELT_3_KEY), 2, g, XOffset, (18 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Use Item 4 = " + charToStr(BELT_4_KEY), 2, g, XOffset, (19 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Do Action = " + charToStr(ACTION_KEY), 2, g, XOffset, (20 * 20) + YOffset, quinoa.getUI().getGraphicsManager());

            dtm.drawString("Save Game = " + charToStr(SAVE_KEY), 2, g, XOffset, (22 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Load Game = " + charToStr(LOAD_KEY), 2, g, XOffset, (23 * 20) + YOffset, quinoa.getUI().getGraphicsManager());

            dtm.drawString("Press " + charToStr(EXIT_KEY) + " to continue", 2, g, XOffset + 90, (28 * 20) + YOffset, quinoa.getUI().getGraphicsManager());
        

        }

        public void drawOverworldMap(Graphics g)
        {
            int sideBarXOffset = MAP_PIXEL_X_OFFSET + (MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) + 10 + 40;
            int sideBarYOffset = MAP_PIXEL_Y_OFFSET + 40;

            int gridSize = 16;
            for(int x=0; x < quinoa.getMap().getOverworldWidth(); x++)
            {
                for(int y=0; y < quinoa.getMap().getOverworldHeight(); y++)
                {
                    OverworldCell cell = quinoa.getMap().getOverworldCells()[x,y];
                    bool drawRoads =false;
                    switch(cell.cellType)
                    {
                        case OverworldCellType.NULL:
                        g.FillRectangle(Brushes.DarkGreen, sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize, gridSize, gridSize);
                        break;

                        case OverworldCellType.FOREST:
                        g.FillRectangle(Brushes.Green, sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize, gridSize, gridSize);
                        drawRoads = true;
                        break;

                        case OverworldCellType.TOWN:
                        g.FillRectangle(Brushes.Magenta, sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize, gridSize, gridSize);
                        drawRoads = true;
                        break;

                        case OverworldCellType.MAIN_TOWN:
                        g.FillRectangle(Brushes.Orange, sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize, gridSize, gridSize);
                        drawRoads = true;
                        break;
                    }

                    if(drawRoads)
                    {
                        if(cell.nExit)
                        {
                            g.FillRectangle(Brushes.Yellow, sideBarXOffset + x * gridSize + (gridSize / 2) - 1, sideBarYOffset + y * gridSize, 3, gridSize / 2);
                        }
                        if(cell.sExit)
                        {
                            g.FillRectangle(Brushes.Yellow, sideBarXOffset + x * gridSize + (gridSize / 2) - 1, sideBarYOffset + y * gridSize + (gridSize / 2), 3, gridSize / 2);
                        }
                        if(cell.eExit)
                        {
                            g.FillRectangle(Brushes.Yellow, sideBarXOffset + x * gridSize + (gridSize / 2), sideBarYOffset + y * gridSize + (gridSize / 2), gridSize / 2, 3);
                        }
                        if(cell.wExit)
                        {
                            g.FillRectangle(Brushes.Yellow, sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize + (gridSize / 2), gridSize / 2, 3);
                        }
                    }

                    //draw depth
                    //dtm.drawString(cell.depth +"", 1.0, g, sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize, quinoa.getUI().getGraphicsManager());

                }
            }
        }

        public void drawMapSideBar(Graphics g)
        {
            //draw item slots
            int itemOffsetX = 195;
            int itemOffsetY = 535;
            int sideBarXOffset = MAP_PIXEL_X_OFFSET + (MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) + 10;
            int sideBarYOffset = MAP_PIXEL_Y_OFFSET;
            for(int i=0; i < 4; i++)
            {
                MonsterItemSlotType tempSlot = (MonsterItemSlotType)Enum.Parse(typeof(MonsterItemSlotType), "BELT_" + (i +1));
                int ts = quinoa.getUI().getGraphicsManager().getTileSize();
                g.DrawRectangle(Pens.LightGray, itemOffsetX + sideBarXOffset + (int)(i * ts * 2.222), itemOffsetY + sideBarYOffset, ts * 2, ts * 2);
                dtm.drawString((i+1) + "", 1.0, g, itemOffsetX + sideBarXOffset + (int)(i * ts * 2.222) + ts, itemOffsetY + sideBarYOffset + (ts * 2) + 5, quinoa.getUI().getGraphicsManager());
                Item tempItem = quinoa.getPlayer().inventory.getItem(tempSlot);
                if(tempItem != null)
                {
                    dtm.drawItem(tempItem, g, itemOffsetX + sideBarXOffset + (int)(i * ts * 2.222) + (ts /2), itemOffsetY + sideBarYOffset + (ts /2), 1.0, quinoa.getUI().getGraphicsManager());
                }
            }
        
            //Draw stats
            dtm.drawString("Life: " + quinoa.getPlayer().stats.getDisplayHealth(), 2, g, sideBarXOffset, itemOffsetY + sideBarYOffset + (20 * 0), quinoa.getUI().getGraphicsManager());
            dtm.drawString(quinoa.getPlayer().stats.getDisplayHunger(), 2, g, sideBarXOffset, itemOffsetY + sideBarYOffset + (20 * 1) , quinoa.getUI().getGraphicsManager());

            //help message
            dtm.drawString("Press " + AdventureScreen.HELP_KEY + " for help", 2, g, sideBarXOffset + 80, sideBarYOffset - 16, quinoa.getUI().getGraphicsManager());
        }

        public void drawInventory(Graphics g)
        {
            int inventoryXOffset = MAP_PIXEL_X_OFFSET + (MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) + 10;
            int inventoryYOffset = MAP_PIXEL_Y_OFFSET + 48;
            int invX = inventoryXOffset;
            int invY = inventoryYOffset;
            for(int x=0; x < MonsterInventory.MAX_WIDTH; x++)
            {
                for(int y=0; y < MonsterInventory.MAX_HEIGHT; y++)
                {
                   int ts = quinoa.getUI().getGraphicsManager().getTileSize();
                   g.DrawRectangle(Pens.DarkGray, invX + (x * ts * 2), invY + (y * ts * 2), ts * 2, ts * 2);
                }
            }
            invX = inventoryXOffset;
            invY = inventoryYOffset;
            for(int x=0; x < quinoa.getPlayer().inventory.width; x++)
            {
                for(int y=0; y < quinoa.getPlayer().inventory.height; y++)
                {
                   int ts = quinoa.getUI().getGraphicsManager().getTileSize();
                   g.DrawRectangle(Pens.LightGray, invX + (x * ts * 2), invY + (y * ts * 2), ts * 2, ts * 2);
                }
            }
            foreach(Item tempItem in quinoa.getPlayer().inventory.getItems())
            {
                int ts = quinoa.getUI().getGraphicsManager().getTileSize();
                dtm.drawItem(tempItem, g, invX + (tempItem.x * ts * 2) + (ts/2), invY + (tempItem.y * ts * 2) + (ts/2), 1.0, quinoa.getUI().getGraphicsManager());
            }

            //draw item slots
            int slotYOffset = MAP_PIXEL_Y_OFFSET;
            for(int i=0; i < Enum.GetValues(typeof(MonsterItemSlotType)).Length; i++)
            {
                MonsterItemSlotType tempSlot = (MonsterItemSlotType)Enum.GetValues(typeof(MonsterItemSlotType)).GetValue(i);
                int ts = quinoa.getUI().getGraphicsManager().getTileSize();
                g.DrawRectangle(Pens.LightGray, inventoryXOffset + (int)(i * ts * 2.222), slotYOffset, ts * 2, ts * 2);
                Item tempItem = quinoa.getPlayer().inventory.getItem(tempSlot);
                if(tempItem != null)
                {
                    dtm.drawItem(tempItem, g, inventoryXOffset + (int)(i * ts * 2.222) + (ts /2), slotYOffset + (ts /2), 1.0, quinoa.getUI().getGraphicsManager());
                }
            }


            //draw item selection cursor
            int drawX = 0;
            int drawY = 0;
            if(inventoryTargetY != -1)
            {
                drawX = inventoryXOffset + (quinoa.getUI().getGraphicsManager().getTileSize() * (inventoryTargetX) * 2);
                drawY = inventoryYOffset + (quinoa.getUI().getGraphicsManager().getTileSize() * (inventoryTargetY) * 2);
            }
            else
            {
                drawX = inventoryXOffset + (int)(inventoryTargetX * quinoa.getUI().getGraphicsManager().getTileSize() * 2.222);
                drawY = slotYOffset;
            }

            //draw selection cursor
            g.DrawRectangle(Pens.Black, drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize() * 2, quinoa.getUI().getGraphicsManager().getTileSize() * 2);
            g.DrawRectangle(Pens.White, drawX - 1, drawY - 1, quinoa.getUI().getGraphicsManager().getTileSize() * 2 + 2, quinoa.getUI().getGraphicsManager().getTileSize() * 2 + 2);
            g.DrawRectangle(Pens.White, drawX + 1, drawY + 1, quinoa.getUI().getGraphicsManager().getTileSize() * 2 - 2, quinoa.getUI().getGraphicsManager().getTileSize() * 2 - 2);

            //draw item information window
            int itemInfoX = inventoryXOffset;
            int itemInfoY = inventoryYOffset + MonsterInventory.MAX_HEIGHT * (quinoa.getUI().getGraphicsManager().getTileSize()*2) + 15;
            g.DrawRectangle(Pens.LightGray, itemInfoX, itemInfoY, MonsterInventory.MAX_WIDTH * (quinoa.getUI().getGraphicsManager().getTileSize() * 2), ATTRIBUTE_WINDOW_SIZE);

            Item selectedItem = null;
            int itemHeaderOffsetY=0;
            if(inventoryTargetY == -1)
            {
                selectedItem = quinoa.getPlayer().inventory.getItem((MonsterItemSlotType)Enum.GetValues(typeof(MonsterItemSlotType)).GetValue(inventoryTargetX));
                String slotName = ((MonsterItemSlotType)Enum.GetValues(typeof(MonsterItemSlotType)).GetValue(inventoryTargetX)).ToString();
                dtm.drawString(slotName, 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7, quinoa.getUI().getGraphicsManager());
                itemHeaderOffsetY = 16;
            }
            else
            {
                selectedItem = quinoa.getPlayer().inventory.getItem(inventoryTargetX, inventoryTargetY);
            }

            if(selectedItem != null)
            {
                dtm.drawString(selectedItem.itemClass.ToString(), 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7, quinoa.getUI().getGraphicsManager());
                dtm.drawString("Type:" + selectedItem.itemCategory.ToString(), 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7 + (20 *1), quinoa.getUI().getGraphicsManager());
                //draw all attributes
                int lineCounter=2;
                foreach(ItemAttribute attribute in selectedItem.attributes)
                {
                    String description = (nameof(attribute).Replace("_", " ")) + " " + attribute.parameter;
                    dtm.drawString(description, 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7 + (20 *lineCounter), quinoa.getUI().getGraphicsManager());
                    lineCounter++;
                }
            }

            String formattedMoney = quinoa.getPlayer().inventory.wealth.ToString("0.##");
            dtm.drawString("$:" + formattedMoney, 2.0, g, itemInfoX + 5, itemInfoY + ATTRIBUTE_WINDOW_SIZE + 10, quinoa.getUI().getGraphicsManager());

            dtm.drawString("Move cursor: = " + charToStr(UP_KEY) + "/"+ charToStr(DOWN_KEY) + "/"+ charToStr(LEFT_KEY) + "/"+ charToStr(RIGHT_KEY), 2.0, g, itemInfoX + 5, itemInfoY + ATTRIBUTE_WINDOW_SIZE + 10 + (20 * 2), quinoa.getUI().getGraphicsManager());
            dtm.drawString("Equip/Unequip = " + charToStr(EQUIP_KEY), 2.0, g, itemInfoX + 5, itemInfoY + ATTRIBUTE_WINDOW_SIZE + 10 + (20 * 3), quinoa.getUI().getGraphicsManager());
            dtm.drawString("Drop = " + charToStr(PICKUP_KEY), 2.0, g, itemInfoX + 5, itemInfoY + ATTRIBUTE_WINDOW_SIZE + 10 + (20 * 4), quinoa.getUI().getGraphicsManager());
            dtm.drawString("Press " + charToStr(EXIT_KEY) + " to continue", 2.0, g, itemInfoX + 5, itemInfoY + ATTRIBUTE_WINDOW_SIZE + 10 + (20 * 6), quinoa.getUI().getGraphicsManager());

        }

        public void drawTrade(Graphics g)
        {
            int tradeXOffset = MAP_PIXEL_X_OFFSET + (MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) + 10;
            int tradeYOffset = MAP_PIXEL_Y_OFFSET;

            int monXOffset = tradeXOffset;
            int monYOffset = tradeYOffset + 25 + (MonsterInventory.MAX_HEIGHT * (quinoa.getUI().getGraphicsManager().getTileSize() * 2));

            drawInventoryGrid(g, quinoa.getPlayer(), tradeXOffset, tradeYOffset);
            drawInventoryGrid(g, tradeMonster, monXOffset, monYOffset);

            //draw item selection cursor
            int drawX = 0;
            int drawY = 0;

            drawX = tradeXOffset + (quinoa.getUI().getGraphicsManager().getTileSize() * (tradeTargetX) * 2);
            if(this.tradePageIsPlayer)
            {
                drawY = tradeYOffset + (quinoa.getUI().getGraphicsManager().getTileSize() * (tradeTargetY) * 2);
            }
            else
            {
                drawY = monYOffset + (quinoa.getUI().getGraphicsManager().getTileSize() * (tradeTargetY) * 2);
            }
        
            //draw selection cursor
            g.DrawRectangle(Pens.Black, drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize() * 2, quinoa.getUI().getGraphicsManager().getTileSize() * 2);
            g.DrawRectangle(Pens.White, drawX-1, drawY-1, quinoa.getUI().getGraphicsManager().getTileSize()*2+2, quinoa.getUI().getGraphicsManager().getTileSize()*2+2);
            g.DrawRectangle(Pens.White, drawX+1, drawY+1, quinoa.getUI().getGraphicsManager().getTileSize()*2-2, quinoa.getUI().getGraphicsManager().getTileSize()*2-2);

            //draw item info window
            int itemInfoX = tradeXOffset;
            int itemInfoY = monYOffset + 35 + (MonsterInventory.MAX_HEIGHT * (quinoa.getUI().getGraphicsManager().getTileSize() * 2));
            g.DrawRectangle(Pens.LightGray, itemInfoX, itemInfoY, MonsterInventory.MAX_WIDTH * (quinoa.getUI().getGraphicsManager().getTileSize()*2), ATTRIBUTE_WINDOW_SIZE);

            Item selectedItem = null;
            int itemHeaderOffsetY=0;
            selectedItem = this.getTradeTarget();

            if(selectedItem != null)
            {
                dtm.drawString(selectedItem.itemClass.ToString(), 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7, quinoa.getUI().getGraphicsManager());
                dtm.drawString("Type:" + selectedItem.itemCategory.ToString(), 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7 + (20 *1), quinoa.getUI().getGraphicsManager());
                //draw all attributes
                int lineCounter=2;
                foreach(ItemAttribute attribute in selectedItem.attributes)
                {
                    String description = (attribute.type.ToString().Replace("_", " ")) + " " + attribute.parameter;
                    dtm.drawString(description, 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7 + (20 *lineCounter), quinoa.getUI().getGraphicsManager());
                    lineCounter++;
                }
            }

            //Draw space action
            if(selectedItem != null)
            {
                String message = "";
                if(this.tradePageIsPlayer)
                {
                    message = "Sell for $";
                }
                else
                {
                    message = "Buy for $";
                }
            
                message = message + selectedItem.worth;

                if(selectedItem.stackSize > 1)
                {
                    message = message + " each";
                }
                    
                dtm.drawString(message, 2.0, g, itemInfoX, itemInfoY - 22, quinoa.getUI().getGraphicsManager());
            }
        }

        public void drawDialog(Graphics g)
        {
            int dx = (Parent.Size.Width / 2) - ((MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) / 2);
            int dy = (Parent.Size.Height / 2) - (((quinoa.getMessageManager().getDialogSize() + 4) * quinoa.getUI().getGraphicsManager().getTileSize() + 5) / 2) - 20;
            g.FillRectangle(Brushes.Black, dx + MAP_PIXEL_X_OFFSET, dy + MAP_PIXEL_Y_OFFSET, MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize(), (quinoa.getMessageManager().getDialogSize() + 2) * quinoa.getUI().getGraphicsManager().getTileSize() + 7);
            g.DrawRectangle(Pens.LightGray, dx + MAP_PIXEL_X_OFFSET-1, dy + MAP_PIXEL_Y_OFFSET-1, MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize() + 2, (quinoa.getMessageManager().getDialogSize() + 2) * quinoa.getUI().getGraphicsManager().getTileSize() + 7);
            for(int i=0; i < quinoa.getMessageManager().getDialogSize(); i++)
            {
                dtm.drawString(quinoa.getMessageManager().getMessage(quinoa.getMessageManager().getDialogSize() - 1 - i), 2, g, dx + 2 + MAP_PIXEL_X_OFFSET, dy + 2 + MAP_PIXEL_Y_OFFSET + i * 16, quinoa.getUI().getGraphicsManager());
            }
            dtm.drawString("Press " + charToStr(EXIT_KEY) + " to continue.", 2, g, dx + 2 + MAP_PIXEL_X_OFFSET, dy + 2 + MAP_PIXEL_Y_OFFSET + (quinoa.getMessageManager().getDialogSize() + 1) * 16, quinoa.getUI().getGraphicsManager());
        }

        public void drawPickVerb(Graphics g)
        {
            List<ItemVerbType> itemVerbs = ItemManager.getVerbs(verbItem);
            int vWidth = 200;
            int vHeight = ((itemVerbs.Count) * 20) + 20;
            int dx = (Parent.Size.Width / 2) - (vWidth / 2);
            int dy = (Parent.Size.Height / 2) - (vHeight / 2) -20;

            g.FillRectangle(Brushes.Black, dx, dy, vWidth, vHeight);
            g.DrawRectangle(Pens.LightGray,dx, dy, vWidth, vHeight);

            int ivCounter=0;
            foreach(ItemVerbType iv in itemVerbs)
            {
                g.FillRectangle((ivCounter == verbIndex) ? Brushes.White : Brushes.LightGray, dx + 10, dy + (ivCounter * 20) + 10, 10, 10);
                dtm.drawString(iv.ToString(), 2.0, g, dx + 25, dy + 10 + (ivCounter * 20), quinoa.getUI().getGraphicsManager());
                ivCounter++;
            }


        }

        public void drawMapSelect(Graphics g)
        {
            int drawX = MAP_PIXEL_X_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * (targetX - tileOffsetX));
                int drawY = MAP_PIXEL_Y_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * (targetY - tileOffsetY));
                g.DrawRectangle(Pens.Black, drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize());
                g.DrawRectangle(Pens.White, drawX-1, drawY-1, quinoa.getUI().getGraphicsManager().getTileSize()+2, quinoa.getUI().getGraphicsManager().getTileSize()+2);
                g.DrawRectangle(Pens.White, drawX+1, drawY+1, quinoa.getUI().getGraphicsManager().getTileSize()-2, quinoa.getUI().getGraphicsManager().getTileSize()-2);
        }

        public void drawInventoryGrid(Graphics g, Monster monster, int tradeXOffset, int tradeYOffset)
        {
            for(int x=0; x < MonsterInventory.MAX_WIDTH; x++)
            {
                for(int y=0; y < MonsterInventory.MAX_HEIGHT; y++)
                {
                   int ts = quinoa.getUI().getGraphicsManager().getTileSize();
                   g.DrawRectangle(Pens.DarkGray, tradeXOffset + (x * ts * 2), tradeYOffset + (y * ts * 2), ts * 2, ts * 2);
                }
            }
            for(int x=0; x < monster.inventory.width; x++)
            {
                for(int y=0; y < monster.inventory.height; y++)
                {
                   int ts = quinoa.getUI().getGraphicsManager().getTileSize();
                   g.DrawRectangle(Pens.LightGray, tradeXOffset + (x * ts * 2), tradeYOffset + (y * ts * 2), ts * 2, ts * 2);
                }
            }
            foreach(Item tempItem in monster.inventory.getItems())
            {
                int ts = quinoa.getUI().getGraphicsManager().getTileSize();
                dtm.drawItem(tempItem, g, tradeXOffset + (tempItem.x * ts * 2) + (ts/2), tradeYOffset + (tempItem.y * ts * 2) + (ts/2), 1.0, quinoa.getUI().getGraphicsManager());
            }

            String name = monster.monsterCode.ToString();
            if(monster.ID.Equals(MonsterActionManager.PLAYER_ID))
            {
                name = "YOU";
            }
            else if(monster.role != MonsterRoleType.NULL)
            {
                name = monster.role.ToString();
            }
            dtm.drawString(name, 2.0, g, tradeXOffset, tradeYOffset - 16, quinoa.getUI().getGraphicsManager());
            String formattedMoney = monster.inventory.wealth.ToString("0.##");
            dtm.drawString("$" + formattedMoney, 2.0, g, tradeXOffset + 175, tradeYOffset - 16, quinoa.getUI().getGraphicsManager());
        }

        public void drawMessages(Graphics g)
        {
            for(int i=0; i < MESSAGES_LINE_NUMBER; i++)
            {
                int fontScale=2;
                dtm.drawString(quinoa.getMessageManager().getMessage((MESSAGES_LINE_NUMBER-1) - i), fontScale, g, MAP_PIXEL_X_OFFSET, MESSAGES_PIXEL_Y_OFFSET + (int)(i * quinoa.getUI().getGraphicsManager().getLetterHeight() * fontScale * quinoa.getUI().getGraphicsManager().getLetterSpacingPercent()) , quinoa.getUI().getGraphicsManager());
            }
        }

        public void drawCharacter(Graphics g)
        {
            int charXOffset = MAP_PIXEL_X_OFFSET + (MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) + 15;
            int charYOffset = MAP_PIXEL_Y_OFFSET;

            dtm.drawString("Life: " + quinoa.getPlayer().stats.getDisplayHealth(), 2.0, g, charXOffset, charYOffset + (20 * 0), quinoa.getUI().getGraphicsManager());
        
            dtm.drawString("Health: " + quinoa.getPlayer().stats.getHealth(), 2.0, g, charXOffset, charYOffset + (20 * 2), quinoa.getUI().getGraphicsManager());
            dtm.drawString("Might: " + quinoa.getPlayer().stats.getMight(), 2.0, g, charXOffset, charYOffset + (20 * 3), quinoa.getUI().getGraphicsManager());
            dtm.drawString("Endurance: " + quinoa.getPlayer().stats.getEndurance(), 2.0, g, charXOffset, charYOffset + (20 * 4), quinoa.getUI().getGraphicsManager());
            dtm.drawString("Agility: " + quinoa.getPlayer().stats.getAgility(), 2.0, g, charXOffset, charYOffset + (20 * 5), quinoa.getUI().getGraphicsManager());

            if(quinoa.getPlayer().stats.getAvailableStatPoints() > 0)
            {
                dtm.drawString(quinoa.getPlayer().stats.getAvailableStatPoints()+"", 2.0, g, charXOffset + 300, charYOffset + (20 * 2) - 22, quinoa.getUI().getGraphicsManager());
                for(int i=0; i < 4; i++)
                {
                    if(characterIndex == i)
                    {
                        g.DrawRectangle(Pens.White, charXOffset + 300, charYOffset + (20 * (2 + i)) - 2, 12,12);
                        g.DrawRectangle(Pens.White, charXOffset + 300 - 2, charYOffset + (20 * (2 + i)) - 4, 16,16);
                        g.DrawLine(Pens.White, charXOffset + 300 + 6, charYOffset + (20 * (2 + i)), charXOffset + 300 + 6, charYOffset + (20 * (2 + i)) + 8);
                        g.DrawLine(Pens.White, charXOffset + 300 + 2, charYOffset + (20 * (2 + i)) + 4, charXOffset + 300 + 10, charYOffset + (20 * (2 + i)) + 4);
                    }
                    else
                    {
                        g.DrawRectangle(Pens.LightGray, charXOffset + 300, charYOffset + (20 * (2 + i)) - 2, 12,12);
                        g.DrawLine(Pens.LightGray, charXOffset + 300 + 6, charYOffset + (20 * (2 + i)), charXOffset + 300 + 6, charYOffset + (20 * (2 + i)) + 8);
                        g.DrawLine(Pens.LightGray, charXOffset + 300 + 2, charYOffset + (20 * (2 + i)) + 4, charXOffset + 300 + 10, charYOffset + (20 * (2 + i)) + 4);
                    }
                
                }
            }

            dtm.drawString("Defense: " + MonsterActionManager.getDefenseRating(quinoa.getPlayer()), 2.0, g, charXOffset, charYOffset + (20 * 7), quinoa.getUI().getGraphicsManager());
            dtm.drawString("Attack: " + MonsterActionManager.getAttackRating(quinoa.getPlayer()), 2.0, g, charXOffset, charYOffset + (20 * 8), quinoa.getUI().getGraphicsManager());
            dtm.drawString("Speed: " + MonsterActionManager.getSpeed(quinoa.getPlayer()) + " ticks per turn", 2.0, g, charXOffset, charYOffset + (20 * 9), quinoa.getUI().getGraphicsManager());

            dtm.drawString("Level: " + quinoa.getPlayer().stats.getLevel(), 2.0, g, charXOffset, charYOffset + (20 * 11), quinoa.getUI().getGraphicsManager());
            dtm.drawString("XP: " + quinoa.getPlayer().stats.getExperience(), 2.0, g, charXOffset, charYOffset + (20 * 12), quinoa.getUI().getGraphicsManager());
            dtm.drawString("Next: " + MonsterActionManager.getNextXPLevel(quinoa.getPlayer()), 2.0, g, charXOffset, charYOffset + (20 * 13), quinoa.getUI().getGraphicsManager());

            dtm.drawString("Hunger: " + quinoa.getPlayer().stats.getDisplayHunger(), 2.0, g, charXOffset, charYOffset + (20 * 15), quinoa.getUI().getGraphicsManager());

            if(quinoa.getPlayer().stats.getAvailableStatPoints() > 0)
            {
                dtm.drawString("Move Cursor = " + charToStr(UP_KEY) + "/" + charToStr(DOWN_KEY), 2.0, g, charXOffset, charYOffset + (20 * 25), quinoa.getUI().getGraphicsManager());
                dtm.drawString("Distribute Point = " + charToStr(ENTER_KEY), 2.0, g, charXOffset, charYOffset + (20 * 26), quinoa.getUI().getGraphicsManager());
            }
            dtm.drawString("Press " + charToStr(EXIT_KEY) + " to continue", 2.0, g, charXOffset, charYOffset + (20 * 28), quinoa.getUI().getGraphicsManager());

        }

        public void cycle()
        {
            if(quinoa.getCurrentRegionHeader().regionIsLoaded())
            {
                //Cycle each monster
                foreach(Monster tempMon in quinoa.getCurrentRegionHeader().getRegion().getMonsters())
                {
                    tempMon.cycle(quinoa);
                }
                quinoa.getCurrentRegionHeader().getRegion().removeExpiredObjects();
                quinoa.getActions().tickClock();

                //Do the region cycle if appropriate
                regionCycleCounter--;
                if(regionCycleCounter == 0)
                {
                    regionCycleCounter = Quinoa.REGION_CYCLE_FREQUENCY;
                    quinoa.getActions().regionCycle();
                }

                //Do half-hourly cycle if appropriate
                if(quinoa.getTicks() % (Quinoa.TICKS_PER_SECOND * 60 * 30) == 0)
                {
                    quinoa.getActions().spawnMonsters();
                }

                //Do two hour cycle
                if(quinoa.getTicks() % (Quinoa.TICKS_PER_SECOND * 60 * 120) == 0)
                {
                    quinoa.getActions().longCycle();
                }

                //Check for player death
                if(quinoa.playerIsDead() && this.readyForInput())
                {
                    quinoa.getMessageManager().addMessage("You have died.");
                    quinoa.getActions().showDialog();
                }

                //Check for regions switching
                quinoa.getActions().checkForPlayerOnExits();

                //Check for monsters to add
                quinoa.getActions().addQueuedMonsters();

                //Inform on hunger change
                if(!lastHungerStatus.Equals(quinoa.getPlayer().stats.getDisplayHunger()))
                {
                    quinoa.getMessageManager().addMessage("You are " + quinoa.getPlayer().stats.getDisplayHunger() + ".");
                    this.lastHungerStatus = quinoa.getPlayer().stats.getDisplayHunger();
                }
            }
        }

        public bool readyForInput()
        {
            return(quinoa.getPlayer().readyForCommand());
        }

        public void displayDialog()
        {
            this.setMode(AdventureScreenModeType.DIALOG);
        }

        public void displayTrade(Monster monster)
        {
            this.setTradeMonster(monster);
            this.setMode(AdventureScreenModeType.TRADE);
        }

        public void doUseItem(Item item)
        {
            if(item != null)
            {
                List<ItemVerbType> itemVerbs = ItemManager.getVerbs(item);
                if(itemVerbs.Count == 0)
                {
                    //do nothing
                }
                else if(itemVerbs.Count == 1)
                {
                    verbIndex = 0;
                    verb = itemVerbs[verbIndex];
                    verbItem = item;
                    targetMaxDistance = ItemManager.verbDistance(verb);
                    if(targetMaxDistance > 0)
                    {
                        this.mapSelectAction = AdvenureScreenMapSelectAction.VERB;
                        this.setMapSelectMaxDistance(targetMaxDistance);
                        this.setMode(AdventureScreenModeType.MAP_SELECT);
                        quinoa.getUI().refresh();
                    }
                    else
                    {
                        MonsterActionManager.setItemVerbCommand(quinoa.getPlayer(), verbItem, verb, 0, 0);
                        this.setMode(AdventureScreenModeType.MAP);
                        quinoa.getUI().refresh();
                        quinoa.cycle();
                    }
                }
                else if(itemVerbs.Count > 1)
                {
                    verbItem = item;
                    this.setMode(AdventureScreenModeType.VERB_PICK);
                    quinoa.getUI().refresh();
                }
            }
        }

        public void doMovementInDirection(Direction direction)
        {
            int newX = quinoa.getPlayer().x;
            int newY = quinoa.getPlayer().y;

            switch(direction)
            {
                case Direction.N: newY--; break;
                case Direction.NE: newY--; newX++; break;
                case Direction.NW: newY--; newX--; break;
                case Direction.S: newY++; break;
                case Direction.SE: newY++; newX++; break;
                case Direction.SW: newY++; newX--; break;
                case Direction.E: newX++; break;
                case Direction.W: newX--; break;
            }

            Monster checkForMonster = quinoa.getMonster(newX, newY);
            if(checkForMonster == null)
            {
                //Do normal movement
                MonsterActionManager.setMoveCommand(quinoa.getPlayer(), direction);
            }
            else
            {
                //Attempt attack
                MonsterActionManager.setAttackCommand(quinoa.getPlayer(), checkForMonster.ID);
            }
            quinoa.cycle();
        }

        public void moveMapTarget(int newX, int newY)
        {
            //Get distance from proposed target to player
            int distance = (int)(Math.Sqrt((newX-quinoa.getPlayer().x) * (newX-quinoa.getPlayer().x) + (newY-quinoa.getPlayer().y) * (newY-quinoa.getPlayer().y)));

            if(distance <= this.targetMaxDistance || this.targetMaxDistance == 0)
            {
                LumberjackRL.Core.Map.Region region = quinoa.getCurrentRegionHeader().getRegion();
                if(newY >= 0 && newY < region.getHeight()
                && newX >= 0 && newX < region.getWidth())
                {
                    targetX = newX;
                    targetY = newY;
                }
                quinoa.getUI().refresh();
            }
        }

        public void moveInventoryTarget(int newX, int newY)
        {
            if(newY == -1)
            {
                if(newX >= Enum.GetValues(typeof(MonsterItemSlotType)).Length)
                {
                    newX = Enum.GetValues(typeof(MonsterItemSlotType)).Length - 1;
                }

                if (newX >= 0 && newX < Enum.GetValues(typeof(MonsterItemSlotType)).Length)
                {
                    inventoryTargetX = newX;
                    inventoryTargetY = newY;
                }
            }
            else
            {
                if(newY >= 0 && newY < quinoa.getPlayer().inventory.height
                && newX >= 0 && newX < quinoa.getPlayer().inventory.width)
                {
                    inventoryTargetX = newX;
                    inventoryTargetY = newY;
                }
            }
            quinoa.getUI().refresh();
        }

        public void moveTradeTarget(int newX, int newY)
        {
            if(tradePageIsPlayer)
            {
                if(newY >= quinoa.getPlayer().inventory.height)
                {
                    if(tradeTargetX >= tradeMonster.inventory.width)
                    {
                        tradeTargetX = tradeMonster.inventory.width-1;
                    }
                    tradeTargetY = 0;
                    tradePageIsPlayer = false;
                }
                else
                {
                    if(newY >= 0 && newX >= 0 && newX < quinoa.getPlayer().inventory.width)
                    {
                        tradeTargetX = newX;
                        tradeTargetY = newY;
                    }
                }
            }
            else
            {
                if(newY < 0)
                {
                    if(tradeTargetX >= quinoa.getPlayer().inventory.width)
                    {
                        tradeTargetX = quinoa.getPlayer().inventory.width-1;
                    }
                    tradeTargetY = quinoa.getPlayer().inventory.height-1;
                    tradePageIsPlayer = true;
                }
                else
                {
                    if(newY < tradeMonster.inventory.height && newX >= 0 && newX < tradeMonster.inventory.width)
                    {
                        tradeTargetX = newX;
                        tradeTargetY = newY;
                    }
                }
            }
            quinoa.getUI().refresh();
        }

        public Item getInventoryTarget()
        {
            if(inventoryTargetY == -1)
            {
                return quinoa.getPlayer().inventory.getItem((MonsterItemSlotType)Enum.GetValues(typeof(MonsterItemSlotType)).GetValue(inventoryTargetX));
            }
            else
            {
                return quinoa.getPlayer().inventory.getItem(inventoryTargetX, inventoryTargetY);
            }

        }

        public Item getTradeTarget()
        {
            if(this.tradePageIsPlayer)
            {
                return quinoa.getPlayer().inventory.getItem(tradeTargetX, tradeTargetY);
            }
            else
            {
                return tradeMonster.inventory.getItem(tradeTargetX, tradeTargetY);
            }
        }

        public void processKey(char key, bool shift, bool alt)
        {
            switch(mode)
            {
                case AdventureScreenModeType.MAP:
                processKeyMap(key,shift,alt);
                break;

                case AdventureScreenModeType.HELP:
                processKeyHelp(key,shift,alt);
                break;

                case AdventureScreenModeType.DIALOG:
                processKeyDialog(key,shift,alt);
                break;

                case AdventureScreenModeType.MAP_SELECT:
                processKeyMapSelect(key,shift,alt);
                break;

                case AdventureScreenModeType.INVENTORY:
                processKeyInventory(key,shift,alt);
                break;

                case AdventureScreenModeType.CHARACTER:
                processKeyCharacter(key,shift,alt);
                break;

                case AdventureScreenModeType.VERB_PICK:
                processKeyVerbPick(key,shift,alt);
                break;

                case AdventureScreenModeType.TRADE:
                processKeyTrade(key,shift,alt);
                break;

            }
        }

        public void processKeyMap(char key, bool shift, bool alt)
        {
            Monster player = quinoa.getPlayer();

            //Check if another command can be entered
            if(!player.readyForCommand())
            {
                return;
            }

            if(key == AdventureScreen.UP_KEY)
            {
                doMovementInDirection(Direction.N);
            }
            else if(key == AdventureScreen.DOWN_KEY)
            {
                doMovementInDirection(Direction.S);
            }
            else if(key == AdventureScreen.LEFT_KEY)
            {
                doMovementInDirection(Direction.W);
            }
            else if(key == AdventureScreen.RIGHT_KEY)
            {
                doMovementInDirection(Direction.E);
            }
            else if(key == AdventureScreen.SAVE_KEY)
            {
                quinoa.getActions().saveGame();
                quinoa.getActions().showDialog();
                quinoa.cycle();
            }
            else if(key == AdventureScreen.LOAD_KEY)
            {
                quinoa.getActions().loadGame();
                quinoa.getActions().showDialog();
                quinoa.cycle();
            }
            else if(key == AdventureScreen.PICKUP_KEY)
            {
                MonsterActionManager.setPickupCommand(quinoa.getPlayer());
                quinoa.cycle();
            }
            else if(key == AdventureScreen.INVENTORY_KEY)
            {
                this.setMode(AdventureScreenModeType.INVENTORY);
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.CHARACTER_KEY)
            {
                this.setMode(AdventureScreenModeType.CHARACTER);
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.HELP_KEY)
            {
                this.setMode(AdventureScreenModeType.HELP);
                quinoa.getUI().refresh();
            }
            else if(key == '-')
            {
                quinoa.getPlayer().stats.setCurrentHealth(quinoa.getPlayer().stats.getCurrentHealth() - 1);
                quinoa.cycle();
            }
            else if(key == AdventureScreen.MESSAGES_KEY)
            {
                quinoa.getMessageManager().dialogLastMessages();
                this.setMode(AdventureScreenModeType.DIALOG);
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.BELT_1_KEY)
            {
                doUseItem(quinoa.getPlayer().inventory.getItem(MonsterItemSlotType.BELT_1));
            }
            else if(key == AdventureScreen.BELT_2_KEY)
            {
                doUseItem(quinoa.getPlayer().inventory.getItem(MonsterItemSlotType.BELT_2));
            }
            else if(key == AdventureScreen.BELT_3_KEY)
            {
                doUseItem(quinoa.getPlayer().inventory.getItem(MonsterItemSlotType.BELT_3));
            }
            else if(key == AdventureScreen.BELT_4_KEY)
            {
                doUseItem(quinoa.getPlayer().inventory.getItem(MonsterItemSlotType.BELT_4));
            }
            else if(key == AdventureScreen.ACTION_KEY)
            {
                verbItem = null;
                verbIndex = 0;
                this.setMode(AdventureScreenModeType.VERB_PICK);
                quinoa.getUI().refresh();
            }
        }

        public void processKeyDialog(char key, bool shift, bool alt)
        {
            if(key == AdventureScreen.EXIT_KEY)
            {
                if(quinoa.playerIsDead())
                {
                    quinoa.getUI().setInterfaceMode(InterfaceMode.MENU);
                    quinoa.cycle();
                }
                else
                {
                    this.setMode(AdventureScreenModeType.MAP);
                    quinoa.cycle();
                }
            }
        }

        public void processKeyHelp(char key, bool shift, bool alt)
        {
            if(key == AdventureScreen.EXIT_KEY)
            {
                this.setMode(AdventureScreenModeType.MAP);
            }
        }
    
        public void processKeyVerbPick(char key, bool shift, bool alt)
        {
            if(key == AdventureScreen.ENTER_KEY)
            {
                List<ItemVerbType> itemVerbs = ItemManager.getVerbs(verbItem);
                verb = itemVerbs[verbIndex];
                this.targetMaxDistance = ItemManager.verbDistance(verb);
                if(targetMaxDistance > 0)
                {
                    this.mapSelectAction = AdvenureScreenMapSelectAction.VERB;
                    this.setMapSelectMaxDistance(targetMaxDistance);
                    this.setMode(AdventureScreenModeType.MAP_SELECT);
                    quinoa.getUI().refresh();
                }
                else
                {
                    MonsterActionManager.setItemVerbCommand(quinoa.getPlayer(), verbItem, verb, quinoa.getPlayer().x, quinoa.getPlayer().y);
                    this.setMode(AdventureScreenModeType.MAP);
                    quinoa.getUI().refresh();
                    quinoa.cycle();
                }
            }
            else if (key == AdventureScreen.UP_KEY)
            {
                if(verbIndex > 0)
                {
                    verbIndex--;
                }
                quinoa.getUI().refresh();
            }
            else if (key == AdventureScreen.DOWN_KEY)
            {
                List<ItemVerbType> itemVerbs = ItemManager.getVerbs(verbItem);
                if(verbIndex < itemVerbs.Count - 1)
                {
                    verbIndex++;
                }
                quinoa.getUI().refresh();
            }
        }

        public void processKeyMapSelect(char key, bool shift, bool alt)
        {
            if(key == AdventureScreen.UP_KEY)
            {
                moveMapTarget(targetX, targetY - 1);
            }
            else if(key == AdventureScreen.DOWN_KEY)
            {
                moveMapTarget(targetX, targetY + 1);
            }
            else if(key == AdventureScreen.LEFT_KEY)
            {
                moveMapTarget(targetX - 1, targetY);
            }
            else if(key == AdventureScreen.RIGHT_KEY)
            {
                moveMapTarget(targetX + 1, targetY);
            }
            else if(key == AdventureScreen.ENTER_KEY)
            {
                switch(mapSelectAction)
                {
                    case AdvenureScreenMapSelectAction.VERB:

                    //reset facing
                    if(targetY > quinoa.getPlayer().y)
                    {
                        quinoa.getPlayer().facing = Direction.S;
                    }
                    else if(targetY < quinoa.getPlayer().y)
                    {
                        quinoa.getPlayer().facing = Direction.N;
                    }
                    else if(targetX > quinoa.getPlayer().x)
                    {
                        quinoa.getPlayer().facing = Direction.E;
                    }
                    else if(targetX < quinoa.getPlayer().x)
                    {
                        quinoa.getPlayer().facing = Direction.W;
                    }

                    MonsterActionManager.setItemVerbCommand(quinoa.getPlayer(), verbItem, verb, targetX, targetY);
                    this.setMode(AdventureScreenModeType.MAP);
                    quinoa.cycle();
                    break;
                }
            }
        }

        public void processKeyInventory(char key, bool shift, bool alt)
        {
            if(key == AdventureScreen.EXIT_KEY)
            {
                this.setMode(AdventureScreenModeType.MAP);
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.CHARACTER_KEY)
            {
                this.setMode(AdventureScreenModeType.CHARACTER);
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.UP_KEY)
            {
                moveInventoryTarget(inventoryTargetX, inventoryTargetY - 1);
            }
            else if(key == AdventureScreen.DOWN_KEY)
            {
                moveInventoryTarget(inventoryTargetX, inventoryTargetY + 1);
            }
            else if(key == AdventureScreen.LEFT_KEY)
            {
                moveInventoryTarget(inventoryTargetX - 1, inventoryTargetY);
            }
            else if(key == AdventureScreen.RIGHT_KEY)
            {
                moveInventoryTarget(inventoryTargetX + 1, inventoryTargetY);
            }
            else if(key == AdventureScreen.PICKUP_KEY)
            {
                Item itemToDrop = getInventoryTarget();
                if(itemToDrop != null)
                {
                    MonsterActionManager.setDropCommand(quinoa.getPlayer(), itemToDrop.ID);
                }
                quinoa.cycle();
            }
            else if(key == AdventureScreen.EQUIP_KEY)
            {
                Item itemToEquip = getInventoryTarget();

                if(itemToEquip != null)
                {
                    if(inventoryTargetY != -1)
                    {
                        MonsterActionManager.setEquipCommand(quinoa.getPlayer(), itemToEquip.ID);
                    }
                    else
                    {
                        MonsterActionManager.setUnequipCommand(quinoa.getPlayer(), itemToEquip.ID);
                    }
                }
                quinoa.cycle();
            }
        }

        public void processKeyTrade(char key, bool shift, bool alt)
        {
            if(key == AdventureScreen.EXIT_KEY)
            {
                this.setMode(AdventureScreenModeType.MAP);
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.UP_KEY)
            {
                moveTradeTarget(tradeTargetX, tradeTargetY - 1);
            }
            else if(key == AdventureScreen.DOWN_KEY)
            {
                moveTradeTarget(tradeTargetX, tradeTargetY + 1);
            }
            else if(key == AdventureScreen.LEFT_KEY)
            {
                moveTradeTarget(tradeTargetX - 1, tradeTargetY);
            }
            else if(key == AdventureScreen.RIGHT_KEY)
            {
                moveTradeTarget(tradeTargetX + 1, tradeTargetY);
            }
            else if(key == AdventureScreen.ENTER_KEY)
            {
                Item selectedItem = this.getTradeTarget();
                if(selectedItem != null)
                {
                    if(this.tradePageIsPlayer)
                    {
                        MonsterActionManager.sellItemToMonster(quinoa.getPlayer(), tradeMonster, selectedItem, quinoa);
                    }
                    else
                    {
                        MonsterActionManager.sellItemToMonster(tradeMonster, quinoa.getPlayer(), selectedItem, quinoa);
                    }
                }
                quinoa.getUI().refresh();
            }
        }

        public void processKeyCharacter(char key, bool shift, bool alt)
        {
            if(key == AdventureScreen.EXIT_KEY)
            {
                this.setMode(AdventureScreenModeType.MAP);
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.INVENTORY_KEY)
            {
                this.setMode(AdventureScreenModeType.INVENTORY);
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.UP_KEY)
            {
                if(characterIndex > 0)
                {
                    characterIndex--;
                }
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.DOWN_KEY)
            {
                if(characterIndex < 3)
                {
                    characterIndex++;
                }
                quinoa.getUI().refresh();
            }
            else if(key == AdventureScreen.ENTER_KEY)
            {
                if(quinoa.getPlayer().stats.getAvailableStatPoints() > 0)
                {
                    switch(characterIndex)
                    {
                        case 0: //Health
                        if(quinoa.getPlayer().stats.getHealth() < MonsterStats.MAX_STAT)
                        {
                            quinoa.getPlayer().stats.setAvailableStatPoints(quinoa.getPlayer().stats.getAvailableStatPoints() - 1);
                            quinoa.getPlayer().stats.setHealth(quinoa.getPlayer().stats.getHealth() + 1);
                            quinoa.getPlayer().stats.setCurrentHealth(quinoa.getPlayer().stats.getMaxHP());
                        }
                        break;

                        case 1: //Might
                        if(quinoa.getPlayer().stats.getMight() < MonsterStats.MAX_STAT)
                        {
                            quinoa.getPlayer().stats.setAvailableStatPoints(quinoa.getPlayer().stats.getAvailableStatPoints() - 1);
                            quinoa.getPlayer().stats.setMight(quinoa.getPlayer().stats.getMight() + 1);
                        }
                        break;

                        case 2: //Endurance
                        if(quinoa.getPlayer().stats.getEndurance() < MonsterStats.MAX_STAT)
                        {
                            quinoa.getPlayer().stats.setAvailableStatPoints(quinoa.getPlayer().stats.getAvailableStatPoints() - 1);
                            quinoa.getPlayer().stats.setEndurance(quinoa.getPlayer().stats.getEndurance() + 1);
                        }
                        break;

                        case 3: //Agility
                        if(quinoa.getPlayer().stats.getAgility() < MonsterStats.MAX_STAT)
                        {
                            quinoa.getPlayer().stats.setAvailableStatPoints(quinoa.getPlayer().stats.getAvailableStatPoints() - 1);
                            quinoa.getPlayer().stats.setAgility(quinoa.getPlayer().stats.getAgility() + 1);
                        }
                        break;
                    }

                    quinoa.getUI().refresh();
                }
            }
        }
    }
}
