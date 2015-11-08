package quinoa.items;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import quinoa.Copyable;
import quinoa.Storable;

public class ItemAttribute implements Storable, Copyable
{
    public enum ItemAttributeType{NULL, 
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
                                  SLEEPS_FOR,}

    private ItemAttributeType type;
    private String parameter;

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

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getType().name()); out.newLine();
        out.write(getParameter()); out.newLine();
    }

    public void load(BufferedReader in) throws Exception
    {
        setType(ItemAttributeType.valueOf(in.readLine()));
        setParameter(in.readLine());
    }

    public Object copy()
    {
        ItemAttribute newIA = new ItemAttribute();
        newIA.setType(this.getType());
        newIA.setParameter(this.getParameter());
        return newIA;
    }

    /**
     * @return the type
     */
    public ItemAttributeType getType() {
        return type;
    }

    /**
     * @param type the type to set
     */
    public void setType(ItemAttributeType type) {
        this.type = type;
    }

    /**
     * @return the parameter
     */
    public String getParameter() {
        return parameter;
    }

    /**
     * @param parameter the parameter to set
     */
    public void setParameter(String parameter) {
        this.parameter = parameter;
    }
}
