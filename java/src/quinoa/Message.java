package quinoa;

import java.io.BufferedReader;
import java.io.BufferedWriter;

public class Message implements Storable, Copyable
{
    public String message;
    public int times;

    public Message()
    {
        message = "";
        times = 1;
    }

    public Message(String message)
    {
        this.message = message;
        this.times = 1;
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(message); out.newLine();
        out.write(times + ""); out.newLine();
    }

    public void load(BufferedReader in) throws Exception
    {
        message = in.readLine();
        times = Integer.parseInt(in.readLine());
    }

    public Object copy()
    {
        Message newMessage = new Message();
        newMessage.message = message;
        newMessage.times = times;
        return newMessage;
    }
}
