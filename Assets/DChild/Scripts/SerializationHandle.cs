using DChild.Serialization;
using Sirenix.OdinInspector;
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

        public static void Save(int slotID, CampaignSlot data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            var filePath = $"{Application.persistentDataPath}/{SaveFileName}{slotID}.{SaveFileExtention}";
            FileStream file = File.Create(filePath);
            bf.Serialize(file, data);
            file.Close();
            Debug.Log("Game Save " +
                       $"\n {filePath}");
        }

        public static void Load(int slotID, ref CampaignSlot output)
        {
            var filePath = $"{Application.persistentDataPath}/{SaveFileName}{slotID}.{SaveFileExtention}";
            if (File.Exists(filePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(filePath, FileMode.Open);
                output = (CampaignSlot)bf.Deserialize(file);
                file.Close();
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