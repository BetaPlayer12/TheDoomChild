using DChild.Configurations;
using Sirenix.Serialization;
using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace DChild.Serialization
{
    public class SerializationHandle
    {
        private const string SaveFileName = "SaveFile";
        private const string SaveFileExtention = "save";
        private const string ConfigurationFileName = "config";
        private const string ConfigurationFileExtention = "config";

        public static string GetSaveFilePath(int ID) => $"{Application.persistentDataPath}/{SaveFileName}{ID}.{SaveFileExtention}";
       

        public static void SaveCampaignSlot(int slotID, CampaignSlot data)
        {
            var filePath = GetSaveFilePath(slotID);
            byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Game Save " +
                       $"\n {filePath}");
        }

        public static async Task<bool> SaveCampaignSlotAsync(int slotID, CampaignSlot data)
        {
            await Task.Run(() => SaveCampaignSlot(slotID, data));
            return true;
        }

        public static void DeleteCampaignSlot(int ID)
        {
            var filePath = GetSaveFilePath(ID);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static void LoadCampaignSlot(int slotID, ref CampaignSlot output)
        {
            var filePath = GetSaveFilePath(slotID);
            if (File.Exists(filePath))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(filePath);
                    output = SerializationUtility.DeserializeValue<CampaignSlot>(bytes, DataFormat.Binary);
                    Debug.Log("Game Loaded " +
                           $"\n {filePath}");
                }
                catch (InvalidOperationException)
                {
                    Debug.LogError("File was corrupted in a way renewing data and deleting corrupted file" +
                     $"\n {filePath}");
                    if (output == null)
                    {
                        output = new CampaignSlot(slotID);
                        output.Reset();
                    }
                    else
                    {
                        output.Reset();
                    }
                    DeleteCampaignSlot(slotID);
                }
            }
            else
            {
                Debug.LogError("File Path does not exist");
            }
        }

        public static async Task<bool> LoadCampaignSlotAsync(int slotID, CampaignSlot data)
        {
            await Task.Run(() => LoadCampaignSlot(slotID, ref data));
            return true;
        }

        public static string GetConfigurationFilePath() => $"{Application.persistentDataPath}/{ConfigurationFileName}.{ConfigurationFileExtention}";

        public static void SaveConfiguration(GameSettingsConfiguration configuration)
        {
            byte[] bytes = SerializationUtility.SerializeValue(configuration, DataFormat.Binary);
            var filePath = GetConfigurationFilePath();
            File.WriteAllBytes(filePath, bytes);

            Debug.Log("Configuration Saved " +
                       $"\n {filePath}");
        }

        public static void LoadConfiguration(ref GameSettingsConfiguration output)
        {
            var filePath = GetConfigurationFilePath();
            if (File.Exists(filePath))
            {
                try
                {
                    byte[] bytes = File.ReadAllBytes(filePath);
                    output = SerializationUtility.DeserializeValue<GameSettingsConfiguration>(bytes, DataFormat.Binary);
                    Debug.Log("Configuration Loaded " +
                           $"\n {filePath}");
                }
                catch (InvalidOperationException)
                {
                    output = null;
                }
            }
            else
            {
                output = null;
                Debug.LogError("File Path does not exist");
            }
        }
    }

}