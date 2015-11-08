package quinoa.ui;

import java.awt.Color;
import java.awt.Graphics;
import java.util.ArrayList;
import quinoa.Quinoa;
import quinoa.items.Item;
import quinoa.items.ItemAttribute;
import quinoa.items.ItemManager;
import quinoa.items.ItemManager.ItemVerb;
import quinoa.monsters.Monster;
import quinoa.monsters.MonsterActionManager;
import quinoa.monsters.MonsterActionManager.MonsterRole;
import quinoa.monsters.MonsterInventory;
import quinoa.monsters.MonsterInventory.ItemSlot;
import quinoa.monsters.MonsterStats;
import quinoa.region.Building;
import quinoa.region.OverworldCell;
import quinoa.region.Region;
import quinoa.region.RegionExit;

public class AdventureScreen implements ScreenInterface
{
    public static final int MAP_PIXEL_X_OFFSET = 12;
    public static final int MAP_PIXEL_Y_OFFSET = 46;
    public static final int MAP_WINDOW_TILE_SIZE_X = 37;
    public static final int MAP_WINDOW_TILE_SIZE_Y = 33;
    public static final int MESSAGES_PIXEL_Y_OFFSET = 583;
    public static final int TIME_DISPLAY_OFFSET = 465;
    public static final int MESSAGES_LINE_NUMBER = 3;
    public static final int ATTRIBUTE_WINDOW_SIZE = 200;

    public static final char UP_KEY = 'w';
    public static final char DOWN_KEY = 's';
    public static final char RIGHT_KEY = 'd';
    public static final char LEFT_KEY = 'a';
    public static final char ENTER_KEY = ' ';
    public static final char EXIT_KEY = 'x';
    public static final char INVENTORY_KEY = 'i';
    public static final char CHARACTER_KEY = 'c';
    public static final char MESSAGES_KEY = 'm';
    public static final char EQUIP_KEY = 'e';
    public static final char PICKUP_KEY = 'p';
    public static final char ACTION_KEY = 'z';
    public static final char BELT_1_KEY = '1';
    public static final char BELT_2_KEY = '2';
    public static final char BELT_3_KEY = '3';
    public static final char BELT_4_KEY = '4';
    public static final char SAVE_KEY = 'v';
    public static final char LOAD_KEY = 'l';
    public static final char HELP_KEY = 'h';


    public enum AdventureScreenMode {MAP, DIALOG, MAP_SELECT, INVENTORY, CHARACTER, VERB_PICK, TRADE, HELP};
    public enum MapSelectAction {VERB};
    
    private DrawManager dtm;
    private Quinoa quinoa;
    private int tileOffsetX, tileOffsetY;
    private int targetX, targetY;
    private int targetMaxDistance;
    private int inventoryTargetX, inventoryTargetY;
    private int tradeTargetX, tradeTargetY;
    private int characterIndex;
    private Item verbItem;
    private ItemVerb verb;
    private int verbIndex;
    private String lastHungerStatus;
    private Monster tradeMonster;
    private boolean tradePageIsPlayer;
    private MapSelectAction mapSelectAction;
    private AdventureScreenMode mode;
    private int regionCycleCounter;
    
    public AdventureScreen(Quinoa quinoa)
    {
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
        this.mapSelectAction = MapSelectAction.VERB;
        this.mode = AdventureScreenMode.MAP;
        this.lastHungerStatus = "";
        this.verb = null;
        this.verbItem = null;
        this.verbIndex = 0;
        this.tradeMonster = null;
        this.tradePageIsPlayer = true;
        this.regionCycleCounter = Quinoa.REGION_CYCLE_FREQUENCY;
    }

    public void setMode(AdventureScreenMode newMode)
    {
        switch(newMode)
        {
            case MAP:
            quinoa.getUI().refresh();
            this.mode = AdventureScreenMode.MAP;
            break;

            case HELP:
            this.mode = AdventureScreenMode.HELP;
            break;

            case DIALOG:
            this.mode = AdventureScreenMode.DIALOG;
            break;

            case MAP_SELECT:
            targetX = quinoa.getPlayer().getX();
            targetY = quinoa.getPlayer().getY();

            switch(quinoa.getPlayer().getFacing())
            {
                case N:
                if(targetY > 0)
                {
                    targetY--;
                }
                break;

                case S:
                if(targetY < quinoa.getCurrentRegionHeader().getRegion().getHeight() - 2)
                {
                    targetY++;
                }
                break;

                case W:
                if(targetX > 0)
                {
                    targetX--;
                }
                break;

                case E:
                if(targetX < quinoa.getCurrentRegionHeader().getRegion().getWidth() - 2)
                {
                    targetX++;
                }
                break;
            }

            this.mode = AdventureScreenMode.MAP_SELECT;
            quinoa.getMessageManager().addMessage("Press " + charToStr(ENTER_KEY) + " to select an area.");
            break;

            case INVENTORY:
            this.mode = AdventureScreenMode.INVENTORY;
            this.inventoryTargetX = 0;
            this.inventoryTargetY = 0;
            break;

            case CHARACTER:
            this.mode = AdventureScreenMode.CHARACTER;
            break;

            case VERB_PICK:
            this.mode = AdventureScreenMode.VERB_PICK;
            quinoa.getMessageManager().addMessage("Press " + charToStr(ENTER_KEY) + " select an action.");
            this.verb = null;
            this.verbIndex = 0;
            break;

            case TRADE:
            this.tradeTargetX = 0;
            this.tradeTargetY = 0;
            this.tradePageIsPlayer = true;
            quinoa.getMessageManager().addMessage("Press " + charToStr(ENTER_KEY) + " to sell or buy the selected item.");
            quinoa.getMessageManager().addMessage("Press " + charToStr(EXIT_KEY) + " to stop trading.");
            this.mode = AdventureScreenMode.TRADE;
            break;
        }
    }

    public void setTradeMonster(Monster monster)
    {
        this.tradeMonster = monster;
    }
    
