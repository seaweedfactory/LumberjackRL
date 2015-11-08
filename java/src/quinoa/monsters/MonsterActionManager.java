package quinoa.monsters;

import java.text.DecimalFormat;
import java.text.NumberFormat;
import quinoa.Quinoa;
import quinoa.items.Item;
import quinoa.items.ItemAttribute;
import quinoa.items.ItemAttribute.ItemAttributeType;
import quinoa.items.ItemManager;
import quinoa.items.ItemManager.ItemCategory;
import quinoa.items.ItemManager.ItemState;
import quinoa.items.ItemManager.ItemVerb;
import quinoa.monsters.Monster.Direction;
import quinoa.monsters.MonsterInventory.ItemSlot;
import quinoa.region.LightMap;
import quinoa.region.Region;
import quinoa.region.Terrain;
import quinoa.region.TerrainManager;
import quinoa.region.TerrainManager.DoorCode;
import quinoa.region.TerrainManager.GraveCode;
import quinoa.region.TerrainManager.MushroomSporeCode;
import quinoa.region.TerrainManager.SeedType;
import quinoa.region.TerrainManager.TerrainParameter;
import quinoa.region.TerrainManager.TreeCode;

public class MonsterActionManager
{
    public static enum MonsterCode {HUMAN, SPONGE, GHOST, SLIME, SMALL_SLIME, TINY_SLIME, DEER, PIG};
    public static enum MonsterCommandCode {NULL, MOVE, ATTACK, PICKUP, DROP, EQUIP, UNEQUIP, ITEM_VERB};
    public static enum MonsterRole {NULL, BANKER, CHEF, HANDYMAN, BROTHER};
    public static final String PLAYER_ID="player";
    public static int[] xpLevels = {1,50,100,200,400,800,1600,3200,6400,12800, //double
                                    22400, 39200, 68600, 120050, 210087, 367653, 643392, 1125937, 1970390, 3448184, //times 1.75
                                    5172276, 7758414, 11637621, 17456432, 26184648, 39276973, 58915459, 88373189, 132559784, 198839676, //times 1.5
                                    248549595, 310686993, 388358742, 485448428, 606810535, 758513168, 948141460, 1185176826, 1481471032, 1851838791}; //times 1.25

    public static void initialize(Monster monster)
    {
        switch(monster.getMonsterCode())
        {
            case HUMAN:
                monster.getStats().setHealth(3);
                monster.getStats().setMight(2);
                monster.getStats().setEndurance(2);
                monster.getStats().setAgility(2);
                monster.getInventory().setDimensions(10, 5);
                monster.getStats().setLevel(1);
                monster.getStats().setExperienceGiven(10);
                monster.getStats().setHungerRate(MonsterStats.DEFAULT_HUNGER_RATE);
                monster.getStats().setRadiance(0.25);
                break;

            case GHOST:
                monster.getStats().setHealth(1);
                monster.getStats().setMight(1);
                monster.getStats().setEndurance(1);
                monster.getStats().setAgility(1);
                monster.getInventory().setDimensions(2, 4);
                monster.getStats().setLevel(1);
                monster.getStats().setExperienceGiven(5);
                monster.getStats().setHungerRate(0);
                monster.getStats().setRadiance(0.5);
                break;

            case SPONGE:
                monster.getStats().setHealth(1);
                monster.getStats().setMight(1);
                monster.getStats().setEndurance(1);
                monster.getStats().setAgility(1);
                monster.getInventory().setDimensions(8, 4);
                monster.getStats().setLevel(1);
                monster.getStats().setExperienceGiven(4);
                monster.getStats().setHungerRate(0);
                monster.getStats().setRadiance(0.1);
                break;

            case SLIME:
                monster.getStats().setHealth(4);
                monster.getStats().setMight(1);
                monster.getStats().setEndurance(1);
                monster.getStats().setAgility(1);
                monster.getInventory().setDimensions(2, 4);
                monster.getStats().setLevel(3);
                monster.getStats().setExperienceGiven(8);
                monster.getStats().setHungerRate(0);
                monster.getStats().setRadiance(LightMap.MAX_LIGHT);
                break;

            case SMALL_SLIME:
                monster.getStats().setHealth(2);
                monster.getStats().setMight(1);
                monster.getStats().setEndurance(1);
                monster.getStats().setAgility(1);
                monster.getInventory().setDimensions(2, 4);
                monster.getStats().setLevel(2);
                monster.getStats().setExperienceGiven(4);
                monster.getStats().setHungerRate(0);
                monster.getStats().setRadiance(LightMap.MAX_LIGHT * 0.75);
                break;

            case TINY_SLIME:
                monster.getStats().setHealth(1);
                monster.getStats().setMight(1);
                monster.getStats().setEndurance(1);
                monster.getStats().setAgility(1);
                monster.getInventory().setDimensions(2, 4);
                monster.getStats().setLevel(1);
                monster.getStats().setExperienceGiven(2);
                monster.getStats().setHungerRate(0);
                monster.getStats().setRadiance(LightMap.MAX_LIGHT * 0.5);
                break;
            

            case DEER:
                monster.getStats().setHealth(10);
                monster.getStats().setMight(1);
                monster.getStats().setEndurance(1);
                monster.getStats().setAgility(5);
                monster.getInventory().setDimensions(1, 4);
                monster.getStats().setLevel(1);
                monster.getStats().setExperienceGiven(4);
                monster.getStats().setHungerRate(MonsterStats.DEFAULT_HUNGER_RATE);
                monster.getStats().setRadiance(0.0);
                break;

            case PIG:
                monster.getStats().setHealth(8);
                monster.getStats().setMight(2);
                monster.getStats().setEndurance(1);
                monster.getStats().setAgility(2);
                monster.getInventory().setDimensions(1, 4);
                monster.getStats().setLevel(1);
                monster.getStats().setExperienceGiven(4);
                monster.getStats().setHungerRate(MonsterStats.DEFAULT_HUNGER_RATE);
                monster.getStats().setRadiance(0.0);
                break;
        }

        monster.getStats().setCurrentHealth(monster.getStats().getMaxHP());
        MonsterActionManager.addDrops(monster);
    }

