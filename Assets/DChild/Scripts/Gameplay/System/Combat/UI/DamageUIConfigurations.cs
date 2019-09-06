using DChild.Gameplay.Databases;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{
    [CreateAssetMenu(fileName = "DamageUIConfigurations", menuName = "DChild/Database/Damage UI Configurations")]
    public class DamageUIConfigurations : ScriptableDatabase
    {
        [System.Serializable]
        public struct Configuration : IDamageUIConfig
        {
#if UNITY_EDITOR
            [SerializeField]
            [ReadOnly]
            private AttackType m_name;

            public AttackType name { set { m_name = value; } }
#endif
            [SerializeField]
            private VertexGradient m_vertexGradient;

#if UNITY_EDITOR
            public Configuration(AttackType name)
            {
                m_name = name;
                m_vertexGradient = new VertexGradient(Color.white, Color.white, Color.white, Color.white);
            }
#endif

            public VertexGradient vertexGradient
            {
                get
                {
                    return m_vertexGradient;
                }
#if UNITY_EDITOR
                set
                {
                    m_vertexGradient = value;
                }
#endif
            }
        }

#if UNITY_EDITOR
        public DamageUIConfigurations()
        {
            m_damageConfigurations = new Configuration[(int)AttackType._COUNT - 1];
            for (int i = 0; i < m_damageConfigurations.Length; i++)
            {
                m_damageConfigurations[i] = new Configuration((AttackType)i);
            }

            m_healConfiguration = new Configuration(AttackType._COUNT);
        } 
#endif

        [SerializeField]
        [ListDrawerSettings(HideAddButton = true, OnEndListElementGUI = "Realign")]
        private Configuration[] m_damageConfigurations;
        [SerializeField]
        private Configuration m_healConfiguration;

        public Configuration healConfiguration => m_healConfiguration;

        public Configuration FindDamageConfiguration(AttackType type)
        {
            return m_damageConfigurations[(int)type];
        }

#if UNITY_EDITOR
        [Button("Update")]
        private void InitializeDamageConfigurations()
        {
            var newConfig = new Configuration[(int)AttackType._COUNT - 1];
            for (int i = 0; i < m_damageConfigurations.Length; i++)
            {
                newConfig[i] = new Configuration((AttackType)i);
            }

            var copyLength = newConfig.Length > m_damageConfigurations.Length ? m_damageConfigurations.Length : newConfig.Length;
            Array.Copy(m_damageConfigurations, newConfig, copyLength);
            m_damageConfigurations = newConfig;
        }

        private void Realign(int index)
        {
            m_damageConfigurations[index].name = (AttackType)(index);
        }
#endif
    }
}