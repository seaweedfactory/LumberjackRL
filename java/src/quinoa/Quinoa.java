package quinoa;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.util.ArrayList;
import pathfinding.ClosestSquaredHeuristic;
import pathfinding.PathFinder;
import quinoa.monsters.Monster;
import quinoa.region.LightMap;
import quinoa.region.RegionHeader;
import quinoa.region.RegionMap;
import quinoa.ui.QuinoaUIInterface;
import quinoa.ui.QuinoaWindow;

public class Quinoa implements Storable
{
    public static final String VERISON = "v 0.7.00";
    public static final String PROGRAM_NAME = "Lumberjack RL";

    public static final int TICKS_PER_SECOND = 100;
    public static final int REGION_CYCLE_FREQUENCY = 10000;
    public static final int OVERWORLD_WIDTH=16;
    public static final int OVERWORLD_HEIGHT=16;
        
    public static final boolean DEBUG_MODE=false;

    private QuinoaUIInterface ui;   //UI currently connected
    private QuinoaActions actions;  //High-level actions
    private RegionMap map;          //Main map structure
    private Monster player;         //Player monster
    private MessageManager messages;//Manages messages
    private LightMap lightMap;      //Manages lighting
    private PathFinder pathFinder;  //Manages pathfinding
    private long ticks;             //Ticks determine what time it is
    
    public Quinoa()
    {
        ticks = 0;
        map = new RegionMap();
        messages = new MessageManager();
        actions = new QuinoaActions(this);
        lightMap = new LightMap(this);
        pathFinder = new PathFinder(this, new ClosestSquaredHeuristic());
        
        ui = new QuinoaWindow();
        ui.register(this);
        ui.setInterfaceMode(QuinoaUIInterface.InterfaceMode.MENU);
        ui.display();
        ui.refresh();
    }

    public void reset()
    {
        ticks = 0;
        map = new RegionMap();
        messages = new MessageManager();
        actions = new QuinoaActions(this);
        lightMap = new LightMap(this);
        pathFinder = new PathFinder(this, new ClosestSquaredHeuristic());
    }

    public static void main(String[] args)
    {
        Quinoa program = new Quinoa();
        program.cycle();
    }

    public boolean playerIsDead()
    {
        if(player != null)
        {
            if(player.getStats().getCurrentHealth() < 1)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return true;
        }
    }

    public Monster getMonster(int x, int y)
    {
        if(x >= 0 && x < getCurrentRegionHeader().getRegion().getWidth() -1
        && y >= 0 && y < getCurrentRegionHeader().getRegion().getWidth() -1)
        {
            Monster returnMonster = null;
            for(Monster tempMon : getCurrentRegionHeader().getRegion().getMonsters())
            {
                if(tempMon.getX() == x && tempMon.getY() == y)
                {
                    returnMonster = tempMon;
                }
            }
            return returnMonster;
        }
        else
        {
            return null;
        }
    }

    public Monster getMonsterByID(String monsterID)
    {
        for(Monster tempMon : this.getCurrentRegionHeader().getRegion().getMonsters())
        {
            if(tempMon.getID().equals(monsterID))
            {
                return tempMon;
            }
        }
        return null;
    }

