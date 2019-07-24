using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IMainController
    {
        event EventAction<EventActionArgs> ControllerDisabled;
        T GetSubController<T>() where T : ISubController;
    }

    public interface ISubController { }
}