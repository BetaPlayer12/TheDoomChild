using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Characters.AI
{
    public struct PhaseHandle<T> where T : System.Enum
    {
        [SerializeField]
        private T m_currentPhase;
        public bool willTransistion;

        public T currentPhase { get => m_currentPhase;}

        public PhaseHandle(T startingPhase)
        {
            this.m_currentPhase = startingPhase;
            willTransistion = false;
        }

        public void ChangePhase(T phase)
        {
            if(m_currentPhase.Equals(phase) != false)
            {
                m_currentPhase = phase;
                willTransistion = true;
            }
        }
    }
}