using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public struct CombatConclusionEventArgs : IEventActionArgs
    {
        public CombatConclusionEventArgs(AttackInfo attacker, ITarget target, DamageInfo result) : this()
        {
            this.attacker = attacker;
            this.target = target;
            this.result = result;
        }

        public AttackInfo attacker { get; }
        public ITarget target { get; }
        public DamageInfo result { get; }
    }
}