using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class AlchemicalDroneActivationHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_alchemicaldrones;
        public void activateBots()
        {
            for (int i = 0; i < m_alchemicaldrones.Length; i++)
            {
                m_alchemicaldrones[i].SetActive(true);
            }
        }
        public void deactivateBots()
        {
            for (int i = 0; i < m_alchemicaldrones.Length; i++)
            {
                m_alchemicaldrones[i].SetActive(false);
            }
        }
    }
}
