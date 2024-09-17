using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    [System.Serializable]
    public class ArmyAttackGroup : ArmyCharacterGroup
    {
        private IAttackingGroup m_reference;

        public ArmyAttackGroup(IAttackingGroup data) : base()
        {
            m_reference = data;
        }

        public ArmyAttackGroup(ArmyAttackGroup reference) : base(reference)
        {
            m_reference = reference.reference;
        }

        public IAttackingGroup reference => m_reference;
        //public UnitType unitType => m_reference.attackType;

        [ShowInInspector, PropertyOrder(0)]
        private int totalPower => GetTotalPower();

        //public string name => m_reference.groupName;

        public void SetMemberAvailability(params bool[] memberAvailability)
        {
            m_armyGroupList.Clear();
            //for (int i = 0; i < m_reference.memberCount; i++)
            //{
            //    if (i >= memberAvailability.Length)
            //    {
            //        break;
            //    }
            //    else if (memberAvailability[i])
            //    {
            //        //m_armyGroupList.Add(reference.GetTroopCount(i));
            //    }
            //}
        }

        public int GetTotalPower()
        {
            var power = 0;
            //if (m_reference.isUsingCharactersForPower)
            //{
            //    for (int i = 0; i < m_armyGroupList.Count; i++)
            //    {
            //        power += m_armyGroupList[i].power;
            //    }
            //}
            //else
            //{
            //    power = m_reference.GetAttackPower();
            //}
            return power;
        }
    }
}