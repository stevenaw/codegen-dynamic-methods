using System.Collections.Generic;
using System.Reflection;

namespace DynamicMethodGeneration
{
    internal class DynamicMethodCache
    {
        private Dictionary<MemberInfo, IMethod> _cache = new Dictionary<MemberInfo, IMethod>();

        public void Add(MemberInfo methodInfo, IMethod method)
        {
            if (!_cache.ContainsKey(methodInfo))
                _cache.Add(methodInfo, method);
        }

        public IMethod Get(MemberInfo methodInfo)
        {
            if (!_cache.ContainsKey(methodInfo))
                return null;

            return _cache[methodInfo];
        }
    }
}
