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
        [SerializeField, Title("Component Data")]
        private LocationBackdrop m_locationBackdropData;

        public CampaignSlotList campaignSlotList { get => m_campaignSlotList; }
        public LocationBackdrop locationBackdropData { get => m_locationBackdropData; }
    }
}