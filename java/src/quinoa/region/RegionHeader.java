package quinoa.region;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.FileReader;
import java.io.FileWriter;
import java.util.ArrayList;
import quinoa.Storable;

public class RegionHeader implements Storable
{
    private String id;                      //ID of the region, used for linking
    private ArrayList<RegionExit> exits;    //Exit data
    private Region region;                  //Terrain level data, loaded/saved as needed
    private String name;                    //Name of the region, if possible
        
    public RegionHeader(String id)
    {
        this.id = id;
        exits = new ArrayList<RegionExit>();
        region = null;
        name = "";
    }

    public boolean regionIsLoaded()
    {
        return (getRegion() != null);
    }

    public void storeRegion(boolean unloadRegionAfterSave) throws Exception
    {
        if(regionIsLoaded())
        {
            //Remove expired or copied items
            getRegion().removeExpiredObjects();

            //Write to region file
            BufferedWriter out = new BufferedWriter(new FileWriter(this.getId() + ".region"));
            getRegion().save(out);
            out.flush();
            out.close();

            if(unloadRegionAfterSave)
            {
                setRegion(null);
            }
        }
        else
        {
            throw new Exception("tried to store null region");
        }
    }

    public void recallRegion() throws Exception
    {
        if(!regionIsLoaded())
        {
            setRegion(new Region(1, 1));
            BufferedReader in = new BufferedReader(new FileReader(this.getId() + ".region"));
            getRegion().load(in);
            in.close();
        }
        else
        {
            throw new Exception("tried to recall non-null region");
        }
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getId()); out.newLine();
        out.write(getName()); out.newLine();
        out.write(getExits().size()+""); out.newLine();
        for(RegionExit tempExit : getExits())
        {
            tempExit.save(out);
        }
    }

    public void load(BufferedReader in) throws Exception
    {
        id = in.readLine();
        setName(in.readLine());
        exits = new ArrayList<RegionExit>();
        int exitsSize = Integer.parseInt(in.readLine());
        for(int i=0; i < exitsSize; i++)
        {
            RegionExit newExit = new RegionExit();
            newExit.load(in);
            getExits().add(newExit);
        }
    }

    /**
     * @return the id
     */
    public String getId() {
        return id;
    }

    /**
     * @return the exits
     */
    public ArrayList<RegionExit> getExits() {
        return exits;
    }

    /**
     * @return the region
     */
    public Region getRegion() {
        return region;
    }

    /**
     * @param region the region to set
     */
    public void setRegion(Region region) {
        this.region = region;
    }

    /**
     * @return the name
     */
    public String getName() {
        return name;
    }

    /**
     * @param name the name to set
     */
    public void setName(String name) {
        this.name = name;
    }

}
