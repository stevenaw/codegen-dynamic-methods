using System;

namespace DynamicMethodGeneration
{
    internal static class Guard
    {
        internal static void CanBindInstance(Type declaringType, Type instanceType, bool isMemberStatic)
        {
            if (isMemberStatic)
                throw new InvalidOperationException("Can not bind instance to a static member");

            if (!declaringType.IsAssignableFrom(instanceType))
                throw new InvalidOperationException("That type is incompatible with this member");
        }

        public static void CanBindInstance<TInstance>(Type declaringType, bool isMemberStatic)
        {
            if (isMemberStatic)
                throw new InvalidOperationException("Can not bind instance to a static member");

            if (!declaringType.IsAssignableFrom(typeof(TInstance)))
                throw new InvalidOperationException("That type is incompatible with this member");
        }
    }
}
