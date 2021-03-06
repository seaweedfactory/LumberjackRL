﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Items;
using LumberjackRL.Core.Utilities;
using LumberjackRL.Core.Map;

namespace LumberjackRL.Core.Monsters
{

    public class MonsterActionManager
    {
        
        public const String PLAYER_ID="player";
        public static int[] xpLevels = {1,50,100,200,400,800,1600,3200,6400,12800, //double
                                        22400, 39200, 68600, 120050, 210087, 367653, 643392, 1125937, 1970390, 3448184, //times 1.75
                                        5172276, 7758414, 11637621, 17456432, 26184648, 39276973, 58915459, 88373189, 132559784, 198839676, //times 1.5
                                        248549595, 310686993, 388358742, 485448428, 606810535, 758513168, 948141460, 1185176826, 1481471032, 1851838791}; //times 1.25


        public static void initialize(Monster monster)
        {
            switch(monster.monsterCode)
            {
                case MonsterClassType.HUMAN:
                    monster.stats.setHealth(3);
                    monster.stats.setMight(2);
                    monster.stats.setEndurance(2);
                    monster.stats.setAgility(2);
                    monster.inventory.setDimensions(10, 5);
                    monster.stats.setLevel(1);
                    monster.stats.setExperienceGiven(10);
                    monster.stats.setHungerRate(MonsterStats.DEFAULT_HUNGER_RATE);
                    monster.stats.setRadiance(0.25);
                    break;

                case MonsterClassType.GHOST:
                    monster.stats.setHealth(1);
                    monster.stats.setMight(1);
                    monster.stats.setEndurance(1);
                    monster.stats.setAgility(1);
                    monster.inventory.setDimensions(2, 4);
                    monster.stats.setLevel(1);
                    monster.stats.setExperienceGiven(5);
                    monster.stats.setHungerRate(0);
                    monster.stats.setRadiance(0.5);
                    break;

                case MonsterClassType.SPONGE:
                    monster.stats.setHealth(1);
                    monster.stats.setMight(1);
                    monster.stats.setEndurance(1);
                    monster.stats.setAgility(1);
                    monster.inventory.setDimensions(8, 4);
                    monster.stats.setLevel(1);
                    monster.stats.setExperienceGiven(4);
                    monster.stats.setHungerRate(0);
                    monster.stats.setRadiance(0.1);
                    break;

                case MonsterClassType.SLIME:
                    monster.stats.setHealth(4);
                    monster.stats.setMight(1);
                    monster.stats.setEndurance(1);
                    monster.stats.setAgility(1);
                    monster.inventory.setDimensions(2, 4);
                    monster.stats.setLevel(3);
                    monster.stats.setExperienceGiven(8);
                    monster.stats.setHungerRate(0);
                    monster.stats.setRadiance(LightMap.MAX_LIGHT);
                    break;

                case MonsterClassType.SMALL_SLIME:
                    monster.stats.setHealth(2);
                    monster.stats.setMight(1);
                    monster.stats.setEndurance(1);
                    monster.stats.setAgility(1);
                    monster.inventory.setDimensions(2, 4);
                    monster.stats.setLevel(2);
                    monster.stats.setExperienceGiven(4);
                    monster.stats.setHungerRate(0);
                    monster.stats.setRadiance(LightMap.MAX_LIGHT * 0.75);
                    break;

                case MonsterClassType.TINY_SLIME:
                    monster.stats.setHealth(1);
                    monster.stats.setMight(1);
                    monster.stats.setEndurance(1);
                    monster.stats.setAgility(1);
                    monster.inventory.setDimensions(2, 4);
                    monster.stats.setLevel(1);
                    monster.stats.setExperienceGiven(2);
                    monster.stats.setHungerRate(0);
                    monster.stats.setRadiance(LightMap.MAX_LIGHT * 0.5);
                    break;


                case MonsterClassType.DEER:
                    monster.stats.setHealth(10);
                    monster.stats.setMight(1);
                    monster.stats.setEndurance(1);
                    monster.stats.setAgility(5);
                    monster.inventory.setDimensions(1, 4);
                    monster.stats.setLevel(1);
                    monster.stats.setExperienceGiven(4);
                    monster.stats.setHungerRate(MonsterStats.DEFAULT_HUNGER_RATE);
                    monster.stats.setRadiance(0.0);
                    break;

                case MonsterClassType.PIG:
                    monster.stats.setHealth(8);
                    monster.stats.setMight(2);
                    monster.stats.setEndurance(1);
                    monster.stats.setAgility(2);
                    monster.inventory.setDimensions(1, 4);
                    monster.stats.setLevel(1);
                    monster.stats.setExperienceGiven(4);
                    monster.stats.setHungerRate(MonsterStats.DEFAULT_HUNGER_RATE);
                    monster.stats.setRadiance(0.0);
                    break;
            }

            monster.stats.setCurrentHealth(monster.stats.getMaxHP());
            MonsterActionManager.addDrops(monster);
        }

        public static double getSpawnRate(MonsterClassType monsterCode)
        {
            switch(monsterCode)
            {
                case MonsterClassType.GHOST:
                return 0.25;

                case MonsterClassType.SPONGE:
                return 0.005;

                case MonsterClassType.SLIME:
                return 0.005;

                case MonsterClassType.TINY_SLIME:
                return 0.005;

                case MonsterClassType.DEER:
                return 0.25;

                case MonsterClassType.PIG:
                return 0.15;

                default:
                return 1.0;
            }
        }

        public static int getMaxMonsterPerRegion(MonsterClassType monsterCode)
        {
            switch(monsterCode)
            {
                case MonsterClassType.GHOST:
                return 6;

                case MonsterClassType.SPONGE:
                return 3;

                case MonsterClassType.SLIME:
                return 3;

                case MonsterClassType.TINY_SLIME:
                return 8;

                case MonsterClassType.DEER:
                return 6;

                case MonsterClassType.PIG:
                return 4;

                default:
                return 1;
            }
        }


