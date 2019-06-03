using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChildDebug.Gameplay
{
    public interface IHoodless : IEventActionArgs
    {
        event EventAction<IHoodless> HoodlessToggled;
        bool isHoodless { get; }
        Vector2 position { get; }
    }
}
