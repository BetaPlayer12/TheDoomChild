using System;
using DChild.Gameplay.Characters.Enemies;
using UnityEngine;
using UnityEngine.Playables;

namespace DChild.Gameplay.Combat
{
    public class BossPhaseTransistor : MonoBehaviour
    {
        [System.Serializable]
        public class TimelineTransistion
        {
            [SerializeField, Min(0)]
            private int m_index;

            public int index { get => m_index; }

            [SerializeField]
            private PlayableDirector m_director;

            public void DoTransistion()
            {
                m_director.Play();
            }
        }

        [SerializeField]
        private Boss m_boss;
        [SerializeField]
        private TimelineTransistion[] m_transistions;

        private void Start()
        {
            m_boss.PhaseChange += OnPhaseChange;
        }

        private void OnPhaseChange(object sender, Boss.PhaseEventArgs eventArgs)
        {
            for (int i = 0; i < m_transistions.Length; i++)
            {
                if (m_transistions[i].index == eventArgs.index)
                {
                    m_transistions[i].DoTransistion();
                    break;
                }
            }
        }
    }
}