        public static void addDrops(Monster monster)
        {
            Item tempItem = null;
            RandomNumberGenerator rng = new RandomNumberGenerator();
            switch (monster.monsterCode)
            {
                case MonsterClassType.SLIME:
                case MonsterClassType.GHOST:
                case MonsterClassType.SPONGE:
                if(rng.RandomDouble() > 0.75d)
                {
                    tempItem = new Item();
                    tempItem.itemClass = ItemClassType.COINS;
                    tempItem.stackSize = (1 + (int)(rng.RandomDouble() * 15d));
                    monster.inventory.addItem(tempItem);
                }
                break;

                case MonsterClassType.PIG:
                tempItem = new Item();
                tempItem.itemClass = ItemClassType.BACON;
                tempItem.stackSize = (1 + (int)(rng.RandomDouble() * (tempItem.maxStackSize - 1)));
                monster.inventory.addItem(tempItem);

                tempItem = new Item();
                tempItem.itemClass = ItemClassType.BONES;
                tempItem.stackSize = 1;
                monster.inventory.addItem(tempItem);
                break;

                case MonsterClassType.DEER:
                tempItem = new Item();
                tempItem.itemClass = ItemClassType.BONES;
                tempItem.stackSize = 1;
                monster.inventory.addItem(tempItem);
                break;

                default:
                //do nothing
                break;
            }
        }

        public static void sellItemToMonster(Monster seller, Monster buyer, Item item, Quinoa quinoa)
        {
            //Check if the buyer has enough money
            if(buyer.inventory.wealth >= item.worth)
            {
                //Do the sale
                Item newItem = (Item)item.CopyObject();
                newItem.stackSize = 1;

                if(!buyer.inventory.stackItem(newItem))
                {
                    if(!buyer.inventory.Full)
                    {
                        //Successfully sold as new item
                        buyer.inventory.addItem(newItem);
                        item.stackSize = item.stackSize-1;
                        if(item.stackSize == 0)
                        {
                            item.RemoveObject();
                            seller.inventory.removedExpiredItems();
                        }
                        seller.inventory.wealth = (seller.inventory.wealth + newItem.worth);
                        buyer.inventory.wealth = (buyer.inventory.wealth - newItem.worth);
                    }
                    else
                    {
                        //No room in buyer inventory
                        //Buyer can not afford
                        if(isPlayer(buyer))
                        {
                            quinoa.getMessageManager().addMessage("You don't have enough room for that.");
                        }
                        else if(isPlayer(seller))
                        {
                            quinoa.getMessageManager().addMessage("They don't have enough room.");
                        }
                    }
                }
                else
                {
                    //Successfully sold as stacked
                    item.stackSize = item.stackSize-1;
                    if(item.stackSize == 0)
                    {
                        item.RemoveObject();
                        seller.inventory.removedExpiredItems();
                    }
                    seller.inventory.wealth = (seller.inventory.wealth + newItem.worth);
                    buyer.inventory.wealth = (buyer.inventory.wealth - newItem.worth);
                }
            }
            else
            {
                //Buyer can not afford
                if(isPlayer(buyer))
                {
                    quinoa.getMessageManager().addMessage("You don't have enough money for that.");
                }
                else if(isPlayer(seller))
                {
                    quinoa.getMessageManager().addMessage("They don't have enough money.");
                }
            }
        }

        public static void waterDamage(Monster monster, double points)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            switch (monster.monsterCode)
            {
                case MonsterClassType.TINY_SLIME:
                //kills small slimes
                monster.stats.setCurrentHealth(0);
                break;

                case MonsterClassType.SPONGE:
                //heals sponges
                if(rng.RandomDouble() > 0.75)
                {
                    monster.stats.setCurrentHealth(monster.stats.getCurrentHealth() + 0.001);
                }
                break;

                default:
                //Wake up if splashed with water
                monster.sleeping = 0;
                break;
            }
        }

        public static void fireDamage(Monster monster, double points)
        {
            //get speed divider based on items
            double firePercent=0;
            foreach(MonsterItemSlotType tempSlot in Enum.GetValues(typeof(MonsterItemSlotType)).Cast<MonsterItemSlotType>())
            {
                Item tempItem = monster.inventory.getItem(tempSlot);
                if(tempItem != null)
                {
                    foreach(ItemAttribute attribute in tempItem.attributes)
                    {
                        if(attribute.type == ItemAttributeType.PROTECT_AGAINST_FIRE)
                        {
                            firePercent = firePercent + Double.Parse(attribute.parameter.Replace("%", ""));
                        }
                    }
                }
            }

            double damage = points * ((100 - firePercent) / 100.0);
            monster.stats.setCurrentHealth(monster.stats.getCurrentHealth() - damage);
        
            //wake up on damage
            if(damage > 0)
            {
                monster.sleeping = 0;
            }
        }


        public static void cashOutInventory(Monster monster)
        {
            if(monster.inventory.Full)
            {
                foreach(Item tempItem in monster.inventory.getItems())
                {
                    monster.inventory.wealth = (monster.inventory.wealth + tempItem.worth);
                    tempItem.RemoveObject();
                }
                monster.inventory.removedExpiredItems();
            }
        }

        public static void leaveWaterTrail(Monster monster, Quinoa quinoa, double strength)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            Terrain terrain = quinoa.getCurrentRegionHeader().getRegion().getTerrain(monster.x, monster.y);

