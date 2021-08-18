using Sirenix.OdinInspector;
using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    public class ValveVisual : MonoBehaviour
    {
        [SerializeField]
        private ValveVisualData m_visualizationData;
        [SerializeField]
        private SpriteRenderer m_valve;
        [SerializeField]
        private SpriteRenderer m_mechanism;
        [SerializeField, HideInInspector]
        private int m_visualIndex;

        [Button("Previous"), HorizontalGroup("Visualization/Buttons")]
        private void UsePreviousVisualization()
        {
            m_visualIndex = (int)Mathf.Repeat(m_visualIndex - 1, m_visualizationData.count);
            SetVisualizationTo(m_visualIndex);
        }

        [Button("Random"), HorizontalGroup("Visualization/Buttons")]
        private void UseRandomVisualization()
        {
            m_visualIndex = UnityEngine.Random.Range(0, m_visualizationData.count);
            SetVisualizationTo(m_visualIndex);
        }

        [Button("Next"), HorizontalGroup("Visualization/Buttons")]
        private void UseNextVisualization()
        {
            m_visualIndex = (int)Mathf.Repeat(m_visualIndex + 1, m_visualizationData.count);
            SetVisualizationTo(m_visualIndex);
        }

        private void SetVisualizationTo(int index)
        {
            var visualInfo = m_visualizationData.GetVisual(index);
            visualInfo.LoadVisualsTo(m_valve, m_mechanism);
#if UNITY_EDITOR
            EditorUtility.SetDirty(m_valve);
            EditorUtility.SetDirty(m_mechanism);
#endif
        }

        private void OnVisualDataChange()
        {
            try
            {
                SetVisualizationTo(m_visualIndex);
            }
            catch (ArgumentOutOfRangeException e)
            {
                m_visualIndex = 0;
                SetVisualizationTo(m_visualIndex);
            }
        }
    }
}