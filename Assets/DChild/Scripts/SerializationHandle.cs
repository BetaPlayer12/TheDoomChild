using DChild.Serialization;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
                byte[] bytes = File.ReadAllBytes(filePath);
                output = SerializationUtility.DeserializeValue<CampaignSlot>(bytes, DataFormat.Binary);

                Debug.Log("Game Loaded " +
                       $"\n {filePath}");
            }
            else
            {
                Debug.LogError("File Path does not exist");
            }
        }
    }

}