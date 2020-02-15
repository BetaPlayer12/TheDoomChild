﻿using System;
using DChild.Gameplay;
using DChild.Menu.Campaign;
using DChild.Serialization;
using Holysoft.Event;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DChild.Menu
{
    public class CampaignHandler : CampaignSelectSubElement
    {
        [SerializeField]
        private CampaignSlotFile m_defaultSave;
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
            LoadingHandle.SetLoadType(LoadingHandle.LoadType.Force);
            GameplaySystem.SetCurrentCampaign(m_campaignSelect.selectedSlot);
            GameSystem.LoadZone(m_campaignSelect.selectedSlot.sceneToLoad.sceneName, true);
            LoadingHandle.UnloadScenes(gameObject.scene.name);
        }

        private void OnDeleteAffirmed(object sender, EventActionArgs eventArgs)
        {
            m_defaultSave.LoadFileTo(m_campaignSelect.selectedSlot);
            //m_campaignSelect.selectedSlot.Reset();
            m_campaignSelect.SendCampaignSelectedEvent();
            SerializationHandle.Delete(m_selectedSlotID);
        }

        protected override void Awake()
        {
            base.Awake();
            m_campaignSelect = GetComponentInParent<ICampaignSelect>();
        }
    }
}