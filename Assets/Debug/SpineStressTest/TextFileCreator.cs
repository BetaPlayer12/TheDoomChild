using Sirenix.OdinInspector;
using System.IO;
using System.Text;
using UnityEngine;

namespace DChildDebug.Spine.Tests
{
    public class TextFileCreator : MonoBehaviour
    {
        [SerializeField, FolderPath]
        private string m_folderPath;

        [SerializeField]
        private string m_fileNamePrefix;

        public void CreateFile(string fileName, string content)
        {
            var filePath = $"{m_folderPath}/{m_fileNamePrefix}_{fileName}.txt";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // Create a new file     
            using (FileStream fs = File.Create(filePath))
            {
                // Add some text to file    
                byte[] title = new UTF8Encoding(true).GetBytes(content);
                fs.Write(title, 0, title.Length);
            }

            Debug.Log($"File Created: {fileName}");
        }
    }
}