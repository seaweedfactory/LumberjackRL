using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Utilities;
using LumberjackRL.Core.Monsters;

namespace LumberjackRL.Core.Items
{
    public enum ItemCategory
    { 
        FOOD, 
        WEAPON, 
        TOOL, 
        HELMET, 
        ARMOR, 
        BOOTS, 
        MONEY, 
        LIGHT, 
        MATERIAL
    }

    public enum ItemClass
    {
        APPLE, 
        KEY, 
        AXE, 
        COINS, 
        JACKET, 
        HAT, 
        BOOTS, 
        LOG, 
        FLAPJACKS,
        LANTERN, 
        ASH, 
        BUCKET,
        WATER_BUCKET,
        SHOVEL,
        TORCH,
        PICKAXE,
        SAPPHIRE,
        RUBY,
        EMERALD,
        DIAMOND,
        AMETHYST,
        BACON,
        TENT,
        DEATH_CAP,
        PUFFBALL, 
        FLY_AGARIC, 
        MOREL, 
        BUTTON_MUSHROOM, 
        GHOST_FUNGUS,
        CORN, 
        CORN_SEED,
        PUMPKIN, 
        PUMPKIN_SEED, 
        FLOODGATE, 
        MOSS, 
        MOP, 
        BONES
    }

    public enum ItemState 
    { 
        DESTROYED, 
        GROUND, 
        INVENTORY 
    }

    public enum ItemVerb 
    { 
        NULL,
        EAT, 
        USE, 
        ACTIVATE, 
        TALK, 
        LOOK, 
        TRADE,
        PLACE, 
        SLEEP 
    }

    public class ItemManager
    {
        public static int WATER_LEVEL_BUCKET_FULL = 250;

        public static void initialize(Item item)
        {
            item.attributes.Clear();
            switch (item.itemClass)
            {
                case ItemClass.BONES:
                    item.maxStackSize = 10;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 1.00;
                    break;

                case ItemClass.MOP:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 2.00;
                    break;

                case ItemClass.MOSS:
                    item.maxStackSize = 200;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 0.01;
                    break;

                case ItemClass.FLOODGATE:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 20.00;
                    break;

                case ItemClass.CORN:
                    item.maxStackSize = 25;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 1.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "350"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "1"));
                    break;

                case ItemClass.CORN_SEED:
                    item.maxStackSize = 200;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 0.03;
                    break;

