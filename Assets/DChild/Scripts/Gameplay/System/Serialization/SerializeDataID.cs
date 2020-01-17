using UnityEngine;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using System.IO;
#endif


namespace DChild.Serialization
{
    [CreateAssetMenu(fileName = "SerializeDataID", menuName = "DChild/Serialization/Serialize Data ID")]
    public class SerializeDataID : ScriptableObject
    {
        [SerializeField, ReadOnly]
        private int m_ID;

        public int value => m_ID;


#if UNITY_EDITOR
        private class JsonID
        {
            public int ID;
        }

        private string ReadFromFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    return reader.ReadToEnd();
                }
            }
            else
            {
                Debug.LogWarning($"File Not Found: {filePath}");
                return string.Empty;
            }
        }

        private void WriteToFile(string filePath, string json)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                writer.Write(json);
            }
        }

        private void Awake()
        {
            var fileName = "ZoneDataLatestID.txt";
            var filePath = $"{Application.dataPath}/DChild/Editor/DevelopmentOnly/{fileName}";
            var idKey = "latestZoneDataID";
            var json = ReadFromFile(filePath);
            JsonID jsonID = new JsonID();
            if (string.IsNullOrEmpty(json))
            {
                m_ID = 0;
                jsonID.ID = m_ID;
            }
            else
            {
                JsonUtility.FromJsonOverwrite(json, jsonID);
                m_ID = jsonID.ID + Random.Range(1, 10);
                jsonID.ID = m_ID;
            }
            var newjson = JsonUtility.ToJson(jsonID);
            WriteToFile(filePath, newjson);
        }
#endif
    }
}