using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Quests
{
    public class QuestObjectInitializer : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_toInitialize;

        private void OnValidate()
        {
            if (Application.isPlaying)
            {
                for (int i = 0; i < m_toInitialize.Length; i++)
                {
                    m_toInitialize[i].SetActive(false);
                }
            }
            
        }

        [Button]
        public void TurnOff()
        {
            for (int i = 0; i < m_toInitialize.Length; i++)
            {
                m_toInitialize[i].SetActive(false);
            }
        }
    }

}
