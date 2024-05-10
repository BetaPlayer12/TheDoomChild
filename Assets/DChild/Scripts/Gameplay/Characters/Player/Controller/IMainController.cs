using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IMainController
    {
        void Enable();
        void Disable();
        event EventAction<EventActionArgs> ControllerDisabled;
        event EventAction<EventActionArgs> ControllerEnabled;
    }
}