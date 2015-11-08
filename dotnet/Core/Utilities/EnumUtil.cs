using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LumberjackRL.Core.Utilities
{
    public class EnumUtil
    {
        public static T RandomEnumValue<T>()
        {
            Random tmpRnd = new Random();

            return Enum
                .GetValues(typeof(T))
                .Cast<T>()
                .OrderBy(x => tmpRnd.Next())
                .FirstOrDefault();
        }

        public static String EnumName<T>(T tmpEnum)
        {
            return Enum.GetName(typeof(T), tmpEnum);
        }
    }
}
