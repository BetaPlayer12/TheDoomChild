using UnityEngine;
using Sirenix.OdinInspector;
using System.Text;
using System.IO;
using System;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{
    public class DesignElementLogger : MonoBehaviour
    {
        [SerializeField, FolderPath]
        private string m_filePath;
        [SerializeField]
        private string m_fileName;
        [SerializeField]
        private GroupDetailLogger[] m_subLoggers;

        [Button]
        public void GenerateLog()
        {
            StringBuilder logBuilder = new StringBuilder();
            for (int i = 0; i < m_subLoggers.Length; i++)
            {
                m_subLoggers[i].GenerateLog(logBuilder);
            }
            CreateLogTextFile(logBuilder);
        }

        private void CreateLogTextFile(StringBuilder logBuilder)
        {
            var fileName = $"{m_filePath}/{m_fileName}.txt";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            using (FileStream fs = File.Create(fileName))
            {
                // Add some text to file    
                Byte[] content = new UTF8Encoding(true).GetBytes(logBuilder.ToString());
                fs.Write(content, 0, content.Length);
            }

            Debug.Log($"Log Generated At: {fileName}");
        }
    }
}