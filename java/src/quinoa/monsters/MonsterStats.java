package quinoa.monsters;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.text.DecimalFormat;
import java.text.NumberFormat;
import quinoa.Copyable;
import quinoa.Storable;

public class MonsterStats implements Storable, Copyable
{
    private int health;     //Max HP
    private int might;      //Like strength and attack-rating combined
    private int endurance;  //Like defense-rating and stamina combined, also hunger rate
    private int agility;    //Like chance-to-hit, intelligence, and speed combined
    
    private double currentHP;       //Current health

    private double currentHunger;   //Current hunger
    private double hungerRate;      //Rate at which hunger occurs, 0 for no hunger

    private int availableStatPoints; //How many stat points are waiting to be redistributed
    private int level;               //Current level of the monster
    private int experience;          //Current experience gained by monster

    private int experienceGiven;     //How much eperience is given when the monster is killed

    private double radiance;         //How light does this monster produce just by itself 0.0-1.0

    public static final int MAX_LEVEL = 40;
    public static final int STAT_POINTS_PER_LEVEL = 3;
    public static final int MAX_STAT = 50;
    public static final int MIN_STAT = 1;

    public static final double MAX_HUNGER = 4000.0;
    public static final double DEFAULT_HUNGER_RATE = 0.001;
    public static final double MIN_SPEED = 50;
    public static final double NORMAL_SPEED = 100;
    public static final double NORMAL_SPEED_AGILITY_VALUE = 10;
    
    public static final double ENDURANCE_PER_DEFENSE = 3.0;
    public static final double MIGHT_PER_ATTACK = 4.0;
    public static final double HEALTH_PER_HP = 4.0;

    public static final double HEAL_WHILE_SLEEPING = 0.000004;

    public MonsterStats()
    {
        health = 1;
        might = 1;
        endurance = 1;
        agility = 1;
        currentHP=1;
        currentHunger = 0;
        availableStatPoints = 0;
        level = 1;
        experience = 1;
        experienceGiven = 1;
        hungerRate = DEFAULT_HUNGER_RATE;
        radiance = 0.0;
    }

    public String getDisplayHealth()
    {
        NumberFormat formatter = new DecimalFormat("#0.0");
        return formatter.format(currentHP);
    }

    public String getDisplayHunger()
    {
        if(currentHunger < MAX_HUNGER * 0.25)
        {
            return "satisfied";
        }
        else if(currentHunger < MAX_HUNGER * 0.50)
        {
            return "hungry";
        }
        else if(currentHunger < MAX_HUNGER * 0.75)
        {
            return "very hungry";
        }
        else if(currentHunger < MAX_HUNGER)
        {
            return "starving";
        }
        else
        {
            return "dying of hunger";
        }
    }

    
    public int getMaxHP()
    {
        return (int)(health * HEALTH_PER_HP);
    }

