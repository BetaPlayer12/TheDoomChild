using Sirenix.OdinInspector;
using System.Collections;
using System.Text;
using UnityEngine;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{
    [RequireComponent(typeof(LevelMapElement))]
    public class ObjectConnectionLogger : HackNPlanLogger
    {
        [SerializeField, ValueDropdown("GetDesignID")]
        private string m_designElementID;
        [SerializeField]
        private string m_connectionDescription;
        [SerializeField, ValueDropdown("GetDesignID")]
        private string m_connectedElementID;
        [SerializeField]
        private bool m_elementIsOnSameLocation;
        [SerializeField, ShowIf("m_elementIsOnSameLocation")]
        private LevelMapElement m_destination;


        public override void GenerateLog(StringBuilder stringBuilder)
        {
            var element = HackNPlanUtils.EmphasizeGuideIDs(GetComponent<LevelMapElement>());

            stringBuilder.Append($"- {m_designElementID}{element} {m_connectionDescription} {m_connectedElementID}");
            if (m_elementIsOnSameLocation)
            {
                stringBuilder.Append($" {HackNPlanUtils.EmphasizeGuideIDs(m_destination)}");
            }
            stringBuilder.Append(".\n");
        }

        private IEnumerable GetDesignID() => HackNPlanManager.instance.GetDesignElements();
    }
}