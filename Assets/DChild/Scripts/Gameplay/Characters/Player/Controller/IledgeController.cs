using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IledgeController 
{
    event EventAction<EventActionArgs> LedgeGrabCall;
}
