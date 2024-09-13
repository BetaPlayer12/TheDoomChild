using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Units
{
    public class ArmyUnitsHandleTest : ArmyUnitsHandle
    {
        [SerializeField]
        private List<ArmyUnit> m_units;

        [Button]
        public override void Attack()
        {
            for (int i = 0; i < m_units.Count; i++)
            {
                m_units[i].Attack();
            }
        }

        public override List<ArmyUnit> GetUnits()
        {
            return m_units;
        }

        [Button]
        public override void Idle()
        {
            for (int i = 0; i < m_units.Count; i++)
            {
                m_units[i].Idle();
            }
        }

        public override void KillOffUnits(int count)
        {
            throw new System.NotImplementedException();
        }

        [Button]
        public override void StopAttack()
        {
            for (int i = 0; i < m_units.Count; i++)
            {
                m_units[i].Idle();
            }
        }
    }
}