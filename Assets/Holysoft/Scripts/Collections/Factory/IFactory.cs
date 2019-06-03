using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Holysoft.Collections
{
    public interface IFactory
    {
        event EventAction<UnityEventActionArgs<GameObject>> InstanceCreated;
    }
}