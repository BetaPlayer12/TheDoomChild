using DChild.Gameplay.ArmyBattle.Battalion;
using DChild.Gameplay.ArmyBattle.Visualizer;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Units
{
    public abstract class ArmyUnitsHandle
    {
        [SerializeField]
        protected Transform m_parent;
        [SerializeField]
        protected ArmyAttackVisualizer m_visualizer;

        public abstract void KillOffUnits(int count);
        public abstract void Idle();
        public abstract void Attack(IArmyBattalion target);
        public abstract void StopAttack();
        public abstract List<ArmyUnit> GetUnits();
        public abstract void SetUnits(ArmyUnit[] units);

        public bool HasUnits() => GetUnits().Count > 0;

        public int GetUnitCount() => GetUnits().Count;
    }
}