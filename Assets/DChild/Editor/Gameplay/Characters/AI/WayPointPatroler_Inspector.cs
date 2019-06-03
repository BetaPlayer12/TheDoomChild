﻿using DChild.Gameplay.Characters.AI;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Characters.AI
{
    [CustomEditor(typeof(WayPointPatroler))]
    public class WayPointPatroler_Inspector : OdinEditor
    {
        private void DrawPatrolPath(Vector2[] waypoints, int startIndex, int iteration)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                if (i == startIndex)
                {
                    Handles.Label(waypoints[i], $"{i}: Start to {i + iteration}");
                }
                else
                {
                    Handles.Label(waypoints[i], i.ToString());
                }
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
            var patrolHandler = target as WayPointPatroler;

            if (patrolHandler.useCurrentPosition && patrolHandler.overridePatrolIndex >= 0)
            {
                patrolHandler.wayPoints[patrolHandler.overridePatrolIndex] = patrolHandler.transform.position;
            }

            var waypoints = patrolHandler.wayPoints;
            Handles.color = Color.blue;

            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = Handles.FreeMoveHandle(waypoints[i], Quaternion.identity, 0.5f, Vector3.one * 0.05f, Handles.CubeHandleCap);
            }

            DrawPatrolPath(waypoints, patrolHandler.startIndex, patrolHandler.iteration);
        }


    }

}