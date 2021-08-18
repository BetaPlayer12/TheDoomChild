using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class SprocketChainVisual : SerializedMonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField,FoldoutGroup("Dimension Configuration")]
        private ISprocketChainDimensionConfiguration m_dimensionConfiguration;
#endif

    }
}