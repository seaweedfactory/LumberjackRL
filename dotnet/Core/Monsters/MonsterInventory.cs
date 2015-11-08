using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Items;
using System.IO;

namespace LumberjackRL.Core.Monsters
{
    public enum ItemSlot 
    {
        NULL,
        BELT_1, 
        BELT_2, 
        BELT_3, 
        BELT_4, 
        HEAD, 
        BODY, 
        FEET
    }

    public class MonsterInventory : IStoreObject, ICopyObject
    {
        public const int MAX_WIDTH = 10;
        public const int MAX_HEIGHT = 5;

        private List<Item> items;  //unequiped items
        private Item beltSlot1;    //individual equipment slots
        private Item beltSlot2;
        private Item beltSlot3;
        private Item beltSlot4;
        private Item rightHandSlot;
        private Item leftHandSlot;
        private Item headSlot;
        private Item bodySlot;
        private Item feetSlot;

        public int width
        {
            get;
            set;
        }

        public int height
        {
            get;
            set;
        }

        /// <summary>
        /// How much money is the monster carrying?
        /// </summary>
        public double wealth
        {
            get;
            set;
        }

        /// <summary>
        /// How much money can the monster carry?
        /// </summary>
        public double maxWealth
        {
            get;
            set;
        }

        /// <summary>
        /// Is the inventory filled?
        /// </summary>
        public bool Full
        {
            get
            {
                return !(items.Count < (width * height));
            }
        }

        public MonsterInventory()
        {
            items = new List<Item>();
            width = 1;
            height = 1;
            wealth = 0.0;
            maxWealth = 100000.0;
        }

