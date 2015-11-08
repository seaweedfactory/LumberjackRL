package quinoa;

public interface Removable
{
    public boolean shouldBeRemoved();   //Should the object be removed?
    public void remove();               //Do any clean up before removing
}
