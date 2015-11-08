package quinoa.items;

import java.util.ArrayList;
import quinoa.items.ItemAttribute.ItemAttributeType;
import quinoa.monsters.Monster;
import quinoa.monsters.MonsterInventory.ItemSlot;

public class ItemManager
{
    public static enum ItemCategory{FOOD, WEAPON, TOOL, HELMET, ARMOR, BOOTS, MONEY, LIGHT, MATERIAL};
    public static enum ItemClass{APPLE, KEY, AXE, COINS, JACKET, HAT, BOOTS, LOG, FLAPJACKS, 
                               LANTERN, ASH, BUCKET, WATER_BUCKET, SHOVEL, TORCH, PICKAXE,
                               SAPPHIRE, RUBY, EMERALD, DIAMOND, AMETHYST, BACON, TENT,
                               DEATH_CAP, PUFFBALL, FLY_AGARIC, MOREL, BUTTON_MUSHROOM, GHOST_FUNGUS,
                               CORN, CORN_SEED, PUMPKIN, PUMPKIN_SEED, FLOODGATE, MOSS, MOP, BONES};
    public static enum ItemState{DESTROYED, GROUND, INVENTORY};
    public static enum ItemVerb{EAT, USE, ACTIVATE, TALK, LOOK, TRADE, PLACE, SLEEP};

    public static int WATER_LEVEL_BUCKET_FULL=250;

