using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public struct CombatConclusionEventArgs : IEventActionArgs
    {
        public CombatConclusionEventArgs(AttackerInfo attacker, TargetInfo target, AttackInfo result) : this()
        {
            this.attacker = attacker;
            this.target = target;
            this.result = result;
        }

        public AttackerInfo attacker { get; }
        public TargetInfo target { get; }
        public AttackInfo result { get; }
    }
}