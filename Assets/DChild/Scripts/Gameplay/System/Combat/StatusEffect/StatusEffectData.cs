using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using DChildEditor;
#endif

namespace DChild.Gameplay.Combat.StatusAilment
{

    [CreateAssetMenu(fileName = "StatusEffectData", menuName = "DChild/Gameplay/Combat/Inflictions/Status Effect")]
    public class StatusEffectData : SerializedScriptableObject
    {
        [SerializeField, OnValueChanged("UpdateAssetName")]
        private StatusEffectType m_type;
        [SerializeField]
        private bool m_hasDuration;
        [SerializeField, ShowIf("m_hasDuration"), OnInspectorGUI("ValidateUpdatbleModules"), Min(0), Indent]
        private float m_duration;

        [SerializeField]
        private IStatusEffectModule[] m_modules;
        [SerializeField]
        private IStatusEffectUpdatableModule[] m_updatableModule;

        public StatusEffectType type => m_type;

        public StatusEffectHandle CreateHandle()
        {
            var baseDuration = m_duration;
            var duration = m_hasDuration ? m_duration : -1;
            
            IStatusEffectUpdatableModule[] list = new IStatusEffectUpdatableModule[m_updatableModule.Length];
            for (int i = 0; i < list.Length; i++)
            {
                list[i] = m_updatableModule[i].CreateCopy();
            }
            return new StatusEffectHandle(m_type, duration, baseDuration ,m_modules, list);
        }

      

#if UNITY_EDITOR
        private void ValidateUpdatbleModules()
        {
            if (m_updatableModule != null)
            {
                for (int i = 0; i < m_updatableModule.Length; i++)
                {
                    m_updatableModule[i].CalculateWithDuration(m_duration);
                }
            }
        }

        private void UpdateAssetName()
        {
            string assetPath = AssetDatabase.GetAssetPath(GetInstanceID());
            var fileName = m_type.ToString().Replace(" ", string.Empty) + "EffectData";
            FileUtility.RenameAsset(this, assetPath, fileName);
            AssetDatabase.SaveAssets();
        }
#endif
    }
}