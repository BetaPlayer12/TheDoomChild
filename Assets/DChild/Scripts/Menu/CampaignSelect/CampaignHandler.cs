using System;
using DChild.Menu.Campaign;
using DChild.Serialization;
using Holysoft.Event;
using UnityEngine;

namespace DChild.Menu
{
    public class CampaignHandler : CampaignSelectSubElement
    {
        private ICampaignSelectEventCaller m_eventCaller;
        private int m_selectedSlotID;

        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            m_selectedSlotID = eventArgs.ID;
        }

        public void RequestDelete()
        {
            MenuSystem.RequestConfirmation(OnDeleteAffirmed, "Do you want to delete this Save File?");
        }

        public void Play()
        {

        }

        private void OnDeleteAffirmed(object sender, EventActionArgs eventArgs)
        {
            GameSystem.dataManager.campaignSlotList.GetSlot(m_selectedSlotID).Reset();
            m_eventCaller.SendCampaignSelectedEvent();
        }

        protected override void Awake()
        {
            base.Awake();
            m_eventCaller = GetComponentInParent<ICampaignSelectEventCaller>();
        }
    }
}