    public int getSpeed()
    {
        double speedSpread = NORMAL_SPEED - MIN_SPEED;
        double speedPointsPerLevel = speedSpread / (MAX_STAT - NORMAL_SPEED_AGILITY_VALUE);

        double adjustedSpeed = MIN_SPEED;
        if(agility >= NORMAL_SPEED_AGILITY_VALUE)
        {
            adjustedSpeed = (int)(NORMAL_SPEED - ((agility - NORMAL_SPEED_AGILITY_VALUE) * speedPointsPerLevel));
            if(adjustedSpeed < MIN_SPEED)
            {
                adjustedSpeed = MIN_SPEED;
            }
        }
        else
        {
            adjustedSpeed = (int)(NORMAL_SPEED + (speedPointsPerLevel * 50 * (NORMAL_SPEED_AGILITY_VALUE - agility)));
        }
        return (int)(adjustedSpeed);
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getHealth()+""); out.newLine();
        out.write(getMight()+""); out.newLine();
        out.write(getEndurance()+""); out.newLine();
        out.write(getAgility()+""); out.newLine();
        out.write(getCurrentHealth()+""); out.newLine();
        out.write(getCurrentHunger()+""); out.newLine();
        out.write(getHungerRate()+""); out.newLine();
        out.write(getAvailableStatPoints()+""); out.newLine();
        out.write(getLevel()+""); out.newLine();
        out.write(getExperience()+""); out.newLine();
        out.write(getExperienceGiven()+""); out.newLine();
        out.write(getRadiance()+""); out.newLine();
    }

    public void load(BufferedReader in) throws Exception
    {
        setHealth(Integer.parseInt(in.readLine()));
        setMight(Integer.parseInt(in.readLine()));
        setEndurance(Integer.parseInt(in.readLine()));
        setAgility(Integer.parseInt(in.readLine()));
        setCurrentHealth(Double.parseDouble(in.readLine()));
        setCurrentHunger(Double.parseDouble(in.readLine()));
        setHungerRate(Double.parseDouble(in.readLine()));
        setAvailableStatPoints(Integer.parseInt(in.readLine()));
        setLevel(Integer.parseInt(in.readLine()));
        setExperience(Integer.parseInt(in.readLine()));
        setExperienceGiven(Integer.parseInt(in.readLine()));
        setRadiance(Double.parseDouble(in.readLine()));
    }

    public Object copy()
    {
        MonsterStats newStats = new MonsterStats();
        newStats.setHealth(this.health);
        newStats.setMight(this.might);
        newStats.setEndurance(this.endurance);
        newStats.setAgility(this.agility);
        newStats.setCurrentHealth(this.currentHP);
        newStats.setCurrentHunger(this.currentHunger);
        newStats.setHungerRate(this.hungerRate);
        newStats.setAvailableStatPoints(this.availableStatPoints);
        newStats.setLevel(this.level);
        newStats.setExperience(this.experience);
        newStats.setExperienceGiven(this.experienceGiven);
        newStats.setRadiance(this.radiance);
        return newStats;
    }

    /**
     * @return the health
     */
    public int getHealth() {
        return health;
    }

    /**
     * @param health the health to set
     */
    public void setHealth(int health) {
        this.health = health;
    }

    /**
     * @return the might
     */
    public int getMight() {
        return might;
    }

    /**
     * @param might the might to set
     */
    public void setMight(int might) {
        this.might = might;
    }

    /**
     * @return the endurance
     */
    public int getEndurance() {
        return endurance;
    }

    /**
     * @param endurance the endurance to set
     */
    public void setEndurance(int endurance) {
        this.endurance = endurance;
    }

    /**
     * @return the agility
     */
    public int getAgility() {
        return agility;
    }

    /**
     * @param agility the agility to set
     */
    public void setAgility(int agility) {
        this.agility = agility;
    }

    /**
     * @return the currentHP
     */
    public double getCurrentHealth() {
        return currentHP;
    }

    /**
     * @param currentHP the currentHP to set
     */
    public void setCurrentHealth(double currentHealth) 
    {
        if(currentHealth <= this.getMaxHP())
        {
            this.currentHP = currentHealth;
        }
        else
        {
            this.currentHP = this.getMaxHP();
        }
    }

    /**
     * @return the currentHunger
     */
    public double getCurrentHunger() {
        return currentHunger;
    }

    /**
     * @param currentHunger the currentHunger to set
     */
    public void setCurrentHunger(double currentHunger) {
        this.currentHunger = currentHunger;
    }

    /**
     * @return the availableStatPoints
     */
    public int getAvailableStatPoints() {
        return availableStatPoints;
    }

    /**
     * @param availableStatPoints the availableStatPoints to set
     */
    public void setAvailableStatPoints(int availableStatPoints) {
        this.availableStatPoints = availableStatPoints;
    }

    /**
     * @return the level
     */
    public int getLevel() {
        return level;
    }

    /**
     * @param level the level to set
     */
    public void setLevel(int level) {
        this.level = level;
    }

    /**
     * @return the experience
     */
    public int getExperience() {
        return experience;
    }

    /**
     * @param experience the experience to set
     */
    public void setExperience(int experience) {
        this.experience = experience;
    }

    /**
     * @return the experienceGiven
     */
    public int getExperienceGiven() {
        return experienceGiven;
    }

    /**
     * @param experienceGiven the experienceGiven to set
     */
    public void setExperienceGiven(int experienceGiven) {
        this.experienceGiven = experienceGiven;
    }

    /**
     * @return the hungerRate
     */
    public double getHungerRate() {
        return hungerRate;
    }

    /**
     * @param hungerRate the hungerRate to set
     */
    public void setHungerRate(double hungerRate) {
        this.hungerRate = hungerRate;
    }

    /**
     * @return the radiance
     */
    public double getRadiance() {
        return radiance;
    }

    /**
     * @param radiance the radiance to set
     */
    public void setRadiance(double radiance) {
        this.radiance = radiance;
    }

}
