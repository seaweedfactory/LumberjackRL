package quinoa.region;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.util.HashMap;
import quinoa.Storable;
import quinoa.region.TerrainManager.TerrainCode;
import quinoa.region.TerrainManager.TerrainParameter;

public class Terrain implements Storable
{
    private TerrainCode code;
    private int water;  //how much water is here?
    private HashMap<TerrainParameter,String> parameters;

    public Terrain()
    {
        code = TerrainCode.DIRT;
        water = 0;
        parameters = new HashMap<TerrainParameter,String>();
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getCode().name()); out.newLine();
        out.write(getWater() + ""); out.newLine();
        out.write(getParameters().size() + ""); out.newLine();
        for(Object tempKey : getParameters().keySet().toArray())
        {
            out.write(((TerrainParameter)tempKey).name()); out.newLine();
            out.write((String)getParameters().get((TerrainParameter)tempKey)); out.newLine();
        }
    }

    public void load(BufferedReader in) throws Exception
    {
        setCode(TerrainCode.valueOf(in.readLine()));
        setWater(Integer.parseInt(in.readLine()));
        getParameters().clear();
        int parameterSize = Integer.parseInt(in.readLine());
        for(int i=0; i < parameterSize; i++)
        {
            TerrainParameter newKey = TerrainParameter.valueOf(in.readLine());
            String newValue = in.readLine();
            getParameters().put(newKey, newValue);
        }
    }

    /**
     * @return the code
     */
    public TerrainCode getCode() {
        return code;
    }

    /**
     * @return the parameters
     */
    public HashMap<TerrainParameter, String> getParameters() {
        return parameters;
    }

    /**
     * @param code the code to set
     */
    public void setCode(TerrainCode code) {
        this.code = code;
    }

    /**
     * @return the water
     */
    public int getWater() {
        return water;
    }

    /**
     * @param water the water to set
     */
    public void setWater(int water) {
        this.water = water;
    }
}
