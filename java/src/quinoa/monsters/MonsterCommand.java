package quinoa.monsters;
import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.util.HashMap;
import quinoa.Copyable;
import quinoa.Storable;
import quinoa.monsters.MonsterActionManager.MonsterCommandCode;

public class MonsterCommand implements Storable, Copyable
{
    private MonsterCommandCode commandCode;
    private int counter;
    private HashMap<String, String> parameters;

    public MonsterCommand()
    {
        commandCode = null;
        counter = 0;
        parameters = new HashMap<String, String>();
    }

    public void addParameter(String name, String value)
    {
        parameters.put(name, value);
    }

    public String getParameter(String name)
    {
        return parameters.get(name);
    }

    public boolean readyToExecute()
    {
        if(getCounter() <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void cycle()
    {
        if(counter > 0)
        {
            counter--;
        }
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(getCommandCode().name()); out.newLine();
        out.write(getCounter() + ""); out.newLine();
        out.write(getParameters().size() + ""); out.newLine();
        for(Object tempParameter : getParameters().keySet().toArray())
        {
            String parameterKey = (String)tempParameter;
            out.write(parameterKey); out.newLine();
            out.write(getParameters().get(parameterKey)); out.newLine();
        }
    }

    public void load(BufferedReader in) throws Exception
    {
        setCommandCode(getCommandCode().valueOf(in.readLine()));
        setCounter(Integer.parseInt(in.readLine()));
        setParameters(new HashMap<String, String>());
        int parametersSize = Integer.parseInt(in.readLine());
        for(int i=0; i < parametersSize; i++)
        {
            getParameters().put(in.readLine(), in.readLine());
        }
    }

    public Object copy()
    {
        MonsterCommand newCommand = new MonsterCommand();
        newCommand.setCommandCode(this.commandCode);
        newCommand.setCounter(this.counter);
        for(Object tempParameter : getParameters().keySet().toArray())
        {
            String parameterKey = (String)tempParameter;
            String parameterValue = this.getParameters().get(parameterKey);
            newCommand.getParameters().put(parameterKey, parameterValue);
        }

        return newCommand;
    }

    /**
     * @return the commandCode
     */
    public MonsterCommandCode getCommandCode() {
        return commandCode;
    }

    /**
     * @param commandCode the commandCode to set
     */
    public void setCommandCode(MonsterCommandCode commandCode) {
        this.commandCode = commandCode;
    }

    /**
     * @return the counter
     */
    public int getCounter() {
        return counter;
    }

    /**
     * @param counter the counter to set
     */
    public void setCounter(int counter) {
        this.counter = counter;
    }

    /**
     * @return the parameters
     */
    public HashMap<String, String> getParameters() {
        return parameters;
    }

    /**
     * @param parameters the parameters to set
     */
    public void setParameters(HashMap<String, String> parameters) {
        this.parameters = parameters;
    }
}
