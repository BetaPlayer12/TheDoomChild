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

        public static string GetSaveFilePath(int ID) => $"{Application.persistentDataPath}/{SaveFileName}{ID}.{SaveFileExtention}";

        public static void Save(int slotID, CampaignSlot data)
        {
            var filePath = GetSaveFilePath(slotID);
            byte[] bytes = SerializationUtility.SerializeValue(data, DataFormat.Binary);
            File.WriteAllBytes(filePath, bytes);
            Debug.Log("Game Save " +
                       $"\n {filePath}");
        }

        public static async Task<bool> SaveAsync(int slotID, CampaignSlot data)
        {
            await Task.Run(() => Save(slotID, data));
            return true;
        }

        public static void Delete(int ID)
        {
            var filePath = GetSaveFilePath(ID);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        public static void Load(int slotID, ref CampaignSlot output)
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
                    Delete(slotID);
                }
            }
            else
            {
                Debug.LogError("File Path does not exist");
            }
        }
        public static async Task<bool> LoadAsync(int slotID, CampaignSlot data)
        {
            await Task.Run(() => Load(slotID, ref data));
            return true;
        }
    }

}