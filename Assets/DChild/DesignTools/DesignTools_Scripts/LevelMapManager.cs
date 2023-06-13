using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChildEditor.DesignTool.LevelMap
{
    public class LevelMapManager : MonoBehaviour
    {
        private static LevelMapManager m_instance;
        public static LevelMapManager instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<LevelMapManager>();
                }
                return m_instance;
            }
        }

        [SerializeField]
        private TextAsset m_idReference;

        public IEnumerable GetReferenceName()
        {
            if (m_idReference == null)
                return null;

            ValueDropdownList<string> list = new ValueDropdownList<string>();
            var referenceRows = m_idReference.text.Split("\n");
            for (int i = 1; i < referenceRows.Length; i++)
            {
                var referenceContent = referenceRows[i].Split(',');
                list.Add(referenceContent[2], referenceContent[0]);
            }
            return list;
        }

        public string GetID(string referenceName)
        {
            var referenceRows = m_idReference.text.Split("\n");
            for (int i = 1; i < referenceRows.Length; i++)
            {
                var referenceContent = referenceRows[i].Split(',');
                if (referenceContent[0] == referenceName)
                {
                    return referenceContent[1];
                }
            }

            return "NULL";
        }

        public string GetName(string referenceName)
        {
            var referenceRows = m_idReference.text.Split("\n");
            for (int i = 1; i < referenceRows.Length; i++)
            {
                var referenceContent = referenceRows[i].Split(',');
                if (referenceContent[0] == referenceName)
                {
                    return referenceContent[2];
                }
            }

            return "NULL";
        }
    }
}