using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Environment
{
    public class IntervalIllusionPlatformHandle : MonoBehaviour
    {
        [System.Serializable]
        public class Info
        {
            [SerializeField,HorizontalGroup,LabelWidth(80)]
            private IllusionPlatform m_platform;
            [SerializeField, MinValue(0), HorizontalGroup]
            private float m_appearanceDuration = 1;

            public IllusionPlatform platform => m_platform;
            public float appearanceDuration => m_appearanceDuration;
        }

        [SerializeField]
        private Info[] m_list;
        private int m_currentRevealedIndex;
        private float m_timer;

        private void Start()
        {
            for (int i = 0; i < m_list.Length; i++)
            {
                if (m_currentRevealedIndex == i)
                {
                    var info = m_list[i];
                    info.platform.Appear(true);
                    m_timer = info.appearanceDuration;
                }
                else
                {
                    m_list[i].platform.Disappear(true);
                }
            }
        }

        private void Update()
        {
            m_timer -= GameplaySystem.time.deltaTime;
            if (m_timer <= 0)
            {
                m_list[m_currentRevealedIndex].platform?.Disappear(false);
                m_currentRevealedIndex = (int)Mathf.Repeat(m_currentRevealedIndex + 1, m_list.Length);
                var info = m_list[m_currentRevealedIndex];
                info.platform?.Appear(false);
                m_timer = info.appearanceDuration;
            }
        }
    }
}
