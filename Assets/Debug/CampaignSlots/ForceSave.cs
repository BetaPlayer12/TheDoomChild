using DChild;
using DChild.Menu;
using DChild.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DChildDebug
{
    public class ForceSave : MonoBehaviour
    {
        [SerializeField]
        private bool m_loadSaveOnAwake;

        [SerializeField]
        private CampaignSlotData[] m_datas;

        public void ForceSystemSaveUpdate()
        {
            GameSystem.dataManager.InitializeCampaignSlotList();
        }
        public void ExecuteForceSave()
        {
            for (int i = 0; i < m_datas.Length; i++)
            {
                m_datas[i].SaveToFile();
            }
        }
        private void Awake()
        {
            if (m_loadSaveOnAwake)
            {
                ExecuteForceSave();
            }
        }

        public void ResetSaves()
        {
            for (int i = 0; i < m_datas.Length; i++)
            {
                if (m_datas[i].slot.demoGame)
                {
                    m_datas[i].slot.Reset();
                    m_datas[i].SaveToFile();
                }
            }

            ForceSystemSaveUpdate(); 
            var campaignSelect = FindObjectOfType<CampaignSelect>();
            campaignSelect.ReloadSlots();

        }
    }

}