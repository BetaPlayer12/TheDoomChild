using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay;
using DChild.Gameplay.Systems;
using UnityEngine;

namespace DChildDebug
{
    public class WorldDebugger : MonoBehaviour
    {
        [SerializeField]
        private float m_timeScale = 1;
        private float m_prevTimeScale;

        private void Awake()
        {
            m_prevTimeScale = m_timeScale;
            GameplaySystem.world.SetTimeScale(m_timeScale);
        }

        private void OnValidate()
        {
            if (m_timeScale != m_prevTimeScale)
            {
                GameplaySystem.world.SetTimeScale(m_timeScale);
                m_prevTimeScale = m_timeScale;
            }
        }
    }

}