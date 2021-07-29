using DChild.Gameplay.Characters.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class AlchemicalDroneActivationHandler : MonoBehaviour
    {
        [SerializeField]
        private AlchemistBotAI[] m_alchemicalDrones;
        public void ActivateBots(bool instant)
        {
            if (instant)
            {
                for (int i = 0; i < m_alchemicalDrones.Length; i++)
                {
                    m_alchemicalDrones[i].Activated(instant);
                }
            }
            else
            {
                for (int i = 0; i < m_alchemicalDrones.Length; i++)
                {
                    m_alchemicalDrones[i].Activated(instant); ;
                }
            }
        }
        public void DeactivateBots(bool instant)
        {
            if (instant)
            {
                for (int i = 0; i < m_alchemicalDrones.Length; i++)
                {
                    m_alchemicalDrones[i].Activated(instant);
                }
            }
            else
            {
                for (int i = 0; i < m_alchemicalDrones.Length; i++)
                {
                    m_alchemicalDrones[i].Activated(instant);
                }
            }
        }
    }
}
