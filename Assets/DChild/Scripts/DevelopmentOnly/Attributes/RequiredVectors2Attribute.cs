using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class RequiredVectors2Attribute : Attribute
    {
        public RequiredVectors2Attribute(uint size)
        {
            this.size = size;
        }

        public uint size { get; }
    }


}