                case ItemClass.PUMPKIN:
                    item.maxStackSize = 5;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 3.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "450"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "2"));
                    break;

                case ItemClass.PUMPKIN_SEED:
                    item.maxStackSize = 200;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 0.03;
                    break;

                case ItemClass.DEATH_CAP:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 0.50;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "-1000"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "-100"));
                    break;

                case ItemClass.PUFFBALL:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 1.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "100"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "1"));
                    break;

                case ItemClass.FLY_AGARIC:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 2.50;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "-1000"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "-5"));
                    break;

                case ItemClass.MOREL:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 1.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "300"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "2"));
                    break;

                case ItemClass.BUTTON_MUSHROOM:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 0.75;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "50"));
                    break;

                case ItemClass.GHOST_FUNGUS:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 5.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "-400"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "-5"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.MAKES_LIGHT, "0.25"));
                    break;

                case ItemClass.SAPPHIRE:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 50.00;
                    break;

                case ItemClass.RUBY:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 500.00;
                    break;

                case ItemClass.EMERALD:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 50.00;
                    break;

                case ItemClass.DIAMOND:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 1000.00;
                    break;

                case ItemClass.AMETHYST:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 30.00;
                    break;

                case ItemClass.APPLE:
                    item.maxStackSize = 10;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 0.25;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "250"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "1"));
                    break;

                case ItemClass.BACON:
                    item.maxStackSize = 6;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 2.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "600"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "2"));
                    break;

                case ItemClass.FLAPJACKS:
                    item.maxStackSize = 4;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 1.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "500"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "5"));
                    break;

                case ItemClass.AXE:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.PROTECT_AGAINST_FIRE, "100%"));
                    item.worth = 5.00;
                    break;

                case ItemClass.TENT:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.SLEEPS_FOR, "2 hours"));
                    item.worth = 50.00;
                    break;

                case ItemClass.PICKAXE:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 5.50;
                    break;

                case ItemClass.KEY:
                    item.maxStackSize = 10;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 1.00;
                    break;

                case ItemClass.COINS:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MONEY;
                    item.worth = 0.01;
                    break;

                case ItemClass.JACKET:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.ARMOR;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.BONUS_TO_DEFENSE, "7"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.SLOW_HUNGER, "50%"));
                    item.worth = 30.00;
                    break;

                case ItemClass.HAT:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.HELMET;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.BONUS_TO_DEFENSE, "3"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.DECREASE_SPEED, "200%"));
                    item.worth = 5.00;
                    break;

                case ItemClass.BOOTS:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.BOOTS;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.BONUS_TO_DEFENSE, "2"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.INCREASE_SPEED, "25%"));
                    item.worth = 10.00;
                    break;

                case ItemClass.LOG:
                    item.maxStackSize = 3;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 2.00;
                    break;

                case ItemClass.ASH:
                    item.maxStackSize = 200;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 0.05;
                    break;

                case ItemClass.LANTERN:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.LIGHT;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.MAKES_LIGHT, "0.90"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.USES, "100"));
                    item.worth = 10.00;
                    break;

                case ItemClass.BUCKET:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 5.25;
                    break;

                case ItemClass.WATER_BUCKET:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.USES, "10"));
                    item.worth = 6.25;
                    break;

                case ItemClass.SHOVEL:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 3.00;
                    break;

                case ItemClass.TORCH:
                    item.maxStackSize = 50;
                    item.itemCategory = ItemCategory.LIGHT;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.MAKES_LIGHT, "0.90"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.USES, "3"));
                    item.worth = 0.20;
                    break;
            }
        }

        /// <summary>
        /// Return a random item of a certain category. 
        /// If the item is a shop, then only return good items
        /// </summary>
        /// <param name="itemCategory"></param>
        /// <param name="shop"></param>
        /// <returns></returns>
        public static Item getRandomItem(ItemCategory itemCategory, bool shop)
        {
            List<ItemClass> ics = new List<ItemClass>();
            switch (itemCategory)
            {
                case ItemCategory.FOOD:
                    ics.Add(ItemClass.APPLE);
                    ics.Add(ItemClass.FLAPJACKS);
                    ics.Add(ItemClass.BACON);
                    ics.Add(ItemClass.PUFFBALL);
                    ics.Add(ItemClass.MOREL);
                    ics.Add(ItemClass.BUTTON_MUSHROOM);
                    ics.Add(ItemClass.CORN);
                    ics.Add(ItemClass.PUMPKIN);
                    if (!shop)
                    {
                        ics.Add(ItemClass.DEATH_CAP);
                        ics.Add(ItemClass.FLY_AGARIC);
                        ics.Add(ItemClass.GHOST_FUNGUS);
                    }
                    break;

                case ItemCategory.WEAPON:
                    return null;

                case ItemCategory.TOOL:
                    ics.Add(ItemClass.AXE);
                    ics.Add(ItemClass.BUCKET);
                    ics.Add(ItemClass.KEY);
                    ics.Add(ItemClass.PICKAXE);
                    ics.Add(ItemClass.SHOVEL);
                    ics.Add(ItemClass.WATER_BUCKET);
                    ics.Add(ItemClass.TENT);
                    ics.Add(ItemClass.CORN_SEED);
                    ics.Add(ItemClass.PUMPKIN_SEED);
                    ics.Add(ItemClass.MOP);
                    break;

                case ItemCategory.HELMET:
                    ics.Add(ItemClass.HAT);
                    break;

                case ItemCategory.ARMOR:
                    ics.Add(ItemClass.JACKET);
                    break;

                case ItemCategory.BOOTS:
                    ics.Add(ItemClass.BOOTS);
                    break;

                case ItemCategory.MONEY:
                    ics.Add(ItemClass.COINS);
                    break;

                case ItemCategory.LIGHT:
                    ics.Add(ItemClass.LANTERN);
                    ics.Add(ItemClass.TORCH);
                    break;

                case ItemCategory.MATERIAL:
                    ics.Add(ItemClass.ASH);
                    ics.Add(ItemClass.FLOODGATE);
                    ics.Add(ItemClass.MOSS);
                    if (!shop)
                    {
                        ics.Add(ItemClass.DIAMOND);
                        ics.Add(ItemClass.EMERALD);
                        ics.Add(ItemClass.LOG);
                        ics.Add(ItemClass.RUBY);
                        ics.Add(ItemClass.SAPPHIRE);
                    }
                    break;
            }

            //Make and return the item
            Item newItem = new Item();
            newItem.itemClass = ics[RandomNumber.RandomInteger(ics.Count)];
            ItemManager.initialize(newItem);
            return newItem;
        }


        public static Item getTreasure(double scale)
        {
            Item newItem = new Item();

            if (scale < 0.01)
            {
                newItem.itemClass = ItemClass.DIAMOND;
            }
            else if (scale < 0.05)
            {
                newItem.itemClass = ItemClass.RUBY;
            }
            else if (scale < 0.25)
            {
                newItem.itemClass = ItemClass.COINS;
                newItem.stackSize = ((int)(RandomNumber.RandomDouble() * (newItem.maxStackSize / 2)) + (newItem.maxStackSize / 2));
            }
            else
            {
                newItem.itemClass = ItemClass.COINS;
                newItem.stackSize = ((int)(RandomNumber.RandomDouble() * (newItem.maxStackSize - 1)) + 1);
            }

            return newItem;
        }


        public static List<ItemVerb> getVerbs(Item item)
        {
            List<ItemVerb> verbs = new List<ItemVerb>();

            //check if the item is there
            if (item != null)
            {
                switch (item.itemCategory)
                {
                    case ItemCategory.FOOD:
                        verbs.Add(ItemVerb.EAT);
                        verbs.Add(ItemVerb.PLACE);
                        break;

                    case ItemCategory.WEAPON:
                        break;

                    case ItemCategory.LIGHT:
                        verbs.Add(ItemVerb.PLACE);
                        verbs.Add(ItemVerb.USE);
                        break;

                    case ItemCategory.TOOL:
                        verbs.Add(ItemVerb.USE);
                        break;

                    case ItemCategory.HELMET:
                        break;

                    case ItemCategory.ARMOR:
                        break;

                    case ItemCategory.BOOTS:
                        break;

                    case ItemCategory.MONEY:
                        break;

                    case ItemCategory.MATERIAL:
                        verbs.Add(ItemVerb.USE);
                        verbs.Add(ItemVerb.PLACE);
                        break;
                }
            }
            else
            {
                verbs.Add(ItemVerb.ACTIVATE);
                verbs.Add(ItemVerb.TALK);
                verbs.Add(ItemVerb.LOOK);
                verbs.Add(ItemVerb.TRADE);
                verbs.Add(ItemVerb.SLEEP);
            }

            return verbs;
        }

        public static int verbDistance(ItemVerb verb)
        {
            switch (verb)
            {
                case ItemVerb.EAT:
                case ItemVerb.SLEEP:
                    return 0;

                case ItemVerb.USE:
                case ItemVerb.ACTIVATE:
                case ItemVerb.PLACE:
                    return 1;

                case ItemVerb.TRADE:
                    return 2;

                case ItemVerb.TALK:
                    return 5;

                case ItemVerb.LOOK:
                    return 10;

            }
            return 0;
        }

        public static ItemSlot getFreeItemSlot(Monster monster, Item item)
        {
            List<ItemSlot> possibleSlots = new List<ItemSlot>();
            switch(item.itemCategory)
            {
                case ItemCategory.FOOD:
                case ItemCategory.TOOL:
                case ItemCategory.MONEY:
                case ItemCategory.MATERIAL:
                case ItemCategory.LIGHT:
                case ItemCategory.WEAPON:
                possibleSlots.Add(ItemSlot.BELT_1);
                possibleSlots.Add(ItemSlot.BELT_2);
                possibleSlots.Add(ItemSlot.BELT_3);
                possibleSlots.Add(ItemSlot.BELT_4);
                break;

                case ItemCategory.HELMET:
                possibleSlots.Add(ItemSlot.HEAD);
                break;

                case ItemCategory.ARMOR:
                possibleSlots.Add(ItemSlot.BODY);
                break;

                case ItemCategory.BOOTS:
                possibleSlots.Add(ItemSlot.FEET);
                break;

            }

            //Search through possible slots and try to find a free one
            foreach(ItemSlot tempSlot in possibleSlots)
            {
                if(monster.inventory.getItem(tempSlot) == null)
                {
                    return tempSlot;
                }
            }

            return ItemSlot.NULL;
        }

        /// <summary>
        /// Does this item use the idea of limited uses?
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool hasUses(Item item)
        {
            foreach(ItemAttribute attr in item.attributes)
            {
                if(attr.type == ItemAttributeType.USES)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Use up one use of this item, return true if out of uses
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static bool decreaseUse(Item item)
        {
            foreach(ItemAttribute attr in item.attributes)
            {
                if(attr.type == ItemAttributeType.USES)
                {
                    int uses = Int32.Parse(attr.parameter);
                    uses = uses - 1;
                    attr.parameter = (uses+"");

                    if(uses == 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