            //leave water trails
            if (rng.RandomDouble() > (1.0 - strength))
            {
                if(TerrainManager.wetable(terrain, monster.x, monster.y, quinoa))
                {
                    if(terrain.getWater() < 3)
                    {
                        terrain.setWater(terrain.getWater()+1);
                    }
                }
            }
        }



        public static void monsterKilled(Monster killer, Monster victim, Quinoa quinoa)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            String playerFeedback="The " + victim.monsterCode.ToString() + " is gone.";

            //Give experience
            MonsterActionManager.giveExperience(killer, victim.stats.getExperienceGiven(), quinoa);

            //Drop items
            foreach(Item tempItem in victim.inventory.getItems())
            {
                Item newItem = (Item)tempItem.CopyObject();
                newItem.setPosition(victim.x, victim.y);
                newItem.itemState = ItemState.GROUND;
                quinoa.getCurrentRegionHeader().getRegion().getItems().Add(newItem);
            }

            //Any monster specific items
            switch(victim.monsterCode)
            {
                case MonsterClassType.HUMAN:
                break;

                case MonsterClassType.GHOST:
                playerFeedback = "The " + victim.monsterCode.ToString() + " fades away...";
                break;

                case MonsterClassType.SPONGE:
                playerFeedback = "The " + victim.monsterCode.ToString() + " crumbles.";
                break;

                case MonsterClassType.SLIME:
                for(int i=0; i < 2; i++)
                {
                    Monster newMon = new Monster();
                    newMon.monsterCode = MonsterClassType.SMALL_SLIME;
                    MonsterActionManager.initialize(newMon);
                    newMon.setPosition(victim.x + rng.RandomInteger(5) - 2, victim.y + rng.RandomInteger(5) - 2);
                    quinoa.getActions().addMonster(newMon);
                }
                playerFeedback = "The " + victim.monsterCode.ToString() + " splits!";
                break;

                case MonsterClassType.SMALL_SLIME:
                for(int i=0; i < 2; i++)
                {
                    Monster newMon = new Monster();
                    newMon.monsterCode = MonsterClassType.TINY_SLIME;
                    MonsterActionManager.initialize(newMon);
                    newMon.setPosition(victim.x + rng.RandomInteger(7) - 4, victim.y + rng.RandomInteger(7) - 4);
                    quinoa.getActions().addMonster(newMon);
                }
                playerFeedback = "The " + victim.monsterCode.ToString() + " splits!";
                break;

                case MonsterClassType.TINY_SLIME:
                playerFeedback = "The " + victim.monsterCode.ToString() + " evaporates!";
                break;

                case MonsterClassType.DEER:
                case MonsterClassType.PIG:
                playerFeedback = "The " + victim.monsterCode.ToString() + " collapses.";
                break;

                default:
                break;
            }

            //Inform plyer
            if(killer.ID.Equals(MonsterActionManager.PLAYER_ID))
            {
                quinoa.getMessageManager().addMessage(playerFeedback);
            }

        }

        public static int getNextXPLevel(Monster monster)
        {
            if((monster.stats.getLevel()-1) >= xpLevels.Length)
            {
                return xpLevels[xpLevels.Length-1] + 1;
            }
            else
            {
                return xpLevels[monster.stats.getLevel()];
            }
        }

        public static double getDefenseRating(Monster monster)
        {
            //get basic stat defense
            double baseDef = monster.stats.getEndurance() * MonsterStats.ENDURANCE_PER_DEFENSE;

            //get item stats defense
            double itemDef = 0.0;
            foreach(MonsterItemSlotType tempSlot in Enum.GetValues(typeof(MonsterItemSlotType)).Cast<MonsterItemSlotType>())
            {
                Item tempItem = monster.inventory.getItem(tempSlot);
                if(tempItem != null)
                {
                    foreach(ItemAttribute attribute in tempItem.attributes)
                    {
                        if(attribute.type == ItemAttributeType.BONUS_TO_DEFENSE)
                        {
                            itemDef = itemDef + Double.Parse(attribute.parameter);
                        }
                    }
                }
            }
        
            return baseDef + itemDef;
        }

        public static double getAttackRating(Monster monster)
        {
            //get basic stat attack
            double baseAtk = monster.stats.getMight() * MonsterStats.MIGHT_PER_ATTACK;

            //get item stats attack
            double itemAtk = 0.0;
            foreach(MonsterItemSlotType tempSlot in Enum.GetValues(typeof(MonsterItemSlotType)).Cast<MonsterItemSlotType>())
            {
                Item tempItem = monster.inventory.getItem(tempSlot);
                if(tempItem != null)
                {
                    foreach(ItemAttribute attribute in tempItem.attributes)
                    {
                        if(attribute.type == ItemAttributeType.BONUS_TO_ATTACK)
                        {
                            itemAtk = itemAtk + Double.Parse(attribute.parameter);
                        }
                    }
                }
            }

            return baseAtk + itemAtk;
        }

        public static int getSpeed(Monster monster)
        {
            //get basic stat speed
            double baseSpd = monster.stats.getSpeed();

            //get speed divider based on items
            double spdPercent=0;
            foreach(MonsterItemSlotType tempSlot in Enum.GetValues(typeof(MonsterItemSlotType)).Cast<MonsterItemSlotType>())
            {
                Item tempItem = monster.inventory.getItem(tempSlot);
                if(tempItem != null)
                {
                    foreach(ItemAttribute attribute in tempItem.attributes)
                    {
                        if(attribute.type == ItemAttributeType.INCREASE_SPEED)
                        {
                            spdPercent = spdPercent + Double.Parse(attribute.parameter.Replace("%", ""));
                        }
                        else if(attribute.type == ItemAttributeType.DECREASE_SPEED)
                        {
                            spdPercent = spdPercent - Double.Parse(attribute.parameter.Replace("%", ""));
                        }
                    }
                }
            }

            int adjustedSpd = (int)(baseSpd - (baseSpd * (spdPercent / 100.0)));
            if(adjustedSpd < 1)
            {
                adjustedSpd = 1;
            }

            return adjustedSpd;
        }

        public static double getHungerRate(Monster monster)
        {
            //get basic stat hunger rate
            double baseHR = monster.stats.getHungerRate();

            //get speed divider based on items
            double hrPercent=0;
            foreach(MonsterItemSlotType tempSlot in Enum.GetValues(typeof(MonsterItemSlotType)).Cast<MonsterItemSlotType>())
            {
                Item tempItem = monster.inventory.getItem(tempSlot);
                if(tempItem != null)
                {
                    foreach(ItemAttribute attribute in tempItem.attributes)
                    {
                        if(attribute.type == ItemAttributeType.QUICKEN_HUNGER)
                        {
                            hrPercent = hrPercent - Double.Parse(attribute.parameter.Replace("%", ""));
                        }
                        else if(attribute.type == ItemAttributeType.SLOW_HUNGER)
                        {
                            hrPercent = hrPercent + Double.Parse(attribute.parameter.Replace("%", ""));
                        }
                    }
                }
            }

            double adjustedHR = baseHR * ((100.0 - hrPercent) / 100.0);
            if(adjustedHR < 0.0)
            {
                adjustedHR = 0.0;
            }

            //if sleeping, then slow to 1/4 hunger rate
            if(monster.isSleeping())
            {
                adjustedHR = adjustedHR / 4.0;
            }

            return adjustedHR;
        }

        public static void combat(Monster attacker, Monster defender, Quinoa quinoa)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            //Wake on being hit
            defender.sleeping = 0;

            //Hit if rnd() < ATK / (ATK + DEF).
            double atk = MonsterActionManager.getAttackRating(attacker);
            double def = MonsterActionManager.getDefenseRating(defender);
            if (rng.RandomDouble() < atk / (atk + def))
            {

                double damage = 0.0;
                if(atk > def)
                {
                    //damage = rnd() * (ATK - DEF)
                    damage = ((rng.RandomDouble() / 2) + 0.5) * (atk - def);
                
                }
                else
                {
                    //damage = rnd()
                    damage = rng.RandomDouble();
                }

                defender.stats.setCurrentHealth(defender.stats.getCurrentHealth() - damage);

                if(MonsterActionManager.isPlayer(attacker))
                {
                    quinoa.getMessageManager().addMessage("You hit the " + defender.monsterCode.ToString() + " for " + damage + ".");

                }
                else if(MonsterActionManager.isPlayer(defender))
                {
                    quinoa.getMessageManager().addMessage("The " + defender.monsterCode.ToString() + " hits for " + damage + ".");
                }

            }
            else
            {
                if(MonsterActionManager.isPlayer(attacker))
                {
                    quinoa.getMessageManager().addMessage("You missed the " + defender.monsterCode.ToString() + ".");
                }
                else if(MonsterActionManager.isPlayer(defender))
                {
                    quinoa.getMessageManager().addMessage("The " + defender.monsterCode.ToString() + " missed you.");
                }
            }
        }

        public static void giveExperience(Monster monster, int amount, Quinoa quinoa)
        {
            monster.stats.setExperience(monster.stats.getExperience() + amount);

            //check for level up
            int startLevel = monster.stats.getLevel();
            int endLevel = monster.stats.getLevel();
            for(int i=0; i < xpLevels.Length; i++)
            {
                if(monster.stats.getExperience() >= xpLevels[i])
                {
                    endLevel = (i + 1);
                }
            }

            if(endLevel > startLevel && startLevel != MonsterStats.MAX_LEVEL)
            {
                monster.stats.setLevel(endLevel);
                monster.stats.setAvailableStatPoints(monster.stats.getAvailableStatPoints() + ((endLevel - startLevel) * MonsterStats.STAT_POINTS_PER_LEVEL ));
                monster.stats.setCurrentHealth(monster.stats.getMaxHP());

                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("Level up!");
                    quinoa.getUI().getScreen().displayDialog();
                }
            }

        }

        public static void executeCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
        {
            switch(command.commandCode)
            {
                case MonsterCommandType.NULL: break; //do nothing
                case MonsterCommandType.MOVE: executeMoveCommand(monster, command, quinoa); break;
                case MonsterCommandType.ATTACK: executeAttackCommand(monster, command, quinoa); break;
                case MonsterCommandType.PICKUP: executePickupCommand(monster, command, quinoa); break;
                case MonsterCommandType.DROP: executeDropCommand(monster, command, quinoa); break;
                case MonsterCommandType.EQUIP: executeEquipCommand(monster, command, quinoa); break;
                case MonsterCommandType.UNEQUIP: executeUnequipCommand(monster, command, quinoa); break;
                case MonsterCommandType.ITEM_VERB: executeItemVerbCommand(monster, command, quinoa); break;
                default: break; //do nothing
            }
        }

        public static void setNullCommand(Monster monster)
        {
            MonsterCommand newCommand = new MonsterCommand();
            newCommand.commandCode = MonsterCommandType.NULL;
            newCommand.counter = 1;
            monster.setCommand(newCommand);
        }

        public static void setMoveCommand(Monster monster, Direction direction)
        {
            MonsterCommand newCommand = new MonsterCommand();
            newCommand.commandCode = MonsterCommandType.MOVE;
            newCommand.counter = MonsterActionManager.getSpeed(monster);
            newCommand.addParameter("direction", direction.ToString());
            monster.setCommand(newCommand);
        }

        public static void setItemVerbCommand(Monster monster, Item item, ItemVerbType verb, int x, int y)
        {
            MonsterCommand newCommand = new MonsterCommand();
            newCommand.commandCode = MonsterCommandType.ITEM_VERB;
            newCommand.counter = MonsterActionManager.getSpeed(monster);
            if(item != null)
            {
                newCommand.addParameter("itemID", item.ID);
            }
            else
            {
                newCommand.addParameter("itemID", "");
            }
            newCommand.addParameter("verb", verb.ToString());
            newCommand.addParameter("x", x+"");
            newCommand.addParameter("y", y+"");
            monster.setCommand(newCommand);
        }

        public static void setAttackCommand(Monster monster, String monsterID)
        {
            MonsterCommand newCommand = new MonsterCommand();
            newCommand.commandCode = MonsterCommandType.ATTACK;
            newCommand.counter = MonsterActionManager.getSpeed(monster)/2;
            newCommand.addParameter("monsterID", monsterID);
            monster.setCommand(newCommand);
        }

        public static void setPickupCommand(Monster monster)
        {
            MonsterCommand newCommand = new MonsterCommand();
            newCommand.commandCode = MonsterCommandType.PICKUP;
            newCommand.counter = MonsterActionManager.getSpeed(monster)/4;
            monster.setCommand(newCommand);
        }

        public static void setDropCommand(Monster monster, String itemID)
        {
            MonsterCommand newCommand = new MonsterCommand();
            newCommand.commandCode = MonsterCommandType.DROP;
            newCommand.counter = MonsterActionManager.getSpeed(monster)/4;
            newCommand.addParameter("itemID", itemID);
            monster.setCommand(newCommand);
        }

        public static void setEquipCommand(Monster monster, String itemID)
        {
            MonsterCommand newCommand = new MonsterCommand();
            newCommand.commandCode = MonsterCommandType.EQUIP;
            newCommand.counter = MonsterActionManager.getSpeed(monster)/2;
            newCommand.addParameter("itemID", itemID);
            monster.setCommand(newCommand);
        }

        public static void setUnequipCommand(Monster monster, String itemID)
        {
            MonsterCommand newCommand = new MonsterCommand();
            newCommand.commandCode = MonsterCommandType.UNEQUIP;
            newCommand.counter = MonsterActionManager.getSpeed(monster)/2;
            newCommand.addParameter("itemID", itemID);
            monster.setCommand(newCommand);
        }

        public static void executeMoveCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
        {
            Region region = quinoa.getCurrentRegionHeader().getRegion();
            Direction direction = (Direction) Enum.Parse(typeof(Direction), command.getParameter("direction"));
            monster.facing = direction;

            int newX = monster.x;
            int newY = monster.y;

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

            if(newX >= 0 && newX < region.getWidth()
            && newY >= 0 && newY < region.getHeight())
            {
                Terrain tmpTerrain = region.getTerrain(newX, newY);
                if (TerrainManager.allowsMonsterToPass(tmpTerrain, monster) && quinoa.getMonster(newX, newY) == null)
                {
                    monster.setPosition(newX, newY);
                    TerrainManager.step(region.getTerrain(newX, newY), newX, newY, region, quinoa);
                }
                else
                {
                    //Do nothing
                }
            }
            else
            {
                //out of bounds, but do not inform the player
            }
        }

        public static void executeItemVerbCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
        {
            Item item = monster.inventory.getItem(command.getParameter("itemID"));
            ItemVerbType verb = (ItemVerbType)Enum.Parse(typeof(ItemVerbType), command.getParameter("verb"));
            Region region = quinoa.getCurrentRegionHeader().getRegion();
            int x = Int32.Parse(command.getParameter("x"));
            int y = Int32.Parse(command.getParameter("y"));
            bool removeItem=false;
            if(item != null)
            {
                switch(verb)
                {
                    case ItemVerbType.EAT:
                    foreach(ItemAttribute attribute in item.attributes)
                    {
                        if(attribute.type == ItemAttributeType.NUTRITION)
                        {
                            double nutrition = Double.Parse(attribute.parameter);
                            monster.stats.setCurrentHunger(monster.stats.getCurrentHunger() - nutrition);
                            if(monster.stats.getCurrentHunger() < 0)
                            {
                                monster.stats.setCurrentHunger(0.0);
                            }

                            if(isPlayer(monster))
                            {
                                quinoa.getMessageManager().addMessage("Ate " + item.itemClass.ToString() + ".");
                            }
                        }

                        if(attribute.type == ItemAttributeType.HEALS)
                        {
                            double points = Double.Parse(attribute.parameter);
                            monster.stats.setCurrentHealth(monster.stats.getCurrentHealth() + points);
                            if(monster.stats.getCurrentHealth() > monster.stats.getMaxHP())
                            {
                                monster.stats.setCurrentHealth(monster.stats.getMaxHP());
                            }

                            if(isPlayer(monster))
                            {
                                if(points >= 0)
                                {
                                    quinoa.getMessageManager().addMessage("You feel better.");
                                }
                                else
                                {
                                    quinoa.getMessageManager().addMessage("You feel worse.");
                                }
                            }
                        }
                    }
                    removeItem = true;
                    break;

                    case ItemVerbType.PLACE:
                    removeItem = MonsterActionManager.place(monster, item, x, y, region, quinoa);
                    break;

                    case ItemVerbType.USE:
                    removeItem = use(monster, item, x, y, region, quinoa);
                    break;

                }

                if(removeItem)
                {
                    if(item.stackSize > 1)
                    {
                        item.stackSize = item.stackSize - 1;
                    }
                    else
                    {
                        item.RemoveObject();
                    }
                }
            }
            else
            {
                //Non-item verbs
                switch(verb)
                {
                    case ItemVerbType.ACTIVATE:
                    MonsterActionManager.activate(monster, x, y, region, quinoa);
                    break;

                    case ItemVerbType.TALK:
                    if(isPlayer(monster))
                    {
                        quinoa.getActions().talk(x, y);
                        quinoa.getActions().showDialog();
                    }
                    break;

                    case ItemVerbType.LOOK:
                    if(isPlayer(monster))
                    {
                        quinoa.getActions().look(x, y);
                        quinoa.getActions().showDialog();
                    }
                    break;

                    case ItemVerbType.TRADE:
                    if(isPlayer(monster))
                    {
                        quinoa.getActions().trade(x, y);
                    }
                    break;

                    case ItemVerbType.SLEEP:
                    Item terrainItem = region.getItem(x, y);

                    if(terrainItem != null)
                    {
                        foreach(ItemAttribute ia in terrainItem.attributes)
                        {
                            if(ia.type == ItemAttributeType.SLEEPS_FOR)
                            {
                                int hours = Int32.Parse(ia.parameter.Split(new char[]{' '})[0]);
                                monster.setPosition(x, y);
                                monster.sleeping = Quinoa.TICKS_PER_SECOND * 60 * 60 * hours;
                                break;
                            }
                        }
                    }

                    if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainParameter.HAS_BED))
                    {
                        int hours = 4;
                        monster.setPosition(x, y);
                        monster.sleeping = Quinoa.TICKS_PER_SECOND * 60 * 60 * hours;
                    }
                    else
                    {
                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("You can't sleep there.");
                        }
                    }
                    break;
                }
            }
        }


        public static void activate(Monster monster, int x, int y, Region region, Quinoa quinoa)
        {
            if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainParameter.HAS_DOOR))
            {
                DoorCode doorCode = (DoorCode)Enum.Parse(typeof(DoorCode), (TerrainManager.getParameter(region.getTerrain(x, y), TerrainParameter.HAS_DOOR)));
                if(doorCode == DoorCode.CLOSED)
                {
                    region.getTerrain(x, y).getParameters()[TerrainParameter.HAS_DOOR] = nameof(DoorCode.OPEN);

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The door opens.");
                    }
                }
                else if(doorCode == DoorCode.OPEN)
                {
                    region.getTerrain(x, y).getParameters()[TerrainParameter.HAS_DOOR] = nameof(DoorCode.CLOSED);

                    if (isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The door closes.");
                    }
                }
            }
            else if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainParameter.HAS_FLOODGATE))
            {
                DoorCode doorCode = (DoorCode)Enum.Parse(typeof(DoorCode), (TerrainManager.getParameter(region.getTerrain(x, y), TerrainParameter.HAS_FLOODGATE)));
                if(doorCode == DoorCode.CLOSED)
                {
                    region.getTerrain(x, y).getParameters().Add(TerrainParameter.HAS_FLOODGATE, DoorCode.OPEN.ToString());

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The floodgate opens.");
                    }
                }
                else if(doorCode == DoorCode.OPEN)
                {
                    region.getTerrain(x, y).getParameters().Add(TerrainParameter.HAS_FLOODGATE, DoorCode.CLOSED.ToString());

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The floodgate closes.");
                    }
                }
            }
            else
            {
                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("Nothing to do there.");
                }
            }
        }


        public static bool place(Monster monster, Item item, int x, int y, Region region, Quinoa quinoa)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            Terrain terrain = region.getTerrain(x,y);
            switch(item.itemClass)
            {
                case ItemClassType.FLOODGATE:
                if(terrain.getCode() == TerrainCode.STREAM_BED)
                {
                    terrain.getParameters().Add(TerrainParameter.HAS_FLOODGATE, DoorCode.CLOSED.ToString());
                    return true;
                }
                else
                {
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("That must be placed in a stream bed.");
                    }
                    return false;
                }

                case ItemClassType.BONES:
                if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
                {
                    GraveCode gc = (GraveCode)Enum.Parse(typeof(GraveCode), TerrainManager.getParameter(terrain, TerrainParameter.HAS_GRAVE));
                    if(gc == GraveCode.BROKEN)
                    {
                        terrain.getParameters().Add(TerrainParameter.HAS_GRAVE, GraveCode.SPECIAL.ToString());
                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("You restore the bones to the grave.");
                        }
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;


                case ItemClassType.APPLE:
                if(rng.RandomDouble() < TerrainManager.APPLE_TREE_FROM_APPLE_CHANCE
                && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE)
                && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN)
                && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR)
                && terrain.getCode() == TerrainCode.GRASS)
                {
                    terrain.getParameters().Add(TerrainParameter.HAS_TREE, TreeCode.APPLE_TREE.ToString());

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("An apple tree sprouts!");
                    }
                }
                else
                {
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The apple rots...");
                    }
                }
                return true;


                default:
                Item newItem = (Item)item.CopyObject();
                newItem.itemState = ItemState.GROUND;
                newItem.stackSize = 1;
                newItem.refreshID();
                newItem.setPosition(x, y);
                quinoa.getCurrentRegionHeader().getRegion().getItems().Add(newItem);
                return true;
            }
        }


        public static bool use(Monster monster, Item item, int x, int y, Region region, Quinoa quinoa)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            Terrain terrain = region.getTerrain(x,y);
            switch(item.itemClass)
            {
                case ItemClassType.AXE:
                if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainParameter.HAS_TREE))
                {
                    TreeCode tc = (TreeCode)Enum.Parse(typeof(TreeCode), TerrainManager.getParameter(region.getTerrain(x, y), TerrainParameter.HAS_TREE));
                    int damage = 0;
                    if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainParameter.DAMAGE))
                    {
                        damage = Int32.Parse(TerrainManager.getParameter(region.getTerrain(x,y), TerrainParameter.DAMAGE));
                    }
                    damage = damage + 1;

                    if(damage < TerrainManager.maxTreeDamage(tc))
                    {
                        if(region.getTerrain(x, y).getParameters().ContainsKey(TerrainParameter.DAMAGE))
                        {
                            region.getTerrain(x, y).getParameters()[TerrainParameter.DAMAGE] = damage.ToString();
                        }
                        else
                        {
                            region.getTerrain(x, y).getParameters().Add(TerrainParameter.DAMAGE, damage.ToString());
                        }

                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("You chop the " + tc.ToString() + ".");
                        }
                    }
                    else
                    {
                        region.getTerrain(x, y).getParameters().Remove(TerrainParameter.HAS_TREE);
                        region.getTerrain(x, y).getParameters().Remove(TerrainParameter.DAMAGE);
                        Item log = new Item();
                        log.itemClass = ItemClassType.LOG;
                        log.itemState = ItemState.GROUND;
                        log.setPosition(x, y);
                        region.getItems().Add(log);

                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("The tree falls.");
                        }
                    }
                }
                return false;


                case ItemClassType.LANTERN:
                case ItemClassType.TORCH:
                return MonsterActionManager.setFire(monster, item, region, x, y, quinoa);


                case ItemClassType.ASH:
                //check for mushrooms spores
                if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES))
                {
                    MushroomSporeCode msc = (MushroomSporeCode)Enum.Parse(typeof(MushroomSporeCode), TerrainManager.getParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES));

                    if(rng.RandomDouble() > 0.25)
                    {
                        //Grow a mushroom
                        Item newMushroom = new Item();
                        newMushroom.itemClass = TerrainManager.mushroomSporeToItemClass(msc);
                        ItemManager.initialize(newMushroom);
                        newMushroom.setPosition(x, y);
                        region.getItems().Add(newMushroom);

                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("A mushroom sprouts!");
                        }
                    }
                    else
                    {
                        //spread the spore
                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("The spores shimmer..");
                        }
                        quinoa.getActions().spreadSpore(x, y, msc);
                    }

                    //Slight chance to remove the spores
                    if (rng.RandomDouble() < 0.02)
                    {
                        terrain.getParameters().Remove(TerrainParameter.HAS_MUSHROOM_SPORES);
                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("The spores crumble...");
                        }
                    }

                    return true;
                }

                //check for clover
                if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_CLOVER))
                {
                    int cloverCount = Int32.Parse(TerrainManager.getParameter(terrain, TerrainParameter.HAS_CLOVER));
                    if(cloverCount > 0)
                    {
                        terrain.getParameters().Add(TerrainParameter.HAS_CLOVER, "0");

                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("The clover blooms!");
                        }

                        return true;
                    }
                }

                //fertilize existing ground
                switch(terrain.getCode())
                {
                    case TerrainCode.DIRT:
                    terrain.setCode(TerrainCode.FERTILE_LAND);
                    terrain.getParameters().Add(TerrainParameter.GROW_COUNTER, TerrainManager.GRASS_GROW_COUNT+"");
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The ground becomes fertile.");
                    }
                    return true;

                    case TerrainCode.FERTILE_LAND:
                    if(TerrainManager.hasParameter(terrain, TerrainParameter.GROW_COUNTER))
                    {
                        int growCount = Int32.Parse(TerrainManager.getParameter(terrain, TerrainParameter.GROW_COUNTER));
                        growCount = growCount / 2;
                        terrain.getParameters().Add(TerrainParameter.GROW_COUNTER, growCount+"");
                        if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The ground becomes more fertile.");
                    }
                    }
                    return true;

                    default:
                    return false;
                }


                case ItemClassType.BUCKET:
                if(terrain.getWater() > 0)
                {
                    int waterLevel = terrain.getWater();
                    if(waterLevel > TerrainManager.DEEP_WATER)
                    {
                        item.itemClass = ItemClassType.WATER_BUCKET;
                        terrain.setWater(waterLevel - TerrainManager.DEEP_WATER);
                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("You fill the bucket with water.");
                        }
                    }
                    else
                    {
                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("There's not enough water there.");
                        }
                    }
                }
                return false;

                case ItemClassType.WATER_BUCKET:
                int waterValue = ItemManager.WATER_LEVEL_BUCKET_FULL;

                //Turn the item back to a bucket after all uses are gone
                if(ItemManager.decreaseUse(item))
                {
                    item.itemClass = ItemClassType.BUCKET;
                }

                //Check for fire extinguishing
                if(TerrainManager.hasParameter(terrain, TerrainParameter.FIRE))
                {
                    terrain.getParameters().Remove(TerrainParameter.FIRE);
                    waterValue = waterValue / 2;

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The fire is extinguished!");
                    }
                }
            
                //Add water to the ground
                if(TerrainManager.wetable(terrain, x, y, quinoa))
                {
                    terrain.setWater(terrain.getWater() + waterValue);
                
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("You splash water around.");
                    }
                }

                return false;


                case ItemClassType.SHOVEL:
                if(TerrainManager.diggable(terrain))
                {
                    TerrainManager.dig(terrain, x, y, quinoa);

                    if(isPlayer(monster))
                    {
                        if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
                        {
                            quinoa.getMessageManager().addMessage("You open the grave.");
                        }
                        else
                        {
                            quinoa.getMessageManager().addMessage("You dig.");
                        }
                    }
                }
                else
                {
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("You can't dig there.");
                    }
                }
                return false;

                case ItemClassType.PICKAXE:
                if(TerrainManager.mineable(terrain))
                {
                    TerrainManager.mine(terrain, x, y, quinoa);

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("You mine.");
                    }
                }
                else
                {
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("You can't mine there.");
                    }
                }
                return false;


                case ItemClassType.CORN_SEED:
                if(terrain.getCode() == TerrainCode.FERTILE_LAND)
                {
                    if(!TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED))
                    {
                        terrain.getParameters().Add(TerrainParameter.HAS_SEED, SeedType.CORN.ToString());
                        terrain.getParameters().Add(TerrainParameter.GROW_COUNTER,TerrainManager.CORN_GROW_COUNT+"");

                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("You sow the corn seed.");
                        }
                    }
                }
                else
                {
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("Plant seeds in fertile ground.");
                    }
                }
                return true;

                case ItemClassType.PUMPKIN_SEED:
                if(terrain.getCode() == TerrainCode.FERTILE_LAND)
                {
                    if(!TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SEED))
                    {
                        terrain.getParameters().Add(TerrainParameter.HAS_SEED,SeedType.PUMPKIN.ToString());
                        terrain.getParameters().Add(TerrainParameter.GROW_COUNTER,TerrainManager.PUMPKIN_GROW_COUNT+"");

                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("Sow the pumpkin seed.");
                            quinoa.getMessageManager().addMessage("A creeping vine appears.");
                        }
                    }
                }
                else
                {
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("Plant seeds in fertile ground.");
                    }
                }
                return true;


                case ItemClassType.MOP:
                if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS))
                {
                    terrain.getParameters().Remove(TerrainParameter.HAS_MOSS);
                }
                if(terrain.getWater() > 0)
                {
                    terrain.setWater(0);
                }
                if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES))
                {
                    terrain.getParameters().Remove(TerrainParameter.HAS_MUSHROOM_SPORES);
                }
                return false;

                default:
                return false;
            }
        }


        public static bool setFire(Monster monster, Item item, Region region, int x, int y, Quinoa quinoa)
        {
            if(TerrainManager.flammable(region.getTerrain(x,y), x, y, quinoa))
                {
                    if (region.getTerrain(x, y).getParameters().ContainsKey(TerrainParameter.FIRE))
                    {
                        region.getTerrain(x, y).getParameters()[TerrainParameter.FIRE] = TerrainManager.BASE_FLAME_RATE.ToString();
                    }
                    else
                    {
                        region.getTerrain(x, y).getParameters().Add(TerrainParameter.FIRE, TerrainManager.BASE_FLAME_RATE.ToString());
                    }

                    if(ItemManager.decreaseUse(item))
                    {
                        item.RemoveObject();
                    }

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("Flames burst forth!");
                    }
                }
                else
                {
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("That won't catch on fire.");
                    }
                }
                return false;
        }


        public static void executeAttackCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
        {
            Monster monsterToAttack = quinoa.getMonsterByID(command.getParameter("monsterID"));
            if(monsterToAttack != null)
            {
                if(quinoa.monsterIsAdjacent(monster.x, monster.y, monsterToAttack.ID))
                {
                    //Do a combat attack
                    MonsterActionManager.combat(monster, monsterToAttack, quinoa);

                    //Check for removal if not player
                    //Also check for kill
                    if(!monsterToAttack.ID.Equals(MonsterActionManager.PLAYER_ID) && !(monsterToAttack.stats.getCurrentHealth() > 1))
                    {
                        monsterToAttack.RemoveObject();
                        monsterKilled(monster, monsterToAttack, quinoa);
                    }

                }
                else
                {
                    //Attack misses automatically
                    if(monster.ID.Equals(MonsterActionManager.PLAYER_ID))
                    {
                        quinoa.getMessageManager().addMessage("You miss the " + monsterToAttack.monsterCode.ToString() + ".");
                    }
                }
            }
            else
            {
                //Monster cannot be found
                if(monster.ID.Equals(MonsterActionManager.PLAYER_ID))
                {
                    quinoa.getMessageManager().addMessage("You swing at nothing.");
                }
            }
        }

        public static void executePickupCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
        {
            bool pickedUpAtLeastOneItem = false;
            Region region = quinoa.getCurrentRegionHeader().getRegion();
            foreach(Item tempItem in region.getItems())
            {
                if(tempItem.x == monster.x
                && tempItem.y == monster.y
                && !tempItem.ShouldBeRemoved)
                {
                    if(monster.inventory.stackItem(tempItem))
                    {
                        //Item stacked on current item
                        tempItem.RemoveObject();
                        pickedUpAtLeastOneItem = true;
                    
                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("Got " + tempItem.itemClass.ToString() + ".");
                        }
                    }
                    else
                    {
                        if(!monster.inventory.Full)
                        {
                            monster.inventory.addItem((Item)tempItem.CopyObject());
                            tempItem.RemoveObject();
                            pickedUpAtLeastOneItem = true;
                        
                            if(isPlayer(monster))
                            {
                                quinoa.getMessageManager().addMessage("Got " + tempItem.itemClass.ToString() + ".");
                            }
                        }
                        else
                        {
                            if(isPlayer(monster))
                            {
                                quinoa.getMessageManager().addMessage("Can't carry any more.");
                                pickedUpAtLeastOneItem = true;
                            }
                        }
                    }
                }
            }

            if(!pickedUpAtLeastOneItem)
            {
                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("Nothing to pick up there.");
                }
            }
        }

        public static void executeDropCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
        {
            String itemID = command.getParameter("itemID");
            Item itemToDrop = monster.inventory.getItem(itemID);
            if(itemToDrop != null)
            {
                Item newItem = (Item)itemToDrop.CopyObject();
                newItem.itemState = ItemState.GROUND;
                newItem.setPosition(monster.x, monster.y);
                quinoa.getCurrentRegionHeader().getRegion().getItems().Add(newItem);
                monster.inventory.removeItem(itemID);
                monster.inventory.removedExpiredItems();
            
                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("Dropped " + itemToDrop.itemClass + ".");
                }
            }
        }

        public static void executeEquipCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
        {
            String itemID = command.getParameter("itemID");
            Item itemToEquip = monster.inventory.getItem(itemID);
            if(itemToEquip != null)
            {
                //check for a free slot
                MonsterItemSlotType freeSlot = ItemManager.getFreeItemSlot(monster, itemToEquip);
                if(freeSlot != MonsterItemSlotType.NULL)
                {
                    Item newItem = (Item)itemToEquip.CopyObject();
                    monster.inventory.removeItem(itemID);
                    monster.inventory.equipItem(newItem, freeSlot);
                
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("Equiped " + newItem.itemClass.ToString() + ".");
                    }
                }
                else
                {
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("No space to equip that.");
                    }
                }
            }
        }

        public static void executeUnequipCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
        {
            String itemID = command.getParameter("itemID");
            Item itemToEquip = monster.inventory.getItem(itemID);

            //Find the slot that the item is equiped in
            MonsterItemSlotType equipSlot = MonsterItemSlotType.NULL;
            foreach (MonsterItemSlotType tempSlot in Enum.GetValues(typeof(MonsterItemSlotType)).Cast<MonsterItemSlotType>())
            {
                Item testItem = monster.inventory.getItem(tempSlot);
                if(testItem != null && testItem.ID.Equals(itemID))
                {
                    equipSlot = tempSlot;
                }
            }

            if(equipSlot != MonsterItemSlotType.NULL)
            {
                if(!monster.inventory.Full)
                {
                    Item newItem = (Item)itemToEquip.CopyObject();
                    monster.inventory.equipItem(null, equipSlot);
                    if(!monster.inventory.stackItem(newItem))
                    {
                        monster.inventory.addItem(newItem);
                    }

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("Unequiped " + newItem.itemClass.ToString() + ".");
                    }
                }
                else
                {
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("No space to unequip that.");
                    }
                }
            }
        }

        public static bool isPlayer(Monster monster)
        {
            return (monster.ID.Equals(MonsterActionManager.PLAYER_ID));
        }


        //Does this role trade?
        public static bool tradingRole(MonsterRoleType role)
        {
            switch(role)
            {
                case MonsterRoleType.NULL:
                case MonsterRoleType.BROTHER:
                return false;

                case MonsterRoleType.BANKER:
                case MonsterRoleType.CHEF:
                case MonsterRoleType.HANDYMAN:
                return true;
            }
            return false;
        }

        //Update role specific features of a monster
        public static void updateRole(Monster monster, Quinoa quinoa)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();

            int stockSize = 0;
            int hour = quinoa.getHour();
            switch(monster.role)
            {
                case MonsterRoleType.NULL:
                break;

                case MonsterRoleType.BROTHER:
                //don't let them starve
                if(monster.stats.getCurrentHunger() > MonsterStats.MAX_HUNGER / 2)
                {
                    monster.stats.setCurrentHunger(0);
                }
                break;

                case MonsterRoleType.BANKER:
                if(monster.inventory.wealth < (monster.inventory.maxWealth / 2))
                {
                    monster.inventory.wealth = (monster.inventory.maxWealth / 2);
                }

                //don't let them starve
                if(monster.stats.getCurrentHunger() > MonsterStats.MAX_HUNGER / 2)
                {
                    monster.stats.setCurrentHunger(0);
                }
                break;

                case MonsterRoleType.CHEF:
                //Have food for sale at certain times of day
                if(hour == 5 || hour == 11 || hour == 17)
                {
                    monster.inventory.getItems().Clear();
                    stockSize = 15 + rng.RandomInteger(5);
                    for(int i=0; i < stockSize; i++)
                    {
                        Item newItem = ItemManager.getRandomItem(ItemCategory.FOOD, true);
                        newItem.itemState = ItemState.INVENTORY;
                        if(!monster.inventory.Full)
                        {
                            if(!monster.inventory.stackItem(newItem))
                            {
                                monster.inventory.addItem(newItem);
                            }
                        }
                    }
                }
                else if(hour >= 19)
                {
                    monster.inventory.getItems().Clear();
                }

                //don't let them starve
                if(monster.stats.getCurrentHunger() > MonsterStats.MAX_HUNGER / 2)
                {
                    monster.stats.setCurrentHunger(0);
                }

                //give spending money
                if(monster.inventory.wealth < 1000)
                {
                    monster.inventory.wealth = 1200;
                }
                break;

                case MonsterRoleType.HANDYMAN:
                if(monster.inventory.getItems().Count == 0 || hour == 11)
                {
                    monster.inventory.getItems().Clear();
                
                    stockSize = 6 + rng.RandomInteger(5);
                    for(int i=0; i < stockSize; i++)
                    {
                        Item newItem = ItemManager.getRandomItem(ItemCategory.TOOL, true);
                        newItem.stackSize = 1 + rng.RandomInteger(newItem.maxStackSize - 1);
                        newItem.itemState = ItemState.INVENTORY;
                        if(!monster.inventory.Full)
                        {
                            monster.inventory.addItem(newItem);
                        }
                    }
                    stockSize = 1 + rng.RandomInteger(3);
                    for(int i=0; i < stockSize; i++)
                    {
                        Item newItem = ItemManager.getRandomItem(ItemCategory.LIGHT, true);
                        newItem.stackSize = 1 + rng.RandomInteger(newItem.maxStackSize - 1);
                        newItem.itemState = ItemState.INVENTORY;
                        if(!monster.inventory.Full)
                        {
                            monster.inventory.addItem(newItem);
                        }
                    }
                    stockSize = 3 + rng.RandomInteger(2);
                    for(int i=0; i < stockSize; i++)
                    {
                        Item newItem = ItemManager.getRandomItem(ItemCategory.MATERIAL, true);
                        newItem.stackSize = 1 + rng.RandomInteger(newItem.maxStackSize - 1);
                        newItem.itemState = ItemState.INVENTORY;
                        if(!monster.inventory.Full)
                        {
                            monster.inventory.addItem(newItem);
                        }
                    }
                }

                //don't let them starve
                if(monster.stats.getCurrentHunger() > MonsterStats.MAX_HUNGER / 2)
                {
                    monster.stats.setCurrentHunger(0);
                }

                //give spending money
                if(monster.inventory.wealth < 1000)
                {
                    monster.inventory.wealth = 1200;
                }
                break;

            }
        }
    }
}
