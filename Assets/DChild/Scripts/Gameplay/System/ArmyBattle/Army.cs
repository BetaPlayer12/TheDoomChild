using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.ArmyBattle
{

    public class Army : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, HideInPlayMode]
        private ArmyCompositionData m_initialComposition;
#endif
        [SerializeField]
        private Health m_troopCount;
        [SerializeField, HideInEditorMode]
        private ArmyComposition m_composition;

        private ArmyUnitPowerModifier m_powerModifier;

        private List<ArmyAttackGroup> cacheAttackGroup;

        public Health troopCount => m_troopCount;

        public bool HasAvailableAttackGroup(UnitType unitType)
        {
            var groups = m_composition.GetAttackGroupsOfUnityType(unitType);
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].isAvailable)
                {
                    return true;
                }
            }

            return false;
        }

        public List<ArmyAttackGroup> GetAvailableAttackGroups(UnitType unitType)
        {
            cacheAttackGroup.Clear();
            var groups = m_composition.GetAttackGroupsOfUnityType(unitType);
            for (int i = 0; i < groups.Count; i++)
            {
                var group = groups[i];
                if (group.isAvailable)
                {
                    cacheAttackGroup.Add(group);
                }
            }
            return cacheAttackGroup;
        }

        public void SetArmyComposition(ArmyComposition armyComposition)
        {
            m_composition = armyComposition;
            SetTroopCount(m_composition.troopCount);
        }

        public void RecordArmyCompositionTo(ref ArmyComposition armyComposition)
        {
            armyComposition.CopyComposition(m_composition);
        }

        public void SetTroopCount(int troopCount)
        {
            m_troopCount.SetMaxValue(troopCount);
            m_troopCount.ResetValueToMax();
        }

        public void Initialize()
        {
            SetTroopCount(m_composition.troopCount);
        }

        private void Awake()
        {
#if UNITY_EDITOR
            if (m_initialComposition != null)
            {
                m_composition = m_initialComposition.GenerateArmyCompositionInstance();
                m_composition.ResetAvailability();
            }

#endif
            cacheAttackGroup = new List<ArmyAttackGroup>();
        }
    }
}