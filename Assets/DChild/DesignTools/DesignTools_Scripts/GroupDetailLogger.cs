using UnityEngine;
using Sirenix.OdinInspector;
using System.Text;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{
    public class GroupDetailLogger : MonoBehaviour
    {
        [SerializeField]
        private string m_header;

        [Button]
        public void GenerateLog(StringBuilder logBuilder)
        {
            var loggers = GetComponentsInChildren<HackNPlanLogger>();
            if (loggers.Length > 0)
            {
                logBuilder.AppendLine(HackNPlanUtils.ConvertToHeader(m_header));
                for (int i = 0; i < loggers.Length; i++)
                {
                    loggers[i].GenerateLog(logBuilder);
                }

                logBuilder.Append("\n\n");
            }
        }
    }
}