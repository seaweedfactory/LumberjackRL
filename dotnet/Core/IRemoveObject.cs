using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumberjackRL.Core
{
    public interface IRemoveObject
    {
        bool ShouldBeRemoved     //Should the object be removed?
        {
            get;
        }   

        void RemoveObject();     //Do any clean up before removing
    }
}
