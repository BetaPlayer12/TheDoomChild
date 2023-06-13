using UnityEngine;
using System.Text;

namespace DChildEditor.DesignTool.LevelMap.HackNPlan
{
    public abstract class HackNPlanLogger : MonoBehaviour
    {
        public abstract void GenerateLog(StringBuilder stringBuilder);
    }
}