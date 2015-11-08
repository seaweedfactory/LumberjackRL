package quinoa.monsters;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.util.ArrayList;
import quinoa.Copyable;
import quinoa.Storable;
import quinoa.items.Item;
import quinoa.items.ItemManager.ItemState;

public class MonsterInventory implements Storable, Copyable
{
    public static final int MAX_WIDTH = 10;
    public static final int MAX_HEIGHT = 5;

    public static enum ItemSlot {BELT_1, BELT_2, BELT_3, BELT_4, HEAD, BODY, FEET};

    private ArrayList<Item> items;  //unequiped items
    private int width, height;      //height and width of the inventory
    private Item beltSlot1;         //individual equipment slots
    private Item beltSlot2;
    private Item beltSlot3;
    private Item beltSlot4;
    private Item rightHandSlot;
    private Item leftHandSlot;
    private Item headSlot;
    private Item bodySlot;
    private Item feetSlot;

    private double wealth;      //how much money is the monster carrying
    private double maxWealth;   //how much money can the monster carry

    public MonsterInventory()
    {
        items = new ArrayList<Item>();
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

    public boolean isFull()
    {
        if(items.size() < getWidth() * getHeight())
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void addItem(Item item)
    {
        //Find a free x, y slot
        boolean foundSlot=false;
        boolean[][] slots = new boolean[getWidth()][getHeight()];
        for(int y=0; y < getHeight(); y++)
        {
            for(int x=0; x < getWidth(); x++)
            {
                slots[x][y] = false;
            }
        }
        for(Item tmpItem : items)
        {
            slots[tmpItem.getX()][tmpItem.getY()] = true;
        }
        for(int y=0; y < getHeight(); y++)
        {
            for(int x=0; x < getWidth(); x++)
            {
                if(slots[x][y] == false && !foundSlot)
                {
                    item.setPosition(x, y);
                    foundSlot = true;
                }
            }
        }

        //set the state of the item to inside inventory
        item.setItemState(ItemState.INVENTORY);

        //actually add the item
        items.add(item);
    }

    public Item getItem(String itemID)
    {
        for(Item tempItem : items)
        {
            if(tempItem.getID().equals(itemID))
            {
                return tempItem;
            }
        }
        for(ItemSlot tempSlot : ItemSlot.values())
        {
            if(getItem(tempSlot) != null && getItem(tempSlot).getID().equals(itemID))
            {
                return getItem(tempSlot);
            }
        }
        return null;
    }

    public Item getItem(int x, int y)
    {
        for(Item tempItem : items)
        {
            if(tempItem.getX() == x && tempItem.getY() == y)
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
            case BELT_1:
            return this.beltSlot1;

            case BELT_2:
            return this.beltSlot2;

            case BELT_3:
            return this.beltSlot3;

            case BELT_4:
            return this.beltSlot4;

            case HEAD:
            return this.headSlot;

            case BODY:
            return this.bodySlot;

            case FEET:
            return this.feetSlot;
        }
        return null;
    }

    public void equipItem(Item item, ItemSlot slot)
    {
        switch(slot)
        {
            case BELT_1:
            this.beltSlot1 = item;
            break;

            case BELT_2:
            this.beltSlot2 = item;
            break;

            case BELT_3:
            this.beltSlot3 = item;
            break;

            case BELT_4:
            this.beltSlot4 = item;
            break;

            case HEAD:
            this.headSlot = item;
            break;

            case BODY:
            this.bodySlot = item;
            break;

            case FEET:
            this.feetSlot = item;
            break;
        }
    }

    public ArrayList<Item> getItems()
    {
        return items;
    }

    //return true to remove the item that is being stacked
    public boolean stackItem(Item newItem)
    {
        for(Item tempItem : items)
        {
            if(newItem.getItemClass() == tempItem.getItemClass()
            && tempItem.getStackSize() < tempItem.getMaxStackSize())
            {
                tempItem.setStackSize(tempItem.getStackSize() + newItem.getStackSize());
                if(tempItem.getStackSize() > tempItem.getMaxStackSize())
                {
                    newItem.setStackSize(tempItem.getStackSize() - tempItem.getMaxStackSize() );
                    tempItem.setStackSize(tempItem.getMaxStackSize());
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
        for(Item tempItem : items)
        {
            if(tempItem.getID().equals(itemID))
            {
                itemToRemove = tempItem;
            }
        }
        for(ItemSlot tempSlot: ItemSlot.values())
        {
            Item tempItem = getItem(tempSlot);
            if(tempItem != null && tempItem.getID().equals(itemID))
            {
                itemToRemove = tempItem;
            }
        }
        if(itemToRemove != null)
        {
            itemToRemove.remove();
        }
    }

    public void removedExpiredItems()
    {
        ArrayList<Item> removeItemList = new ArrayList<Item>();
        for(Item tempItem : items)
        {
            if(tempItem.shouldBeRemoved())
            {
                removeItemList.add(tempItem);
            }
        }
        for(Item tempItem : removeItemList)
        {
            items.remove(tempItem);
        }

        //remove items in each slot if applicable
        for(ItemSlot tempSlot : ItemSlot.values())
        {
            if(getItem(tempSlot) != null && getItem(tempSlot).shouldBeRemoved())
            {
                equipItem(null, tempSlot);
            }
        }
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getWidth() + ""); out.newLine();
        out.write(getHeight() + ""); out.newLine();
        out.write(getWealth() + ""); out.newLine();
        out.write(getMaxWealth() + ""); out.newLine();
        out.write(items.size() + ""); out.newLine();
        for(Item tempItem : items)
        {
            tempItem.save(out);
        }
        out.write(ItemSlot.values().length + ""); out.newLine();
        for(ItemSlot tempSlot : ItemSlot.values())
        {
            out.write(tempSlot.name()); out.newLine();
            if(getItem(tempSlot) != null)
            {
                out.write("true"); out.newLine();
                getItem(tempSlot).save(out);
            }
            else
            {
                out.write("false"); out.newLine();
            }
        }
    }

    public void load(BufferedReader in) throws Exception 
    {
        this.setDimensions(Integer.parseInt(in.readLine()), Integer.parseInt(in.readLine()));
        this.setWealth(Double.parseDouble(in.readLine()));
        this.setMaxWealth(Double.parseDouble(in.readLine()));
        items = new ArrayList<Item>();
        int itemsSize = Integer.parseInt(in.readLine());
        for(int i=0; i < itemsSize; i++)
        {
            Item newItem = new Item();
            newItem.load(in);
            items.add(newItem);
        }
        int slotArrayLength = Integer.parseInt(in.readLine());
        for(int i=0; i < slotArrayLength; i++)
        {
            ItemSlot tempSlot = ItemSlot.valueOf(in.readLine());
            boolean itemExists = Boolean.parseBoolean(in.readLine());
            Item newItem = new Item();
            if(itemExists)
            {
                newItem.load(in);
            }
            else
            {
                newItem = null;
            }
            equipItem(newItem, tempSlot);
        }
    }

    public Object copy()
    {
        MonsterInventory newMI = new MonsterInventory();
        for(Item tempItem : items)
        {
            newMI.addItem((Item)tempItem.copy());
        }
        for(ItemSlot tempSlot : ItemSlot.values())
        {
            Item newItem = this.getItem(tempSlot);
            if(newItem != null)
            {
                newMI.equipItem((Item)newItem.copy(), tempSlot);
            }
            else
            {
                newMI.equipItem(null, tempSlot);
            }
        }
        newMI.setWealth(this.getWealth());
        newMI.setMaxWealth(this.getMaxWealth());
        return newMI;
    }

    /**
     * @return the width
     */
    public int getWidth() {
        return width;
    }

    /**
     * @return the height
     */
    public int getHeight() {
        return height;
    }

    /**
     * @return the wealth
     */
    public double getWealth() {
        return wealth;
    }

    /**
     * @param wealth the wealth to set
     */
    public void setWealth(double wealth) {
        this.wealth = wealth;
    }

    /**
     * @return the maxWealth
     */
    public double getMaxWealth() {
        return maxWealth;
    }

    /**
     * @param maxWealth the maxWealth to set
     */
    public void setMaxWealth(double maxWealth) {
        this.maxWealth = maxWealth;
    }

}
