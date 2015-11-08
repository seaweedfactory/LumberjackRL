package quinoa.items;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.util.ArrayList;
import java.util.UUID;
import quinoa.Copyable;
import quinoa.Removable;
import quinoa.Storable;
import quinoa.items.ItemManager.ItemCategory;
import quinoa.items.ItemManager.ItemClass;
import quinoa.items.ItemManager.ItemState;

public class Item implements Removable, Storable, Copyable
{
    private ItemCategory itemCategory;
    private ItemClass itemClass;
    private ItemState itemState;
    private int x, y;
    private String ID;
    private int maxStackSize;
    private int stackSize;
    private double worth;
    private ArrayList<ItemAttribute> attributes;

    public Item()
    {
        x = 0;
        y = 0;
        itemState = ItemState.GROUND;
        itemCategory = ItemCategory.TOOL;
        ID = UUID.randomUUID().toString();
        maxStackSize = 1;
        stackSize = 1;
        worth = 0.01;
        attributes = new ArrayList<ItemAttribute>();
        setItemClass(ItemClass.LOG);
    }

    //Make a new ID for the item
    public void refreshID()
    {
        ID = UUID.randomUUID().toString();
    }

    public void setPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public boolean shouldBeRemoved()
    {
        if(getItemState() == ItemState.DESTROYED)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void remove()
    {
        setItemState(ItemState.DESTROYED);
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(this.getItemCategory().name()); out.newLine();
        out.write(this.getItemClass().name()); out.newLine();
        out.write(this.getItemState().name()); out.newLine();
        out.write(this.getX() + ""); out.newLine();
        out.write(this.getY() + ""); out.newLine();
        out.write(this.getID()); out.newLine();
        out.write(this.maxStackSize + ""); out.newLine();
        out.write(this.stackSize + ""); out.newLine();
        out.write(this.getWorth() + ""); out.newLine();
        out.write(this.getAttributes().size() + ""); out.newLine();
        for(ItemAttribute tempIA : getAttributes())
        {
            tempIA.save(out);
        }
    }

    public void load(BufferedReader in) throws Exception
    {
        this.setItemCategory(getItemCategory().valueOf(in.readLine()));
        this.setItemClass(getItemClass().valueOf(in.readLine()));
        this.setItemState(getItemState().valueOf(in.readLine()));
        this.x = Integer.parseInt(in.readLine());
        this.y = Integer.parseInt(in.readLine());
        this.ID = in.readLine();
        this.maxStackSize = Integer.parseInt(in.readLine());
        this.stackSize = Integer.parseInt(in.readLine());
        this.setWorth(Double.parseDouble(in.readLine()));
        attributes = new ArrayList<ItemAttribute>();
        int attributeSize = Integer.parseInt(in.readLine());
        for(int i=0; i < attributeSize; i++)
        {
            ItemAttribute newIA = new ItemAttribute();
            newIA.setType(ItemAttribute.ItemAttributeType.valueOf(in.readLine()));
            newIA.setParameter(in.readLine());
            getAttributes().add(newIA);
        }
    }

    public Object copy()
    {
        Item newItem = new Item();
        newItem.setItemCategory(this.getItemCategory());
        newItem.setItemClass(this.getItemClass());
        newItem.setItemState(this.getItemState());
        newItem.x = this.getX();
        newItem.y = this.getY();
        newItem.ID = this.getID();
        newItem.maxStackSize = this.getMaxStackSize();
        newItem.stackSize = this.getStackSize();
        newItem.setWorth(this.getWorth());
        newItem.getAttributes().clear();
        for(ItemAttribute tempIA : this.getAttributes())
        {
            newItem.getAttributes().add((ItemAttribute)tempIA.copy());
        }
        return newItem;
    }

    /**
     * @return the itemCategory
     */
    public ItemCategory getItemCategory() {
        return itemCategory;
    }

    /**
     * @return the itemClass
     */
    public ItemClass getItemClass() {
        return itemClass;
    }

    /**
     * @return the itemState
     */
    public ItemState getItemState() {
        return itemState;
    }

    /**
     * @param itemState the itemState to set
     */
    public void setItemState(ItemState itemState) {
        this.itemState = itemState;
    }

    /**
     * @return the x
     */
    public int getX() {
        return x;
    }

    /**
     * @return the y
     */
    public int getY() {
        return y;
    }

    /**
     * @return the ID
     */
    public String getID() {
        return ID;
    }

    /**
     * @param itemCategory the itemCategory to set
     */
    public void setItemCategory(ItemCategory itemCategory) {
        this.itemCategory = itemCategory;
    }

    /**
     * @param itemClass the itemClass to set
     */
    public void setItemClass(ItemClass itemClass)
    {
        this.itemClass = itemClass;
        ItemManager.initialize(this);
    }

    /**
     * @return the maxStackSize
     */
    public int getMaxStackSize() {
        return maxStackSize;
    }

    /**
     * @param maxStackSize the maxStackSize to set
     */
    public void setMaxStackSize(int maxStackSize) {
        this.maxStackSize = maxStackSize;
    }

    /**
     * @return the stackSize
     */
    public int getStackSize() {
        return stackSize;
    }

    /**
     * @param stackSize the stackSize to set
     */
    public void setStackSize(int stackSize) {
        this.stackSize = stackSize;
    }

    /**
     * @return the attributes
     */
    public ArrayList<ItemAttribute> getAttributes() {
        return attributes;
    }

    /**
     * @return the worth
     */
    public double getWorth() {
        return worth;
    }

    /**
     * @param worth the worth to set
     */
    public void setWorth(double worth) {
        this.worth = worth;
    }
}
