using UnityEngine;
using Sirenix.OdinInspector;
using System.Text;
using System.Collections;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{

    [RequireComponent(typeof(LevelMapElementGroup))]
    public class SwitchElementListLogger : HackNPlanLogger
    {
        [System.Serializable]
        private class Sublogs
        {
            [SerializeField, ChildGameObjectsOnly]
            private LevelMapElement[] m_triggers;
            [SerializeField]
            private ActivityLogger[] m_activity;

            public void GenerateLog(StringBuilder stringBuilder, string designElementID)
            {
                stringBuilder.Append($"- {designElementID} {HackNPlanUtils.EmphasizeGuideIDs(m_triggers)} when activated the ");
                for (int i = 0; i < m_activity.Length; i++)
                {
                    m_activity[i].GenerateLog(stringBuilder);
                    if (i + 1 < m_activity.Length)
                    {
                        stringBuilder.Append("; ");
                    }
                }
                stringBuilder.Append(".");
            }

            private IEnumerable GetDesignID() => HackNPlanManager.instance.GetDesignElements();
        }

        [SerializeField, ValueDropdown("GetDesignID")]
        private string m_designElementID;
        [SerializeField, TableList]
        private Sublogs[] m_sublogs;

        public override void GenerateLog(StringBuilder stringBuilder)
        {
            for (int i = 0; i < m_sublogs.Length; i++)
            {
                m_sublogs[i].GenerateLog(stringBuilder, m_designElementID);
                stringBuilder.Append("\n");
            }
            Debug.Log(stringBuilder.ToString());
        }

        private IEnumerable GetDesignID() => HackNPlanManager.instance.GetDesignElements();
    }
}