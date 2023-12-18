using DChild.Gameplay;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Testing.PreAlpha
{
    public static class PreAlphaUtility
    {
        public const string COMPONENTMENU_ADDRESS = "Testing/PreAlpha/";
    }

    [AddComponentMenu(PreAlphaUtility.COMPONENTMENU_ADDRESS+ "PlayerActivityTracker")]
    public class PlayerActivityTracker : MonoBehaviour
    {
        
    }

    public class PreAlphaSerializer : MonoBehaviour
    {
        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
        }
    }

    [System.Serializable]
    public class PlayerActivityData
    {

    }

}