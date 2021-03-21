using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Utilities;
using System.IO;

namespace LumberjackRL.Core.Items
{
    public class Item : IRemoveObject, IStoreObject, ICopyObject
    {
        private ItemClass m_itemClass;

        public ItemCategory itemCategory
        {
            get;
            set;
        }

        public ItemClass itemClass
        {
            get
            {
                return m_itemClass;
            }

            set
            {
                m_itemClass = value;
                ItemManager.initialize(this);
            }
        }

        public ItemState itemState
        {
            get;
            set;
        }
        
        public bool ShouldBeRemoved
        {
            get
            {
                return (itemState == ItemState.DESTROYED);
            }
        }

        public int x
        {
            get;
            set;
        }
        
        public int y
        {
            get;
            set;
        }
        
        public String ID
        {
            get;
            set;
        }
        
        public int maxStackSize
        {
            get;
            set;
        }
        
        public int stackSize
        {
            get;
            set;
        }
        
        public double worth
        {
            get;
            set;
        }

        public List<ItemAttribute> attributes
        {
            get;
            set;
        }

        public Item()
        {
            x = 0;
            y = 0;
            itemState = ItemState.GROUND;
            itemCategory = ItemCategory.TOOL;
            ID = RandomNumber.RandomUUID().ToString();
            maxStackSize = 1;
            stackSize = 1;
            worth = 0.01;
            attributes = new List<ItemAttribute>();
            itemClass = ItemClass.LOG;
        }

        /// <summary>
        /// Make a new ID for the item.
        /// </summary>
        public void refreshID()
        {
            ID = RandomNumber.RandomUUID().ToString();
        }

        public void setPosition(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public void RemoveObject()
        {
            itemState = ItemState.DESTROYED;
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(itemCategory.ToString());
            outStream.WriteLine(itemClass.ToString());
            outStream.WriteLine(itemState.ToString());
            outStream.WriteLine(this.x + "");
            outStream.WriteLine(this.y + "");
            outStream.WriteLine(this.ID);
            outStream.WriteLine(this.maxStackSize + "");
            outStream.WriteLine(this.stackSize + "");
            outStream.WriteLine(this.worth + "");
            outStream.WriteLine(this.attributes.Count + "");
            foreach(ItemAttribute tempIA in attributes)
            {
                tempIA.SaveObject(outStream);
            }
        }

        public void LoadObject(StreamReader inStream)
        {
            this.itemCategory = (ItemCategory)Enum.Parse(typeof(ItemCategory), inStream.ReadLine());
            this.itemClass = (ItemClass)Enum.Parse(typeof(ItemClass), inStream.ReadLine());
            this.itemState = (ItemState)Enum.Parse(typeof(ItemState), inStream.ReadLine());
            this.x = Int32.Parse(inStream.ReadLine());
            this.y = Int32.Parse(inStream.ReadLine());
            this.ID = inStream.ReadLine();
            this.maxStackSize = Int32.Parse(inStream.ReadLine());
            this.stackSize = Int32.Parse(inStream.ReadLine());
            this.worth = Double.Parse(inStream.ReadLine());
            attributes = new List<ItemAttribute>();
            int attributeSize = Int32.Parse(inStream.ReadLine());
            for (int i = 0; i < attributeSize; i++)
            {
                ItemAttribute newIA = new ItemAttribute();
                newIA.type = (ItemAttributeType)Enum.Parse(typeof(ItemAttributeType), inStream.ReadLine());
                newIA.parameter = inStream.ReadLine();
                attributes.Add(newIA);
            }
        }

        public Object CopyObject()
        {
            Item newItem = new Item();
            newItem.itemCategory = itemCategory;
            newItem.itemClass = itemClass;
            newItem.itemState = itemState;
            newItem.x = x;
            newItem.y = y;
            newItem.ID = ID;
            newItem.maxStackSize = maxStackSize;
            newItem.stackSize = stackSize;
            newItem.worth = worth;
            newItem.attributes.Clear();
            foreach(ItemAttribute tempIA in attributes)
            {
                newItem.attributes.Add((ItemAttribute)tempIA.CopyObject());
            }
            return newItem;
        }
    }
}