    public boolean monsterIsAdjacent(int x, int y, String monsterID)
    {
        ArrayList<Monster> monList = new ArrayList<Monster>();
        monList.add(this.getMonster(x-1, y));
        monList.add(this.getMonster(x+1, y));
        monList.add(this.getMonster(x, y-1));
        monList.add(this.getMonster(x, y+1));
        //diagonals are turned off
        //monList.add(this.getMonster(x-1, y-1));
        //monList.add(this.getMonster(x-1, y+1));
        //monList.add(this.getMonster(x+1, y+1));
        //monList.add(this.getMonster(x+1, y-1));

        for(Monster tempMon : monList)
        {
            if(tempMon != null)
            {
                if(tempMon.getID().equals(monsterID))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public int getHour()
    {
        int ticksPerMin = Quinoa.TICKS_PER_SECOND * 60;
        int ticksPerHour = ticksPerMin * 60;

        int hours = (int)(getTicks() / ticksPerHour);

        return hours;
    }

    public int getMinute()
    {
        int ticksPerMin = Quinoa.TICKS_PER_SECOND * 60;
        int ticksPerHour = ticksPerMin * 60;

        int hours = (int)(getTicks() / ticksPerHour);
        int mins = (int)((getTicks() - (hours * ticksPerHour)) / ticksPerMin);

        return mins;
    }

    public String getTime()
    {
        int ticksPerMin = Quinoa.TICKS_PER_SECOND * 60;
        int ticksPerHour = ticksPerMin * 60;

        int hours = (int)(getTicks() / ticksPerHour);
        int mins = (int)((getTicks() - (hours * ticksPerHour)) / ticksPerMin);
        int secs = (int)(((getTicks() - (hours * ticksPerHour)) - (mins * ticksPerMin)) / Quinoa.TICKS_PER_SECOND);

        hours = hours + 1;

        String hoursPad = "";
        String minsPad = "";
        String secsPad = "";

        if(hours < 10)
        {
            hoursPad = "0";
        }

        if(mins < 10)
        {
            minsPad = "0";
        }

        if(secs < 10)
        {
            secsPad = "0";
        }

        return hoursPad + hours + "h " + minsPad + mins + "m " + secsPad + secs + "s";
    }

    public QuinoaUIInterface getUI()
    {
        return ui;
    }

    public RegionHeader getCurrentRegionHeader()
    {
        return getMap().getCurrentRegionHeader();
    }

    public MessageManager getMessageManager()
    {
        return messages;
    }

    public void cycle()
    {
        do
        {
            ui.getScreen().cycle();
            ui.refresh();
        }
        while(!ui.getScreen().readyForInput());
        
    }

    public void processKey(char key, boolean shift, boolean alt)
    {
        if(ui.getScreen().readyForInput())
        {
            ui.getScreen().processKey(key, shift, alt);
        }
    }

    
    public void save(BufferedWriter out) throws Exception
    {
        getMap().getCurrentRegionHeader().storeRegion(false);
        getPlayer().save(out);
        getMap().save(out);
        messages.save(out);
        out.write(getTicks() + ""); out.newLine();

        saveRegionFiles(out);

        getMap().changeCurrentRegion(getMap().getCurrentRegionID());
        actions.insertPlayerInRegion(getPlayer().getX(), getPlayer().getY());
    }

    public void saveRegionFiles(BufferedWriter out) throws Exception
    {
        File dir = new File(".");

        String[] children = dir.list();
        for (int i=0; i<children.length; i++)
        {
            if(children[i].endsWith(".region"))
            {
                out.write("REGION_FILE:"); out.newLine();
                out.write(children[i]); out.newLine();
                BufferedReader in = new BufferedReader(new FileReader(children[i]));
                String str;
                while ((str = in.readLine()) != null)
                {
                    out.write(str); out.newLine();
                }
                in.close();
            }
        }
        out.write("END_REGION_FILES"); out.newLine();
    }

    public void loadRegionFiles(BufferedReader in) throws Exception
    {
        String str;
        BufferedWriter out = null;
        while (!(str = in.readLine()).equals("END_REGION_FILES"))
        {
            if(str.startsWith("REGION_FILE:"))
            {
                if(out != null)
                {
                    out.close();
                }
                out = new BufferedWriter(new FileWriter(in.readLine()));
            }
            else
            {
                out.write(str); out.newLine();
            }
        }
        if(out != null)
        {
            out.close();
        }
    }

    public void load(BufferedReader in) throws Exception
    {
        setMap(new RegionMap());
        setPlayer(new Monster());
        messages = new MessageManager();
        
        getPlayer().load(in);
        getMap().load(in);
        messages.load(in);
        setTicks(Long.parseLong(in.readLine()));

        loadRegionFiles(in);

        getMap().changeCurrentRegion(getMap().getCurrentRegionID());
        actions.insertPlayerInRegion(getPlayer().getX(), getPlayer().getY());
    }

    /**
     * @return the player
     */
    public Monster getPlayer() {
        return player;
    }

    /**
     * @param player the player to set
     */
    public void setPlayer(Monster player) {
        this.player = player;
    }

    /**
     * @return the map
     */
    public RegionMap getMap() {
        return map;
    }

    /**
     * @return the actions
     */
    public QuinoaActions getActions() {
        return actions;
    }

    /**
     * @param actions the actions to set
     */
    public void setActions(QuinoaActions actions) {
        this.actions = actions;
    }

    /**
     * @return the ticks
     */
    public long getTicks() {
        return ticks;
    }

    /**
     * @param map the map to set
     */
    public void setMap(RegionMap map) {
        this.map = map;
    }

    /**
     * @param ticks the ticks to set
     */
    public void setTicks(long ticks) {
        this.ticks = ticks;
    }

     /**
     * @return the lightMap
     */
    public LightMap getLightMap() {
        return lightMap;
    }

    /**
     * @return the pathFinder
     */
    public PathFinder getPathFinder() {
        return pathFinder;
    }

}
