package quinoa.region;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import quinoa.Storable;
import quinoa.region.RegionHeader;

public class OverworldCell implements Storable
{

    public static enum CellType {FOREST, MAIN_TOWN, TOWN, NULL};
    
    public RegionHeader header; //non-peristant, only used during generation

    public CellType cellType;
    public boolean nExit;
    public boolean eExit;
    public boolean sExit;
    public boolean wExit;
    public int depth;

    public OverworldCell()
    {
        header = null;
        cellType = CellType.NULL;

        nExit = false;
        eExit = false;
        sExit = false;
        wExit = false;
        depth = 0;
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(cellType.name()); out.newLine();
        out.write(Boolean.toString(nExit)); out.newLine();
        out.write(Boolean.toString(eExit)); out.newLine();
        out.write(Boolean.toString(sExit)); out.newLine();
        out.write(Boolean.toString(wExit)); out.newLine();
        out.write(depth + ""); out.newLine();
    }

    public void load(BufferedReader in) throws Exception
    {
        cellType = CellType.valueOf(in.readLine());
        nExit = Boolean.valueOf(in.readLine());
        eExit = Boolean.valueOf(in.readLine());
        sExit = Boolean.valueOf(in.readLine());
        wExit = Boolean.valueOf(in.readLine());
        depth = Integer.parseInt(in.readLine());
    }
}
