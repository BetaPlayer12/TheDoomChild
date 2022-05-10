using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DChild.Gameplay.Characters.Players;
using Holysoft.Collections;
using Holysoft.Event;

namespace DChild.Menu.Bestiary
{
    public class BestiaryIndexHandle : MonoBehaviour, IPageHandle
    {
        [SerializeField]
        private BestiaryList m_bestiaryList;
        [SerializeField, InlineEditor]
        private BestiaryProgress m_tracker;
        [SerializeField, MinValue(1), PropertyOrder(-1)]
        private int m_page;
        [SerializeField, MinValue(1), PropertyOrder(-1)]
        private int m_contentSkipCountPerPage;
        private BestiaryIndexButton[] m_buttons;
        private int m_buttonCount;
        private int m_startIndex;
        private int m_availableButton;
        private int[] m_IDs;

        public int currentPage => m_page;

        public event EventAction<EventActionArgs> PageChange;

        public int GetTotalPages()
        {
            throw new System.NotImplementedException();
        }

        public void NextPage()
        {
            if ((m_page * m_contentSkipCountPerPage) + m_buttonCount <= m_IDs.Length)
            {
                m_page++;
                SetPage(m_page);
                UpdateUI();
            }
        }

        public void PreviousPage()
        {
            if (m_page > 1)
            {
                m_page--;
                SetPage(m_page);
                UpdateUI();
            }
        }

        public void SetPage(int pageNumber)
        {
            m_page = pageNumber;
            m_startIndex = ((pageNumber - 1) + m_contentSkipCountPerPage) - 1;
            var endIndex = m_startIndex + m_buttonCount;
            if (endIndex >= m_IDs.Length)
            {
                m_availableButton = (m_IDs.Length - 1) - m_startIndex;
            }
            else
            {
                m_availableButton = m_buttonCount - 1;
            }
            PageChange?.Invoke(this, EventActionArgs.Empty);
        }

        [Button, HideInEditorMode, PropertyOrder(-1)]
        public void UpdateUI()
        {
            int i = 0;
            for (; i <= m_availableButton; i++)
            {
                var itemIndex = m_startIndex + i;
                var ID = m_IDs[itemIndex];
                var data = m_bestiaryList.GetInfo(ID);
                m_buttons[i].SetData(data);
                m_buttons[i].Show();
                m_buttons[i].SetInteractable(m_tracker?.HasInfoOf(ID) ?? true);
            }

            for (; i < m_buttonCount; i++)
            {
                m_buttons[i].Hide();
            }
        }

        private void Awake()
        {
            m_IDs = m_bestiaryList.GetIDs();
            m_buttons = GetComponentsInChildren<BestiaryIndexButton>();
            m_buttonCount = m_buttons.Length;
        }
    }
}