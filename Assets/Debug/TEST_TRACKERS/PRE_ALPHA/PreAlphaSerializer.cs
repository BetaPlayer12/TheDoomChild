using DChild.Gameplay;
using Sirenix.Serialization;
using System;
using System.IO;
using UnityEngine;

namespace DChild.Testing.PreAlpha
{
    public class PreAlphaSerializer : MonoBehaviour
    {
        [SerializeField]
        private PlayTimeTracker m_playTimeTracker;
        [SerializeField]
        private PlayerDeathCountTracker m_deathCountTracker;
        [SerializeField]
        private ItemUsageTracker m_itemUsageTracker;
        [SerializeField]
        private bool m_isTestingEnvironment;

        private void OnPostDeserialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
            Save();
        }

        private void OnPreSerialization(object sender, CampaignSlotUpdateEventArgs eventArgs)
        {
        }

        private string GetSaveFilePath()
        {
            var id = GameplaySystem.campaignSerializer.slot.id;

            string fileName;
            if (m_isTestingEnvironment)
            {
                fileName = $"Pra_Test";
            }
            else
            {
                fileName = $"PRa{id}";
            }

            return $"{Application.persistentDataPath}/{fileName}";
        }

        private void Save()
        {
            var filePath = GetSaveFilePath();
            PreAlphaData data = CreateSaveData();
            byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.JSON);
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("PreAlpha Logged " +
                       $"\n {filePath}");
        }
        private void Load()
        {
            PreAlphaData data = null;
            var filePath = GetSaveFilePath();
            if (File.Exists(filePath))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(filePath);
                    data = SerializationUtility.DeserializeValue<PreAlphaData>(bytes, DataFormat.JSON);
                    Debug.Log("Game Loaded " +
                           $"\n {filePath}");
                }
                catch (InvalidOperationException)
                {
                    Debug.LogError("File was corrupted in a way renewing data and deleting corrupted file" +
                     $"\n {filePath}");
                }
            }
            else
            {
                Debug.LogError("File Path does not exist");
            }

            if (data != null)
            {
                m_playTimeTracker.Load(data.playTimeData);
                m_deathCountTracker.Load(data.deathCountData);
                m_itemUsageTracker.Load(data.itemUsageData);
            }
        }

        private PreAlphaData CreateSaveData()
        {
            return new PreAlphaData(m_playTimeTracker.Save(), m_deathCountTracker.Save(), m_itemUsageTracker.Save());
        }


        private void Start()
        {
            GameplaySystem.campaignSerializer.PreSerialization += OnPreSerialization;
            GameplaySystem.campaignSerializer.PostDeserialization += OnPostDeserialization;

            Load();
        }

        private void OnDestroy()
        {
            Save();
        }
    }

}