using System;
using DChild.Menu.Campaign;
using DChild.Serialization;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu
{
    public class CampaignHandler : CampaignSelectSubElement
    {
        private ICampaignSelect m_campaignSelect;
        private int m_selectedSlotID;

        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            m_selectedSlotID = eventArgs.ID;
        }

        public void RequestDelete()
        {
            GameSystem.RequestConfirmation(OnDeleteAffirmed, "Do you want to delete this Save File?");
        }

        public void Play()
        {

        }

        private void OnDeleteAffirmed(object sender, EventActionArgs eventArgs)
        {
            m_campaignSelect.selectedSlot.Reset();
            m_campaignSelect.SendCampaignSelectedEvent();
        }

        protected override void Awake()
        {
            base.Awake();
            m_campaignSelect = GetComponentInParent<ICampaignSelect>();
        }
    }
}