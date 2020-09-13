using System;

namespace DevOpen.Framework.Domain
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class EventTypeAttribute : Attribute
    {
        public string Name { get; }

        public int Version { get; }

        public EventTypeAttribute(string name, int version = 1)
        {
            Name = name;
            Version = version;
        }
    }
}