﻿using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu.Codex
{
    public abstract class CodexIndexHandle<DatabaseAssetType> : MonoBehaviour, IPageHandle where DatabaseAssetType : DatabaseAsset
    {
        public abstract int buttonCount { get; }
        public abstract int currentPage { get; }

        public event EventAction<EventActionArgs> PageChange;

        public abstract int GetTotalPages();

        public abstract void NextPage();
        public abstract void PreviousPage();

        public abstract void SetPage(int pageNumber);

        public abstract void UpdateUI();

        public abstract CodexIndexButton<DatabaseAssetType> GetButton(int index);

        protected void InvokePageChange() => PageChange?.Invoke(this, EventActionArgs.Empty);

    }

    public abstract class CodexIndexHandle<DatabaseAssetType, DatabaseAssetListType> : CodexIndexHandle<DatabaseAssetType> where DatabaseAssetType : DatabaseAsset
                                                                                            where DatabaseAssetListType : DatabaseAssetList<DatabaseAssetType>
    {
        [SerializeField]
        private DatabaseAssetListType m_assetList;
        [SerializeField]
        private bool m_revealAllData;
        [SerializeField, InlineEditor]
        private CodexProgressTracker m_tracker;
        [SerializeField, MinValue(1), PropertyOrder(-1)]
        private int m_page;
        [SerializeField, MinValue(1), PropertyOrder(-1)]
        private int m_contentSkipCountPerPage;
        private CodexIndexButton<DatabaseAssetType>[] m_buttons;
        private int m_buttonCount;
        private int m_startIndex;
        private int m_availableButton;  
        private int[] m_IDs;

        public override int currentPage => m_page;
        public override int buttonCount => m_buttonCount;

        public override int GetTotalPages()
        {
            var itemCount = m_buttonCount;
            var pageCount = 1;

            while (itemCount < m_IDs.Length)
            {
                itemCount += m_contentSkipCountPerPage;
                pageCount++;
            }

            return pageCount;
        }

        public override void NextPage()
        {
            if ((m_page * m_contentSkipCountPerPage) + m_buttonCount <= m_IDs.Length)
            {
                m_page++;
                SetPage(m_page);
            }
        }

        public override void PreviousPage()
        {
            if (m_page > 1)
            {
                m_page--;
                SetPage(m_page);
            }
        }

        public override void SetPage(int pageNumber)
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
            UpdateUI();
            InvokePageChange();
        }

        [Button, HideInEditorMode, PropertyOrder(-1)]
        public override void UpdateUI()
        {
            int i = 0;
            for (; i <= m_availableButton; i++)
            {
                var itemIndex = m_startIndex + i;
                var ID = m_IDs[itemIndex];
                var data = m_assetList.GetInfo(ID);
                m_buttons[i].SetData(data);
                m_buttons[i].Show();
                var hasInfoOnID = m_tracker?.HasInfoOf(ID) ?? true;
                m_buttons[i].SetInteractable(m_revealAllData || hasInfoOnID);
            }

            for (; i < m_buttonCount; i++)
            {
                m_buttons[i].Hide();
            }
        }

        public override CodexIndexButton<DatabaseAssetType> GetButton(int index) => m_buttons[index];

        private void Awake()
        {
            m_IDs = m_assetList.GetIDs();
            m_buttons = GetComponentsInChildren<CodexIndexButton<DatabaseAssetType>>();
            m_buttonCount = m_buttons.Length;
        }
    }
}