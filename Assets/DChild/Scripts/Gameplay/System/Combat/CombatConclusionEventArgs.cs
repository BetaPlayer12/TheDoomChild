using DChild.Gameplay.Environment;
using Holysoft.Event;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Combat
{
    public class CombatSummary
    {
        public GameObject attacker { get; private set; }
        public IDamageable target { get; private set; }
        public AttackSummaryInfo result { get; private set; }

        public void Initialize(AttackerCombatInfo attacker, TargetInfo target, AttackSummaryInfo result)
        {
            this.attacker = attacker.instance;
            this.target = target.instance;
            this.result = result;
        }
    }

    public class CombatConclusionEventArgs : IEventActionArgs
    {
        public void Initialize(AttackerCombatInfo attacker, TargetInfo target, AttackSummaryInfo result)
        {
            this.attacker = attacker;
            this.target = target;
            this.result = result;
        }

        public AttackerCombatInfo attacker { get; private set; }
        public TargetInfo target { get; private set; }
        public AttackSummaryInfo result { get; private set; }
    }

    public class BreakableObjectEventArgs : IEventActionArgs
    {
        public void Initialize(BreakableObject instance) => this.instance = instance;

        public BreakableObject instance { get; private set; }
    }
}