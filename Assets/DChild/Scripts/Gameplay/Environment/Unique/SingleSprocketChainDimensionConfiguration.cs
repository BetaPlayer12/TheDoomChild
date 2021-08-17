using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    [System.Serializable]
    public class SingleSprocketChainDimensionConfiguration : ISprocketChainDimensionConfiguration
    {
        [SerializeField, FoldoutGroup("References")]
        private Transform m_leftBracket;
        [SerializeField, FoldoutGroup("References")]
        private Transform m_rightBracket;
        [SerializeField, OnValueChanged("UpdateDimensions")]
        private float m_offset;
        [SerializeField, OnValueChanged("UpdateDimensions")]
        private float m_length;

        private void UpdateDimensions()
        {
            var extent = m_length / 2f;
            m_leftBracket.localPosition = new Vector3(-extent, m_offset);
            m_rightBracket.localPosition = new Vector3(extent, m_offset);

#if UNITY_EDITOR
            EditorUtility.SetDirty(m_leftBracket);
            EditorUtility.SetDirty(m_rightBracket);
#endif
        }
    }
}