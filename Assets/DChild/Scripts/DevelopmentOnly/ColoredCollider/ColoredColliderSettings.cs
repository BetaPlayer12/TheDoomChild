using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace DChild.Configurations
{
    [System.Serializable]
    public class ColoredColliderSettings : SerializedScriptableObject
    {
#if UNITY_EDITOR
        private const string FILENAME = "ColoredColliderSettings.asset";
        private const string FILEDIRECTORY = "Assets/DChild/Objects/DevelopementOnly/Editor/Resources/";
        private static string filepath => FILEDIRECTORY + FILENAME;
        private static ColoredColliderSettings m_instance;
        public static ColoredColliderSettings instance
        {
            get
            {
                //if (m_instance == null)
                //{
                //    m_instance = AssetDatabase.LoadAssetAtPath<ColoredColliderSettings>(filepath);
                //    if (m_instance == null)
                //    {
                //        m_instance = CreateInstance<ColoredColliderSettings>();
                //        AssetDatabase.CreateAsset(m_instance, filepath);
                //    }
                //}
                return m_instance;
            }

        }

        [SerializeField]
        private Dictionary<string, Color> list = new Dictionary<string, Color>();

        public string[] GetOptions() => list.Keys.ToArray();
        public Color GetColor(string colorLabel) => list[colorLabel]; 
#endif
    }
}