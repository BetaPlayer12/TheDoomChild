using Sirenix.OdinInspector;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
namespace DChild.Gameplay.Environment.Interractables
{
    public abstract class VisualHandle<T> : MonoBehaviour where T: IVisualData
    {
        [SerializeField]
        protected T m_data;
        [SerializeField, ReadOnly]
        private int m_visualIndex;
        protected abstract void SetVisuals(int index);

        [Button("Previous"), HorizontalGroup("Buttons"), HideInPrefabAssets]
        private void UsePreviousVisualization()
        {
            m_visualIndex = (int)Mathf.Repeat(m_visualIndex - 1, m_data.count);
            SetVisuals(m_visualIndex);
        }

        [Button("Random"), HorizontalGroup("Buttons"), HideInPrefabAssets]
        private void UseRandomVisualization()
        {
            m_visualIndex = UnityEngine.Random.Range(0, m_data.count);
            SetVisuals(m_visualIndex);
        }

        [Button("Next"), HorizontalGroup("Buttons"), HideInPrefabAssets]
        private void UseNextVisualization()
        {
            m_visualIndex = (int)Mathf.Repeat(m_visualIndex + 1, m_data.count);
            SetVisuals(m_visualIndex);
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