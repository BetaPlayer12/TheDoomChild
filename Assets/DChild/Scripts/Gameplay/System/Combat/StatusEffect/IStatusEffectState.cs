using Holysoft.Event;
using System;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat.StatusInfliction
{
    public struct StatusEffectAfflictionEventArgs : IEventActionArgs
    {
        public StatusEffectAfflictionEventArgs(StatusEffectType statusEffectType, bool isAffected) : this()
        {
            this.statusEffectType = statusEffectType;
            this.isAffected = isAffected;
        }

        public StatusEffectType statusEffectType { get; }
       public bool isAffected { get; }
    }

    public interface IStatusEffectState
    {
        event EventAction<StatusEffectAfflictionEventArgs> StateChange;
        void ChangeStatus(StatusEffectType type, bool isAffected);
        bool IsInflictedWith(StatusEffectType type);
    }
}