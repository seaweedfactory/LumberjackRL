package quinoa.region;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.util.HashMap;
import quinoa.Quinoa;
import quinoa.Storable;

public class RegionMap implements Storable
{
    private HashMap<String, RegionHeader> regions;
    private String currentRegionID;
    private OverworldCell[][] overworldCells;
    private int overworldWidth;
    private int overworldHeight;
    
    public RegionMap()
    {
        regions = new HashMap<String, RegionHeader>();
        currentRegionID = "";
        overworldWidth = Quinoa.OVERWORLD_WIDTH;
        overworldHeight = Quinoa.OVERWORLD_HEIGHT;
        overworldCells = new OverworldCell[overworldWidth][overworldHeight];

        for(int x=0; x < overworldWidth; x++)
        {
            for(int y=0; y < overworldHeight; y++)
            {
                overworldCells[x][y] = new OverworldCell();
            }
        }
    }
    
    public void changeCurrentRegion(String newRegionID) throws Exception
    {
        if(!currentRegionID.equals(""))
        {
            RegionHeader tempHeader = regions.get(getCurrentRegionID());
            if(tempHeader.regionIsLoaded())
            {
                tempHeader.storeRegion(true);
            }
            currentRegionID = newRegionID;
            tempHeader = regions.get(getCurrentRegionID());
            tempHeader.recallRegion();
        }
        else
        {
            currentRegionID = newRegionID;
            RegionHeader tempHeader = regions.get(getCurrentRegionID());
            tempHeader.recallRegion();
        }
    }

    public RegionHeader getCurrentRegionHeader()
    {
        RegionHeader tempHeader = regions.get(getCurrentRegionID());
        return tempHeader;
    }

    public void addRegionHeader(RegionHeader regionHeader) throws Exception
    {
        if(regions.containsKey(regionHeader.getId()))
        {
            throw new Exception("region ID already defined");
        }
        else
        {
            regions.put(regionHeader.getId(), regionHeader);
        }
    }

    public RegionHeader getRegionHeaderByID(String ID)
    {
        return regions.get(ID);
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getCurrentRegionID()); out.newLine();
        out.write(regions.size() + ""); out.newLine();
        for(Object regionKey : regions.keySet().toArray())
        {
            regions.get((String)regionKey).save(out);
        }

        out.write(getOverworldWidth() + ""); out.newLine();
        out.write(getOverworldHeight() + ""); out.newLine();
        for(int x=0; x < getOverworldWidth(); x++)
        {
            for(int y=0; y < getOverworldHeight(); y++)
            {
                getOverworldCells()[x][y].save(out);
            }
        }
    }

    public void load(BufferedReader in) throws Exception
    {
        currentRegionID = in.readLine();
        regions.clear();
        int regionsSize = Integer.parseInt(in.readLine());
        for(int i=0; i < regionsSize; i++)
        {
            RegionHeader tempRegionHeader = new RegionHeader("");
            tempRegionHeader.load(in);
            regions.put(tempRegionHeader.getId(), tempRegionHeader);
        }
        setOverworldWidth(Integer.parseInt(in.readLine()));
        setOverworldHeight(Integer.parseInt(in.readLine()));
        for(int x=0; x < getOverworldWidth(); x++)
        {
            for(int y=0; y < getOverworldHeight(); y++)
            {
                getOverworldCells()[x][y].load(in);
            }
        }
    }

    /**
     * @return the currentRegionID
     */
    public String getCurrentRegionID() {
        return currentRegionID;
    }

    /**
     * @return the overworldCells
     */
    public OverworldCell[][] getOverworldCells() {
        return overworldCells;
    }

    /**
     * @param overworldCells the overworldCells to set
     */
    public void setOverworldCells(OverworldCell[][] overworldCells) {
        this.overworldCells = overworldCells;
    }

    /**
     * @return the overworldWidth
     */
    public int getOverworldWidth() {
        return overworldWidth;
    }

    /**
     * @param overworldWidth the overworldWidth to set
     */
    public void setOverworldWidth(int overworldWidth) {
        this.overworldWidth = overworldWidth;
    }

    /**
     * @return the overworldHeight
     */
    public int getOverworldHeight() {
        return overworldHeight;
    }

    /**
     * @param overworldHeight the overworldHeight to set
     */
    public void setOverworldHeight(int overworldHeight) {
        this.overworldHeight = overworldHeight;
    }

}
