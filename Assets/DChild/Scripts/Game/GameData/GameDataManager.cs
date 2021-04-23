using Sirenix.OdinInspector;
using UnityEngine;
using System.IO;
using DChild.Serialization;
using Sirenix.Serialization;

namespace DChild
{
    public class GameDataManager : SerializedMonoBehaviour
    {
        [SerializeField]
        private bool m_doNotUseExistingFiles; 

        [OdinSerialize,HideReferenceObjectPicker, Title("Save File"), HideLabel]
        private CampaignSlotList m_campaignSlotList = new CampaignSlotList();

        public CampaignSlotList campaignSlotList => m_campaignSlotList;

        public void InitializeCampaignSlotList()
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
            if(m_doNotUseExistingFiles == false)
            {
                InitializeCampaignSlotList();
            }
        }
    }
}