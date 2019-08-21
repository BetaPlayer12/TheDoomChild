using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using Refactor.DChild.Gameplay.Characters.Players;
#if UNITY_EDITOR
#endif

namespace DChild.Menu.Bestiary
{
    public class BestiaryIndexPage : MonoBehaviour
    {
        [SerializeField]
        private BestiaryList m_bestiaryList;
        [SerializeField, InlineEditor]
        private BestiaryProgress m_tracker;
        [SerializeField, MinValue(1), PropertyOrder(-1)]
        private int m_page;
        private BestiaryIndexButton[] m_buttons;
        private int m_buttonCount;
        private int m_startIndex;
        private int m_availableButton;
        private int[] m_IDs;

        public void SetPage(int pageNumber)
        {
            m_page = pageNumber;
            m_startIndex = (pageNumber - 1) * m_buttonCount;
            var endIndex = m_startIndex + m_buttonCount;
            if (endIndex >= m_IDs.Length)
            {
                m_availableButton = (m_IDs.Length - 1) - m_startIndex;
            }
            else
            {
                m_availableButton = m_buttonCount-1;
            }
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
                m_buttons[i].SetInteractable(m_tracker.HasInfoOf(ID));
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