using DChild.Gameplay.Characters.Players.Modules;
using Holysoft.Event;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IMainController
    {
<<<<<<< HEAD
        event EventAction<EventActionArgs> ControllerDisabled;
        T GetSubController<T>() where T : ISubController;
    }

    public interface ISubController { }
=======
        void Enable();
        void Disable();
        event EventAction<EventActionArgs> ControllerDisabled;
    }
>>>>>>> 4653686e5010b0329a8f8f935f22a3799c3b1818
}