using DChild.Serialization;
using Holysoft.Event;
using Holysoft.Menu;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu
{
    public class CampaignSelect : AdjacentNavigation, ICampaignSelect, ICampaignSelectEventCaller
    {
        [SerializeField]
        private CampaignSelectAnimation m_animation;
#if UNITY_EDITOR
        [SerializeField,HideLabel,BoxGroup("Campagin Slot List")]
#endif
        private CampaignSlotList m_slotList;
        private bool m_canNavigate;

        protected override int lastNavigationIndex => m_slotList.slotCount - 1;
        public event EventAction<SelectedCampaignSlotEventArgs> CampaignSelected;

        public override void Next()
        {

            if (m_canNavigate)
            {
                var previousIndex = m_currentNavigationIndex;
                base.Next();
                if (previousIndex < lastNavigationIndex)
                {
                    m_animation.PlayNext();
                }
                else
                {
                    m_animation.PlayLast();

                }
            }
        }

        public override void Previous()
        {
            if (m_canNavigate)
            {
                var previousIndex = m_currentNavigationIndex;
                base.Previous();
                if (previousIndex > 0)
                {
                    m_animation.PlayPrevious();
                }
                else
                {
                    m_animation.PlayFirst();
                }
            }
        }

        public void SendCampaignSelectedEvent()
        {
            CampaignSelected?.Invoke(this, new SelectedCampaignSlotEventArgs(m_slotList.GetSlot(m_currentNavigationIndex)));
        }

        protected override void GoToNextItem()
        {
            m_currentNavigationIndex++;
            m_canNavigate = false;
        }

        protected override void GoToPreviousItem()
        {
            m_currentNavigationIndex--;
            m_canNavigate = false;
        }

        private void OnAnimationEnd(object sender, EventActionArgs eventArgs)
        {
            m_canNavigate = true;
        }

        private void OnInfoChangeRequest(object sender, EventActionArgs eventArgs)
        {
            SendCampaignSelectedEvent();
        }

        protected override void Start()
        {
            base.Start();
            if (GameSystem.dataManager != null)
            {
                m_slotList = GameSystem.dataManager.campaignSlotList;
            }
            m_canNavigate = true;
            m_animation.InfoChangeRequest += OnInfoChangeRequest;
            m_animation.AnimationEnd += OnAnimationEnd;
            SendCampaignSelectedEvent();
        }


    }
}