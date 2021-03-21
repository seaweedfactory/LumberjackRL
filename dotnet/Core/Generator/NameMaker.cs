using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Generator
{

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
            AdjectivePlaceType ap = EnumUtil.RandomEnumValue<AdjectivePlaceType>();
            NounPlaceType np = EnumUtil.RandomEnumValue <NounPlaceType>();
            return easyCase(ap.ToString() + " " + easyCase(np.ToString()));
        }

        public static String makeFoodName()
        {
            RandomNumberGenerator rng = new RandomNumberGenerator();
            AdjectivePlaceType ap = EnumUtil.RandomEnumValue<AdjectivePlaceType>();
            NounFoodType np = EnumUtil.RandomEnumValue<NounFoodType>();
            NicknameFoodType nick = EnumUtil.RandomEnumValue<NicknameFoodType>();
            if(rng.RandomDouble() > 0.05)
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
            RandomNumberGenerator rng = new RandomNumberGenerator();
            double rnd = rng.RandomDouble();
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
