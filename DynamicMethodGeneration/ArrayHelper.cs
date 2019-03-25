using System;

namespace DynamicMethodGeneration
{
    internal static class ArrayHelper
    {
        public static T[] Prepend<T>(T[] items, T item)
        {
            var newItems = new T[items.Length + 1];
            Array.Copy(items, 0, newItems, 1, items.Length);

            newItems[0] = item;

            return newItems;
        }

        public static T[] Append<T>(T[] items, T item)
        {
            var newItems = new T[items.Length + 1];
            Array.Copy(items, 0, newItems, 0, items.Length);

            newItems[items.Length] = item;

            return newItems;
        }
    }
}
