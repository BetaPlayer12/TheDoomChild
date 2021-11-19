using DChild.Gameplay.Combat;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor; 
#endif

namespace DChild.Gameplay.Environment.Interractables
{
    public class PoleGuyVisualHandle : MonoBehaviour
    {
        [SerializeField]
        private PoleGuyVisualData visualData;
        [SerializeField, ReadOnly]
        private int m_visualIndex;

        [Button("Previous"), HorizontalGroup("Buttons"), HideInPrefabAssets]
        private void UsePreviousVisualization()
        {
            m_visualIndex = (int)Mathf.Repeat(m_visualIndex - 1, visualData.count);
            SetVisuals(m_visualIndex);
        }

        [Button("Random"), HorizontalGroup("Buttons"), HideInPrefabAssets]
        private void UseRandomVisualization()
        {
            m_visualIndex = UnityEngine.Random.Range(0, visualData.count);
            SetVisuals(m_visualIndex);
        }

        [Button("Next"), HorizontalGroup("Buttons"), HideInPrefabAssets]
        private void UseNextVisualization()
        {
            m_visualIndex = (int)Mathf.Repeat(m_visualIndex + 1, visualData.count);
            SetVisuals(m_visualIndex);
        }

        private void SetVisuals(int index)
        {
            var visualInfo = visualData.GetVisualInfo(index);
            visualInfo.LoadVisualsTo(GetComponentInChildren<SkeletonAnimation>(), GetComponentInChildren<BoxCollider2D>());
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
            {
                return;
            }

#endif
            SetVisuals(m_visualIndex);
            if (Application.isPlaying)
            {
                Destroy(this);
            }
        }
    }
}