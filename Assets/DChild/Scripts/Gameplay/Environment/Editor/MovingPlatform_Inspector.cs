using DChild.Gameplay.Environment;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment
{
    [CustomEditor(typeof(MovingPlatform))]
    public class MovingPlatform_Inspector : OdinEditor
    {
        private void DrawPatrolPath(Vector2[] waypoints)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                Handles.Label(waypoints[i], i.ToString());
            }

            Handles.color = new Color(0, 1f, 0f, 0.25f);
            Vector3[] point3D = new Vector3[waypoints.Length];
            for (int i = 0; i < waypoints.Length; i++)
            {
                point3D[i] = waypoints[i];
            }
            Handles.DrawAAPolyLine(10f, point3D);
        }

        private void OnSceneGUI()
        {
            var movingPlatform = target as MovingPlatform;

            var waypoints = movingPlatform.waypoints;
            Handles.color = Color.blue;

            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = Handles.FreeMoveHandle(waypoints[i], Quaternion.identity, 0.5f, Vector3.one * 0.05f, Handles.CubeHandleCap);
            }

            DrawPatrolPath(waypoints);
            EditorUtility.SetDirty(target);
        }
    }

}