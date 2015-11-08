using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LumberjackRL.Core
{
    public interface IStoreObject
    {
        void SaveObject(StreamWriter outStream);
        void LoadObject(StreamReader inStream);
    }
}
