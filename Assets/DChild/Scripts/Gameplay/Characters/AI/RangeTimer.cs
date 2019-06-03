using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace DChild.Gameplay.Characters.AI
{
    [System.Serializable]
    public class RangeTimer
    {
        [SerializeField]
        [MinValue(0f)]
        private float m_min;
        [SerializeField]
        [MinValue(0f)]
        private float m_max;

        private float m_timer;

        public bool isOver => m_timer <= 0f;

        public void Stop() => m_timer = 0f;

        public void Start() => m_timer = Random.Range(m_min, m_max);

        public void UpdateTimer(float deltaTime)
        {
            if (m_timer > 0)
            {
                m_timer -= deltaTime;
            }
        }
    }
}