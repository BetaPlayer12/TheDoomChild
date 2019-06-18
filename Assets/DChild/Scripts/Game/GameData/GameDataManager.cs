using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace DChild
{
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField, Title("Save File"), HideLabel]
        private CampaignSlotList m_campaignSlotList;

        public CampaignSlotList campaignSlotList { get => m_campaignSlotList; }
    }
}