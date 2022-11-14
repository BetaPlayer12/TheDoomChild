﻿using System;
using DChild.Gameplay;
using DChild.Menu.Campaign;
using DChild.Serialization;
using DChildDebug;
using Holysoft.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Menu
{
    public class CampaignHandler : CampaignSelectSubElement
    {
        [SerializeField]
        private ConfirmationRequestHandle m_deleteRequester;

        [SerializeField]
        private CampaignSlotData m_defaultSave;
        private ICampaignSelect m_campaignSelect;
        private int m_selectedSlotID;

        protected override void OnCampaignSelected(object sender, SelectedCampaignSlotEventArgs eventArgs)
        {
            m_selectedSlotID = eventArgs.ID;
        }

        public void RequestDelete()
        {
            m_deleteRequester.Execute(OnDeleteAffirmed);
        }

        public void Play()
        {
            LoadingHandle.SetLoadType(LoadingHandle.LoadType.Force);
            GameplaySystem.SetCurrentCampaign(m_campaignSelect.selectedSlot);
            LoadingHandle.UnloadScenes(gameObject.scene.name);
            GameSystem.LoadZone(m_campaignSelect.selectedSlot.sceneToLoad.sceneName, true);
        }

        private void OnDeleteAffirmed(object sender, EventActionArgs eventArgs)
        {
            //m_defaultSave.LoadFileTo(m_campaignSelect.selectedSlot);
            m_campaignSelect.selectedSlot.Reset();
            m_campaignSelect.SendCampaignSelectedEvent();
            if (m_campaignSelect.selectedSlot.allowWriteToDisk)
            {
                SerializationHandle.DeleteCampaignSlot(m_selectedSlotID);
            }
            SerializationHandle.SaveCampaignSlot(m_defaultSave.slot.id, m_defaultSave.slot);
        }

        protected override void Awake()
        {
            base.Awake();
            m_campaignSelect = GetComponentInParent<ICampaignSelect>();
        }
    }
}