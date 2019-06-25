using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILedgeController 
{
    event EventAction<EventActionArgs> LedgeGrabCall;
}
