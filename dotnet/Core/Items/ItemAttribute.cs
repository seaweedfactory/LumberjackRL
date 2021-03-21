using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Items
{
    public enum ItemAttributeType
    {
        NULL, 
        BONUS_TO_ATTACK,
        BONUS_TO_DEFENSE,
        NUTRITION,
        INCREASE_SPEED,
        DECREASE_SPEED,
        SLOW_HUNGER,
        QUICKEN_HUNGER,
        HEALS, 
        MAKES_LIGHT,
        PROTECT_AGAINST_FIRE,
        USES,
        SLEEPS_FOR
    }

    public class ItemAttribute: IStoreObject, ICopyObject
    {
        public ItemAttributeType type
        {
            get;
            set;
        }

        public String parameter
        {
            get;
            set;
        }

        public ItemAttribute()
        {
            type = ItemAttributeType.NULL;
            parameter = "";
        }

        public ItemAttribute(ItemAttributeType type, String parameter)
        {
            this.type = type;
            this.parameter = parameter;
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(type.ToString());
            outStream.WriteLine(parameter);
        }

        public void LoadObject(StreamReader inStream)
        {
            type = (ItemAttributeType)Enum.Parse(typeof(ItemAttributeType), inStream.ReadLine());
            parameter = inStream.ReadLine();
        }

        public Object CopyObject()
        {
            ItemAttribute newIA = new ItemAttribute();
            newIA.type = type;
            newIA.parameter = parameter;
            return newIA;
        }
    }
}
