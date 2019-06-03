using DChild.Serialization;
using Holysoft.UI;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DChild.Menu.Bestiary
{
    public class BestiaryPage : SimpleUICanvas
    {
        [SerializeField]
        private BestiaryList m_list;
        [SerializeField]
        private SimpleUICanvas m_indexPage;
        [SerializeField]
        private BestiaryInfoPage m_infoPage;
        private BestiaryProgress m_progress;
        private IndexHandler m_indexHandler;

        public override void Show()
        {
            base.Show();
            if (m_indexHandler.pageNumber != 1)
            {
                m_indexHandler.SetPageNumber(1);
                m_indexHandler.UpdateInfo();
                m_indexHandler.UpdateButtonInteractability();
            }
            m_indexPage.Show();
            m_infoPage.Hide();
        }

        public void SetProgress(BestiaryProgress reference)
        {
            m_indexHandler.SetProgress(reference);
        }

        public void NextIndexPage()
        {
            if (m_indexHandler.NextIndexPage())
            {
                m_indexHandler.UpdateInfo();
                m_indexHandler.UpdateButtonInteractability();
            }
        }

        public void PrevIndexPage()
        {
            if (m_indexHandler.PrevIndexPage())
            {
                m_indexHandler.UpdateInfo();
                m_indexHandler.UpdateButtonInteractability();
            }
        }

        private void OnIndexButtonClick(object sender, NavigationButtonEventArgs eventArgs)
        {
            var creatureID = eventArgs.buttonID;
            m_infoPage.ShowInfo(m_list.GetInfo(creatureID));
            if (MenuSystem.backTracker != null)
            {
                MenuSystem.backTracker.Stack(m_indexPage);
                MenuSystem.backTracker.Stack(m_infoPage);
            }
            m_indexPage.Hide();
            m_infoPage.Show();
        }

        private void Awake()
        {
            var indexButtons = GetComponentsInChildren<BestiaryIndexButton>();
            for (int i = 0; i < indexButtons.Length; i++)
            {
                indexButtons[i].SetButtonID(i);
                indexButtons[i].ButtonClick += OnIndexButtonClick;
            }
            m_indexHandler = new IndexHandler(m_list, indexButtons);
            m_indexHandler.UpdateInfo();
        }
    }
}