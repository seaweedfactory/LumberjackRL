package quinoa.monsters;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.util.HashMap;
import java.util.UUID;
import quinoa.Copyable;
import quinoa.Quinoa;
import quinoa.Removable;
import quinoa.Storable;
import quinoa.monsters.MonsterActionManager.MonsterCode;
import quinoa.monsters.MonsterActionManager.MonsterCommandCode;
import quinoa.monsters.MonsterActionManager.MonsterRole;
import quinoa.region.TerrainManager;

public class Monster implements Removable, Storable, Copyable
{

    public enum Direction {N, NE, E, SE, S, SW, W, NW};
    public enum MonsterState {DESTROYED, ACTIVE};

    private int x, y;                   //Position within the region
    private Direction facing;           //What direction is the monster facing
    private MonsterState state;         //What is the overall state of the monster
    private MonsterCode monsterCode;    //What type of monster is it?
    private MonsterRole role;           //Does the monster have a job?
    private String ID;                  //UUID in string acts as unique ID, filled at creation time
    private MonsterCommand command;     //The current command waiting to execute
    private HashMap<String, String> AIParameters;   //Holds variables used by monster AI
    private MonsterStats stats;         //Holds stat data
    private MonsterInventory inventory; //Holds items and equipment
    private int sleeping;               //How much longer should this monster sleep?


    public Monster()
    {
        x = 0;
        y = 0;
        facing = Direction.S;
        state = MonsterState.ACTIVE;
        monsterCode = MonsterCode.HUMAN;
        role = MonsterRole.NULL;
        ID = UUID.randomUUID().toString();
        AIParameters = new HashMap<String,String>();
        stats = new MonsterStats();
        inventory = new MonsterInventory();
        sleeping = 0;
        MonsterActionManager.initialize(this);
        MonsterActionManager.setNullCommand(this);
    }

