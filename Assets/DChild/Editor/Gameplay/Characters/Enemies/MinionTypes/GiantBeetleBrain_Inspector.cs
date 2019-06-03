using DChild.Gameplay.Characters.Enemies;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Enemies
{
    [CustomEditor(typeof(GiantBeetleBrain))][CanEditMultipleObjects]
    public class GiantBeetleBrain_Inspector : OdinEditor
    {
        private void DrawActivityRadius(Vector2 position, float radius)
        {
            Handles.color = new Color(0, 1, 0, 0.1f);
            Handles.DrawSolidDisc(position, Vector3.forward, radius);
        }

        private void OnSceneGUI()
        {
            var brain = target as GiantBeetleBrain;
            var forward = Vector3.right * brain.transform.localScale.x;
            var startPoint = brain.transform.position;
            var attackDistanceEndPoint = startPoint + (forward * brain.attackDistance);

            if (Application.isPlaying)
            {
                DrawActivityRadius(startPoint, brain.activityRadius);
            }
            else
            {
                DrawActivityRadius(startPoint, brain.activityRadius);
            }

            Handles.color = Color.red;
            Handles.DrawAAPolyLine(6f, new Vector3[] { startPoint, attackDistanceEndPoint});
        }
    }
}
