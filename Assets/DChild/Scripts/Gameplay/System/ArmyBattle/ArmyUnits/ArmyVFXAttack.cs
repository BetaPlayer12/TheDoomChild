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
        private ParticleSystem m_fx;

        public override void Attack(List<ArmyUnit> units, IArmyBattalion target)
        {
            if (m_spawnAtTarget)
            {
                m_fx.transform.position = target.centerPosition;
            }
            m_fx.Play(true);
        }

        public override void StopAttack(List<ArmyUnit> units)
        {
            m_fx.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}