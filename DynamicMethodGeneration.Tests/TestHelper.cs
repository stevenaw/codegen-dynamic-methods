using System;
using System.Collections.Generic;
using System.Text;

namespace DynamicMethodGeneration.Tests
{
    internal class TestHelper
    {
        public static object[] GetArgs(object[] args, object instance)
        {
            if (instance == null)
                return args ?? new object[0];
            else if (args == null)
                return new object[] { instance };
            else
                return ArrayHelper.Prepend(args, instance);
        }
    }
}
