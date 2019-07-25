using Holysoft.Event;
using System;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public interface IBasicAttackController
    {
        event EventAction<CombatEventArgs> BasicAttackCall;
        event EventAction<CombatEventArgs> JumpAttackUpwardCall;
        event EventAction<CombatEventArgs> JumpAttackDownwardCall;
        event EventAction<CombatEventArgs> JumpAttackForwardCall;
        event EventAction<CombatEventArgs> UpwardAttackCall;
    }
}