    public static double getSpawnRate(MonsterCode monsterCode)
    {
        switch(monsterCode)
        {
            case GHOST:
            return 0.25;

            case SPONGE:
            return 0.005;

            case SLIME:
            return 0.005;

            case TINY_SLIME:
            return 0.005;

            case DEER:
            return 0.25;

            case PIG:
            return 0.15;

            default:
            return 1.0;
        }
    }

    public static int getMaxMonsterPerRegion(MonsterCode monsterCode)
    {
        switch(monsterCode)
        {
            case GHOST:
            return 6;

            case SPONGE:
            return 3;

            case SLIME:
            return 3;

            case TINY_SLIME:
            return 8;

            case DEER:
            return 6;

            case PIG:
            return 4;

            default:
            return 1;
        }
    }


    public static void addDrops(Monster monster)
    {
        Item tempItem = null;
        switch(monster.getMonsterCode())
        {
            case SLIME:
            case GHOST:
            case SPONGE:
            if(Math.random() > 0.75)
            {
                tempItem = new Item();
                tempItem.setItemClass(ItemManager.ItemClass.COINS);
                tempItem.setStackSize(1 + (int)(Math.random() * 15));
                monster.getInventory().addItem(tempItem);
            }
            break;

            case PIG:
            tempItem = new Item();
            tempItem.setItemClass(ItemManager.ItemClass.BACON);
            tempItem.setStackSize(1 + (int)(Math.random() * (tempItem.getMaxStackSize() - 1)));
            monster.getInventory().addItem(tempItem);

            tempItem = new Item();
            tempItem.setItemClass(ItemManager.ItemClass.BONES);
            tempItem.setStackSize(1);
            monster.getInventory().addItem(tempItem);
            break;

            case DEER:
            tempItem = new Item();
            tempItem.setItemClass(ItemManager.ItemClass.BONES);
            tempItem.setStackSize(1);
            monster.getInventory().addItem(tempItem);
            break;

            default:
            //do nothing
            break;
        }
    }

