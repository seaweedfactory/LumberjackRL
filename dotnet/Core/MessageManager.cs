using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LumberjackRL.Core
{
    public class MessageManager : IStoreObject
    {
        private List<Message> messages;
        private int dialogIndex; //If dialog text has been added, this is where it starts.
        private const int BUFFER_SIZE=100;
        private const int LINE_SIZE=50;

        public MessageManager()
        {
            dialogIndex = 0;
            messages = new List<Message>();
            for(int i=0; i < BUFFER_SIZE; i++)
            {
                messages.Add(new Message());
            }
        }

        public void clearMessages()
        {
            messages = new List<Message>();
            for(int i=0; i < BUFFER_SIZE; i++)
            {
                messages.Add(new Message());
            }
        }

        public int getDialogIndex()
        {
            return dialogIndex;
        }

        public int getDialogSize()
        {
            return messages.Count - dialogIndex;
        }

        public void dialogLastMessages()
        {
            dialogIndex = messages.Count - 30;
        }

        public void addMessage(String message)
        {
            if(message.Length <= LINE_SIZE)
            {
                if(message.Equals(this.messages[messages.Count-1].Text))
                {
                    this.messages[messages.Count-1].Times++;
                }
                else
                {
                    dialogIndex = messages.Count - 1;
                    messages.Add(new Message(message));
                    messages.RemoveAt(0);
                }
            }
            else
            {
                int linesCreated=0;
                String[] words = message.Split(new char[]{' '});
                int wordIndex=0;
                String newString = "";
                while(wordIndex < words.Length)
                {
                    if((newString + words[wordIndex] + " ").Length >= LINE_SIZE)
                    {
                        messages.Add(new Message(newString));
                        messages.RemoveAt(0);
                        newString = "";
                        linesCreated++;
                    }
                    else
                    {
                        newString = newString + words[wordIndex] + " ";
                        wordIndex++;
                    }
                }

                if(newString.Length > 0)
                {
                    messages.Add(new Message(newString));
                    messages.RemoveAt(0);
                    linesCreated++;
                }

                dialogIndex = messages.Count - linesCreated;
            }
        }

        public String getMessage(int lineCount)
        {
            String message = messages[(BUFFER_SIZE -1) - lineCount].Text;
            if(messages[(BUFFER_SIZE -1) - lineCount].Times > 1)
            {
                message = message + " (x" + messages[(BUFFER_SIZE -1) - lineCount].Times + ")";
            }
            return message;
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(dialogIndex + "");
            outStream.WriteLine(messages.Count + "");
            foreach(Message message in messages)
            {
                message.SaveObject(outStream);
            }
        }

        public void LoadObject(StreamReader inStream)
        {
            dialogIndex = Int32.Parse(inStream.ReadLine());
            messages = new List<Message>();
            int messageSize = Int32.Parse(inStream.ReadLine());
            for(int i=0; i < messageSize; i++)
            {
                Message newMessage = new Message();
                newMessage.LoadObject(inStream);
                messages.Add(newMessage);
            }
        }
    }
}
