using UnityEngine;
using Sirenix.OdinInspector;
using System.Text;
using System.Collections;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{
    [RequireComponent(typeof(LevelMapElement))]
    public class ChestLogger : HackNPlanLogger
    {
        [SerializeField, ValueDropdown("GetDesignID")]
        private string m_lootDesignID;

        public override void GenerateLog(StringBuilder stringBuilder)
        {
            var parentLogger = GetComponentInParent<LevelElementListLogger>();
            var designElement = parentLogger?.designElementID ?? "NULL";
            var element = HackNPlanUtils.EmphasizeGuideIDs(GetComponent<LevelMapElement>());
            stringBuilder.AppendLine($" {designElement}{element} contains {m_lootDesignID}");
        }

        private IEnumerable GetDesignID() => HackNPlanManager.instance.GetDesignElements();
    }
}