package quinoa;

import java.io.BufferedReader;
import java.io.BufferedWriter;

public interface Storable
{
    public void save(BufferedWriter out) throws Exception;
    public void load(BufferedReader in) throws Exception;
}
