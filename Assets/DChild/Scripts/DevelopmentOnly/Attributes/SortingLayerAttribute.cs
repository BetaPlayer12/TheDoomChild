using System;

namespace DChild
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class SortingLayerAttribute : Attribute
    { }
}