    public static void sellItemToMonster(Monster seller, Monster buyer, Item item, Quinoa quinoa)
    {
        //Check if the buyer has enough money
        if(buyer.getInventory().getWealth() >= item.getWorth())
        {
            //Do the sale
            Item newItem = (Item)item.copy();
            newItem.setStackSize(1);

            if(!buyer.getInventory().stackItem(newItem))
            {
                if(!buyer.getInventory().isFull())
                {
                    //Successfully sold as new item
                    buyer.getInventory().addItem(newItem);
                    item.setStackSize(item.getStackSize()-1);
                    if(item.getStackSize() == 0)
                    {
                        item.remove();
                        seller.getInventory().removedExpiredItems();
                    }
                    seller.getInventory().setWealth(seller.getInventory().getWealth() + newItem.getWorth());
                    buyer.getInventory().setWealth(buyer.getInventory().getWealth() - newItem.getWorth());
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
                item.setStackSize(item.getStackSize()-1);
                if(item.getStackSize() == 0)
                {
                    item.remove();
                    seller.getInventory().removedExpiredItems();
                }
                seller.getInventory().setWealth(seller.getInventory().getWealth() + newItem.getWorth());
                buyer.getInventory().setWealth(buyer.getInventory().getWealth() - newItem.getWorth());
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
        switch(monster.getMonsterCode())
        {
            case TINY_SLIME:
            //kills small slimes
            monster.getStats().setCurrentHealth(0);
            break;

            case SPONGE:
            //heals sponges
            if(Math.random() > 0.75)
            {
                monster.getStats().setCurrentHealth(monster.getStats().getCurrentHealth() + 0.001);
            }
            break;

            default:
            //Wake up if splashed with water
            monster.setSleeping(0);
            break;
        }
    }

    public static void fireDamage(Monster monster, double points)
    {
        //get speed divider based on items
        double firePercent=0;
        for(ItemSlot tempSlot : MonsterInventory.ItemSlot.values())
        {
            Item tempItem = monster.getInventory().getItem(tempSlot);
            if(tempItem != null)
            {
                for(ItemAttribute attribute : tempItem.getAttributes())
                {
                    if(attribute.getType() == ItemAttributeType.PROTECT_AGAINST_FIRE)
                    {
                        firePercent = firePercent + Double.parseDouble(attribute.getParameter().replace("%", ""));
                    }
                }
            }
        }

        double damage = points * ((100 - firePercent) / 100.0);
        monster.getStats().setCurrentHealth(monster.getStats().getCurrentHealth() - damage);
        
        //wake up on damage
        if(damage > 0)
        {
            monster.setSleeping(0);
        }
    }


    public static void cashOutInventory(Monster monster)
    {
        if(monster.getInventory().isFull())
        {
            for(Item tempItem : monster.getInventory().getItems())
            {
                monster.getInventory().setWealth(monster.getInventory().getWealth() + tempItem.getWorth());
                tempItem.remove();
            }
            monster.getInventory().removedExpiredItems();
        }
    }

    public static void leaveWaterTrail(Monster monster, Quinoa quinoa, double strength)
    {
        Terrain terrain = quinoa.getCurrentRegionHeader().getRegion().getTerrain(monster.getX(), monster.getY());

        //leave water trails
        if(Math.random() > (1.0 - strength))
        {
            if(TerrainManager.wetable(terrain, monster.getX(), monster.getY(), quinoa))
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
        String playerFeedback="The " + victim.getMonsterCode().name() + " is gone.";

        //Give experience
        MonsterActionManager.giveExperience(killer, victim.getStats().getExperienceGiven(), quinoa);

        //Drop items
        for(Item tempItem: victim.getInventory().getItems())
        {
            Item newItem = (Item)tempItem.copy();
            newItem.setPosition(victim.getX(), victim.getY());
            newItem.setItemState(ItemState.GROUND);
            quinoa.getCurrentRegionHeader().getRegion().getItems().add(newItem);
        }

        //Any monster specific items
        switch(victim.getMonsterCode())
        {
            case HUMAN:
            break;

            case GHOST:
            playerFeedback = "The " + victim.getMonsterCode().name() + " fades away...";
            break;

            case SPONGE:
            playerFeedback = "The " + victim.getMonsterCode().name() + " crumbles.";
            break;

            case SLIME:
            for(int i=0; i < 2; i++)
            {
                Monster newMon = new Monster();
                newMon.setMonsterCode(MonsterCode.SMALL_SLIME);
                MonsterActionManager.initialize(newMon);
                newMon.setPosition(victim.getX() + (int)(Math.random() * 5) - 2, victim.getY() + (int)(Math.random() * 5) - 2);
                quinoa.getActions().addMonster(newMon);
            }
            playerFeedback = "The " + victim.getMonsterCode().name() + " splits!";
            break;

            case SMALL_SLIME:
            for(int i=0; i < 2; i++)
            {
                Monster newMon = new Monster();
                newMon.setMonsterCode(MonsterCode.TINY_SLIME);
                MonsterActionManager.initialize(newMon);
                newMon.setPosition(victim.getX() + (int)(Math.random() * 7) - 4, victim.getY() + (int)(Math.random() * 7) - 4);
                quinoa.getActions().addMonster(newMon);
            }
            playerFeedback = "The " + victim.getMonsterCode().name() + " splits!";
            break;

            case TINY_SLIME:
            playerFeedback = "The " + victim.getMonsterCode().name() + " evaporates!";
            break;

            case DEER:
            case PIG:
            playerFeedback = "The " + victim.getMonsterCode().name() + " collapses.";
            break;

            default:
            break;
        }

        //Inform plyer
        if(killer.getID().equals(MonsterActionManager.PLAYER_ID))
        {
            quinoa.getMessageManager().addMessage(playerFeedback);
        }

    }

    public static int getNextXPLevel(Monster monster)
    {
        if((monster.getStats().getLevel()-1) >= xpLevels.length)
        {
            return xpLevels[xpLevels.length-1] + 1;
        }
        else
        {
            return xpLevels[monster.getStats().getLevel()];
        }
    }

    public static double getDefenseRating(Monster monster)
    {
        //get basic stat defense
        double baseDef = monster.getStats().getEndurance() * MonsterStats.ENDURANCE_PER_DEFENSE;

        //get item stats defense
        double itemDef = 0.0;
        for(ItemSlot tempSlot : MonsterInventory.ItemSlot.values())
        {
            Item tempItem = monster.getInventory().getItem(tempSlot);
            if(tempItem != null)
            {
                for(ItemAttribute attribute : tempItem.getAttributes())
                {
                    if(attribute.getType() == ItemAttributeType.BONUS_TO_DEFENSE)
                    {
                        itemDef = itemDef + Double.parseDouble(attribute.getParameter());
                    }
                }
            }
        }
        
        return baseDef + itemDef;
    }

    public static double getAttackRating(Monster monster)
    {
        //get basic stat attack
        double baseAtk = monster.getStats().getMight() * MonsterStats.MIGHT_PER_ATTACK;

        //get item stats attack
        double itemAtk = 0.0;
        for(ItemSlot tempSlot : MonsterInventory.ItemSlot.values())
        {
            Item tempItem = monster.getInventory().getItem(tempSlot);
            if(tempItem != null)
            {
                for(ItemAttribute attribute : tempItem.getAttributes())
                {
                    if(attribute.getType() == ItemAttributeType.BONUS_TO_ATTACK)
                    {
                        itemAtk = itemAtk + Double.parseDouble(attribute.getParameter());
                    }
                }
            }
        }

        return baseAtk + itemAtk;
    }

    public static int getSpeed(Monster monster)
    {
        //get basic stat speed
        double baseSpd = monster.getStats().getSpeed();

        //get speed divider based on items
        double spdPercent=0;
        for(ItemSlot tempSlot : MonsterInventory.ItemSlot.values())
        {
            Item tempItem = monster.getInventory().getItem(tempSlot);
            if(tempItem != null)
            {
                for(ItemAttribute attribute : tempItem.getAttributes())
                {
                    if(attribute.getType() == ItemAttributeType.INCREASE_SPEED)
                    {
                        spdPercent = spdPercent + Double.parseDouble(attribute.getParameter().replace("%", ""));
                    }
                    else if(attribute.getType() == ItemAttributeType.DECREASE_SPEED)
                    {
                        spdPercent = spdPercent - Double.parseDouble(attribute.getParameter().replace("%", ""));
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
        double baseHR = monster.getStats().getHungerRate();

        //get speed divider based on items
        double hrPercent=0;
        for(ItemSlot tempSlot : MonsterInventory.ItemSlot.values())
        {
            Item tempItem = monster.getInventory().getItem(tempSlot);
            if(tempItem != null)
            {
                for(ItemAttribute attribute : tempItem.getAttributes())
                {
                    if(attribute.getType() == ItemAttributeType.QUICKEN_HUNGER)
                    {
                        hrPercent = hrPercent - Double.parseDouble(attribute.getParameter().replace("%", ""));
                    }
                    else if(attribute.getType() == ItemAttributeType.SLOW_HUNGER)
                    {
                        hrPercent = hrPercent + Double.parseDouble(attribute.getParameter().replace("%", ""));
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
        //Wake on being hit
        defender.setSleeping(0);

        //Hit if rnd() < ATK / (ATK + DEF).
        double atk = MonsterActionManager.getAttackRating(attacker);
        double def = MonsterActionManager.getDefenseRating(defender);
        if(Math.random() < atk / (atk + def))
        {

            double damage = 0.0;
            if(atk > def)
            {
                //damage = rnd() * (ATK - DEF)
                damage = ((Math.random() / 2) + 0.5) * (atk - def);
                
            }
            else
            {
                //damage = rnd()
                damage = Math.random();
            }

            defender.getStats().setCurrentHealth(defender.getStats().getCurrentHealth() - damage);

            if(MonsterActionManager.isPlayer(attacker))
            {
                if(quinoa.DEBUG_MODE)
                {
                    NumberFormat formatter = new DecimalFormat("#0.0");
                    quinoa.getMessageManager().addMessage("You hit the " + defender.getMonsterCode().name() + " for " + formatter.format(damage) + ".");
                }
                else
                {
                    quinoa.getMessageManager().addMessage("You hit the " + defender.getMonsterCode().name() + ".");
                }
            }
            else if(MonsterActionManager.isPlayer(defender))
            {
                if(quinoa.DEBUG_MODE)
                {
                    NumberFormat formatter = new DecimalFormat("#0.0");
                    quinoa.getMessageManager().addMessage("The " + attacker.getMonsterCode().name() + " hits for " + formatter.format(damage) + ".");
                }
                else
                {
                    quinoa.getMessageManager().addMessage("The " + attacker.getMonsterCode().name() + " hits you!");
                }
            }

        }
        else
        {
            if(MonsterActionManager.isPlayer(attacker))
            {
                quinoa.getMessageManager().addMessage("You missed the " + defender.getMonsterCode().name() + ".");
            }
            else if(MonsterActionManager.isPlayer(defender))
            {
                quinoa.getMessageManager().addMessage("The " + attacker.getMonsterCode().name() + " missed you.");
            }
        }
    }

    public static void giveExperience(Monster monster, int amount, Quinoa quinoa)
    {
        monster.getStats().setExperience(monster.getStats().getExperience() + amount);

        //check for level up
        int startLevel = monster.getStats().getLevel();
        int endLevel = monster.getStats().getLevel();
        for(int i=0; i < xpLevels.length; i++)
        {
            if(monster.getStats().getExperience() >= xpLevels[i])
            {
                endLevel = (i + 1);
            }
        }

        if(endLevel > startLevel && startLevel != MonsterStats.MAX_LEVEL)
        {
            monster.getStats().setLevel(endLevel);
            monster.getStats().setAvailableStatPoints(monster.getStats().getAvailableStatPoints() + ((endLevel - startLevel) * MonsterStats.STAT_POINTS_PER_LEVEL ));
            monster.getStats().setCurrentHealth(monster.getStats().getMaxHP());

            if(isPlayer(monster))
            {
                quinoa.getMessageManager().addMessage("Level up!");
                quinoa.getUI().getScreen().displayDialog();
            }
        }

    }

    public static void executeCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
    {
        switch(command.getCommandCode())
        {
            case NULL: break; //do nothing
            case MOVE: executeMoveCommand(monster, command, quinoa); break;
            case ATTACK: executeAttackCommand(monster, command, quinoa); break;
            case PICKUP: executePickupCommand(monster, command, quinoa); break;
            case DROP: executeDropCommand(monster, command, quinoa); break;
            case EQUIP: executeEquipCommand(monster, command, quinoa); break;
            case UNEQUIP: executeUnequipCommand(monster, command, quinoa); break;
            case ITEM_VERB: executeItemVerbCommand(monster, command, quinoa); break;
            default: break; //do nothing
        }
        quinoa.getUI().refresh();
    }

    public static void setNullCommand(Monster monster)
    {
        MonsterCommand newCommand = new MonsterCommand();
        newCommand.setCommandCode(MonsterCommandCode.NULL);
        newCommand.setCounter(1);
        monster.setCommand(newCommand);
    }

    public static void setMoveCommand(Monster monster, Direction direction)
    {
        MonsterCommand newCommand = new MonsterCommand();
        newCommand.setCommandCode(MonsterCommandCode.MOVE);
        newCommand.setCounter(MonsterActionManager.getSpeed(monster));
        newCommand.addParameter("direction", direction.name());
        monster.setCommand(newCommand);
    }

    public static void setItemVerbCommand(Monster monster, Item item, ItemVerb verb, int x, int y)
    {
        MonsterCommand newCommand = new MonsterCommand();
        newCommand.setCommandCode(MonsterCommandCode.ITEM_VERB);
        newCommand.setCounter(MonsterActionManager.getSpeed(monster));
        if(item != null)
        {
            newCommand.addParameter("itemID", item.getID());
        }
        else
        {
            newCommand.addParameter("itemID", "");
        }
        newCommand.addParameter("verb", verb.name());
        newCommand.addParameter("x", x+"");
        newCommand.addParameter("y", y+"");
        monster.setCommand(newCommand);
    }

    public static void setAttackCommand(Monster monster, String monsterID)
    {
        MonsterCommand newCommand = new MonsterCommand();
        newCommand.setCommandCode(MonsterCommandCode.ATTACK);
        newCommand.setCounter(MonsterActionManager.getSpeed(monster)/2);
        newCommand.addParameter("monsterID", monsterID);
        monster.setCommand(newCommand);
    }

    public static void setPickupCommand(Monster monster)
    {
        MonsterCommand newCommand = new MonsterCommand();
        newCommand.setCommandCode(MonsterCommandCode.PICKUP);
        newCommand.setCounter(MonsterActionManager.getSpeed(monster)/4);
        monster.setCommand(newCommand);
    }

    public static void setDropCommand(Monster monster, String itemID)
    {
        MonsterCommand newCommand = new MonsterCommand();
        newCommand.setCommandCode(MonsterCommandCode.DROP);
        newCommand.setCounter(MonsterActionManager.getSpeed(monster)/4);
        newCommand.addParameter("itemID", itemID);
        monster.setCommand(newCommand);
    }

    public static void setEquipCommand(Monster monster, String itemID)
    {
        MonsterCommand newCommand = new MonsterCommand();
        newCommand.setCommandCode(MonsterCommandCode.EQUIP);
        newCommand.setCounter(MonsterActionManager.getSpeed(monster)/2);
        newCommand.addParameter("itemID", itemID);
        monster.setCommand(newCommand);
    }

    public static void setUnequipCommand(Monster monster, String itemID)
    {
        MonsterCommand newCommand = new MonsterCommand();
        newCommand.setCommandCode(MonsterCommandCode.UNEQUIP);
        newCommand.setCounter(MonsterActionManager.getSpeed(monster)/2);
        newCommand.addParameter("itemID", itemID);
        monster.setCommand(newCommand);
    }

    public static void executeMoveCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
    {
        Region region = quinoa.getCurrentRegionHeader().getRegion();
        Direction direction = Direction.valueOf(command.getParameter("direction"));
        monster.setFacing(direction);

        int newX = monster.getX();
        int newY = monster.getY();

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

        if(newX >= 0 && newX < region.getWidth()
        && newY >= 0 && newY < region.getHeight())
        {
            if(TerrainManager.allowsMonsterToPass(region.getTerrain(newX, newY), monster) && quinoa.getMonster(newX, newY) == null)
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
        Item item = monster.getInventory().getItem(command.getParameter("itemID"));
        ItemVerb verb = ItemVerb.valueOf(command.getParameter("verb"));
        Region region = quinoa.getCurrentRegionHeader().getRegion();
        int x = Integer.parseInt(command.getParameter("x"));
        int y = Integer.parseInt(command.getParameter("y"));
        boolean removeItem=false;
        if(item != null)
        {
            switch(verb)
            {
                case EAT:
                for(ItemAttribute attribute : item.getAttributes())
                {
                    if(attribute.getType() == ItemAttributeType.NUTRITION)
                    {
                        double nutrition = Double.parseDouble(attribute.getParameter());
                        monster.getStats().setCurrentHunger(monster.getStats().getCurrentHunger() - nutrition);
                        if(monster.getStats().getCurrentHunger() < 0)
                        {
                            monster.getStats().setCurrentHunger(0.0);
                        }

                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("Ate " + item.getItemClass().name() + ".");
                        }
                    }

                    if(attribute.getType() == ItemAttributeType.HEALS)
                    {
                        double points = Double.parseDouble(attribute.getParameter());
                        monster.getStats().setCurrentHealth(monster.getStats().getCurrentHealth() + points);
                        if(monster.getStats().getCurrentHealth() > monster.getStats().getMaxHP())
                        {
                            monster.getStats().setCurrentHealth(monster.getStats().getMaxHP());
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

                case PLACE:
                removeItem = MonsterActionManager.place(monster, item, x, y, region, quinoa);
                break;

                case USE:
                removeItem = use(monster, item, x, y, region, quinoa);
                break;

            }

            if(removeItem)
            {
                if(item.getStackSize() > 1)
                {
                    item.setStackSize(item.getStackSize() - 1);
                }
                else
                {
                    item.remove();
                }
            }
        }
        else
        {
            //Non-item verbs
            switch(verb)
            {
                case ACTIVATE:
                MonsterActionManager.activate(monster, x, y, region, quinoa);
                break;

                case TALK:
                if(isPlayer(monster))
                {
                    quinoa.getActions().talk(x, y);
                    quinoa.getActions().showDialog();
                }
                break;

                case LOOK:
                if(isPlayer(monster))
                {
                    quinoa.getActions().look(x, y);
                    quinoa.getActions().showDialog();
                }
                break;

                case TRADE:
                if(isPlayer(monster))
                {
                    quinoa.getActions().trade(x, y);
                }
                break;

                case SLEEP:
                Item terrainItem = region.getItem(x, y);

                if(terrainItem != null)
                {
                    for(ItemAttribute ia : terrainItem.getAttributes())
                    {
                        if(ia.getType() == ItemAttribute.ItemAttributeType.SLEEPS_FOR)
                        {
                            int hours = Integer.parseInt(ia.getParameter().split(" ")[0]);
                            monster.setPosition(x, y);
                            monster.setSleeping(Quinoa.TICKS_PER_SECOND * 60 * 60 * hours);
                            break;
                        }
                    }
                }

                if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_BED))
                {
                    int hours = 4;
                    monster.setPosition(x, y);
                    monster.setSleeping(Quinoa.TICKS_PER_SECOND * 60 * 60 * hours);
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
        if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_DOOR))
        {
            DoorCode doorCode = DoorCode.valueOf(TerrainManager.getParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_DOOR));
            if(doorCode == DoorCode.CLOSED)
            {
                region.getTerrain(x, y).getParameters().put(TerrainManager.TerrainParameter.HAS_DOOR, DoorCode.OPEN.name());

                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("The door opens.");
                }
            }
            else if(doorCode == DoorCode.OPEN)
            {
                region.getTerrain(x, y).getParameters().put(TerrainManager.TerrainParameter.HAS_DOOR, DoorCode.CLOSED.name());

                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("The door closes.");
                }
            }
        }
        else if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_FLOODGATE))
        {
            DoorCode doorCode = DoorCode.valueOf(TerrainManager.getParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_FLOODGATE));
            if(doorCode == DoorCode.CLOSED)
            {
                region.getTerrain(x, y).getParameters().put(TerrainManager.TerrainParameter.HAS_FLOODGATE, DoorCode.OPEN.name());

                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("The floodgate opens.");
                }
            }
            else if(doorCode == DoorCode.OPEN)
            {
                region.getTerrain(x, y).getParameters().put(TerrainManager.TerrainParameter.HAS_FLOODGATE, DoorCode.CLOSED.name());

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


    public static boolean place(Monster monster, Item item, int x, int y, Region region, Quinoa quinoa)
    {
        Terrain terrain = region.getTerrain(x,y);
        switch(item.getItemClass())
        {
            case FLOODGATE:
            if(terrain.getCode() == TerrainManager.TerrainCode.STREAM_BED)
            {
                terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_FLOODGATE, DoorCode.CLOSED.name());
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

            case BONES:
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_GRAVE))
            {
                GraveCode gc = GraveCode.valueOf(TerrainManager.getParameter(terrain, TerrainParameter.HAS_GRAVE));
                if(gc == GraveCode.BROKEN)
                {
                    terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_GRAVE, GraveCode.SPECIAL.name());
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


            case APPLE:
            if(Math.random() < TerrainManager.APPLE_TREE_FROM_APPLE_CHANCE
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_TREE)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_SIGN)
            && !TerrainManager.hasParameter(terrain, TerrainParameter.HAS_DOOR)
            && terrain.getCode() == TerrainManager.TerrainCode.GRASS)
            {
                terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_TREE, TreeCode.APPLE_TREE.name());

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
            Item newItem = (Item)item.copy();
            newItem.setItemState(ItemState.GROUND);
            newItem.setStackSize(1);
            newItem.refreshID();
            newItem.setPosition(x, y);
            quinoa.getCurrentRegionHeader().getRegion().getItems().add(newItem);
            return true;
        }
    }


    public static boolean use(Monster monster, Item item, int x, int y, Region region, Quinoa quinoa)
    {
        Terrain terrain = region.getTerrain(x,y);
        switch(item.getItemClass())
        {
            case AXE:
            if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_TREE))
            {
                TreeCode tc = TreeCode.valueOf(TerrainManager.getParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.HAS_TREE));
                int damage = 0;
                if(TerrainManager.hasParameter(region.getTerrain(x, y), TerrainManager.TerrainParameter.DAMAGE))
                {
                    damage = Integer.parseInt(TerrainManager.getParameter(region.getTerrain(x,y), TerrainManager.TerrainParameter.DAMAGE));
                }
                damage = damage + 1;

                if(damage < TerrainManager.maxTreeDamage(tc))
                {
                    region.getTerrain(x,y).getParameters().put(TerrainManager.TerrainParameter.DAMAGE, damage + "");

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("You chop the " + tc.name() + ".");
                    }
                }
                else
                {
                    region.getTerrain(x, y).getParameters().remove(TerrainManager.TerrainParameter.HAS_TREE);
                    region.getTerrain(x, y).getParameters().remove(TerrainManager.TerrainParameter.DAMAGE);
                    Item log = new Item();
                    log.setItemClass(ItemManager.ItemClass.LOG);
                    log.setItemState(ItemState.GROUND);
                    log.setPosition(x, y);
                    region.getItems().add(log);

                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The tree falls.");
                    }
                }
            }
            return false;


            case LANTERN:
            case TORCH:
            return MonsterActionManager.setFire(monster, item, region, x, y, quinoa);


            case ASH:
            //check for mushrooms spores
            if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES))
            {
                MushroomSporeCode msc = MushroomSporeCode.valueOf(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES));

                if(Math.random() > 0.25)
                {
                    //Grow a mushroom
                    Item newMushroom = new Item();
                    newMushroom.setItemClass(TerrainManager.mushroomSporeToItemClass(msc));
                    ItemManager.initialize(newMushroom);
                    newMushroom.setPosition(x, y);
                    region.getItems().add(newMushroom);

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
                if(Math.random() < 0.02)
                {
                    terrain.getParameters().remove(TerrainManager.TerrainParameter.HAS_MUSHROOM_SPORES);
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("The spores crumble...");
                    }
                }

                return true;
            }

            //check for clover
            if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_CLOVER))
            {
                int cloverCount = Integer.parseInt(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.HAS_CLOVER));
                if(cloverCount > 0)
                {
                    terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_CLOVER, "0");

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
                case DIRT:
                terrain.setCode(TerrainManager.TerrainCode.FERTILE_LAND);
                terrain.getParameters().put(TerrainManager.TerrainParameter.GROW_COUNTER, TerrainManager.GRASS_GROW_COUNT+"");
                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("The ground becomes fertile.");
                }
                return true;
                    
                case FERTILE_LAND:
                if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.GROW_COUNTER))
                {
                    int growCount = Integer.parseInt(TerrainManager.getParameter(terrain, TerrainManager.TerrainParameter.GROW_COUNTER));
                    growCount = growCount / 2;
                    terrain.getParameters().put(TerrainManager.TerrainParameter.GROW_COUNTER, growCount+"");
                    if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("The ground becomes more fertile.");
                }
                }
                return true;

                default:
                return false;
            }


            case BUCKET:
            if(terrain.getWater() > 0)
            {
                int waterLevel = terrain.getWater();
                if(waterLevel > TerrainManager.DEEP_WATER)
                {
                    item.setItemClass(ItemManager.ItemClass.WATER_BUCKET);
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
            
            case WATER_BUCKET:
            int waterValue = ItemManager.WATER_LEVEL_BUCKET_FULL;

            //Turn the item back to a bucket after all uses are gone
            if(ItemManager.decreaseUse(item))
            {
                item.setItemClass(ItemManager.ItemClass.BUCKET);
            }

            //Check for fire extinguishing
            if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.FIRE))
            {
                terrain.getParameters().remove(TerrainManager.TerrainParameter.FIRE);
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


            case SHOVEL:
            if(TerrainManager.diggable(terrain))
            {
                TerrainManager.dig(terrain, x, y, quinoa);

                if(isPlayer(monster))
                {
                    if(TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_GRAVE))
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

            case PICKAXE:
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


            case CORN_SEED:
            if(terrain.getCode() == TerrainManager.TerrainCode.FERTILE_LAND)
            {
                if(!TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_SEED))
                {
                    terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_SEED,SeedType.CORN.name());
                    terrain.getParameters().put(TerrainManager.TerrainParameter.GROW_COUNTER,TerrainManager.CORN_GROW_COUNT+"");

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

            case PUMPKIN_SEED:
            if(terrain.getCode() == TerrainManager.TerrainCode.FERTILE_LAND)
            {
                if(!TerrainManager.hasParameter(terrain, TerrainManager.TerrainParameter.HAS_SEED))
                {
                    terrain.getParameters().put(TerrainManager.TerrainParameter.HAS_SEED,SeedType.PUMPKIN.name());
                    terrain.getParameters().put(TerrainManager.TerrainParameter.GROW_COUNTER,TerrainManager.PUMPKIN_GROW_COUNT+"");

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


            case MOP:
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MOSS))
            {
                terrain.getParameters().remove(TerrainParameter.HAS_MOSS);
            }
            if(terrain.getWater() > 0)
            {
                terrain.setWater(0);
            }
            if(TerrainManager.hasParameter(terrain, TerrainParameter.HAS_MUSHROOM_SPORES))
            {
                terrain.getParameters().remove(TerrainParameter.HAS_MUSHROOM_SPORES);
            }
            return false;

            default:
            return false;
        }
    }


    public static boolean setFire(Monster monster, Item item, Region region, int x, int y, Quinoa quinoa)
    {
        if(TerrainManager.flammable(region.getTerrain(x,y), x, y, quinoa))
            {
                region.getTerrain(x,y).getParameters().put(TerrainManager.TerrainParameter.FIRE, TerrainManager.BASE_FLAME_RATE+"");
                if(ItemManager.decreaseUse(item))
                {
                    item.remove();
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
            if(quinoa.monsterIsAdjacent(monster.getX(), monster.getY(), monsterToAttack.getID()))
            {
                //Do a combat attack
                MonsterActionManager.combat(monster, monsterToAttack, quinoa);

                //Check for removal if not player
                //Also check for kill
                if(!monsterToAttack.getID().equals(MonsterActionManager.PLAYER_ID) && !(monsterToAttack.getStats().getCurrentHealth() > 1))
                {
                    monsterToAttack.remove();
                    monsterKilled(monster, monsterToAttack, quinoa);
                }

            }
            else
            {
                //Attack misses automatically
                if(monster.getID().equals(MonsterActionManager.PLAYER_ID))
                {
                    quinoa.getMessageManager().addMessage("You miss the " + monsterToAttack.getMonsterCode().name() + ".");
                }
            }
        }
        else
        {
            //Monster cannot be found
            if(monster.getID().equals(MonsterActionManager.PLAYER_ID))
            {
                quinoa.getMessageManager().addMessage("You swing at nothing.");
            }
        }
    }

    public static void executePickupCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
    {
        boolean pickedUpAtLeastOneItem = false;
        Region region = quinoa.getCurrentRegionHeader().getRegion();
        for(Item tempItem : region.getItems())
        {
            if(tempItem.getX() == monster.getX()
            && tempItem.getY() == monster.getY()
            && !tempItem.shouldBeRemoved())
            {
                if(monster.getInventory().stackItem(tempItem))
                {
                    //Item stacked on current item
                    tempItem.remove();
                    pickedUpAtLeastOneItem = true;
                    
                    if(isPlayer(monster))
                    {
                        quinoa.getMessageManager().addMessage("Got " + tempItem.getItemClass().name() + ".");
                    }
                }
                else
                {
                    if(!monster.getInventory().isFull())
                    {
                        monster.getInventory().addItem((Item)tempItem.copy());
                        tempItem.remove();
                        pickedUpAtLeastOneItem = true;
                        
                        if(isPlayer(monster))
                        {
                            quinoa.getMessageManager().addMessage("Got " + tempItem.getItemClass().name() + ".");
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
        Item itemToDrop = monster.getInventory().getItem(itemID);
        if(itemToDrop != null)
        {
            Item newItem = (Item)itemToDrop.copy();
            newItem.setItemState(ItemState.GROUND);
            newItem.setPosition(monster.getX(), monster.getY());
            quinoa.getCurrentRegionHeader().getRegion().getItems().add(newItem);
            monster.getInventory().removeItem(itemID);
            monster.getInventory().removedExpiredItems();
            
            if(isPlayer(monster))
            {
                quinoa.getMessageManager().addMessage("Dropped " + itemToDrop.getItemClass() + ".");
            }
        }
    }

    public static void executeEquipCommand(Monster monster, MonsterCommand command, Quinoa quinoa)
    {
        String itemID = command.getParameter("itemID");
        Item itemToEquip = monster.getInventory().getItem(itemID);
        if(itemToEquip != null)
        {
            //check for a free slot
            ItemSlot freeSlot = ItemManager.getFreeItemSlot(monster, itemToEquip);
            if(freeSlot != null)
            {
                Item newItem = (Item)itemToEquip.copy();
                monster.getInventory().removeItem(itemID);
                monster.getInventory().equipItem(newItem, freeSlot);
                
                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("Equiped " + newItem.getItemClass().name() + ".");
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
        Item itemToEquip = monster.getInventory().getItem(itemID);

        //Find the slot that the item is equiped in
        ItemSlot equipSlot = null;
        for(ItemSlot tempSlot : ItemSlot.values())
        {
            Item testItem = monster.getInventory().getItem(tempSlot);
            if(testItem != null && testItem.getID().equals(itemID))
            {
                equipSlot = tempSlot;
            }
        }

        if(equipSlot != null)
        {
            if(!monster.getInventory().isFull())
            {
                Item newItem = (Item)itemToEquip.copy();
                monster.getInventory().equipItem(null, equipSlot);
                if(!monster.getInventory().stackItem(newItem))
                {
                    monster.getInventory().addItem(newItem);
                }

                if(isPlayer(monster))
                {
                    quinoa.getMessageManager().addMessage("Unequiped " + newItem.getItemClass().name() + ".");
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

    public static boolean isPlayer(Monster monster)
    {
        if(monster.getID().equals(MonsterActionManager.PLAYER_ID))
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //Does this role trade?
    public static boolean tradingRole(MonsterRole role)
    {
        switch(role)
        {
            case NULL:
            case BROTHER:
            return false;

            case BANKER:
            case CHEF:
            case HANDYMAN:
            return true;
        }
        return false;
    }

    //Update role specific features of a monster
    public static void updateRole(Monster monster, Quinoa quinoa)
    {
        int stockSize = 0;
        int hour = quinoa.getHour();
        switch(monster.getRole())
        {
            case NULL:
            break;

            case BROTHER:
            //don't let them starve
            if(monster.getStats().getCurrentHunger() > MonsterStats.MAX_HUNGER / 2)
            {
                monster.getStats().setCurrentHunger(0);
            }
            break;

            case BANKER:
            if(monster.getInventory().getWealth() < (monster.getInventory().getMaxWealth() / 2))
            {
                monster.getInventory().setWealth(monster.getInventory().getMaxWealth() / 2);
            }

            //don't let them starve
            if(monster.getStats().getCurrentHunger() > MonsterStats.MAX_HUNGER / 2)
            {
                monster.getStats().setCurrentHunger(0);
            }
            break;

            case CHEF:
            //Have food for sale at certain times of day
            if(hour == 5 || hour == 11 || hour == 17)
            {
                monster.getInventory().getItems().clear();
                stockSize = 15 + (int)(Math.random() * 5);
                for(int i=0; i < stockSize; i++)
                {
                    Item newItem = ItemManager.getRandomItem(ItemCategory.FOOD, true);
                    newItem.setItemState(ItemState.INVENTORY);
                    if(!monster.getInventory().isFull())
                    {
                        if(!monster.getInventory().stackItem(newItem))
                        {
                            monster.getInventory().addItem(newItem);
                        }
                    }
                }
            }
            else if(hour >= 19)
            {
                monster.getInventory().getItems().clear();
            }

            //don't let them starve
            if(monster.getStats().getCurrentHunger() > MonsterStats.MAX_HUNGER / 2)
            {
                monster.getStats().setCurrentHunger(0);
            }

            //give spending money
            if(monster.getInventory().getWealth() < 1000)
            {
                monster.getInventory().setWealth(1200);
            }
            break;

            case HANDYMAN:
            if(monster.getInventory().getItems().isEmpty() || hour == 11)
            {
                monster.getInventory().getItems().clear();
                
                stockSize = 6 + (int)(Math.random() * 5);
                for(int i=0; i < stockSize; i++)
                {
                    Item newItem = ItemManager.getRandomItem(ItemCategory.TOOL, true);
                    newItem.setStackSize(1 + (int)(Math.random() * (newItem.getMaxStackSize() - 1)));
                    newItem.setItemState(ItemState.INVENTORY);
                    if(!monster.getInventory().isFull())
                    {
                        monster.getInventory().addItem(newItem);
                    }
                }
                stockSize = 1 + (int)(Math.random() * 3);
                for(int i=0; i < stockSize; i++)
                {
                    Item newItem = ItemManager.getRandomItem(ItemCategory.LIGHT, true);
                    newItem.setStackSize(1 + (int)(Math.random() * (newItem.getMaxStackSize() - 1)));
                    newItem.setItemState(ItemState.INVENTORY);
                    if(!monster.getInventory().isFull())
                    {
                        monster.getInventory().addItem(newItem);
                    }
                }
                stockSize = 3 + (int)(Math.random() * 2);
                for(int i=0; i < stockSize; i++)
                {
                    Item newItem = ItemManager.getRandomItem(ItemCategory.MATERIAL, true);
                    newItem.setStackSize(1 + (int)(Math.random() * (newItem.getMaxStackSize() - 1)));
                    newItem.setItemState(ItemState.INVENTORY);
                    if(!monster.getInventory().isFull())
                    {
                        monster.getInventory().addItem(newItem);
                    }
                }
            }

            //don't let them starve
            if(monster.getStats().getCurrentHunger() > MonsterStats.MAX_HUNGER / 2)
            {
                monster.getStats().setCurrentHunger(0);
            }

            //give spending money
            if(monster.getInventory().getWealth() < 1000)
            {
                monster.getInventory().setWealth(1200);
            }
            break;

        }
    }
    
}
