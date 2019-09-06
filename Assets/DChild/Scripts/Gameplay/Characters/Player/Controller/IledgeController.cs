using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILedgeController  : ISubController
{
    event EventAction<EventActionArgs> LedgeGrabCall;
}
