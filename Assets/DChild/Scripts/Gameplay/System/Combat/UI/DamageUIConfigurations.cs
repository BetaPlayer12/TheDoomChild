using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChild.Gameplay.Combat.UI
{

    [CreateAssetMenu(fileName = "DamageUIConfigurations", menuName = "DChild/Database/Damage UI Configurations")]
    public class DamageUIConfigurations : SerializedScriptableObject
    {
        [SerializeField]
        private Dictionary<DamageType, TMP_ColorGradient> m_configurations = new Dictionary<DamageType, TMP_ColorGradient>();
        [SerializeField]
        private TMP_ColorGradient m_healConfiguration;

        public TMP_ColorGradient healConfiguration => m_healConfiguration;

        public TMP_ColorGradient FindDamageConfiguration(DamageType type)
        {
            return m_configurations[type];
        }

#if UNITY_EDITOR
        [Button("Update")]
        private void InitializeDamageConfigurations()
        {
            m_configurations.Clear();
            var size = (int)DamageType.All - 1;
            for (int i = 0; i < size; i++)
            {
                m_configurations.Add((DamageType)i, null);
            }
            m_configurations.Add(DamageType.True, null);
        }
#endif
    }
}