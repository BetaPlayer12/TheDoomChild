using DChild;
using DChild.Menu;
using DChild.Menu.Campaign;
using DChild.Serialization;
using Holysoft.Event;
using Holysoft.Menu;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Menu
{
    public class CampaignSelect : AdjacentNavigation, ICampaignSelect
    {
        [SerializeField]
        private CampaignVideoHandler m_animation;
#if UNITY_EDITOR
        [SerializeField]
#endif
        private CampaignSlot m_selectedSlot;
#if UNITY_EDITOR
        [SerializeField, HideLabel, BoxGroup("Campagin Slot List")]
#endif
        private CampaignSlotList m_slotList;

        public CampaignSlot selectedSlot => m_selectedSlot;
        protected override int lastNavigationIndex => m_slotList.slotCount - 1;
        public event EventAction<SelectedCampaignSlotEventArgs> CampaignSelected;

        public void SetList(CampaignSlotList list)
        {
            m_slotList = list;
            if (m_currentNavigationIndex > m_slotList.slotCount - 1)
            {
                m_currentNavigationIndex = m_slotList.slotCount - 1;
            }
            m_selectedSlot = m_selectedSlot = m_slotList.GetSlot(m_currentNavigationIndex);
            SendCampaignSelectedEvent();
        }

        public override void Next()
        {
            var previousIndex = m_currentNavigationIndex;
            base.Next();
            if (previousIndex < lastNavigationIndex)
            {
                m_animation.Play(CampaignVideoHandler.Type.Next);
            }
            else
            {
                m_animation.Play(CampaignVideoHandler.Type.Last);
            }
            m_selectedSlot = m_slotList.GetSlot(m_currentNavigationIndex);
        }

        public override void Previous()
        {
            var previousIndex = m_currentNavigationIndex;
            base.Previous();
            if (previousIndex > 0)
            {
                m_animation.Play(CampaignVideoHandler.Type.Previous);
            }
            else
            {
                m_animation.Play(CampaignVideoHandler.Type.First);
            }
            m_selectedSlot = m_slotList.GetSlot(m_currentNavigationIndex);
        }

        [Button]
        public void SendCampaignSelectedEvent()
        {
            CampaignSelected?.Invoke(this, new SelectedCampaignSlotEventArgs(m_selectedSlot));
        }

        protected override void GoToNextItem()
        {
            m_currentNavigationIndex++;
        }

        protected override void GoToPreviousItem()
        {
            m_currentNavigationIndex--;
        }

        protected override void Start()
        {
            base.Start();
            if (GameSystem.dataManager != null)
            {
                m_slotList = GameSystem.dataManager.campaignSlotList;
            }
            m_selectedSlot = m_slotList.GetSlot(m_currentNavigationIndex);
            SendCampaignSelectedEvent();
        }

        public void ReloadSlots()
        {
            m_slotList = GameSystem.dataManager.campaignSlotList;
            m_selectedSlot = m_slotList.GetSlot(m_currentNavigationIndex);
            SendCampaignSelectedEvent();
        }

    }

}