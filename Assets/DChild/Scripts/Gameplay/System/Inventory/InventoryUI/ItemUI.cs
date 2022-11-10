﻿using UnityEngine;

namespace DChild.Gameplay.Inventories.UI
{
    public abstract class ItemUI : MonoBehaviour
    {
        [SerializeField]
        protected ItemDetailsUI m_detailsUI;

        protected IStoredItem m_reference;

        public IStoredItem reference => m_reference;

        public abstract void Show();
        public abstract void Hide();

        public void SetReference(IStoredItem reference)
        {
            m_reference = reference;
            UpdateCurrentReferenceDetails();
        }

        public void UpdateCurrentReferenceDetails()
        {
            ShowDetailsOf(m_reference);
        }

        protected virtual void ShowDetailsOf(IStoredItem reference)
        {
            m_detailsUI.ShowDetails(reference);
        }
    }
}