        public void setDimensions(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void addItem(Item item)
        {
            //Find a free x, y slot
            bool foundSlot=false;
            bool[,] slots = new bool[width, height];
            
            for(int y=0; y < height; y++)
            {
                for(int x=0; x < width; x++)
                {
                    slots[x,y] = false;
                }
            }

            foreach (Item tmpItem in items)
            {
                slots[tmpItem.x,tmpItem.y] = true;
            }
            
            for(int y=0; y < height; y++)
            {
                for(int x=0; x < width; x++)
                {
                    if(slots[x,y] == false && !foundSlot)
                    {
                        item.setPosition(x, y);
                        foundSlot = true;
                    }
                }
            }

            //set the state of the item to inside inventory
            item.itemState = ItemState.INVENTORY;

            //actually add the item
            items.Add(item);
        }

        public Item getItem(String itemID)
        {
            foreach(Item tempItem in items)
            {
                if(tempItem.ID.Equals(itemID))
                {
                    return tempItem;
                }
            }

            foreach(ItemSlot tempSlot in Enum.GetValues(typeof(ItemSlot)).Cast<ItemSlot>())
            {
                if(getItem(tempSlot) != null && getItem(tempSlot).ID.Equals(itemID))
                {
                    return getItem(tempSlot);
                }
            }

            return null;
        }

        public Item getItem(int x, int y)
        {
            foreach(Item tempItem in items)
            {
                if(tempItem.x == x && tempItem.y == y)
                {
                    return tempItem;
                }
            }
            return null;
        }

        public Item getItem(ItemSlot slot)
        {
            switch(slot)
            {
                case ItemSlot.BELT_1:
                return this.beltSlot1;

                case ItemSlot.BELT_2:
                return this.beltSlot2;

                case ItemSlot.BELT_3:
                return this.beltSlot3;

                case ItemSlot.BELT_4:
                return this.beltSlot4;

                case ItemSlot.HEAD:
                return this.headSlot;

                case ItemSlot.BODY:
                return this.bodySlot;

                case ItemSlot.FEET:
                return this.feetSlot;

                default:
                return null;
            }
        }

        public void equipItem(Item item, ItemSlot slot)
        {
            switch(slot)
            {
                case ItemSlot.BELT_1:
                this.beltSlot1 = item;
                break;

                case ItemSlot.BELT_2:
                this.beltSlot2 = item;
                break;

                case ItemSlot.BELT_3:
                this.beltSlot3 = item;
                break;

                case ItemSlot.BELT_4:
                this.beltSlot4 = item;
                break;

                case ItemSlot.HEAD:
                this.headSlot = item;
                break;

                case ItemSlot.BODY:
                this.bodySlot = item;
                break;

                case ItemSlot.FEET:
                this.feetSlot = item;
                break;
            }
        }

        public List<Item> getItems()
        {
            return items;
        }

        /// <summary>
        /// Return true to remove the item that is being stacked
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns></returns>
        public bool stackItem(Item newItem)
        {
            foreach(Item tempItem in items)
            {
                if(newItem.itemClass == tempItem.itemClass
                && tempItem.stackSize < tempItem.maxStackSize)
                {
                    tempItem.stackSize = (tempItem.stackSize + newItem.stackSize);
                    if(tempItem.stackSize > tempItem.maxStackSize)
                    {
                        newItem.stackSize = (tempItem.stackSize - tempItem.maxStackSize);
                        tempItem.stackSize = tempItem.maxStackSize;
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void removeItem(String itemID)
        {
            Item itemToRemove = null;
            foreach(Item tempItem in items)
            {
                if(tempItem.ID.Equals(itemID))
                {
                    itemToRemove = tempItem;
                }
            }

            foreach(ItemSlot tempSlot in Enum.GetValues(typeof(ItemSlot)).Cast<ItemSlot>())
            {
                Item tempItem = getItem(tempSlot);
                if(tempItem != null && tempItem.ID.Equals(itemID))
                {
                    itemToRemove = tempItem;
                }
            }

            if(itemToRemove != null)
            {
                itemToRemove.RemoveObject();
            }
        }

        public void removedExpiredItems()
        {
            List<Item> removeItemList = new List<Item>();

            foreach(Item tempItem in items)
            {
                if(tempItem.ShouldBeRemoved)
                {
                    removeItemList.Add(tempItem);
                }
            }

            foreach(Item tempItem in removeItemList)
            {
                items.Remove(tempItem);
            }

            //remove items in each slot if applicable
            foreach(ItemSlot tempSlot in Enum.GetValues(typeof(ItemSlot)).Cast<ItemSlot>())
            {
                if(getItem(tempSlot) != null && getItem(tempSlot).ShouldBeRemoved)
                {
                    equipItem(null, tempSlot);
                }
            }
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(width + "");
            outStream.WriteLine(height + "");
            outStream.WriteLine(wealth + "");
            outStream.WriteLine(maxWealth + "");
            outStream.WriteLine(items.Count + "");
            foreach(Item tempItem in items)
            {
                tempItem.SaveObject(outStream);
            }
            List<ItemSlot> slots = Enum.GetValues(typeof(ItemSlot)).Cast<ItemSlot>().ToList<ItemSlot>();
            outStream.WriteLine(slots.Count + "");
            foreach(ItemSlot tempSlot in slots)
            {
                outStream.WriteLine(Enum.GetName(typeof(ItemSlot),tempSlot));
                if(getItem(tempSlot) != null)
                {
                    outStream.WriteLine("true");
                    getItem(tempSlot).SaveObject(outStream);
                }
                else
                {
                    outStream.WriteLine("false");
                }
            }
        }

        public void LoadObject(StreamReader inStream)
        {
            this.setDimensions(Int32.Parse(inStream.ReadLine()), Int32.Parse(inStream.ReadLine()));
            this.wealth = Double.Parse(inStream.ReadLine());
            this.maxWealth = Double.Parse(inStream.ReadLine());
            items = new List<Item>();
            int itemsSize = Int32.Parse(inStream.ReadLine());
            for(int i=0; i < itemsSize; i++)
            {
                Item newItem = new Item();
                newItem.LoadObject(inStream);
                items.Add(newItem);
            }
            int slotArrayLength = Int32.Parse(inStream.ReadLine());
            for(int i=0; i < slotArrayLength; i++)
            {
                ItemSlot tempSlot = (ItemSlot)Enum.Parse(typeof(ItemSlot), inStream.ReadLine());
                bool itemExists = Boolean.Parse(inStream.ReadLine());
                Item newItem = new Item();
                if(itemExists)
                {
                    newItem.LoadObject(inStream);
                }
                else
                {
                    newItem = null;
                }
                equipItem(newItem, tempSlot);
            }
        }

        public Object CopyObject()
        {
            MonsterInventory newMI = new MonsterInventory();
            
            foreach(Item tempItem in items)
            {
                newMI.addItem((Item)tempItem.CopyObject());
            }

            foreach(ItemSlot tempSlot in Enum.GetValues(typeof(ItemSlot)).Cast<ItemSlot>())
            {
                Item newItem = this.getItem(tempSlot);
                if(newItem != null)
                {
                    newMI.equipItem((Item)newItem.CopyObject(), tempSlot);
                }
                else
                {
                    newMI.equipItem(null, tempSlot);
                }
            }
            newMI.wealth = wealth;
            newMI.maxWealth = maxWealth;
            return newMI;
        }
    }
}
