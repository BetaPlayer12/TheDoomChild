using DChild.Gameplay.Combat;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DChild.Gameplay.ArmyBattle
{
    public class Army : MonoBehaviour
    {
        //#if UNITY_EDITOR
        [SerializeField, HideInPlayMode]
        private ArmyCompositionData m_initialComposition;
        //#endif
        [SerializeField]
        private int m_troopCount;
        [SerializeField, HideInEditorMode, TabGroup("Composition")]
        private ArmyComposition m_composition;
        [ShowInInspector, HideInEditorMode, ReadOnly]
        private ArmyDamageTypeModifier m_powerModifier;
        [ShowInInspector, HideInEditorMode, ReadOnly]
        private ArmyDamageTypeModifier m_damageReductionModifier;

        [SerializeField]
        private ArmyInfo m_info;

        private List<ArmyAttackGroup> cacheAttackGroup;
        private List<ArmyAbilityGroup> cacheAbilityGroup;
        [SerializeField]
        private List<ISpecialSkillGroup> m_availableSpecialSkills;

        public int troopCount => m_troopCount;
        public ArmyDamageTypeModifier powerModifier => m_powerModifier;
        public ArmyDamageTypeModifier damageReductionModifier => m_damageReductionModifier;

        public Army(ArmyInfo info)
        {
            m_info = info;
        }

        public int GetTroopCount()
        {
            return m_troopCount;
        }

        public int AddTroopCount(int additionalTroops)
        {
            return m_troopCount + additionalTroops;
        }

        public void ResetTroopCount()
        {
            throw new NotImplementedException(); 
        }
        
        public List<IAttackGroup> GetAvailableGroups(DamageType damageType)
        {
            throw new NotImplementedException();
        }

        public List<IAttackGroup> SetAttackingGroups(DamageType damageType)
        {
            throw new NotImplementedException();
        }

        public void ResetGrouAvailability()
        {
            throw new NotImplementedException();
        }

        public List<SpecialSkill> GetAvailableSkills()
        {
            throw new NotImplementedException();
        }

        public List<SpecialSkill> SetSpecialSkillAvailability()
        {
            throw new NotImplementedException();
        }

        public bool HasAvailableAttackGroup(UnitType unitType)
        {
            var groups = m_composition.attacks.GetAttackGroupsOfUnityType(unitType);
            for (int i = 0; i < groups.Count; i++)
            {
                if (groups[i].isAvailable)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasAvailableAbilityGroup()
        {
            var abilityList = m_composition.abilities;
            for (int i = 0; i < abilityList.count; i++)
            {
                var group = abilityList.GetAbilityGroup(i);
                if (group.isAvailable)
                {
                    return true;
                }
            }
            return false;
        }

        public List<ArmyAttackGroup> GetAvailableAttackGroups(UnitType unitType)
        {
            cacheAttackGroup.Clear();
            var groups = m_composition.attacks.GetAttackGroupsOfUnityType(unitType);
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

        public List<ArmyAbilityGroup> GetAvailableAbilityGroups()
        {
            cacheAbilityGroup.Clear();
            var abilityList = m_composition.abilities;
            for (int i = 0; i < abilityList.count; i++)
            {
                var group = abilityList.GetAbilityGroup(i);
                if (group.isAvailable)
                {
                    cacheAbilityGroup.Add(group);
                }
            }

            return cacheAbilityGroup;
        }

        public void SetArmyComposition(ArmyComposition armyComposition)
        {
            m_composition = armyComposition;
        }

        public void RecordArmyCompositionTo(ref ArmyComposition armyComposition)
        {
            //armyComposition.CopyComposition(m_composition);
        }

        public void Initialize()
        {
            //m_powerModifier = new ArmyUnitModifier(1, 1, 1);
            //m_damageReductionModifier = new ArmyUnitModifier(0, 0, 0);
            m_composition.attacks.ResetAvailability();
        }

        public override string ToString()
        {
            return $"{gameObject.name}(Army[{m_composition.name}])";
        }

        private void Awake()
        {
            //#if UNITY_EDITOR
            if (m_initialComposition != null)
            {
                m_composition = m_initialComposition.GenerateArmyCompositionInstance();
                m_composition.attacks.ResetAvailability();
            }

            //#endif
            //m_powerModifier = new ArmyUnitModifier(1, 1, 1);
            //m_damageReductionModifier = new ArmyUnitModifier(0, 0, 0);
            cacheAttackGroup = new List<ArmyAttackGroup>();
            cacheAbilityGroup = new List<ArmyAbilityGroup>();
        }
    }
}