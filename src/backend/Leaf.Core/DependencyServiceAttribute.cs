using System;

namespace Leaf
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class DependencyServiceAttribute : Attribute
    {
        public DependencyServiceAttribute(Type implemenType = null, bool singleton = true)
        {
            ImplemenType = implemenType;
            Singleton = singleton;
        }

        public Type ImplemenType { get; }
        public bool Singleton { get; }
    }
}