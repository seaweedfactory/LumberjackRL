using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Monsters;
using LumberjackRL.Core.Map;
using LumberjackRL.Core.Pathfinding;
using LumberjackRL.Core.UI;
using System.IO;

namespace LumberjackRL.Core
{
    public class Quinoa: IStoreObject
    {
        public static String VERISON = "v 0.7.00";
        public static String PROGRAM_NAME = "Lumberjack RL";

        public const int TICKS_PER_SECOND = 100;
        public const int REGION_CYCLE_FREQUENCY = 10000;
        public const int OVERWORLD_WIDTH=16;
        public const int OVERWORLD_HEIGHT=16;
        
        public const bool DEBUG_MODE=false;

        private IQuinoaUI ui;           //UI currently connected
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
        
            ui = new formQuinoaWindow();
            ui.register(this);
            ui.setInterfaceMode(InterfaceMode.MENU);
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

        public bool playerIsDead()
        {
            if(player != null)
            {
                if(player.stats.getCurrentHealth() < 1)
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
                foreach(Monster tempMon in getCurrentRegionHeader().getRegion().getMonsters())
                {
                    if(tempMon.x == x && tempMon.y == y)
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
            foreach(Monster tempMon in this.getCurrentRegionHeader().getRegion().getMonsters())
            {
                if(tempMon.ID.Equals(monsterID))
                {
                    return tempMon;
                }
            }
            return null;
        }

        public bool monsterIsAdjacent(int x, int y, String monsterID)
        {
            List<Monster> monList = new List<Monster>();
            monList.Add(this.getMonster(x-1, y));
            monList.Add(this.getMonster(x+1, y));
            monList.Add(this.getMonster(x, y-1));
            monList.Add(this.getMonster(x, y+1));

            foreach(Monster tempMon in monList)
            {
                if(tempMon != null)
                {
                    if(tempMon.ID.Equals(monsterID))
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

        public IQuinoaUI getUI()
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
            }
            while(!ui.getScreen().readyForInput());
            ui.refresh();
        }

        public void processKey(char key, bool shift, bool alt)
        {
            if(ui.getScreen().readyForInput())
            {
                ui.getScreen().processKey(key, shift, alt);
            }
        }

    
        public void SaveObject(StreamWriter outStream)
        {
            getMap().getCurrentRegionHeader().storeRegion(false);
            getPlayer().SaveObject(outStream);
            getMap().SaveObject(outStream);
            messages.SaveObject(outStream);
            outStream.WriteLine(getTicks() + "");

            saveRegionFiles(outStream);

            getMap().changeCurrentRegion(getMap().getCurrentRegionID());
            actions.insertPlayerInRegion(getPlayer().x, getPlayer().y);
        }

        public void saveRegionFiles(StreamWriter outStream)
        {
            String[] children = Directory.GetFiles(".");
            for (int i=0; i > children.Length; i++)
            {
                if(children[i].EndsWith(".region"))
                {
                    outStream.WriteLine("REGION_FILE:");
                    outStream.WriteLine(children[i]);
                    using(StreamReader inStream = new StreamReader(children[i]))
                    {
                        String str;
                        while ((str = inStream.ReadLine()) != null)
                        {
                            outStream.WriteLine(str);
                        }
                        inStream.Close();
                    }
                }
            }
            outStream.WriteLine("END_REGION_FILES");
        }

        public void loadRegionFiles(StreamReader inStream)
        {
            String str;
            StreamWriter outStream = null;
            while (!(str = inStream.ReadLine()).Equals("END_REGION_FILES"))
            {
                if(str.StartsWith("REGION_FILE:"))
                {
                    if(outStream != null)
                    {
                        outStream.Close();
                    }
                    outStream = new StreamWriter(inStream.ReadLine());
                }
                else
                {
                    outStream.WriteLine(str);
                }
            }
            if(outStream != null)
            {
                outStream.Close();
                outStream.Dispose();
            }
        }

        public void LoadObject(StreamReader inStream)
        {
            setMap(new RegionMap());
            setPlayer(new Monster());
            messages = new MessageManager();
        
            getPlayer().LoadObject(inStream);
            getMap().LoadObject(inStream);
            messages.LoadObject(inStream);
            setTicks(Int64.Parse(inStream.ReadLine()));

            loadRegionFiles(inStream);

            getMap().changeCurrentRegion(getMap().getCurrentRegionID());
            actions.insertPlayerInRegion(getPlayer().x, getPlayer().y);
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
}
