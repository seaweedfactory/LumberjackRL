using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LumberjackRL.Core
{
    public class Message : IStoreObject, ICopyObject
    {
        public String Text
        {
            get;
            set;
        }

        public int Times
        {
            get;
            set;
        }

        public Message()
        {
            Text = "";
            Times = 1;
        }

        public Message(String text)
        {
            this.Text = text;
            this.Times = 1;
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(Text);
            outStream.WriteLine(Times.ToString());
        }

        public void LoadObject(StreamReader inStream)
        {
            Text = inStream.ReadLine();
            Times = Int32.Parse(inStream.ReadLine());
        }

        public Object CopyObject()
        {
            Message newMessage = new Message();
            newMessage.Text = Text;
            newMessage.Times = Times;
            return newMessage;
        }

    }
}
