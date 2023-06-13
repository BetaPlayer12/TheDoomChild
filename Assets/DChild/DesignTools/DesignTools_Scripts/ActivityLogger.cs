using UnityEngine;
using Sirenix.OdinInspector;
using System.Text;
using System.Collections;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{
    [System.Serializable]
    public class ActivityLogger
    {
        [SerializeField, ValueDropdown("GetDesignID")]
        private string m_designElementID;
        [SerializeField]
        private string m_activityDescription;
        [SerializeField]
        private LevelMapElement[] m_targets;

        public void GenerateLog(StringBuilder stringBuilder)
        {
            stringBuilder.Append($"{m_designElementID} {HackNPlanUtils.EmphasizeGuideIDs(m_targets)} {m_activityDescription}");
        }

        private IEnumerable GetDesignID() => HackNPlanManager.instance.GetDesignElements();
    }
}