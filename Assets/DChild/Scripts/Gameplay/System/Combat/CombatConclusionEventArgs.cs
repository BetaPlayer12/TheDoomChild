using DChild.Gameplay.Environment;
using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public class CombatConclusionEventArgs : IEventActionArgs
    {
        public void Initialize(AttackerCombatInfo attacker, TargetInfo target, AttackInfo result)
        {
            this.attacker = attacker;
            this.target = target;
            this.result = result;
        }

        public AttackerCombatInfo attacker { get; private set; }
        public TargetInfo target { get; private set; }
        public AttackInfo result { get; private set; }
    }

    public class BreakableObjectEventArgs : IEventActionArgs
    {
        public void Initialize(BreakableObject instance) => this.instance = instance;

        public BreakableObject instance { get; private set; }
    }
}