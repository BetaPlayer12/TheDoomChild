using DChild.Gameplay.ArmyBattle.Battalion;
using DChild.Gameplay.ArmyBattle.Units;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Visualizer
{
    public class ArmyVFXAttack : ArmyAttackVisualizer
    {
        [SerializeField]
        private bool m_spawnAtTarget;
        [SerializeField]
        private ArmySpecialSkillVfx m_fx;

        public override void Attack(List<ArmyUnit> units, IArmyBattalion target)
        {
            if (m_spawnAtTarget)
            {
                m_fx.transform.position = target.centerPosition;
            }
            if (m_fx!=null)
            {
                m_fx.PlayEffects();
            }
            
        }

        public override void StopAttack(List<ArmyUnit> units)
        {
            if (m_fx != null)
            {
                m_fx.StopEffects();
            }
        }
    }
}