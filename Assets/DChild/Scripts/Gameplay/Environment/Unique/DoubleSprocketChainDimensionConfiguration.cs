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
        private const string REFERENCEGROUP_NAME = "Reference";
        private const string BRACKETGROUP_NAME = REFERENCEGROUP_NAME + "/Brackets";
        private const string CHAINSGROUP_NAME = REFERENCEGROUP_NAME + "/Chains";

        [FoldoutGroup(REFERENCEGROUP_NAME)]
        [SerializeField, BoxGroup(BRACKETGROUP_NAME)]
        private Transform m_upperLeftBracket;
        [SerializeField, BoxGroup(BRACKETGROUP_NAME)]
        private Transform m_upperRightBracket;
        [SerializeField, BoxGroup(BRACKETGROUP_NAME)]
        private Transform m_lowerLeftBracket;
        [SerializeField, BoxGroup(BRACKETGROUP_NAME)]
        private Transform m_lowerRightBracket;
        [SerializeField, BoxGroup(CHAINSGROUP_NAME)]
        private Transform m_upperChain;
        [SerializeField, BoxGroup(CHAINSGROUP_NAME)]
        private Transform m_lowerChain;
        [SerializeField, BoxGroup(CHAINSGROUP_NAME)]
        private Transform m_leftChain;
        [SerializeField, BoxGroup(CHAINSGROUP_NAME)]
        private Transform m_rightChain;
        [SerializeField, TabGroup(CHAINSGROUP_NAME + "/Tab", "Horizontal Chain")]
        private SpriteRenderer[] m_horizontalChains;
        [SerializeField, TabGroup(CHAINSGROUP_NAME + "/Tab", "Vertical Chain")]
        private SpriteRenderer[] m_verticalChains;
        [SerializeField, OnValueChanged("UpdateDimensions")]
        private Vector2 m_dimensions;

        private void UpdateDimensions()
        {
            var horizontalExtent = m_dimensions.x / 2f;
            var verticalExtent = m_dimensions.y / 2f;

            m_upperLeftBracket.localPosition = new Vector2(-horizontalExtent, verticalExtent) ;
            m_upperRightBracket.localPosition = new Vector2(horizontalExtent, verticalExtent) ;
            m_lowerLeftBracket.localPosition = new Vector2(-horizontalExtent, -verticalExtent) ;
            m_lowerRightBracket.localPosition = new Vector2(horizontalExtent, -verticalExtent) ;

            m_upperChain.localPosition = new Vector2(0, verticalExtent);
            m_lowerChain.localPosition = new Vector2(0, -verticalExtent);
            m_leftChain.localPosition = new Vector2(-horizontalExtent, 0);
            m_rightChain.localPosition = new Vector2(horizontalExtent, 0);

            foreach (var chain in m_horizontalChains)
            {
                var size = chain.size;
                size.x = m_dimensions.x / chain.transform.lossyScale.x;
                chain.size = size;
#if UNITY_EDITOR
                EditorUtility.SetDirty(chain);
#endif
            }

            foreach (var chain in m_verticalChains)
            {
                var size = chain.size;
                size.x = m_dimensions.y / chain.transform.lossyScale.x;
                chain.size = size;
#if UNITY_EDITOR
                EditorUtility.SetDirty(chain);
#endif
            }

#if UNITY_EDITOR
            EditorUtility.SetDirty(m_upperLeftBracket);
            EditorUtility.SetDirty(m_upperRightBracket);
            EditorUtility.SetDirty(m_lowerLeftBracket);
            EditorUtility.SetDirty(m_lowerRightBracket);
            EditorUtility.SetDirty(m_upperChain);
            EditorUtility.SetDirty(m_lowerChain);
            EditorUtility.SetDirty(m_leftChain);
            EditorUtility.SetDirty(m_rightChain);
#endif
        }
    }
}