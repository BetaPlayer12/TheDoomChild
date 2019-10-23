using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using DChild.Serialization;

namespace DChild
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField, Title("Save File"), HideLabel]
        private CampaignSlotList m_campaignSlotList;

        public CampaignSlotList campaignSlotList { get => m_campaignSlotList; }

        private void InitializeCampaginSlotList()
        {
            CampaignSlot[] slots = new CampaignSlot[m_campaignSlotList.slotCount];
            for (int i = 0; i < slots.Length; i++)
            {
                var ID = i + 1;
                if (File.Exists(SerializationHandle.GetSaveFilePath(ID)))
                {
                    SerializationHandle.Load(ID, ref slots[i]);
                }
                else
                {
                    slots[i] = new CampaignSlot(ID);
                    slots[i].Reset();
                }
            }
            m_campaignSlotList.SetSlots(slots);
        }

        private void Awake()
        {
            InitializeCampaginSlotList();
        }
    }
}