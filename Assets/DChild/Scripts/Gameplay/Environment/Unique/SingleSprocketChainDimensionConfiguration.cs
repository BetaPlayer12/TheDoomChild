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
        [SerializeField, FoldoutGroup("References")]
        private SpriteRenderer[] m_chains;
        [SerializeField, OnValueChanged("UpdateDimensions")]
        private float m_length;

        private void UpdateDimensions()
        {
            var extent = m_length / 2f;
            var leftPos = m_leftBracket.localPosition;
            leftPos.x = -extent;
            m_leftBracket.localPosition = leftPos;
            var rightPos = m_rightBracket.localPosition;
            rightPos.x = extent;
            m_rightBracket.localPosition = rightPos;
            foreach (var chain in m_chains)
            {
                var size = chain.size;
                size.x = m_length/chain.transform.lossyScale.x;
                chain.size = size;
#if UNITY_EDITOR
                EditorUtility.SetDirty(chain);
#endif
            }
#if UNITY_EDITOR
            EditorUtility.SetDirty(m_leftBracket);
            EditorUtility.SetDirty(m_rightBracket);
#endif
        }
    }
}