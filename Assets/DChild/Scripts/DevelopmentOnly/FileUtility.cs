#if UNITY_EDITOR
using System;
using UnityEditor;

namespace DChildEditor
{
    public static class FileUtility
    {
        public static void RenameAsset<T>(T reference, string assetPath, string newName) where T : UnityEngine.Object
        {
            if (AssetDatabase.LoadAssetAtPath<T>(assetPath) != null)
            {
                string fileName = GetIndexedFileName(reference, GetDirectoryOf(assetPath), newName, GetAssetExtention(assetPath));
                AssetDatabase.RenameAsset(assetPath, fileName);
                AssetDatabase.SaveAssets();
            }
            else
            {
                throw new Exception($"Path does not lead to an existing asset:: {assetPath}");
            }
        }

        public static string GetDirectoryOf(string filePath)
        {
            var splitPath = filePath.Split('/');
            string directory = "";
            for (int i = 0; i < splitPath.Length - 1; i++)
            {
                directory += splitPath[i] + "/";
            }
            return directory;
        }

        public static string GetFileName(string filePath)
        {
            var splitPath = filePath.Split('/');
            string filename = splitPath[splitPath.Length - 1];
            if (filename.Contains(".")) ;
            var fileNameSplit = filename.Split('.');
            return fileNameSplit[0];
        }

        public static string GetAssetExtention(string filePath)
        {
            if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filePath) != null)
            {
                var split = filePath.Split('.');
                return "." + split[1];
            }
            else
            {
                throw new Exception($"Path does not lead to an existing asset:: {filePath}");
            }
        }

        private static string GetIndexedFileName<T>(T reference, string folderPath, string assetName, string extention) where T : UnityEngine.Object
        {
            int index = 0;
            var indexedName = assetName;
            var existingFile = AssetDatabase.LoadAssetAtPath<T>(folderPath + assetName + extention);
            while (existingFile != null && existingFile != reference)
            {
                index++;
                indexedName = assetName + index;
                existingFile = AssetDatabase.LoadAssetAtPath<T>(folderPath + indexedName + extention);
            }
            return indexedName;
        }
    }
}
#endif