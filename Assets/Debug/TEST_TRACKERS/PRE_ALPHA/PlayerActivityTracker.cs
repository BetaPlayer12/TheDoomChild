using DChild.Gameplay;
using DChild.Menu;
using Sirenix.Serialization;
using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace DChild.Testing.PreAlpha
{

    [AddComponentMenu(PreAlphaUtility.COMPONENTMENU_ADDRESS + "PlayerActivityTracker")]
    public class PlayerActivityTracker : MonoBehaviour
    {

    }

    public class PreAlphaSerializer : MonoBehaviour
    {
        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            Save();
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
        }

        private void Save()
        {
            var id = GameplaySystem.campaignSerializer.slot.id;
            var filePath = $"{Application.persistentDataPath}/PRa{id}.txt";
            PreAlphaData data = null;
            byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.JSON);
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("PreAlpha Logged " +
                       $"\n {filePath}");
        }

        private void Load(PreAlphaData data)
        {

        }

        private void Start()
        {
            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;
        }

        private void OnDestroy()
        {
            Save();
        }
    }

    [System.Serializable]
    public class PreAlphaData
    {
        [SerializeField]
        private PlayerDeathCountTracker.SaveData m_deathCount;
    }

}