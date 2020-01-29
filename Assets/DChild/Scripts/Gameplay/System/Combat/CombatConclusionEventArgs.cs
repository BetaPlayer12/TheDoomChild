using DChild.Gameplay.Environment;
using Holysoft.Event;
using System.Collections.Generic;

namespace DChild.Gameplay.Combat
{
    public class CombatConclusionEventArgs : IEventActionArgs
    {
<<<<<<< HEAD
        public CombatConclusionEventArgs(AttackerInfo attacker, TargetInfo target, AttackInfo result) : this()
=======
        public void Initialize(AttackerCombatInfo attacker, TargetInfo target, AttackInfo result)
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
        {
            this.attacker = attacker;
            this.target = target;
            this.result = result;
        }

<<<<<<< HEAD
        public AttackerInfo attacker { get; }
        public TargetInfo target { get; }
        public AttackInfo result { get; }
=======
        public AttackerCombatInfo attacker { get; private set; }
        public TargetInfo target { get; private set; }
        public AttackInfo result { get; private set; }
    }

    public class BreakableObjectEventArgs : IEventActionArgs
    {
        public void Initialize(BreakableObject instance) => this.instance = instance;

        public BreakableObject instance { get; private set; }
>>>>>>> 1da651e7110817459d92af99c3db2a4e35b13b23
    }
}