﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Utilities;
using LumberjackRL.Core.Monsters;

namespace LumberjackRL.Core.Items
{
    public class ItemManager
    {
        public static int WATER_LEVEL_BUCKET_FULL = 250;

        public static void initialize(Item item)
        {
            item.attributes.Clear();
            switch (item.itemClass)
            {
                case ItemClassType.BONES:
                    item.maxStackSize = 10;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 1.00;
                    break;

                case ItemClassType.MOP:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 2.00;
                    break;

                case ItemClassType.MOSS:
                    item.maxStackSize = 200;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 0.01;
                    break;

                case ItemClassType.FLOODGATE:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 20.00;
                    break;

                case ItemClassType.CORN:
                    item.maxStackSize = 25;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 1.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "350"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "1"));
                    break;

                case ItemClassType.CORN_SEED:
                    item.maxStackSize = 200;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 0.03;
                    break;

                case ItemClassType.PUMPKIN:
                    item.maxStackSize = 5;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 3.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "450"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "2"));
                    break;

                case ItemClassType.PUMPKIN_SEED:
                    item.maxStackSize = 200;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 0.03;
                    break;

                case ItemClassType.DEATH_CAP:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 0.50;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "-1000"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "-100"));
                    break;

                case ItemClassType.PUFFBALL:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 1.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "100"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "1"));
                    break;

                case ItemClassType.FLY_AGARIC:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 2.50;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "-1000"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "-5"));
                    break;

                case ItemClassType.MOREL:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 1.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "300"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "2"));
                    break;

                case ItemClassType.BUTTON_MUSHROOM:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 0.75;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "50"));
                    break;

                case ItemClassType.GHOST_FUNGUS:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 5.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "-400"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "-5"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.MAKES_LIGHT, "0.25"));
                    break;

                case ItemClassType.SAPPHIRE:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 50.00;
                    break;

                case ItemClassType.RUBY:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 500.00;
                    break;

                case ItemClassType.EMERALD:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 50.00;
                    break;

                case ItemClassType.DIAMOND:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 1000.00;
                    break;

                case ItemClassType.AMETHYST:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 30.00;
                    break;

                case ItemClassType.APPLE:
                    item.maxStackSize = 10;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 0.25;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "250"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "1"));
                    break;

                case ItemClassType.BACON:
                    item.maxStackSize = 6;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 2.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "600"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "2"));
                    break;

                case ItemClassType.FLAPJACKS:
                    item.maxStackSize = 4;
                    item.itemCategory = ItemCategory.FOOD;
                    item.worth = 1.00;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.NUTRITION, "500"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.HEALS, "5"));
                    break;

                case ItemClassType.AXE:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.PROTECT_AGAINST_FIRE, "100%"));
                    item.worth = 5.00;
                    break;

                case ItemClassType.TENT:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.SLEEPS_FOR, "2 hours"));
                    item.worth = 50.00;
                    break;

                case ItemClassType.PICKAXE:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 5.50;
                    break;

                case ItemClassType.KEY:
                    item.maxStackSize = 10;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 1.00;
                    break;

                case ItemClassType.COINS:
                    item.maxStackSize = 100;
                    item.itemCategory = ItemCategory.MONEY;
                    item.worth = 0.01;
                    break;

                case ItemClassType.JACKET:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.ARMOR;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.BONUS_TO_DEFENSE, "7"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.SLOW_HUNGER, "50%"));
                    item.worth = 30.00;
                    break;

                case ItemClassType.HAT:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.HELMET;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.BONUS_TO_DEFENSE, "3"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.DECREASE_SPEED, "200%"));
                    item.worth = 5.00;
                    break;

                case ItemClassType.BOOTS:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.BOOTS;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.BONUS_TO_DEFENSE, "2"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.INCREASE_SPEED, "25%"));
                    item.worth = 10.00;
                    break;

                case ItemClassType.LOG:
                    item.maxStackSize = 3;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 2.00;
                    break;

                case ItemClassType.ASH:
                    item.maxStackSize = 200;
                    item.itemCategory = ItemCategory.MATERIAL;
                    item.worth = 0.05;
                    break;

                case ItemClassType.LANTERN:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.LIGHT;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.MAKES_LIGHT, "0.90"));
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.USES, "100"));
                    item.worth = 10.00;
                    break;

                case ItemClassType.BUCKET:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 5.25;
                    break;

                case ItemClassType.WATER_BUCKET:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.attributes.Add(new ItemAttribute(ItemAttributeType.USES, "10"));
                    item.worth = 6.25;
                    break;

                case ItemClassType.SHOVEL:
                    item.maxStackSize = 1;
                    item.itemCategory = ItemCategory.TOOL;
                    item.worth = 3.00;
                    break;

                case ItemClassType.TORCH:
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
            RandomNumberGenerator rng = new RandomNumberGenerator();
            List<ItemClassType> ics = new List<ItemClassType>();
            switch (itemCategory)
            {
                case ItemCategory.FOOD:
                    ics.Add(ItemClassType.APPLE);
                    ics.Add(ItemClassType.FLAPJACKS);
                    ics.Add(ItemClassType.BACON);
                    ics.Add(ItemClassType.PUFFBALL);
                    ics.Add(ItemClassType.MOREL);
                    ics.Add(ItemClassType.BUTTON_MUSHROOM);
                    ics.Add(ItemClassType.CORN);
                    ics.Add(ItemClassType.PUMPKIN);
                    if (!shop)
                    {
                        ics.Add(ItemClassType.DEATH_CAP);
                        ics.Add(ItemClassType.FLY_AGARIC);
                        ics.Add(ItemClassType.GHOST_FUNGUS);
                    }
                    break;

                case ItemCategory.WEAPON:
                    return null;

                case ItemCategory.TOOL:
                    ics.Add(ItemClassType.AXE);
                    ics.Add(ItemClassType.BUCKET);
                    ics.Add(ItemClassType.KEY);
                    ics.Add(ItemClassType.PICKAXE);
                    ics.Add(ItemClassType.SHOVEL);
                    ics.Add(ItemClassType.WATER_BUCKET);
                    ics.Add(ItemClassType.TENT);
                    ics.Add(ItemClassType.CORN_SEED);
                    ics.Add(ItemClassType.PUMPKIN_SEED);
                    ics.Add(ItemClassType.MOP);
                    break;

                case ItemCategory.HELMET:
                    ics.Add(ItemClassType.HAT);
                    break;

                case ItemCategory.ARMOR:
                    ics.Add(ItemClassType.JACKET);
                    break;

                case ItemCategory.BOOTS:
                    ics.Add(ItemClassType.BOOTS);
                    break;

                case ItemCategory.MONEY:
                    ics.Add(ItemClassType.COINS);
                    break;

                case ItemCategory.LIGHT:
                    ics.Add(ItemClassType.LANTERN);
                    ics.Add(ItemClassType.TORCH);
                    break;

                case ItemCategory.MATERIAL:
                    ics.Add(ItemClassType.ASH);
                    ics.Add(ItemClassType.FLOODGATE);
                    ics.Add(ItemClassType.MOSS);
                    if (!shop)
                    {
                        ics.Add(ItemClassType.DIAMOND);
                        ics.Add(ItemClassType.EMERALD);
                        ics.Add(ItemClassType.LOG);
                        ics.Add(ItemClassType.RUBY);
                        ics.Add(ItemClassType.SAPPHIRE);
                    }
                    break;
            }

            //Make and return the item
            Item newItem = new Item();
            newItem.itemClass = ics[rng.RandomInteger(ics.Count)];
            ItemManager.initialize(newItem);
            return newItem;
        }

        public static Item getTreasure(double scale)
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            Item newItem = new Item();

            if (scale < 0.01)
            {
                newItem.itemClass = ItemClassType.DIAMOND;
            }
            else if (scale < 0.05)
            {
                newItem.itemClass = ItemClassType.RUBY;
            }
            else if (scale < 0.25)
            {
                newItem.itemClass = ItemClassType.COINS;
                newItem.stackSize = ((int)(rng.RandomDouble() * (newItem.maxStackSize / 2)) + (newItem.maxStackSize / 2));
            }
            else
            {
                newItem.itemClass = ItemClassType.COINS;
                newItem.stackSize = ((int)(rng.RandomDouble() * (newItem.maxStackSize - 1)) + 1);
            }

            return newItem;
        }

        public static List<ItemVerbType> getVerbs(Item item)
        {
            List<ItemVerbType> verbs = new List<ItemVerbType>();

            //check if the item is there
            if (item != null)
            {
                switch (item.itemCategory)
                {
                    case ItemCategory.FOOD:
                        verbs.Add(ItemVerbType.EAT);
                        verbs.Add(ItemVerbType.PLACE);
                        break;

                    case ItemCategory.WEAPON:
                        break;

                    case ItemCategory.LIGHT:
                        verbs.Add(ItemVerbType.PLACE);
                        verbs.Add(ItemVerbType.USE);
                        break;

                    case ItemCategory.TOOL:
                        verbs.Add(ItemVerbType.USE);
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
                        verbs.Add(ItemVerbType.USE);
                        verbs.Add(ItemVerbType.PLACE);
                        break;
                }
            }
            else
            {
                verbs.Add(ItemVerbType.ACTIVATE);
                verbs.Add(ItemVerbType.TALK);
                verbs.Add(ItemVerbType.LOOK);
                verbs.Add(ItemVerbType.TRADE);
                verbs.Add(ItemVerbType.SLEEP);
            }

            return verbs;
        }

        public static int verbDistance(ItemVerbType verb)
        {
            switch (verb)
            {
                case ItemVerbType.EAT:
                case ItemVerbType.SLEEP:
                    return 0;

                case ItemVerbType.USE:
                case ItemVerbType.ACTIVATE:
                case ItemVerbType.PLACE:
                    return 1;

                case ItemVerbType.TRADE:
                    return 2;

                case ItemVerbType.TALK:
                    return 5;

                case ItemVerbType.LOOK:
                    return 10;

            }
            return 0;
        }

        public static MonsterItemSlotType getFreeItemSlot(Monster monster, Item item)
        {
            List<MonsterItemSlotType> possibleSlots = new List<MonsterItemSlotType>();
            switch(item.itemCategory)
            {
                case ItemCategory.FOOD:
                case ItemCategory.TOOL:
                case ItemCategory.MONEY:
                case ItemCategory.MATERIAL:
                case ItemCategory.LIGHT:
                case ItemCategory.WEAPON:
                possibleSlots.Add(MonsterItemSlotType.BELT_1);
                possibleSlots.Add(MonsterItemSlotType.BELT_2);
                possibleSlots.Add(MonsterItemSlotType.BELT_3);
                possibleSlots.Add(MonsterItemSlotType.BELT_4);
                break;

                case ItemCategory.HELMET:
                possibleSlots.Add(MonsterItemSlotType.HEAD);
                break;

                case ItemCategory.ARMOR:
                possibleSlots.Add(MonsterItemSlotType.BODY);
                break;

                case ItemCategory.BOOTS:
                possibleSlots.Add(MonsterItemSlotType.FEET);
                break;

            }

            //Search through possible slots and try to find a free one
            foreach(MonsterItemSlotType tempSlot in possibleSlots)
            {
                if(monster.inventory.getItem(tempSlot) == null)
                {
                    return tempSlot;
                }
            }

            return MonsterItemSlotType.NULL;
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
