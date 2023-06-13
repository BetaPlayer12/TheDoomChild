using UnityEngine;
using Sirenix.OdinInspector;
using System.Text;
using System.Collections;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{
    [RequireComponent(typeof(LevelMapElement))]
    public class PassagewayLogger : HackNPlanLogger
    {
        [SerializeField, ValueDropdown("GetDesignID")]
        private string m_designElementID;

        [SerializeField, ValueDropdown("GetDesignID")]
        private string m_destinationElementID;
        [SerializeField]
        private LevelMapElement m_destination;
        [SerializeField, HideIf("@m_destination != null")]
        private string m_customDestinationID;

        public override void GenerateLog(StringBuilder stringBuilder)
        {
            var element = HackNPlanUtils.EmphasizeGuideIDs(GetComponent<LevelMapElement>());

            var destinationID = "";
            if (m_destination)
            {
                destinationID = m_destination.elementName;
            }
            else
            {
                destinationID = m_customDestinationID;
            }
            destinationID = HackNPlanUtils.EmphasizeGuideIDs(destinationID);

            stringBuilder.AppendLine($"- {m_designElementID}{element} connects to {m_destinationElementID}{destinationID}.");
        }

        private IEnumerable GetDesignID() => HackNPlanManager.instance.GetDesignElements();
    }
}