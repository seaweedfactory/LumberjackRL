using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LumberjackRL.Core.Utilities;

namespace LumberjackRL.Core.Monsters
{
    public class MonsterCommand : IStoreObject, ICopyObject
    {
        public MonsterCommandCode commandCode
        {
            get;
            set;
        }

        public Dictionary<String, String> parameters
        {
            get;
            set;
        }

        public int counter
        {
            get;
            set;
        }

        public MonsterCommand()
        {
            commandCode = MonsterCommandCode.NULL;
            counter = 0;
            parameters = new Dictionary<String, String>();
        }

        public void addParameter(String name, String value)
        {
            parameters.Add(name, value);
        }

        public String getParameter(String name)
        {
            return parameters[name];
        }

        public bool readyToExecute()
        {
            return (counter <= 0);
        }

        public void cycle()
        {
            if(counter > 0)
            {
                counter--;
            }
        }

        public void SaveObject(StreamWriter outStream)
        {
            outStream.WriteLine(EnumUtil.EnumName<MonsterCommandCode>(commandCode));
            outStream.WriteLine(counter + "");
            outStream.WriteLine(parameters.Count + "");
            foreach(String parameterKey in parameters.Keys)
            {
                outStream.WriteLine(parameterKey);
                outStream.WriteLine(parameters[parameterKey]);
            }
        }

        public void LoadObject(StreamReader inStream)
        {
            commandCode = (MonsterCommandCode)Enum.Parse(typeof(MonsterCommandCode), inStream.ReadLine());
            counter = Int32.Parse(inStream.ReadLine());
            parameters = new Dictionary<String, String>();
            int parametersSize = Int32.Parse(inStream.ReadLine());
            for (int i = 0; i < parametersSize; i++)
            {
                parameters.Add(inStream.ReadLine(), inStream.ReadLine());
            }
        }

        public Object CopyObject()
        {
            MonsterCommand newCommand = new MonsterCommand();
            newCommand.commandCode = commandCode;
            newCommand.counter = counter;

            newCommand.parameters = new Dictionary<String,String>();
            foreach(String tempKey in parameters.Keys.ToArray<String>())
            {
                newCommand.parameters.Add(tempKey, parameters[tempKey]);
            }

            return newCommand;
        }
    }
}
