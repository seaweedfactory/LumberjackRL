using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Utilities;
using LumberjackRL.Core.Map;
using System.IO;

namespace LumberjackRL.Core.Monsters
{    public class Monster : IStoreObject, ICopyObject, IRemoveObject
    {
        /// <summary>
        /// X position within the region
        /// </summary>
        public int x
        {
            get;
            private set;
        }

        /// <summary>
        /// Y position within the region
        /// </summary>
        public int y
        {
            get;
            private set;
        }

        /// <summary>
        /// What direction is the monster facing?
        /// </summary>
        public Direction facing
        {
            get;
            set;
        }
        
        /// <summary>
        /// What is the overall state of the monster?
        /// </summary>
        public MonsterState state
        {
            get;
            set;
        }
        
        /// <summary>
        /// What type of monster is it?
        /// </summary>
        public MonsterClassType monsterCode
        {
            get;
            set;
        }

        /// <summary>
        /// UUID in string acts as unique ID, filled at creation time
        /// </summary>
        public String ID
        {
            get;
            set;
        }

        /// <summary>
        /// Does the monster have a job?
        /// </summary>
        public MonsterRoleType role
        {
            set;
            get;
        }
        
        /// <summary>
        /// The current command waiting to execute.
        /// </summary>
        public MonsterCommand command
        {
            set;
            get;
        }

        /// <summary>
        /// Holds stat data.
        /// </summary>
        public MonsterStats stats
        {
            set;
            get;
        }

        /// <summary>
        /// Holds items and equipment.
        /// </summary>
        public MonsterInventory inventory
        {
            get;
            private set;
        }
        
        /// <summary>
        /// How much longer should this monster sleep?
        /// </summary>
        public int sleeping
        {
            set;
            get;
        }

        private Dictionary<String, String> AIParameters;   //Holds variables used by monster AI

        public Monster()
        {
            x = 0;
            y = 0;
            facing = Direction.S;
            state = MonsterState.ACTIVE;
            monsterCode = MonsterClassType.HUMAN;
            role = MonsterRoleType.NULL;
            ID = RandomNumber.RandomUUID().ToString();
            AIParameters = new Dictionary<String,String>();
            stats = new MonsterStats();
            inventory = new MonsterInventory();
            sleeping = 0;
            MonsterActionManager.initialize(this);
            MonsterActionManager.setNullCommand(this);
        }

        public bool isSleeping()
        {
            return (sleeping > 0);
        }

        public String getAIParameter(String key)
        {
            return AIParameters[key];
        }

        public void setAIParameter(String key, String value)
        {
            AIParameters.Add(key, value);
        }

        public bool hasAIParameter(String key)
        {
            return (getAIParameter(key) != null);
        }

        public void cycle(Quinoa quinoa)
        {
            if((stats.getCurrentHealth() < 1))
            {
                if(!this.ID.Equals(MonsterActionManager.PLAYER_ID))
                {
                    MonsterActionManager.monsterKilled(this, this, quinoa);
                    RemoveObject();
                }
            }

            //Cycle status counters
            if(isSleeping())
            {
                sleeping = sleeping - 1;
            }

            //Handle hunger
            if(stats.getHungerRate() > 0.0)
            {
                if (stats.getCurrentHunger() < MonsterStats.MAX_HUNGER)
                {
                    stats.setCurrentHunger(stats.getCurrentHunger() + MonsterActionManager.getHungerRate(this));
                }
                else
                {
                    //Hunger effects
                    //Remove small amount of HP if fully hungry and awake
                    //This prevents starving during sleep, which seems unfair
                    if(!this.isSleeping())
                    {
                        stats.setCurrentHealth(stats.getCurrentHealth() - (MonsterActionManager.getHungerRate(this) / 100.0));
                    }
                    else
                    {
                        this.sleeping = 0;
                    }
                }
            }

            //Handle standing in fire
            if(TerrainManager.hasParameter(quinoa.getCurrentRegionHeader().getRegion().getTerrain(x, y), TerrainParameter.FIRE))
            {
                MonsterActionManager.fireDamage(this, 0.001);
            }

            //Handle standing in water
            Terrain tempWaterCheck = quinoa.getCurrentRegionHeader().getRegion().getTerrain(x, y);
            if (tempWaterCheck != null && tempWaterCheck.getWater() > 0)
            {
                MonsterActionManager.waterDamage(this, 0.001);
            }

            //Do a role update at the top of each hour
            if(quinoa.getTicks() % (Quinoa.TICKS_PER_SECOND * 60 * 60) == 0)
            {
                MonsterActionManager.updateRole(this, quinoa);
            }

            //Do actual AI routines, if not asleep
            if(!this.ID.Equals(MonsterActionManager.PLAYER_ID))
            {
                if(!isSleeping())
                {
                    MonsterAI.think(this, quinoa);
                }
            }

            //Heal a small amount while sleeping
            if (this.stats.getCurrentHealth() < this.stats.getMaxHP())
            {
                this.stats.setCurrentHealth(this.stats.getCurrentHealth() + MonsterStats.HEAL_WHILE_SLEEPING);
            }

            if(command != null)
            {
                command.cycle();
                if(command.readyToExecute())
                {
                    MonsterActionManager.executeCommand(this, command, quinoa);
                    MonsterActionManager.setNullCommand(this);
                }
            }
        }

        public void setCommand(MonsterCommand newCommand)
        {
            this.command = newCommand;
        }

        public bool readyForCommand()
        {
            if(isSleeping())
            {
                return false;
            }
            else if(command == null)
            {
                return true;
            }
            else if(command.commandCode == MonsterCommandType.NULL)
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

        public Object CopyObject()
        {
            Monster newMonster = new Monster();
            newMonster.x = x;
            newMonster.y = y;
            newMonster.facing = facing;
            newMonster.state = state;
            newMonster.monsterCode = monsterCode;
            newMonster.role = role;
            newMonster.ID = ID;
            foreach(Object tempParameter in this.AIParameters.Keys)
            {
                String parameterKey = (String)tempParameter;
                String parameterValue = this.AIParameters[parameterKey];
                newMonster.setAIParameter(parameterKey, parameterValue);
            }
            newMonster.command = (MonsterCommand)command.CopyObject();
            newMonster.stats = (MonsterStats)stats.CopyObject();
            newMonster.sleeping= sleeping;
            return newMonster;
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(x + "");
            outStream.WriteLine(y + "");
            outStream.WriteLine(facing.ToString());
            outStream.WriteLine(state.ToString());
            outStream.WriteLine(monsterCode.ToString());
            outStream.WriteLine(role.ToString());
            outStream.WriteLine(ID);
            outStream.WriteLine(sleeping + "");
            outStream.WriteLine(AIParameters.Count + "");
            foreach(String tempKey in AIParameters.Keys)
            {
                outStream.WriteLine(tempKey);
                outStream.WriteLine(AIParameters[tempKey]);
            }
            stats.SaveObject(outStream);
            command.SaveObject(outStream);
            inventory.SaveObject(outStream);
        }

        public void LoadObject(StreamReader inStream)
        {
            this.x = Int32.Parse(inStream.ReadLine());
            this.y = Int32.Parse(inStream.ReadLine());
            this.facing = (Direction)Enum.Parse(typeof(Direction), inStream.ReadLine());
            this.state = (MonsterState)Enum.Parse(typeof(MonsterState), inStream.ReadLine());
            this.monsterCode = (MonsterClassType)Enum.Parse(typeof(MonsterClassType), inStream.ReadLine());
            this.role = (MonsterRoleType)Enum.Parse(typeof(MonsterRoleType), inStream.ReadLine());
            this.ID = inStream.ReadLine();
            this.sleeping = Int32.Parse(inStream.ReadLine());
            this.AIParameters.Clear();
            int parameterSize = Int32.Parse(inStream.ReadLine());
            for(int i=0; i < parameterSize; i++)
            {
                String newKey = inStream.ReadLine();
                String newValue = inStream.ReadLine();
                AIParameters.Add(newKey, newValue);
            }
            stats.LoadObject(inStream);
            command.LoadObject(inStream);
            inventory.LoadObject(inStream);
        }


        public bool ShouldBeRemoved
        {
            get
            {
                return (state == MonsterState.DESTROYED);
            }
        }

        public void RemoveObject()
        {
            this.state = MonsterState.DESTROYED;
        }
    }
}
