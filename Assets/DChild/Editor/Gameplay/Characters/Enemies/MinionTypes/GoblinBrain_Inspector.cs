using DChild.Gameplay.Characters.Enemies;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Enemies
{
    [CustomEditor(typeof(GoblinBrain))]
    public class GoblinBrain_Inspector : OdinEditor
    {
        private void OnSceneGUI()
        {
            var brain = target as GoblinBrain;
            var forward = Vector3.right * brain.transform.localScale.x;
            var startPoint = brain.transform.position;
            var attackRangeEndPoint = startPoint + (forward * brain.attackDistance);
            Handles.color = Color.red;
            Handles.DrawAAPolyLine(6f, new Vector3[] {startPoint, attackRangeEndPoint});
           
        }
    }
}
