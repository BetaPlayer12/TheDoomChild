using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Environment.VisualConfigurators
{
    [System.Serializable]
    public class CapsuleHighlightHandle
    {
        [SerializeField, FoldoutGroup("Visualization"), OnValueChanged("OnMonsterVisualDataChange")]
        private SpriteList m_visualizationData;
        [SerializeField, FoldoutGroup("Visualization")]
        private SpriteRenderer m_highlight;
        [SerializeField, HideInInspector]
        private int m_highlightIndex;

        [Button("Previous"), HorizontalGroup("Visualization/Buttons")]
        private void UsePreviousVisualization()
        {
            m_highlightIndex = (int)Mathf.Repeat(m_highlightIndex - 1, m_visualizationData.count);
            SetMonsterVisualizationTo(m_highlightIndex);
        }

        [Button("Next"), HorizontalGroup("Visualization/Buttons")]
        private void UseNextVisualization()
        {
            m_highlightIndex = (int)Mathf.Repeat(m_highlightIndex + 1, m_visualizationData.count);
            SetMonsterVisualizationTo(m_highlightIndex);
        }

        private void SetMonsterVisualizationTo(int index)
        {
            m_highlight.sprite = m_visualizationData.GetSprite(index);
        }

        private void OnMonsterVisualDataChange()
        {
            try
            {
                SetMonsterVisualizationTo(m_highlightIndex);
            }
            catch (ArgumentOutOfRangeException e)
            {
                m_highlightIndex = 0;
                SetMonsterVisualizationTo(m_highlightIndex);
            }
        }

    }
}
