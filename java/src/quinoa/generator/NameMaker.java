package quinoa.generator;

public class NameMaker
{
    
    public static enum AdjectivePlace {DUSTY, OLDE, SOGGY, LOYAL, ROCKY, RICKETY, RUSTY,
                                      BROWN, GREEN, TAN, LIVELY, BLASTED, BROKEN, SHADY};
    public static enum NounPlace {TRAIL, BEND, WOOD, ROAD, CUT, VALLEY, PASS, ROCK, PITCH,
                                 FIELD, PLACE, SHELF, RAVINE, POINT, HILL, BROOK, RIDGE};
    public static enum AdjectiveFood {GREASY, SAVOURY, TASTY, RIPE, HUNGRY, SALTY, CRAFTY, MELLOW};
    public static enum NounFood {SPOON, BIRD, TABLE, PLATE, POT, PAN, HALL, SHACK};
    public static enum NicknameFood {PAPA, MAMA, BUBBA, CAPTAIN};

    public static String easyCase(String string)
    {
        String newString = string.toLowerCase().replace("_", " ");
        newString = newString.substring(1);
        newString = string.toUpperCase().charAt(0) + newString;
        return newString;
    }
                                 
    public static String makeTrivialPlaceName()
    {
        AdjectivePlace ap = AdjectivePlace.values()[(int)(Math.random() * AdjectivePlace.values().length)];
        NounPlace np = NounPlace.values()[(int)(Math.random() * NounPlace.values().length)];
        return easyCase(ap.name()) + " " + easyCase(np.name());
    }

    public static String makeFoodName()
    {
        AdjectiveFood ap = AdjectiveFood.values()[(int)(Math.random() * AdjectiveFood.values().length)];
        NounFood np = NounFood.values()[(int)(Math.random() * NounFood.values().length)];
        NicknameFood nick = NicknameFood.values()[(int)(Math.random() * NicknameFood.values().length)];
        if(Math.random() > 0.05)
        {
            return "The " + easyCase(ap.name()) + " " + easyCase(np.name());
        }
        else
        {
            return easyCase(nick.name()) + "'s " + easyCase(ap.name()) + " " + easyCase(np.name());
        }
    }

    public static String makeBankName()
    {
        double rnd = Math.random();
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
