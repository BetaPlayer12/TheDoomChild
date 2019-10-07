using DChild.Gameplay.Environment;
using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public struct CombatConclusionEventArgs : IEventActionArgs
    {
        public CombatConclusionEventArgs(AttackerCombatInfo attacker, TargetInfo target, AttackInfo result) : this()
        {
            this.attacker = attacker;
            this.target = target;
            this.result = result;
        }

        public AttackerCombatInfo attacker { get; }
        public TargetInfo target { get; }
        public AttackInfo result { get; }
    }

    public struct BreakableObjectEventArgs : IEventActionArgs
    {
        public BreakableObjectEventArgs(BreakableObject instance) : this()
        {
            this.instance = instance;
        }

        public BreakableObject instance { get; }
    }
}