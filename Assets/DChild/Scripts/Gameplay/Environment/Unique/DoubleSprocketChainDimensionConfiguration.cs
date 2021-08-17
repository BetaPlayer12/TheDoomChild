using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    [System.Serializable]
    public class DoubleSprocketChainDimensionConfiguration : ISprocketChainDimensionConfiguration
    {
        [SerializeField, FoldoutGroup("References")]
        private Transform m_upperLeftBracket;
        [SerializeField, FoldoutGroup("References")]
        private Transform m_upperRightBracket;
        [SerializeField, FoldoutGroup("References")]
        private Transform m_bottomLeftBracket;
        [SerializeField, FoldoutGroup("References")]
        private Transform m_bottomRightBracket;
        [SerializeField, OnValueChanged("UpdateDimensions")]
        private Vector2 m_offset;
        [SerializeField, OnValueChanged("UpdateDimensions")]
        private Vector2 m_dimensions;

        private void UpdateDimensions()
        {
            var horizontalExtent = m_dimensions.x / 2f ;
            var verticalExtent = m_dimensions.y / 2f ;
            m_upperLeftBracket.localPosition = new Vector2(-horizontalExtent, verticalExtent) + m_offset;
            m_upperRightBracket.localPosition = new Vector2(horizontalExtent, verticalExtent) + m_offset;
            m_bottomLeftBracket.localPosition = new Vector2(-horizontalExtent, -verticalExtent) + m_offset;
            m_bottomRightBracket.localPosition = new Vector2(horizontalExtent, -verticalExtent) + m_offset;
#if UNITY_EDITOR
            EditorUtility.SetDirty(m_upperLeftBracket);
            EditorUtility.SetDirty(m_upperRightBracket);
#endif
        }
    }
}