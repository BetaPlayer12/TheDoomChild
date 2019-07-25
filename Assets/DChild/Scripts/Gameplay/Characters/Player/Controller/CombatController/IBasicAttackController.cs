using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IBasicAttackController : ISubController
    {
        event EventAction<CombatEventArgs> BasicAttackCall;
    }
}