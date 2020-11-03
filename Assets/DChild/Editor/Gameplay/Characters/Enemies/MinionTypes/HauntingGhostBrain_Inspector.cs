using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.Enemies;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
namespace DChildEditor.Gameplay.Enemies
{
    [CustomEditor(typeof(HauntingGhostBrain))]
    public class HauntingGhostBrain_Inspector : OdinEditor
    {
        private void OnSceneGUI()
        {
            var brain = target as HauntingGhostBrain;
            var forward = Vector3.right * brain.transform.localScale.x;
            var startPoint = brain.transform.position;
            var attackRangeEndPoint = startPoint + (forward * brain.attackRange);
            var dashDistanceEndPoint = startPoint + (forward * brain.dashDistance);
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(6f,new Vector3[] { startPoint, dashDistanceEndPoint });
            Handles.color = Color.white;
            Handles.DrawAAPolyLine(3f,new Vector3[] { startPoint, attackRangeEndPoint });
        }
    }
}