    public boolean isSleeping()
    {
        if(sleeping > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public String getAIParameter(String key)
    {
        return AIParameters.get(key);
    }

    public void setAIParameter(String key, String value)
    {
        AIParameters.put(key, value);
    }

    public boolean hasAIParameter(String key)
    {
        if(getAIParameter(key) != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void cycle(Quinoa quinoa)
    {
        if((getStats().getCurrentHealth() < 1))
        {
            if(!this.getID().equals(MonsterActionManager.PLAYER_ID))
            {
                MonsterActionManager.monsterKilled(this, this, quinoa);
                remove();
            }
        }

        //Cycle status counters
        if(isSleeping())
        {
            sleeping = sleeping - 1;
        }

        //Handle hunger
        if(getStats().getHungerRate() > 0.0)
        {
            if(getStats().getCurrentHunger() < MonsterStats.MAX_HUNGER)
            {
                getStats().setCurrentHunger(getStats().getCurrentHunger() + MonsterActionManager.getHungerRate(this));
            }
            else
            {
                //Hunger effects
                //Remove small amount of HP if fully hungry and awake
                //This prevents starving during sleep, which seems unfair
                if(!this.isSleeping())
                {
                    getStats().setCurrentHealth(getStats().getCurrentHealth() - (MonsterActionManager.getHungerRate(this) / 100.0));
                }
                else
                {
                    this.sleeping = 0;
                }
            }
        }

        //Handle standing in fire
        if(TerrainManager.hasParameter(quinoa.getCurrentRegionHeader().getRegion().getTerrain(x, y), TerrainManager.TerrainParameter.FIRE))
        {
            MonsterActionManager.fireDamage(this, 0.001);
        }

        //Handle standing in water
        if(quinoa.getCurrentRegionHeader().getRegion().getTerrain(x, y).getWater() > 0)
        {
            MonsterActionManager.waterDamage(this, 0.001);
        }

        //Do a role update at the top of each hour
        if(quinoa.getTicks() % (Quinoa.TICKS_PER_SECOND * 60 * 60) == 0)
        {
            MonsterActionManager.updateRole(this, quinoa);
        }

        //Do actual AI routines, if not asleep
        if(!this.getID().equals(MonsterActionManager.PLAYER_ID))
        {
            if(!isSleeping())
            {
                MonsterAI.think(this, quinoa);
            }
        }

        //Heal a small amount while sleeping
        if(this.getStats().getCurrentHealth() < this.getStats().getMaxHP())
        {
            this.getStats().setCurrentHealth(this.getStats().getCurrentHealth() + MonsterStats.HEAL_WHILE_SLEEPING);
        }

        if(getCommand() != null)
        {
            getCommand().cycle();
            if(getCommand().readyToExecute())
            {
                MonsterActionManager.executeCommand(this, getCommand(), quinoa);
                MonsterActionManager.setNullCommand(this);
            }
        }
    }

    public void setCommand(MonsterCommand newCommand)
    {
        this.command = newCommand;
    }

    public boolean readyForCommand()
    {
        if(isSleeping())
        {
            return false;
        }
        else if(getCommand() == null)
        {
            return true;
        }
        else if(getCommand().getCommandCode() == MonsterCommandCode.NULL)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void setPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public Object copy()
    {
        Monster newMonster = new Monster();
        newMonster.x = this.getX();
        newMonster.y = this.getY();
        newMonster.setFacing(this.getFacing());
        newMonster.setState(this.getState());
        newMonster.setMonsterCode(this.getMonsterCode());
        newMonster.setRole(this.getRole());
        newMonster.setID(this.getID());
        for(Object tempParameter : this.AIParameters.keySet().toArray())
        {
            String parameterKey = (String)tempParameter;
            String parameterValue = this.AIParameters.get(parameterKey);
            newMonster.setAIParameter(parameterKey, parameterValue);
        }
        newMonster.command = (MonsterCommand)this.getCommand().copy();
        newMonster.setStats((MonsterStats) this.getStats().copy());
        newMonster.setSleeping(this.getSleeping());
        return newMonster;
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getX() + ""); out.newLine();
        out.write(getY() + ""); out.newLine();
        out.write(getFacing().name()); out.newLine();
        out.write(getState().name()); out.newLine();
        out.write(monsterCode.name()); out.newLine();
        out.write(role.name()); out.newLine();
        out.write(ID); out.newLine();
        out.write(getSleeping() + ""); out.newLine();
        out.write(AIParameters.size() + ""); out.newLine();
        for(Object tempKey : AIParameters.keySet().toArray())
        {
            out.write((String)tempKey); out.newLine();
            out.write((String)AIParameters.get((String)tempKey)); out.newLine();
        }
        getStats().save(out);
        getCommand().save(out);
        getInventory().save(out);
    }

    public void load(BufferedReader in) throws Exception
    {
        this.x = Integer.parseInt(in.readLine());
        this.y = Integer.parseInt(in.readLine());
        this.setFacing(getFacing().valueOf(in.readLine()));
        this.setState(getState().valueOf(in.readLine()));
        this.setMonsterCode(monsterCode.valueOf(in.readLine()));
        this.setRole(role.valueOf(in.readLine()));
        this.setID(in.readLine());
        this.setSleeping(Integer.parseInt(in.readLine()));
        this.AIParameters.clear();
        int parameterSize = Integer.parseInt(in.readLine());
        for(int i=0; i < parameterSize; i++)
        {
            String newKey = in.readLine();
            String newValue = in.readLine();
            AIParameters.put(newKey, newValue);
        }
        getStats().load(in);
        getCommand().load(in);
        getInventory().load(in);
    }


    public boolean shouldBeRemoved()
    {
        if(getState() == MonsterState.DESTROYED)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void remove()
    {
        this.state = MonsterState.DESTROYED;
    }

    /**
     * @return the state
     */
    public MonsterState getState() {
        return state;
    }

    /**
     * @param state the state to set
     */
    public void setState(MonsterState state) {
        this.state = state;
    }

    /**
     * @return the monsterCode
     */
    public MonsterCode getMonsterCode() {
        return monsterCode;
    }

    /**
     * @param monsterCode the monsterCode to set
     */
    public void setMonsterCode(MonsterCode monsterCode) {
        this.monsterCode = monsterCode;
    }

    /**
     * @return the ID
     */
    public String getID() {
        return ID;
    }

    /**
     * @param ID the ID to set
     */
    public void setID(String ID) {
        this.ID = ID;
    }

    /**
     * @return the facing
     */
    public Direction getFacing() {
        return facing;
    }

    /**
     * @param facing the facing to set
     */
    public void setFacing(Direction facing) {
        this.facing = facing;
    }


    /**
     * @return the x
     */
    public int getX() {
        return x;
    }

    /**
     * @return the y
     */
    public int getY() {
        return y;
    }

    /**
     * @return the command
     */
    public MonsterCommand getCommand() {
        return command;
    }

    /**
     * @return the stats
     */
    public MonsterStats getStats() {
        return stats;
    }

    /**
     * @param stats the stats to set
     */
    public void setStats(MonsterStats stats) {
        this.stats = stats;
    }

    /**
     * @return the inventory
     */
    public MonsterInventory getInventory() {
        return inventory;
    }

    /**
     * @return the role
     */
    public MonsterRole getRole() {
        return role;
    }

    /**
     * @param role the role to set
     */
    public void setRole(MonsterRole role) {
        this.role = role;
    }

    /**
     * @return the sleeping
     */
    public int getSleeping() {
        return sleeping;
    }

    /**
     * @param sleeping the sleeping to set
     */
    public void setSleeping(int sleeping) {
        this.sleeping = sleeping;
    }
    
}
