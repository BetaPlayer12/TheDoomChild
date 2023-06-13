using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{
    public class HackNPlanManager : MonoBehaviour
    {
        private static HackNPlanManager m_instance;
        public static HackNPlanManager instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<HackNPlanManager>();
                }
                return m_instance;
            }
        }

        [SerializeField]
        private TextAsset m_hackNPlanReference;

        public IEnumerable GetDesignElements()
        {
            if (m_hackNPlanReference == null)
                return null;

            ValueDropdownList<string> list = new ValueDropdownList<string>();
            var referenceRows = m_hackNPlanReference.text.Split("141373");

            for (int i = 1; i < referenceRows.Length; i++)
            {
                var referenceContent = referenceRows[i].Split(',');
                list.Add(referenceContent[6], "%" + referenceContent[1]);
            }
            return list;
        }
    }
}