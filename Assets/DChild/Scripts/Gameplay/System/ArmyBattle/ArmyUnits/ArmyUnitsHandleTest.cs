using DChild.Gameplay.ArmyBattle.Battalion;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle.Units
{
    [System.Serializable]
    public class ArmyUnitsHandleTest : ArmyUnitsHandle
    {
        [SerializeField]
        private List<ArmyUnit> m_liveUnits;
        [SerializeField]
        private List<ArmyUnit> m_deadUnits;


        public override void Attack(IArmyBattalion target)
        {
            m_visualizer.Attack(m_liveUnits, target);
        }

        public override List<ArmyUnit> GetUnits()
        {
            return m_liveUnits;
        }

        public override void Idle()
        {
            for (int i = 0; i < m_liveUnits.Count; i++)
            {
                m_liveUnits[i].Idle();
            }
        }

        public override void KillOffUnits(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var index = Random.Range(0, m_liveUnits.Count);
                var unit = m_liveUnits[index];
                unit.Die();
                m_liveUnits.RemoveAt(index);
                m_deadUnits.Add(unit);
            }
        }

        public override void SetUnits(ArmyUnit[] units)
        {
            m_liveUnits.Clear();
            m_liveUnits.AddRange(units);
            for (int i = 0; i < m_liveUnits.Count; i++)
            {
                m_liveUnits[i].transform.SetParent(m_parent);
            }
        }

        public override void StopAttack()
        {
            m_visualizer.StopAttack(m_liveUnits);
        }
    }
}