using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMainController 
{
    event EventAction<EventActionArgs> ControllerDisabled;
}
