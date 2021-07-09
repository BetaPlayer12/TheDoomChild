using Sirenix.OdinInspector;
using Spine.Unity;
using System;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    public class CapsuleMonsterVisualHandle
    {
        [SerializeField, FoldoutGroup("Visualization"), OnValueChanged("OnMonsterVisualDataChange")]
        private MonsterCapsuleVisualData m_visualizationData;
        [SerializeField, FoldoutGroup("Visualization")]
        private SkeletonAnimation m_monsterVisual;
        [SerializeField, HideInInspector]
        private int m_monsterIndex;

        [Button("Previous"), HorizontalGroup("Visualization/Buttons")]
        private void UsePreviousVisualization()
        {
            m_monsterIndex = (int)Mathf.Repeat(m_monsterIndex - 1, m_visualizationData.count);
            SetMonsterVisualizationTo(m_monsterIndex);
        }

        [Button("Random"), HorizontalGroup("Visualization/Buttons")]
        private void UseRandomVisualization()
        {
            m_monsterIndex = UnityEngine.Random.Range(0,m_visualizationData.count);
            SetMonsterVisualizationTo(m_monsterIndex);
        }

        [Button("Next"), HorizontalGroup("Visualization/Buttons")]
        private void UseNextVisualization()
        {
            m_monsterIndex = (int)Mathf.Repeat(m_monsterIndex + 1, m_visualizationData.count);
            SetMonsterVisualizationTo(m_monsterIndex);
        }

        private void SetMonsterVisualizationTo(int index)
        {
            var visualInfo = m_visualizationData.GetMonsterVisual(index);
            visualInfo.LoadVisualsTo(m_monsterVisual);
        }

        private void OnMonsterVisualDataChange()
        {
            try
            {
                SetMonsterVisualizationTo(m_monsterIndex);
            }
            catch (ArgumentOutOfRangeException e)
            {
                m_monsterIndex = 0;
                SetMonsterVisualizationTo(m_monsterIndex);
            }
        }
    }
}
