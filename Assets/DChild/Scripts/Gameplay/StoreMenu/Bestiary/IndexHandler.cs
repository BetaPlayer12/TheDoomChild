using DChild.Serialization;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.Bestiary
{
    [System.Serializable]
    public class IndexHandler
    {
        private BestiaryList m_list;
        private int[] m_bestiaryIDs;
        private BestiaryProgress m_progress;
        private BestiaryIndexButton[] m_buttons;
        private int m_pageNumber;
        private int m_maxPageNumber;
        private bool m_hasDisabledButtons;
        private List<BestiaryIndexButton> m_disabledButtons;

        public int pageNumber => m_pageNumber;

        public IndexHandler(BestiaryList m_list, BestiaryIndexButton[] m_buttons)
        {
            this.m_list = m_list;
            m_bestiaryIDs = m_list.GetIDs();
            this.m_buttons = m_buttons;
            m_pageNumber = 1;
            m_maxPageNumber = Mathf.CeilToInt((float)m_list.Count / m_buttons.Length);
            m_hasDisabledButtons = false;
            m_disabledButtons = new List<BestiaryIndexButton>();
        }

        public void SetProgress(BestiaryProgress reference)
        {
            m_progress = reference;
            UpdateButtonInteractability();
        }

        public void SetPageNumber(int pageNumber) => m_pageNumber = pageNumber;

        public void UpdateInfo()
        {
            var currentIndex = (m_pageNumber - 1) * m_buttons.Length;
            var endIndex = m_pageNumber * m_buttons.Length;
            if (endIndex > m_list.Count)
            {
                var enabledButtonCount = endIndex - m_list.Count;
                var disabledButtonIndexStart = enabledButtonCount;
                m_hasDisabledButtons = true;
                for (int i = 0; i < enabledButtonCount; i++)
                {
                    m_buttons[i].SetInfo(m_list.GetInfo(m_bestiaryIDs[currentIndex]));
                    currentIndex++;
                }

                for (int i = disabledButtonIndexStart; i < m_buttons.Length; i++)
                {
                    m_buttons[i].Hide();
                    m_disabledButtons.Add(m_buttons[i]);
                }
            }
            else
            {
                if (m_hasDisabledButtons)
                {
                    for (int i = 0; i < m_disabledButtons.Count; i++)
                    {
                        m_disabledButtons[i].Show();
                    }
                    m_disabledButtons.Clear();
                    m_hasDisabledButtons = false;
                }

                for (int i = 0; i < m_buttons.Length; i++)
                {
                    m_buttons[i].SetInfo(m_list.GetInfo(m_bestiaryIDs[currentIndex]));
                    currentIndex++;
                }
            }
        }

        public void UpdateButtonInteractability()
        {
            for (int i = 0; i < m_buttons.Length; i++)
            {
                var hasEncountered = m_progress.HasEncountered(m_buttons[i].creatureID);
                m_buttons[i].SetInteractability(hasEncountered);
            }
        }

        public bool NextIndexPage()
        {
            if (m_pageNumber < m_maxPageNumber)
            {
                m_pageNumber++;
                return true;
            }
            return false;
        }

        public bool PrevIndexPage()
        {
            if (m_pageNumber > 1)
            {
                m_pageNumber--;
                return true;
            }
            return false;
        }
    }
}