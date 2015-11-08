using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LumberjackRL.Core.Monsters
{
    public class MonsterStats : IStoreObject, ICopyObject
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

        public const int MAX_LEVEL = 40;
        public const int STAT_POINTS_PER_LEVEL = 3;
        public const int MAX_STAT = 50;
        public const int MIN_STAT = 1;

        public const double MAX_HUNGER = 4000.0;
        public const double DEFAULT_HUNGER_RATE = 0.001;
        public const double MIN_SPEED = 50;
        public const double NORMAL_SPEED = 100;
        public const double NORMAL_SPEED_AGILITY_VALUE = 10;
    
        public const double ENDURANCE_PER_DEFENSE = 3.0;
        public const double MIGHT_PER_ATTACK = 4.0;
        public const double HEALTH_PER_HP = 4.0;

        public const double HEAL_WHILE_SLEEPING = 0.000004;

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
            //Format to 0.00, to decimal places
            return currentHP.ToString("C");
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

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(getHealth()+"");
            outStream.WriteLine(getMight()+"");
            outStream.WriteLine(getEndurance()+"");
            outStream.WriteLine(getAgility()+"");
            outStream.WriteLine(getCurrentHealth()+"");
            outStream.WriteLine(getCurrentHunger()+"");
            outStream.WriteLine(getHungerRate()+"");
            outStream.WriteLine(getAvailableStatPoints()+"");
            outStream.WriteLine(getLevel()+"");
            outStream.WriteLine(getExperience()+"");
            outStream.WriteLine(getExperienceGiven()+"");
            outStream.WriteLine(getRadiance()+"");
        }

        public void LoadObject(StreamReader inStream)
        {
            setHealth(Int32.Parse(inStream.ReadLine()));
            setMight(Int32.Parse(inStream.ReadLine()));
            setEndurance(Int32.Parse(inStream.ReadLine()));
            setAgility(Int32.Parse(inStream.ReadLine()));
            setCurrentHealth(Double.Parse(inStream.ReadLine()));
            setCurrentHunger(Double.Parse(inStream.ReadLine()));
            setHungerRate(Double.Parse(inStream.ReadLine()));
            setAvailableStatPoints(Int32.Parse(inStream.ReadLine()));
            setLevel(Int32.Parse(inStream.ReadLine()));
            setExperience(Int32.Parse(inStream.ReadLine()));
            setExperienceGiven(Int32.Parse(inStream.ReadLine()));
            setRadiance(Double.Parse(inStream.ReadLine()));
        }

        public Object CopyObject()
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
}
