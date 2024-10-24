using System;
using DChild.Gameplay;
using DChild.Gameplay.Systems;
using DChild.Gameplay.Systems.Serialization;
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

        public void RequestReset()
        {
            m_deleteRequester.Execute(OnResetAffirmed);
        }

        public void Play()
        {
            LoadingHandle.SetLoadType(LoadingHandle.LoadType.Force);
            GameplaySystem.SetCurrentCampaign(m_campaignSelect.selectedSlot);
            LoadingHandle.UnloadScenes(gameObject.scene.name);
            if (GameSystem.m_useGameModeValidator)
            {
                var WorldTypeVar = FindObjectOfType<WorldTypeManager>();
                WorldTypeVar.SetCurrentWorldType(m_campaignSelect.selectedSlot.location);

                switch (WorldTypeVar.CurrentWorldType)
                {
                    case WorldType.Underworld:
                        GameSystem.LoadZone(GameMode.Underworld, m_campaignSelect.selectedSlot.sceneToLoad, true);
                        break;
                    case WorldType.Overworld:
                        GameSystem.LoadZone(GameMode.Overworld, m_campaignSelect.selectedSlot.sceneToLoad, true);
                        break;
                    case WorldType.ArmyBattle:
                        GameSystem.LoadZone(GameMode.ArmyBattle, m_campaignSelect.selectedSlot.sceneToLoad, true);
                        break;
                }
            }
            else
            {
                GameSystem.LoadZone(m_campaignSelect.selectedSlot.sceneToLoad, true);
            }
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

        private void OnResetAffirmed(object sender, EventActionArgs eventArgs)
        {
            OverrideCurrentSlotWithDefaultSave();
        }

        protected override void Awake()
        {
            base.Awake();
            m_campaignSelect = GetComponentInParent<ICampaignSelect>();
        }

        private void OverrideCurrentSlotWithDefaultSave()
        {
            //This Logic is Temporary cuz its quite heavy and bypasses too much of other scripts responsibility
            var resetCampaignSlot = new CampaignSlot(m_selectedSlotID);
            resetCampaignSlot.Copy(m_defaultSave.slot);

            CampaignSlot[] newSlotList = new CampaignSlot[GameSystem.dataManager.campaignSlotList.slotCount];

            for (int i = 0; i < newSlotList.Length; i++)
            {
                newSlotList[i] = GameSystem.dataManager.campaignSlotList.GetSlot(i);
            }

            for (int i = 0; i < newSlotList.Length; i++)
            {
                if(newSlotList[i].id == m_selectedSlotID)
                {
                    newSlotList[i] = resetCampaignSlot;
                }
            }

            GameSystem.dataManager.campaignSlotList.SetSlots(newSlotList);
            var campaignSelectStrongValue = (CampaignSelect)m_campaignSelect;
            campaignSelectStrongValue.SetList(GameSystem.dataManager.campaignSlotList);

            //Keep This Logic when cleaning Up
            SerializationHandle.SaveCampaignSlot(m_selectedSlotID, resetCampaignSlot);
        }
    }
}