using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [CreateAssetMenu(fileName = "CombatArtData", menuName = "DChild/Database/Combat Art Data")]
    public class CombatArtData : SerializedScriptableObject
    {
        [SerializeField]
        private BattleAbility m_ability;
        [SerializeField]
        private string m_name;
#if UNITY_EDITOR
        [SerializeField, ValueDropdown("GetCombatArtConfigrationClasses")]
        private string m_configurationType; 
#endif
        [SerializeField, MinValue(1)]
        private int m_maxLevel = 1;
        [SerializeField]
        private IBatttleAbilityScalableConfiguration[] m_configurations;

#if UNITY_EDITOR
        private IEnumerable GetCombatArtConfigrationClasses()
        {
            return DChildUtility.GetDerivedInterfacesNames<IBatttleAbilityScalableConfiguration>();
        }

        private void CreateConfigurations()
        {
            var currentType = Type.GetType(m_configurationType);
            if (m_configurationType == null || m_configurations[0].GetType() != currentType)
            {
                m_configurations = new IBatttleAbilityScalableConfiguration[m_maxLevel];
                for (int i = 0; i < m_maxLevel; i++)
                {
                    m_configurations[i] = (IBatttleAbilityScalableConfiguration)Activator.CreateInstance(currentType);
                }
            }
            else
            {
                var newConfiguration = new IBatttleAbilityScalableConfiguration[m_maxLevel];
                var createNewInstances = m_configurations.Length < m_maxLevel;
                var numberOfInstancesToCopy = createNewInstances ? m_configurations.Length : m_maxLevel;
                int i = 0;
                for (; i < numberOfInstancesToCopy; i++)
                {
                    newConfiguration[i] = m_configurations[i];
                }
                if (createNewInstances)
                {
                    for (; i < newConfiguration.Length; i++)
                    {
                        newConfiguration[i] = (IBatttleAbilityScalableConfiguration)Activator.CreateInstance(currentType);
                    }
                }

                m_configurations = newConfiguration;
            }
        }
#endif
    }
}