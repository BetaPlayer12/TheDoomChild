using DChild.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DChildDebug
{
    public class ForceSave : MonoBehaviour
    {
        [SerializeField]
        private CampaignSlotData[] m_datas;

        private void Awake()
        {
            for (int i = 0; i < m_datas.Length; i++)
            {
                m_datas[i].SaveToFile();
            }
        }
    }

}