    public void setMapSelectAction(MapSelectAction msa)
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
        g.setColor(Color.black);
        g.fillRect(0,0,QuinoaWindow.UI_PIXEL_WIDTH, QuinoaWindow.UI_PIXEL_HEIGHT);

        switch(mode)
        {
            case MAP:
            this.centerOnPoint(quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
            if(quinoa.getPlayer() != null && !quinoa.getPlayer().isSleeping())
            {
                drawMap(g);
            }
            drawMapSideBar(g);
            drawOverworldMap(g);
            drawMessages(g);
            break;

            case HELP:
            this.centerOnPoint(quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
            if(quinoa.getPlayer() != null && !quinoa.getPlayer().isSleeping())
            {
                drawMap(g);
            }
            drawMapSideBar(g);
            drawMessages(g);
            drawHelpSideBar(g);
            break;
            
            case INVENTORY:
            this.centerOnPoint(quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
            drawMap(g);
            drawInventory(g);
            drawMessages(g);
            break;

            case DIALOG:
            this.centerOnPoint(quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
            drawMap(g);
            drawMapSideBar(g);
            drawOverworldMap(g);
            drawDialog(g);
            break;

            case MAP_SELECT:
            this.centerOnPoint(targetX, targetY);
            drawMap(g);
            drawMapSideBar(g);
            drawMapSelect(g);
            drawOverworldMap(g);
            drawMessages(g);
            break;

            case CHARACTER:
            this.centerOnPoint(quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
            drawMap(g);
            drawCharacter(g);
            drawMessages(g);
            break;

            case VERB_PICK:
            this.centerOnPoint(quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
            drawMap(g);
            drawMapSideBar(g);
            drawOverworldMap(g);
            drawMessages(g);
            drawPickVerb(g);
            break;

            case TRADE:
            this.centerOnPoint(quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
            drawMap(g);
            drawTrade(g);
            drawMessages(g);
            break;

        }
        
    }

    public void drawMap(Graphics g)
    {
        Region region = quinoa.getCurrentRegionHeader().getRegion();

        //Recalculate the lightmap, if applicable
        quinoa.getLightMap().calculate(region);

        //Draw the map, monsters, and items
        for(int x=0; x < MAP_WINDOW_TILE_SIZE_X; x++)
        {
            for(int y=0; y < MAP_WINDOW_TILE_SIZE_Y; y++)
            {
                dtm.drawTerrain(region.getTerrain(tileOffsetX + x, tileOffsetY + y), g, MAP_PIXEL_X_OFFSET + x*quinoa.getUI().getGraphicsManager().getTileSize(), MAP_PIXEL_Y_OFFSET + y*quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager());
            }
        }

        for(Item tempItem: region.getItems())
        {
            if(tempItem.getX() - tileOffsetX < MAP_WINDOW_TILE_SIZE_X
            && tempItem.getY() - tileOffsetY < MAP_WINDOW_TILE_SIZE_Y)
            {
                int itemX = tempItem.getX() - tileOffsetX;
                int itemY = tempItem.getY() - tileOffsetY;

                if(itemX >= 0 && itemX < MAP_WINDOW_TILE_SIZE_X
                && itemY >= 0 && itemY < MAP_WINDOW_TILE_SIZE_Y)
                {
                    int drawX = MAP_PIXEL_X_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * itemX);
                    int drawY = MAP_PIXEL_Y_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * itemY);
                    dtm.drawItem(tempItem, g, drawX, drawY, 1.0, quinoa.getUI().getGraphicsManager(), false);
                }
            }
        }

        for(Monster tempMon: region.getMonsters())
        {
            if(tempMon.getX() - tileOffsetX < MAP_WINDOW_TILE_SIZE_X && tempMon.getY() - tileOffsetY < MAP_WINDOW_TILE_SIZE_Y)
            {
                int monX = tempMon.getX() - tileOffsetX;
                int monY = tempMon.getY() - tileOffsetY;

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
        for(Building tempBuild : quinoa.getCurrentRegionHeader().getRegion().getBuildings())
        {
            int px = quinoa.getPlayer().getX();
            int py = quinoa.getPlayer().getY();
            if(px >= tempBuild.getX() && px <= tempBuild.getX() + tempBuild.getWidth() - 1
            && py >= tempBuild.getY() && py <= tempBuild.getY() + tempBuild.getHeight() - 1)
            {
                playerInside = tempBuild;
            }
        }

        for(Building tempBuild : quinoa.getCurrentRegionHeader().getRegion().getBuildings())
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
                                g.drawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.ROOF, tempBuild.getRoofType()), drawX, drawY, null);
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
                g.drawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.TRANSPARENT, index), drawX, drawY, null);
            }
        }

        //draw exits
        for(RegionExit tempExit: quinoa.getCurrentRegionHeader().getExits())
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
                        case UP_STAIR:
                        g.drawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.EXIT, 0), drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize(), null);
                        break;

                        case DOWN_STAIR:
                        g.drawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.EXIT, 1), drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize(), null);
                        break;

                        case CAVE:
                        g.drawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.EXIT, 2), drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize(), null);
                        break;

                        case NONE:
                        g.drawImage(quinoa.getUI().getGraphicsManager().getImage(GraphicsManager.SPARKLE, (int)(Math.random() * 3)), drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize(), null);
                        break;
                    }
                    if(quinoa.DEBUG_MODE)
                    {
                        dtm.drawString(tempExit.getDestinationRegionID(), 1, g, drawX, drawY - 8, quinoa.getUI().getGraphicsManager());
                    }
                }
            }
        }


        //outline map
        g.setColor(new Color(128,128,128));
        g.drawRect(MAP_PIXEL_X_OFFSET-1, MAP_PIXEL_Y_OFFSET-1, (MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) + 1, (MAP_WINDOW_TILE_SIZE_Y * quinoa.getUI().getGraphicsManager().getTileSize()) + 1);

        //Draw map region name and time
        dtm.drawString(quinoa.getCurrentRegionHeader().getName(), 2, g, MAP_PIXEL_X_OFFSET, MAP_PIXEL_Y_OFFSET - 16 , quinoa.getUI().getGraphicsManager());
        dtm.drawString(quinoa.getTime(), 2, g, MAP_PIXEL_X_OFFSET + TIME_DISPLAY_OFFSET, MAP_PIXEL_Y_OFFSET - 16 , quinoa.getUI().getGraphicsManager());
        
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
        g.setColor(Color.black);
        g.fillRect(QuinoaWindow.UI_PIXEL_WIDTH / 2 - 225, 30, 450, QuinoaWindow.UI_PIXEL_HEIGHT - 50);
        g.setColor(new Color(128,128,128));
        g.drawRect(QuinoaWindow.UI_PIXEL_WIDTH / 2 - 225, 30, 450, QuinoaWindow.UI_PIXEL_HEIGHT - 50);
        
        int XOffset = QuinoaWindow.UI_PIXEL_WIDTH / 2 - 225 + 10;
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
                OverworldCell cell = quinoa.getMap().getOverworldCells()[x][y];
                boolean drawRoads =false;
                switch(cell.cellType)
                {
                    case NULL:
                    g.setColor(new Color(0,15,0));
                    g.fillRect(sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize, gridSize, gridSize);
                    break;

                    case FOREST:
                    g.setColor(Color.green);
                    g.fillRect(sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize, gridSize, gridSize);
                    drawRoads = true;
                    break;

                    case TOWN:
                    g.setColor(Color.MAGENTA);
                    g.fillRect(sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize, gridSize, gridSize);
                    drawRoads = true;
                    break;

                    case MAIN_TOWN:
                    g.setColor(Color.orange);
                    g.fillRect(sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize, gridSize, gridSize);
                    drawRoads = true;
                    break;
                }

                if(drawRoads)
                {
                    g.setColor(Color.yellow);
                    if(cell.nExit)
                    {
                        g.fillRect(sideBarXOffset + x * gridSize + (gridSize / 2) - 1, sideBarYOffset + y * gridSize, 3, gridSize / 2);
                    }
                    if(cell.sExit)
                    {
                        g.fillRect(sideBarXOffset + x * gridSize + (gridSize / 2) - 1, sideBarYOffset + y * gridSize + (gridSize / 2), 3, gridSize / 2);
                    }
                    if(cell.eExit)
                    {
                        g.fillRect(sideBarXOffset + x * gridSize + (gridSize / 2), sideBarYOffset + y * gridSize + (gridSize / 2), gridSize / 2, 3);
                    }
                    if(cell.wExit)
                    {
                        g.fillRect(sideBarXOffset + x * gridSize, sideBarYOffset + y * gridSize + (gridSize / 2), gridSize / 2, 3);
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
            ItemSlot tempSlot = MonsterInventory.ItemSlot.valueOf("BELT_" + (i +1));
            int ts = quinoa.getUI().getGraphicsManager().getTileSize();
            g.setColor(new Color(128,128,128));
            g.drawRect(itemOffsetX + sideBarXOffset + (int)(i * ts * 2.222), itemOffsetY + sideBarYOffset, ts * 2, ts * 2);
            dtm.drawString((i+1) + "", 1.0, g, itemOffsetX + sideBarXOffset + (int)(i * ts * 2.222) + ts, itemOffsetY + sideBarYOffset + (ts * 2) + 5, quinoa.getUI().getGraphicsManager());
            Item tempItem = quinoa.getPlayer().getInventory().getItem(tempSlot);
            if(tempItem != null)
            {
                dtm.drawItem(tempItem, g, itemOffsetX + sideBarXOffset + (int)(i * ts * 2.222) + (ts /2), itemOffsetY + sideBarYOffset + (ts /2), 1.0, quinoa.getUI().getGraphicsManager());
            }
        }
        
        //Draw stats
        dtm.drawString("Life: " + quinoa.getPlayer().getStats().getDisplayHealth(), 2, g, sideBarXOffset, itemOffsetY + sideBarYOffset + (20 * 0), quinoa.getUI().getGraphicsManager());
        dtm.drawString(quinoa.getPlayer().getStats().getDisplayHunger(), 2, g, sideBarXOffset, itemOffsetY + sideBarYOffset + (20 * 1) , quinoa.getUI().getGraphicsManager());

        //help message
        dtm.drawString("Press " + AdventureScreen.HELP_KEY + " for help", 2, g, sideBarXOffset + 80, sideBarYOffset - 16, quinoa.getUI().getGraphicsManager());
    }

    public void drawInventory(Graphics g)
    {
        int inventoryXOffset = MAP_PIXEL_X_OFFSET + (MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) + 10;
        int inventoryYOffset = MAP_PIXEL_Y_OFFSET + 48;
        g.setColor(new Color(20,20,20));
        int invX = inventoryXOffset;
        int invY = inventoryYOffset;
        for(int x=0; x < MonsterInventory.MAX_WIDTH; x++)
        {
            for(int y=0; y < MonsterInventory.MAX_HEIGHT; y++)
            {
               int ts = quinoa.getUI().getGraphicsManager().getTileSize();
               g.drawRect(invX + (x * ts * 2), invY + (y * ts * 2), ts * 2, ts * 2);
            }
        }
        g.setColor(new Color(128,128,128));
        invX = inventoryXOffset;
        invY = inventoryYOffset;
        for(int x=0; x < quinoa.getPlayer().getInventory().getWidth(); x++)
        {
            for(int y=0; y < quinoa.getPlayer().getInventory().getHeight(); y++)
            {
               int ts = quinoa.getUI().getGraphicsManager().getTileSize();
               g.drawRect(invX + (x * ts * 2), invY + (y * ts * 2), ts * 2, ts * 2);
            }
        }
        for(Item tempItem : quinoa.getPlayer().getInventory().getItems())
        {
            int ts = quinoa.getUI().getGraphicsManager().getTileSize();
            dtm.drawItem(tempItem, g, invX + (tempItem.getX() * ts * 2) + (ts/2), invY + (tempItem.getY() * ts * 2) + (ts/2), 1.0, quinoa.getUI().getGraphicsManager());
        }

        //draw item slots
        int slotYOffset = MAP_PIXEL_Y_OFFSET;
        for(int i=0; i < MonsterInventory.ItemSlot.values().length; i++)
        {
            ItemSlot tempSlot = MonsterInventory.ItemSlot.values()[i];
            int ts = quinoa.getUI().getGraphicsManager().getTileSize();
            g.setColor(new Color(128,128,128));
            g.drawRect(inventoryXOffset + (int)(i * ts * 2.222), slotYOffset, ts * 2, ts * 2);
            Item tempItem = quinoa.getPlayer().getInventory().getItem(tempSlot);
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
        g.setColor(Color.black);
        g.drawRect(drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize() * 2, quinoa.getUI().getGraphicsManager().getTileSize() * 2);
        g.setColor(new Color(255,255,255));
        g.drawRect(drawX-1, drawY-1, quinoa.getUI().getGraphicsManager().getTileSize()*2+2, quinoa.getUI().getGraphicsManager().getTileSize()*2+2);
        g.drawRect(drawX+1, drawY+1, quinoa.getUI().getGraphicsManager().getTileSize()*2-2, quinoa.getUI().getGraphicsManager().getTileSize()*2-2);

        //draw item information window
        g.setColor(new Color(128,128,128));
        int itemInfoX = inventoryXOffset;
        int itemInfoY = inventoryYOffset + MonsterInventory.MAX_HEIGHT * (quinoa.getUI().getGraphicsManager().getTileSize()*2) + 15;
        g.drawRect(itemInfoX, itemInfoY, MonsterInventory.MAX_WIDTH * (quinoa.getUI().getGraphicsManager().getTileSize()*2), ATTRIBUTE_WINDOW_SIZE);

        Item selectedItem = null;
        int itemHeaderOffsetY=0;
        if(inventoryTargetY == -1)
        {
            selectedItem = quinoa.getPlayer().getInventory().getItem(MonsterInventory.ItemSlot.values()[inventoryTargetX]);
            dtm.drawString(MonsterInventory.ItemSlot.values()[inventoryTargetX].name(), 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7, quinoa.getUI().getGraphicsManager());
            itemHeaderOffsetY = 16;
        }
        else
        {
            selectedItem = quinoa.getPlayer().getInventory().getItem(inventoryTargetX, inventoryTargetY);
        }

        if(selectedItem != null)
        {
            dtm.drawString(selectedItem.getItemClass().name(), 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Type:" + selectedItem.getItemCategory().name(), 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7 + (20 *1), quinoa.getUI().getGraphicsManager());
            //draw all attributes
            int lineCounter=2;
            for(ItemAttribute attribute : selectedItem.getAttributes())
            {
                String description = (attribute.getType().name().replace("_", " ")) + " " + attribute.getParameter();
                dtm.drawString(description, 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7 + (20 *lineCounter), quinoa.getUI().getGraphicsManager());
                lineCounter++;
            }
        }

        String formattedMoney = String.format("%.2f", quinoa.getPlayer().getInventory().getWealth());
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
        g.setColor(Color.black);
        g.drawRect(drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize() * 2, quinoa.getUI().getGraphicsManager().getTileSize() * 2);
        g.setColor(new Color(255,255,255));
        g.drawRect(drawX-1, drawY-1, quinoa.getUI().getGraphicsManager().getTileSize()*2+2, quinoa.getUI().getGraphicsManager().getTileSize()*2+2);
        g.drawRect(drawX+1, drawY+1, quinoa.getUI().getGraphicsManager().getTileSize()*2-2, quinoa.getUI().getGraphicsManager().getTileSize()*2-2);

        //draw item info window
        g.setColor(new Color(128,128,128));
        int itemInfoX = tradeXOffset;
        int itemInfoY = monYOffset + 35 + (MonsterInventory.MAX_HEIGHT * (quinoa.getUI().getGraphicsManager().getTileSize() * 2));
        g.drawRect(itemInfoX, itemInfoY, MonsterInventory.MAX_WIDTH * (quinoa.getUI().getGraphicsManager().getTileSize()*2), ATTRIBUTE_WINDOW_SIZE);

        Item selectedItem = null;
        int itemHeaderOffsetY=0;
        selectedItem = this.getTradeTarget();

        if(selectedItem != null)
        {
            dtm.drawString(selectedItem.getItemClass().name(), 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7, quinoa.getUI().getGraphicsManager());
            dtm.drawString("Type:" + selectedItem.getItemCategory().name(), 2.0, g, itemInfoX + 5, itemHeaderOffsetY + itemInfoY + 7 + (20 *1), quinoa.getUI().getGraphicsManager());
            //draw all attributes
            int lineCounter=2;
            for(ItemAttribute attribute : selectedItem.getAttributes())
            {
                String description = (attribute.getType().name().replace("_", " ")) + " " + attribute.getParameter();
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
            
            message = message + String.format("%.2f", selectedItem.getWorth());

            if(selectedItem.getStackSize() > 1)
            {
                message = message + " each";
            }
                    
            dtm.drawString(message, 2.0, g, itemInfoX, itemInfoY - 22, quinoa.getUI().getGraphicsManager());
        }
    }

    public void drawDialog(Graphics g)
    {
        int dx = (QuinoaWindow.UI_PIXEL_WIDTH / 2) - ((MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize()) / 2);
        int dy = (QuinoaWindow.UI_PIXEL_HEIGHT / 2) - (((quinoa.getMessageManager().getDialogSize() + 4) * quinoa.getUI().getGraphicsManager().getTileSize() + 5) / 2) - 20;
        g.setColor(Color.black);
        g.fillRect(dx + MAP_PIXEL_X_OFFSET, dy + MAP_PIXEL_Y_OFFSET, MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize(), (quinoa.getMessageManager().getDialogSize() + 2) * quinoa.getUI().getGraphicsManager().getTileSize() + 7);
        g.setColor(new Color(128,128,128));
        g.drawRect(dx + MAP_PIXEL_X_OFFSET-1, dy + MAP_PIXEL_Y_OFFSET-1, MAP_WINDOW_TILE_SIZE_X * quinoa.getUI().getGraphicsManager().getTileSize() + 2, (quinoa.getMessageManager().getDialogSize() + 2) * quinoa.getUI().getGraphicsManager().getTileSize() + 7);
        for(int i=0; i < quinoa.getMessageManager().getDialogSize(); i++)
        {
            dtm.drawString(quinoa.getMessageManager().getMessage(quinoa.getMessageManager().getDialogSize() - 1 - i), 2, g, dx + 2 + MAP_PIXEL_X_OFFSET, dy + 2 + MAP_PIXEL_Y_OFFSET + i * 16, quinoa.getUI().getGraphicsManager());
        }
        dtm.drawString("Press " + charToStr(EXIT_KEY) + " to continue.", 2, g, dx + 2 + MAP_PIXEL_X_OFFSET, dy + 2 + MAP_PIXEL_Y_OFFSET + (quinoa.getMessageManager().getDialogSize() + 1) * 16, quinoa.getUI().getGraphicsManager());
    }

    public void drawPickVerb(Graphics g)
    {
        ArrayList<ItemVerb> itemVerbs = ItemManager.getVerbs(verbItem);
        int vWidth = 200;
        int vHeight = ((itemVerbs.size()) * 20) + 20;
        int dx = (QuinoaWindow.UI_PIXEL_WIDTH / 2) - (vWidth / 2);
        int dy = (QuinoaWindow.UI_PIXEL_HEIGHT / 2) - (vHeight / 2) -20;

        g.setColor(Color.black);
        g.fillRect(dx, dy, vWidth, vHeight);
        g.setColor(new Color(128,128,128));
        g.drawRect(dx, dy, vWidth, vHeight);

        int ivCounter=0;
        for(ItemVerb iv : itemVerbs)
        {
            if(ivCounter == verbIndex)
            {
                g.setColor(Color.white);
            }
            else
            {
                g.setColor(new Color(128,128,128));
            }
            g.fillRect(dx + 10, dy + (ivCounter * 20) + 10, 10, 10);
            dtm.drawString(iv.name(), 2.0, g, dx + 25, dy + 10 + (ivCounter * 20), quinoa.getUI().getGraphicsManager());
            ivCounter++;
        }


    }

    public void drawMapSelect(Graphics g)
    {
        int drawX = MAP_PIXEL_X_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * (targetX - tileOffsetX));
            int drawY = MAP_PIXEL_Y_OFFSET + (quinoa.getUI().getGraphicsManager().getTileSize() * (targetY - tileOffsetY));
            g.setColor(Color.black);
            g.drawRect(drawX, drawY, quinoa.getUI().getGraphicsManager().getTileSize(), quinoa.getUI().getGraphicsManager().getTileSize());
            g.setColor(new Color(255,255,255));
            g.drawRect(drawX-1, drawY-1, quinoa.getUI().getGraphicsManager().getTileSize()+2, quinoa.getUI().getGraphicsManager().getTileSize()+2);
            g.drawRect(drawX+1, drawY+1, quinoa.getUI().getGraphicsManager().getTileSize()-2, quinoa.getUI().getGraphicsManager().getTileSize()-2);
    }

    public void drawInventoryGrid(Graphics g, Monster monster, int tradeXOffset, int tradeYOffset)
    {
        g.setColor(new Color(20,20,20));
        for(int x=0; x < MonsterInventory.MAX_WIDTH; x++)
        {
            for(int y=0; y < MonsterInventory.MAX_HEIGHT; y++)
            {
               int ts = quinoa.getUI().getGraphicsManager().getTileSize();
               g.drawRect(tradeXOffset + (x * ts * 2), tradeYOffset + (y * ts * 2), ts * 2, ts * 2);
            }
        }
        g.setColor(new Color(128,128,128));
        for(int x=0; x < monster.getInventory().getWidth(); x++)
        {
            for(int y=0; y < monster.getInventory().getHeight(); y++)
            {
               int ts = quinoa.getUI().getGraphicsManager().getTileSize();
               g.drawRect(tradeXOffset + (x * ts * 2), tradeYOffset + (y * ts * 2), ts * 2, ts * 2);
            }
        }
        for(Item tempItem : monster.getInventory().getItems())
        {
            int ts = quinoa.getUI().getGraphicsManager().getTileSize();
            dtm.drawItem(tempItem, g, tradeXOffset + (tempItem.getX() * ts * 2) + (ts/2), tradeYOffset + (tempItem.getY() * ts * 2) + (ts/2), 1.0, quinoa.getUI().getGraphicsManager());
        }

        String name = monster.getMonsterCode().name();
        if(monster.getID().equals(MonsterActionManager.PLAYER_ID))
        {
            name = "YOU";
        }
        else if(monster.getRole() != MonsterRole.NULL)
        {
            name = monster.getRole().name();
        }
        dtm.drawString(name, 2.0, g, tradeXOffset, tradeYOffset - 16, quinoa.getUI().getGraphicsManager());
        String formattedMoney = String.format("%.2f", monster.getInventory().getWealth());
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

        dtm.drawString("Life: " + quinoa.getPlayer().getStats().getDisplayHealth(), 2.0, g, charXOffset, charYOffset + (20 * 0), quinoa.getUI().getGraphicsManager());
        
        dtm.drawString("Health: " + quinoa.getPlayer().getStats().getHealth(), 2.0, g, charXOffset, charYOffset + (20 * 2), quinoa.getUI().getGraphicsManager());
        dtm.drawString("Might: " + quinoa.getPlayer().getStats().getMight(), 2.0, g, charXOffset, charYOffset + (20 * 3), quinoa.getUI().getGraphicsManager());
        dtm.drawString("Endurance: " + quinoa.getPlayer().getStats().getEndurance(), 2.0, g, charXOffset, charYOffset + (20 * 4), quinoa.getUI().getGraphicsManager());
        dtm.drawString("Agility: " + quinoa.getPlayer().getStats().getAgility(), 2.0, g, charXOffset, charYOffset + (20 * 5), quinoa.getUI().getGraphicsManager());

        if(quinoa.getPlayer().getStats().getAvailableStatPoints() > 0)
        {
            dtm.drawString(quinoa.getPlayer().getStats().getAvailableStatPoints()+"", 2.0, g, charXOffset + 300, charYOffset + (20 * 2) - 22, quinoa.getUI().getGraphicsManager());
            for(int i=0; i < 4; i++)
            {
                if(characterIndex == i)
                {
                    g.setColor(new Color(255,255,255));
                    g.drawRect(charXOffset + 300, charYOffset + (20 * (2 + i)) - 2, 12,12);
                    g.drawRect(charXOffset + 300 - 2, charYOffset + (20 * (2 + i)) - 4, 16,16);
                    g.drawLine(charXOffset + 300 + 6, charYOffset + (20 * (2 + i)), charXOffset + 300 + 6, charYOffset + (20 * (2 + i)) + 8);
                    g.drawLine(charXOffset + 300 + 2, charYOffset + (20 * (2 + i)) + 4, charXOffset + 300 + 10, charYOffset + (20 * (2 + i)) + 4);
                }
                else
                {
                    g.setColor(new Color(128,128,128));
                    g.drawRect(charXOffset + 300, charYOffset + (20 * (2 + i)) - 2, 12,12);
                    g.drawLine(charXOffset + 300 + 6, charYOffset + (20 * (2 + i)), charXOffset + 300 + 6, charYOffset + (20 * (2 + i)) + 8);
                    g.drawLine(charXOffset + 300 + 2, charYOffset + (20 * (2 + i)) + 4, charXOffset + 300 + 10, charYOffset + (20 * (2 + i)) + 4);
                }
                
            }
        }

        dtm.drawString("Defense: " + MonsterActionManager.getDefenseRating(quinoa.getPlayer()), 2.0, g, charXOffset, charYOffset + (20 * 7), quinoa.getUI().getGraphicsManager());
        dtm.drawString("Attack: " + MonsterActionManager.getAttackRating(quinoa.getPlayer()), 2.0, g, charXOffset, charYOffset + (20 * 8), quinoa.getUI().getGraphicsManager());
        dtm.drawString("Speed: " + MonsterActionManager.getSpeed(quinoa.getPlayer()) + " ticks per turn", 2.0, g, charXOffset, charYOffset + (20 * 9), quinoa.getUI().getGraphicsManager());

        dtm.drawString("Level: " + quinoa.getPlayer().getStats().getLevel(), 2.0, g, charXOffset, charYOffset + (20 * 11), quinoa.getUI().getGraphicsManager());
        dtm.drawString("XP: " + quinoa.getPlayer().getStats().getExperience(), 2.0, g, charXOffset, charYOffset + (20 * 12), quinoa.getUI().getGraphicsManager());
        dtm.drawString("Next: " + MonsterActionManager.getNextXPLevel(quinoa.getPlayer()), 2.0, g, charXOffset, charYOffset + (20 * 13), quinoa.getUI().getGraphicsManager());

        dtm.drawString("Hunger: " + quinoa.getPlayer().getStats().getDisplayHunger(), 2.0, g, charXOffset, charYOffset + (20 * 15), quinoa.getUI().getGraphicsManager());

        if(quinoa.getPlayer().getStats().getAvailableStatPoints() > 0)
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
            for(Monster tempMon : quinoa.getCurrentRegionHeader().getRegion().getMonsters())
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
            if(!lastHungerStatus.equals(quinoa.getPlayer().getStats().getDisplayHunger()))
            {
                quinoa.getMessageManager().addMessage("You are " + quinoa.getPlayer().getStats().getDisplayHunger() + ".");
                this.lastHungerStatus = quinoa.getPlayer().getStats().getDisplayHunger();
            }
        }
    }

    public boolean readyForInput()
    {
        return(quinoa.getPlayer().readyForCommand());
    }

    public void displayDialog()
    {
        this.setMode(AdventureScreenMode.DIALOG);
    }

    public void displayTrade(Monster monster)
    {
        this.setTradeMonster(monster);
        this.setMode(AdventureScreenMode.TRADE);
    }

    public void doUseItem(Item item)
    {
        if(item != null)
        {
            ArrayList<ItemVerb> itemVerbs = ItemManager.getVerbs(item);
            if(itemVerbs.isEmpty())
            {
                //do nothing
            }
            else if(itemVerbs.size() == 1)
            {
                verbIndex = 0;
                verb = itemVerbs.get(verbIndex);
                verbItem = item;
                targetMaxDistance = ItemManager.verbDistance(verb);
                if(targetMaxDistance > 0)
                {
                    this.mapSelectAction = MapSelectAction.VERB;
                    this.setMapSelectMaxDistance(targetMaxDistance);
                    this.setMode(AdventureScreenMode.MAP_SELECT);
                    quinoa.getUI().refresh();
                }
                else
                {
                    MonsterActionManager.setItemVerbCommand(quinoa.getPlayer(), verbItem, verb, 0, 0);
                    this.setMode(AdventureScreenMode.MAP);
                    quinoa.getUI().refresh();
                    quinoa.cycle();
                }
            }
            else if(itemVerbs.size() > 1)
            {
                verbItem = item;
                this.setMode(AdventureScreenMode.VERB_PICK);
                quinoa.getUI().refresh();
            }
        }
    }

    public void doMovementInDirection(Monster.Direction direction)
    {
        int newX = quinoa.getPlayer().getX();
        int newY = quinoa.getPlayer().getY();

        switch(direction)
        {
            case N: newY--; break;
            case NE: newY--; newX++; break;
            case NW: newY--; newX--; break;
            case S: newY++; break;
            case SE: newY++; newX++; break;
            case SW: newY++; newX--; break;
            case E: newX++; break;
            case W: newX--; break;
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
            MonsterActionManager.setAttackCommand(quinoa.getPlayer(), checkForMonster.getID());
        }
        quinoa.cycle();
    }

    public void moveMapTarget(int newX, int newY)
    {
        //Get distance from proposed target to player
        int distance = (int)(Math.sqrt((newX-quinoa.getPlayer().getX()) * (newX-quinoa.getPlayer().getX()) + (newY-quinoa.getPlayer().getY()) * (newY-quinoa.getPlayer().getY())));

        if(distance <= this.targetMaxDistance || this.targetMaxDistance == 0)
        {
            Region region = quinoa.getCurrentRegionHeader().getRegion();
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
            if(newX >= MonsterInventory.ItemSlot.values().length)
            {
                newX = MonsterInventory.ItemSlot.values().length-1;
            }

            if(newX >= 0 && newX < MonsterInventory.ItemSlot.values().length)
            {
                inventoryTargetX = newX;
                inventoryTargetY = newY;
            }
        }
        else
        {
            if(newY >= 0 && newY < quinoa.getPlayer().getInventory().getHeight()
            && newX >= 0 && newX < quinoa.getPlayer().getInventory().getWidth())
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
            if(newY >= quinoa.getPlayer().getInventory().getHeight())
            {
                if(tradeTargetX >= tradeMonster.getInventory().getWidth())
                {
                    tradeTargetX = tradeMonster.getInventory().getWidth()-1;
                }
                tradeTargetY = 0;
                tradePageIsPlayer = false;
            }
            else
            {
                if(newY >= 0 && newX >= 0 && newX < quinoa.getPlayer().getInventory().getWidth())
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
                if(tradeTargetX >= quinoa.getPlayer().getInventory().getWidth())
                {
                    tradeTargetX = quinoa.getPlayer().getInventory().getWidth()-1;
                }
                tradeTargetY = quinoa.getPlayer().getInventory().getHeight()-1;
                tradePageIsPlayer = true;
            }
            else
            {
                if(newY < tradeMonster.getInventory().getHeight() && newX >= 0 && newX < tradeMonster.getInventory().getWidth())
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
            return quinoa.getPlayer().getInventory().getItem(ItemSlot.values()[inventoryTargetX]);
        }
        else
        {
            return quinoa.getPlayer().getInventory().getItem(inventoryTargetX, inventoryTargetY);
        }

    }

    public Item getTradeTarget()
    {
        if(this.tradePageIsPlayer)
        {
            return quinoa.getPlayer().getInventory().getItem(tradeTargetX, tradeTargetY);
        }
        else
        {
            return tradeMonster.getInventory().getItem(tradeTargetX, tradeTargetY);
        }
    }

    public void processKey(char key, boolean shift, boolean alt)
    {
        switch(mode)
        {
            case MAP:
            processKeyMap(key,shift,alt);
            break;

            case HELP:
            processKeyHelp(key,shift,alt);
            break;

            case DIALOG:
            processKeyDialog(key,shift,alt);
            break;

            case MAP_SELECT:
            processKeyMapSelect(key,shift,alt);
            break;

            case INVENTORY:
            processKeyInventory(key,shift,alt);
            break;

            case CHARACTER:
            processKeyCharacter(key,shift,alt);
            break;

            case VERB_PICK:
            processKeyVerbPick(key,shift,alt);
            break;

            case TRADE:
            processKeyTrade(key,shift,alt);
            break;

        }
    }

    public void processKeyMap(char key, boolean shift, boolean alt)
    {
        Monster player = quinoa.getPlayer();

        //Check if another command can be entered
        if(!player.readyForCommand())
        {
            return;
        }

        if(key == AdventureScreen.UP_KEY)
        {
            doMovementInDirection(Monster.Direction.N);
        }
        else if(key == AdventureScreen.DOWN_KEY)
        {
            doMovementInDirection(Monster.Direction.S);
        }
        else if(key == AdventureScreen.LEFT_KEY)
        {
            doMovementInDirection(Monster.Direction.W);
        }
        else if(key == AdventureScreen.RIGHT_KEY)
        {
            doMovementInDirection(Monster.Direction.E);
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
            this.setMode(AdventureScreenMode.INVENTORY);
            quinoa.getUI().refresh();
        }
        else if(key == AdventureScreen.CHARACTER_KEY)
        {
            this.setMode(AdventureScreenMode.CHARACTER);
            quinoa.getUI().refresh();
        }
        else if(key == AdventureScreen.HELP_KEY)
        {
            this.setMode(AdventureScreenMode.HELP);
            quinoa.getUI().refresh();
        }
        else if(key == '-')
        {
            quinoa.getPlayer().getStats().setCurrentHealth(quinoa.getPlayer().getStats().getCurrentHealth() - 1);
            quinoa.cycle();
        }
        else if(key == AdventureScreen.MESSAGES_KEY)
        {
            quinoa.getMessageManager().dialogLastMessages();
            this.setMode(AdventureScreenMode.DIALOG);
            quinoa.getUI().refresh();
        }
        else if(key == AdventureScreen.BELT_1_KEY)
        {
            doUseItem(quinoa.getPlayer().getInventory().getItem(ItemSlot.BELT_1));
        }
        else if(key == AdventureScreen.BELT_2_KEY)
        {
            doUseItem(quinoa.getPlayer().getInventory().getItem(ItemSlot.BELT_2));
        }
        else if(key == AdventureScreen.BELT_3_KEY)
        {
            doUseItem(quinoa.getPlayer().getInventory().getItem(ItemSlot.BELT_3));
        }
        else if(key == AdventureScreen.BELT_4_KEY)
        {
            doUseItem(quinoa.getPlayer().getInventory().getItem(ItemSlot.BELT_4));
        }
        else if(key == AdventureScreen.ACTION_KEY)
        {
            verbItem = null;
            verbIndex = 0;
            this.setMode(AdventureScreenMode.VERB_PICK);
            quinoa.getUI().refresh();
        }
    }

    public void processKeyDialog(char key, boolean shift, boolean alt)
    {
        if(key == AdventureScreen.EXIT_KEY)
        {
            if(quinoa.playerIsDead())
            {
                quinoa.getUI().setInterfaceMode(QuinoaUIInterface.InterfaceMode.MENU);
                quinoa.cycle();
            }
            else
            {
                this.setMode(AdventureScreenMode.MAP);
                quinoa.cycle();
            }
        }
    }

    public void processKeyHelp(char key, boolean shift, boolean alt)
    {
        if(key == AdventureScreen.EXIT_KEY)
        {
            this.setMode(AdventureScreenMode.MAP);
        }
    }
    
    public void processKeyVerbPick(char key, boolean shift, boolean alt)
    {
        if(key == AdventureScreen.ENTER_KEY)
        {
            ArrayList<ItemVerb> itemVerbs = ItemManager.getVerbs(verbItem);
            verb = itemVerbs.get(verbIndex);
            this.targetMaxDistance = ItemManager.verbDistance(verb);
            if(targetMaxDistance > 0)
            {
                this.mapSelectAction = MapSelectAction.VERB;
                this.setMapSelectMaxDistance(targetMaxDistance);
                this.setMode(AdventureScreenMode.MAP_SELECT);
                quinoa.getUI().refresh();
            }
            else
            {
                MonsterActionManager.setItemVerbCommand(quinoa.getPlayer(), verbItem, verb, quinoa.getPlayer().getX(), quinoa.getPlayer().getY());
                this.setMode(AdventureScreenMode.MAP);
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
            ArrayList<ItemVerb> itemVerbs = ItemManager.getVerbs(verbItem);
            if(verbIndex < itemVerbs.size() - 1)
            {
                verbIndex++;
            }
            quinoa.getUI().refresh();
        }
    }

    public void processKeyMapSelect(char key, boolean shift, boolean alt)
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
                case VERB:

                //reset facing
                if(targetY > quinoa.getPlayer().getY())
                {
                    quinoa.getPlayer().setFacing(Monster.Direction.S);
                }
                else if(targetY < quinoa.getPlayer().getY())
                {
                    quinoa.getPlayer().setFacing(Monster.Direction.N);
                }
                else if(targetX > quinoa.getPlayer().getX())
                {
                    quinoa.getPlayer().setFacing(Monster.Direction.E);
                }
                else if(targetX < quinoa.getPlayer().getX())
                {
                    quinoa.getPlayer().setFacing(Monster.Direction.W);
                }

                MonsterActionManager.setItemVerbCommand(quinoa.getPlayer(), verbItem, verb, targetX, targetY);
                this.setMode(AdventureScreenMode.MAP);
                quinoa.cycle();
                break;
            }
        }
    }

    public void processKeyInventory(char key, boolean shift, boolean alt)
    {
        if(key == AdventureScreen.EXIT_KEY)
        {
            this.setMode(AdventureScreenMode.MAP);
            quinoa.getUI().refresh();
        }
        else if(key == AdventureScreen.CHARACTER_KEY)
        {
            this.setMode(AdventureScreenMode.CHARACTER);
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
                MonsterActionManager.setDropCommand(quinoa.getPlayer(), itemToDrop.getID());
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
                    MonsterActionManager.setEquipCommand(quinoa.getPlayer(), itemToEquip.getID());
                }
                else
                {
                    MonsterActionManager.setUnequipCommand(quinoa.getPlayer(), itemToEquip.getID());
                }
            }
            quinoa.cycle();
        }
    }

    public void processKeyTrade(char key, boolean shift, boolean alt)
    {
        if(key == AdventureScreen.EXIT_KEY)
        {
            this.setMode(AdventureScreenMode.MAP);
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

    public void processKeyCharacter(char key, boolean shift, boolean alt)
    {
        if(key == AdventureScreen.EXIT_KEY)
        {
            this.setMode(AdventureScreenMode.MAP);
            quinoa.getUI().refresh();
        }
        else if(key == AdventureScreen.INVENTORY_KEY)
        {
            this.setMode(AdventureScreenMode.INVENTORY);
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
            if(quinoa.getPlayer().getStats().getAvailableStatPoints() > 0)
            {
                switch(characterIndex)
                {
                    case 0: //Health
                    if(quinoa.getPlayer().getStats().getHealth() < MonsterStats.MAX_STAT)
                    {
                        quinoa.getPlayer().getStats().setAvailableStatPoints(quinoa.getPlayer().getStats().getAvailableStatPoints() - 1);
                        quinoa.getPlayer().getStats().setHealth(quinoa.getPlayer().getStats().getHealth() + 1);
                        quinoa.getPlayer().getStats().setCurrentHealth(quinoa.getPlayer().getStats().getMaxHP());
                    }
                    break;

                    case 1: //Might
                    if(quinoa.getPlayer().getStats().getMight() < MonsterStats.MAX_STAT)
                    {
                        quinoa.getPlayer().getStats().setAvailableStatPoints(quinoa.getPlayer().getStats().getAvailableStatPoints() - 1);
                        quinoa.getPlayer().getStats().setMight(quinoa.getPlayer().getStats().getMight() + 1);
                    }
                    break;

                    case 2: //Endurance
                    if(quinoa.getPlayer().getStats().getEndurance() < MonsterStats.MAX_STAT)
                    {
                        quinoa.getPlayer().getStats().setAvailableStatPoints(quinoa.getPlayer().getStats().getAvailableStatPoints() - 1);
                        quinoa.getPlayer().getStats().setEndurance(quinoa.getPlayer().getStats().getEndurance() + 1);
                    }
                    break;

                    case 3: //Agility
                    if(quinoa.getPlayer().getStats().getAgility() < MonsterStats.MAX_STAT)
                    {
                        quinoa.getPlayer().getStats().setAvailableStatPoints(quinoa.getPlayer().getStats().getAvailableStatPoints() - 1);
                        quinoa.getPlayer().getStats().setAgility(quinoa.getPlayer().getStats().getAgility() + 1);
                    }
                    break;
                }

                quinoa.getUI().refresh();
            }
        }
    }

}
