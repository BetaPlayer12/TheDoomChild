using System;

namespace DChild
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ItemDataIDAttribute : Attribute { }
}