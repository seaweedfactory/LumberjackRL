package quinoa;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.util.ArrayList;

public class MessageManager implements Storable
{
    private ArrayList<Message> messages;
    private int dialogIndex; //If dialog text has been added, this is where it starts.
    private static final int BUFFER_SIZE=100;
    private static final int LINE_SIZE=50;

    public MessageManager()
    {
        dialogIndex = 0;
        messages = new ArrayList<Message>();
        for(int i=0; i < BUFFER_SIZE; i++)
        {
            messages.add(new Message());
        }
    }

    public void clearMessages()
    {
        messages = new ArrayList<Message>();
        for(int i=0; i < BUFFER_SIZE; i++)
        {
            messages.add(new Message());
        }
    }

    public int getDialogIndex()
    {
        return dialogIndex;
    }

    public int getDialogSize()
    {
        return messages.size() - dialogIndex;
    }

    public void dialogLastMessages()
    {
        dialogIndex = messages.size() - 30;
    }

    public void addMessage(String message)
    {
        if(message.length() <= LINE_SIZE)
        {
            if(message.equals(this.messages.get(messages.size()-1).message))
            {
                this.messages.get(messages.size()-1).times++;
            }
            else
            {
                dialogIndex = messages.size() - 1;
                messages.add(new Message(message));
                messages.remove(0);
            }
        }
        else
        {
            int linesCreated=0;
            String[] words = message.split(" ");
            int wordIndex=0;
            String newString = "";
            while(wordIndex < words.length)
            {
                if((newString + words[wordIndex] + " ").length() >= LINE_SIZE)
                {
                    messages.add(new Message(newString));
                    messages.remove(0);
                    newString = "";
                    linesCreated++;
                }
                else
                {
                    newString = newString + words[wordIndex] + " ";
                    wordIndex++;
                }
            }

            if(newString.length() > 0)
            {
                messages.add(new Message(newString));
                messages.remove(0);
                linesCreated++;
            }

            dialogIndex = messages.size() - linesCreated;
        }
    }

    public String getMessage(int lineCount)
    {
        String message = messages.get((BUFFER_SIZE -1) - lineCount).message;
        if(messages.get((BUFFER_SIZE -1) - lineCount).times > 1)
        {
            message = message + " (x" + messages.get((BUFFER_SIZE -1) - lineCount).times + ")";
        }
        return message;
    }

    public void save(BufferedWriter out) throws Exception
    {
        out.write(dialogIndex + ""); out.newLine();
        out.write(messages.size() + ""); out.newLine();
        for(Message message : messages)
        {
            message.save(out);
        }
    }

    public void load(BufferedReader in) throws Exception
    {
        dialogIndex = Integer.parseInt(in.readLine());
        messages = new ArrayList<Message>();
        int messageSize = Integer.parseInt(in.readLine());
        for(int i=0; i < messageSize; i++)
        {
            Message newMessage = new Message();
            newMessage.load(in);
            messages.add(newMessage);
        }
    }
    
}
