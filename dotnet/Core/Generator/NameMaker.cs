using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Generator
{
    public enum AdjectivePlace 
    {
        DUSTY, 
        OLDE, 
        SOGGY, 
        LOYAL,
        ROCKY, 
        RICKETY, 
        RUSTY,
        BROWN, 
        GREEN, 
        TAN, 
        LIVELY, 
        BLASTED, 
        BROKEN, 
        SHADY
    }

    public enum NounPlace 
    {
        TRAIL, 
        BEND, 
        WOOD, 
        ROAD, 
        CUT, 
        VALLEY, 
        PASS,
        ROCK, 
        PITCH,
        FIELD, 
        PLACE, 
        SHELF, 
        RAVINE, 
        POINT, 
        HILL, 
        BROOK, 
        RIDGE
    }
    
    public enum AdjectiveFood 
    {
        GREASY, 
        SAVOURY, 
        TASTY, 
        RIPE, 
        HUNGRY, 
        SALTY, 
        CRAFTY, 
        MELLOW
    }
    
    public enum NounFood 
    {
        SPOON, 
        BIRD, 
        TABLE, 
        PLATE, 
        POT, 
        PAN, 
        HALL, 
        SHACK
    }
    
    public enum NicknameFood 
    {
        PAPA, 
        MAMA, 
        BUBBA, 
        CAPTAIN
    }

    public class NameMaker
    {
        public static String easyCase(String tmpStr)
        {
            String newString = tmpStr.ToLower().Replace("_", " ");
            newString = newString.Substring(0,1);
            newString = tmpStr.ToUpper().ToCharArray()[0] + newString;
            return newString;
        }
                                 
        public static String makeTrivialPlaceName()
        {
            AdjectivePlace ap = EnumUtil.RandomEnumValue<AdjectivePlace>();
            NounPlace np = EnumUtil.RandomEnumValue <NounPlace>();
            return easyCase(ap.ToString() + " " + easyCase(np.ToString()));
        }

        public static String makeFoodName()
        {
            AdjectivePlace ap = EnumUtil.RandomEnumValue<AdjectivePlace>();
            NounFood np = EnumUtil.RandomEnumValue<NounFood>();
            NicknameFood nick = EnumUtil.RandomEnumValue<NicknameFood>();
            if(RandomNumber.RandomDouble() > 0.05)
            {
                return "The " + easyCase(ap.ToString()) + " " + easyCase(np.ToString());
            }
            else
            {
                return easyCase(nick.ToString()) + "'s " + easyCase(ap.ToString()) + " " + easyCase(np.ToString());
            }
        }

        public static String makeBankName()
        {
            double rnd = RandomNumber.RandomDouble();
            if(rnd < 0.25)
            {
                return NameMaker.makeTrivialPlaceName() + " Bank";
            }
            else if(rnd < 0.50)
            {
                return NameMaker.makeTrivialPlaceName() + " Savings & Loans";
            }
            else
            {
                return NameMaker.makeTrivialPlaceName() + " Credit Agency";
            }
        }
    }
}
