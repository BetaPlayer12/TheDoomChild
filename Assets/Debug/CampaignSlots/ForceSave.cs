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

        public void ResetSaves()
        {
            ForceSystemSaveUpdate();
            var campaignSelect = FindObjectOfType<CampaignSelect>();
            //campaignSelect.ReloadSlots();
        }

        private void Awake()
        {
            if (m_loadSaveOnAwake)
            {
                ExecuteForceSave();
            }
        }
    }

}