    public static void initialize(Item item)
    {
        item.getAttributes().clear();
        switch(item.getItemClass())
        {
            case BONES:
            item.setMaxStackSize(10);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(1.00);
            break;

            case MOP:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.TOOL);
            item.setWorth(2.00);
            break;

            case MOSS:
            item.setMaxStackSize(200);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(0.01);
            break;

            case FLOODGATE:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(20.00);
            break;

            case CORN:
            item.setMaxStackSize(25);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(1.00);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "350"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "1"));
            break;

            case CORN_SEED:
            item.setMaxStackSize(200);
            item.setItemCategory(ItemCategory.TOOL);
            item.setWorth(0.03);
            break;

            case PUMPKIN:
            item.setMaxStackSize(5);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(3.00);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "450"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "2"));
            break;

            case PUMPKIN_SEED:
            item.setMaxStackSize(200);
            item.setItemCategory(ItemCategory.TOOL);
            item.setWorth(0.03);
            break;

            case DEATH_CAP:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(0.50);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "-1000"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "-100"));
            break;

            case PUFFBALL:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(1.00);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "100"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "1"));
            break;

            case FLY_AGARIC:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(2.50);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "-1000"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "-5"));
            break;

            case MOREL:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(1.00);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "300"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "2"));
            break;

            case BUTTON_MUSHROOM:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(0.75);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "50"));
            break;

            case GHOST_FUNGUS:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(5.00);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "-400"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "-5"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.MAKES_LIGHT, "0.25"));
            break;

            case SAPPHIRE:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(50.00);
            break;

            case RUBY:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(500.00);
            break;

            case EMERALD:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(50.00);
            break;

            case DIAMOND:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(1000.00);
            break;

            case AMETHYST:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(30.00);
            break;

            case APPLE:
            item.setMaxStackSize(10);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(0.25);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "250"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "1"));
            break;

            case BACON:
            item.setMaxStackSize(6);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(2.00);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "600"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "2"));
            break;

            case FLAPJACKS:
            item.setMaxStackSize(4);
            item.setItemCategory(ItemCategory.FOOD);
            item.setWorth(1.00);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.NUTRITION, "500"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.HEALS, "5"));
            break;

            case AXE:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.TOOL);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.PROTECT_AGAINST_FIRE, "100%"));
            item.setWorth(5.00);
            break;

            case TENT:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.TOOL);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.SLEEPS_FOR, "2 hours"));
            item.setWorth(50.00);
            break;

            case PICKAXE:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.TOOL);
            item.setWorth(5.50);
            break;

            case KEY:
            item.setMaxStackSize(10);
            item.setItemCategory(ItemCategory.TOOL);
            item.setWorth(1.00);
            break;

            case COINS:
            item.setMaxStackSize(100);
            item.setItemCategory(ItemCategory.MONEY);
            item.setWorth(0.01);
            break;

            case JACKET:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.ARMOR);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.BONUS_TO_DEFENSE, "7"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.SLOW_HUNGER, "50%"));
            item.setWorth(30.00);
            break;

            case HAT:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.HELMET);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.BONUS_TO_DEFENSE, "3"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.DECREASE_SPEED, "200%"));
            item.setWorth(5.00);
            break;

            case BOOTS:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.BOOTS);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.BONUS_TO_DEFENSE, "2"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.INCREASE_SPEED, "25%"));
            item.setWorth(10.00);
            break;

            case LOG:
            item.setMaxStackSize(3);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(2.00);
            break;

            case ASH:
            item.setMaxStackSize(200);
            item.setItemCategory(ItemCategory.MATERIAL);
            item.setWorth(0.05);
            break;

            case LANTERN:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.LIGHT);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.MAKES_LIGHT, "0.90"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.USES, "100"));
            item.setWorth(10.00);
            break;

            case BUCKET:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.TOOL);
            item.setWorth(5.25);
            break;

            case WATER_BUCKET:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.TOOL);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.USES, "10"));
            item.setWorth(6.25);
            break;

            case SHOVEL:
            item.setMaxStackSize(1);
            item.setItemCategory(ItemCategory.TOOL);
            item.setWorth(3.00);
            break;

            case TORCH:
            item.setMaxStackSize(50);
            item.setItemCategory(ItemCategory.LIGHT);
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.MAKES_LIGHT, "0.90"));
            item.getAttributes().add(new ItemAttribute(ItemAttributeType.USES, "3"));
            item.setWorth(0.20);
            break;
        }
    }


    //Return a random item of a certain category
    //If the item is a shop, then only return good items
    public static Item getRandomItem(ItemCategory itemCategory, boolean shop)
    {
        ArrayList<ItemClass> ics = new ArrayList<ItemClass>();
        switch(itemCategory)
        {
            case FOOD:
            ics.add(ItemClass.APPLE);
            ics.add(ItemClass.FLAPJACKS);
            ics.add(ItemClass.BACON);
            ics.add(ItemClass.PUFFBALL);
            ics.add(ItemClass.MOREL);
            ics.add(ItemClass.BUTTON_MUSHROOM);
            ics.add(ItemClass.CORN);
            ics.add(ItemClass.PUMPKIN);
            if(!shop)
            {
                ics.add(ItemClass.DEATH_CAP);
                ics.add(ItemClass.FLY_AGARIC);
                ics.add(ItemClass.GHOST_FUNGUS);
            }
            break;

            case WEAPON:
            return null;

            case TOOL:
            ics.add(ItemClass.AXE);
            ics.add(ItemClass.BUCKET);
            ics.add(ItemClass.KEY);
            ics.add(ItemClass.PICKAXE);
            ics.add(ItemClass.SHOVEL);
            ics.add(ItemClass.WATER_BUCKET);
            ics.add(ItemClass.TENT);
            ics.add(ItemClass.CORN_SEED);
            ics.add(ItemClass.PUMPKIN_SEED);
            ics.add(ItemClass.MOP);
            break;

            case HELMET:
            ics.add(ItemClass.HAT);
            break;

            case ARMOR:
            ics.add(ItemClass.JACKET);
            break;

            case BOOTS:
            ics.add(ItemClass.BOOTS);
            break;

            case MONEY:
            ics.add(ItemClass.COINS);
            break;

            case LIGHT:
            ics.add(ItemClass.LANTERN);
            ics.add(ItemClass.TORCH);
            break;

            case MATERIAL:
            ics.add(ItemClass.ASH);
            ics.add(ItemClass.FLOODGATE);
            ics.add(ItemClass.MOSS);
            if(!shop)
            {
                ics.add(ItemClass.DIAMOND);
                ics.add(ItemClass.EMERALD);
                ics.add(ItemClass.LOG);
                ics.add(ItemClass.RUBY);
                ics.add(ItemClass.SAPPHIRE);
            }
            break;
        }

        //Make and return the item
        Item newItem = new Item();
        newItem.setItemClass(ics.get((int)(Math.random() * ics.size())));
        ItemManager.initialize(newItem);
        return newItem;
    }
    

    public static Item getTreasure(double scale)
    {
        Item newItem = new Item();

        if(scale < 0.01)
        {
            newItem.setItemClass(ItemClass.DIAMOND);
        }
        else if(scale < 0.05)
        {
            newItem.setItemClass(ItemClass.RUBY);
        }
        else if(scale < 0.25)
        {
            newItem.setItemClass(ItemClass.COINS);
            newItem.setStackSize((int)(Math.random() * (newItem.getMaxStackSize() / 2)) + (newItem.getMaxStackSize() / 2));
        }
        else
        {
            newItem.setItemClass(ItemClass.COINS);
            newItem.setStackSize((int)(Math.random() * (newItem.getMaxStackSize()-1)) + 1);
        }

        return newItem;
    }


    public static ArrayList<ItemVerb> getVerbs(Item item)
    {
        ArrayList<ItemVerb> verbs = new ArrayList<ItemVerb>();

        //check if the item is there
        if(item != null)
        {
            switch(item.getItemCategory())
            {
                case FOOD:
                verbs.add(ItemVerb.EAT);
                verbs.add(ItemVerb.PLACE);
                break;

                case WEAPON:
                break;

                case LIGHT:
                verbs.add(ItemVerb.PLACE);
                verbs.add(ItemVerb.USE);
                break;

                case TOOL:
                verbs.add(ItemVerb.USE);
                break;

                case HELMET:
                break;

                case ARMOR:
                break;

                case BOOTS:
                break;

                case MONEY:
                break;

                case MATERIAL:
                verbs.add(ItemVerb.USE);
                verbs.add(ItemVerb.PLACE);
                break;

            }
        }
        else
        {
            verbs.add(ItemVerb.ACTIVATE);
            verbs.add(ItemVerb.TALK);
            verbs.add(ItemVerb.LOOK);
            verbs.add(ItemVerb.TRADE);
            verbs.add(ItemVerb.SLEEP);
        }

        return verbs;
    }

    public static int verbDistance(ItemVerb verb)
    {
        switch(verb)
        {
            case EAT:
            case SLEEP:
            return 0;

            case USE:
            case ACTIVATE:
            case PLACE:
            return 1;

            case TRADE:
            return 2;

            case TALK:
            return 5;

            case LOOK:
            return 10;

        }
        return 0;
    }

    public static ItemSlot getFreeItemSlot(Monster monster, Item item)
    {
        ArrayList<ItemSlot> possibleSlots = new ArrayList<ItemSlot>();
        switch(item.getItemCategory())
        {
            case FOOD:
            case TOOL:
            case MONEY:
            case MATERIAL:
            case LIGHT:
            case WEAPON:
            possibleSlots.add(ItemSlot.BELT_1);
            possibleSlots.add(ItemSlot.BELT_2);
            possibleSlots.add(ItemSlot.BELT_3);
            possibleSlots.add(ItemSlot.BELT_4);
            break;

            case HELMET:
            possibleSlots.add(ItemSlot.HEAD);
            break;

            case ARMOR:
            possibleSlots.add(ItemSlot.BODY);
            break;

            case BOOTS:
            possibleSlots.add(ItemSlot.FEET);
            break;

        }

        //Search through possible slots and try to find a free one
        for(ItemSlot tempSlot : possibleSlots)
        {
            if(monster.getInventory().getItem(tempSlot) == null)
            {
                return tempSlot;
            }
        }
        
        return null;
    }

    //Does this item use the idea of limited uses?
    public static boolean hasUses(Item item)
    {
        for(ItemAttribute attr : item.getAttributes())
        {
            if(attr.getType() == ItemAttributeType.USES)
            {
                return true;
            }
        }
        return false;
    }

    //Use up one use of this item, return true if out of uses
    public static boolean decreaseUse(Item item)
    {
        for(ItemAttribute attr : item.getAttributes())
        {
            if(attr.getType() == ItemAttributeType.USES)
            {
                int uses = Integer.parseInt(attr.getParameter());
                uses = uses - 1;
                attr.setParameter(uses+"");

                if(uses == 0)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
