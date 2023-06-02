using UnityEngine;
using Doozy.Runtime.UIManager.Components;
using System.Collections.Generic;
#if UNITY_EDITOR
#endif

namespace DChild.Gameplay.Systems
{
    public class TabGroupNavigation : MonoBehaviour
    {
        [SerializeField]
        private UIToggleGroup m_toggleGroup;
        [SerializeField]
        private bool m_wrapCycle;
        [SerializeField]
        private UIToggle[] m_cycleList;

        private int m_currentIndex = 0;

        private int numberOfTabs => m_cycleList.Length;

        public void PreviousTab()
        {
            m_currentIndex--;

            if (m_currentIndex < 0)
            {
                m_currentIndex = 0;
                if (m_wrapCycle)
                {
                    m_currentIndex = numberOfTabs - 1;
                }
            }
            SelectTab(m_currentIndex);
        }

        public void NextTab()
        {
            m_currentIndex++;
            if (m_currentIndex >= numberOfTabs)
            {
                m_currentIndex = numberOfTabs - 1;
                if (m_wrapCycle)
                {
                    m_currentIndex = 0;
                }
            }
            SelectTab(m_currentIndex);
        }

        public void Initialize()
        {
            SelectTab(m_currentIndex);
        }

        private void SelectTab(int index)
        {
            var tab = m_cycleList[index];
            // tab.Select();
            tab.SetIsOn(true);
            //tab.SendSignal(true);
        }
    }
}