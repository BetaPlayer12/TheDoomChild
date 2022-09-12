using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class EnvironmentRandomizer : MonoBehaviour
    {
        [SerializeField, ListDrawerSettings(DraggableItems = false), OnValueChanged("OnEnvironemntsChanged")]
        private GameObject[] m_environments;

#if UNITY_EDITOR
        [SerializeField, ReadOnly]
        private int m_currentEnvironmentIndex;

        [Button, ShowIf("@m_environments.Length > 1")]
        private void NextEnvironment()
        {
            m_currentEnvironmentIndex = (int)Mathf.Repeat(m_currentEnvironmentIndex + 1, m_environments.Length);
            EnableChosenEnvironment(m_currentEnvironmentIndex);
        }

        [Button, ShowIf("@m_environments.Length > 1")]
        private void PrevEnvironment()
        {
            m_currentEnvironmentIndex = (int)Mathf.Repeat(m_currentEnvironmentIndex - 1, m_environments.Length);
            EnableChosenEnvironment(m_currentEnvironmentIndex);
        }

        private void OnEnvironemntsChanged()
        {
            m_currentEnvironmentIndex = (int)Mathf.Repeat(m_currentEnvironmentIndex, m_environments.Length);
            if (m_environments.Length > 0)
            {
                EnableChosenEnvironment(m_currentEnvironmentIndex);
            }
        }
#endif

        private void Start()
        {
            var index = Random.Range(0, m_environments.Length);
            EnableChosenEnvironment(index);
        }

        private void EnableChosenEnvironment(int index)
        {
            for (int i = 0; i < m_environments.Length; i++)
            {
                m_environments[i].SetActive(i == index);
            }
        }
    }
}
