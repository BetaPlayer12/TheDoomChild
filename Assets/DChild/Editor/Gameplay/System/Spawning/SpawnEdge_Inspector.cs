using DChild.Gameplay;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay
{
    [CustomEditor(typeof(SpawnEdge))]
    public class SpawnEdge_Inspector : OdinEditor
    {
        private void OnSceneGUI()
        {
            var spawnEdge = target as SpawnEdge;
            var points = spawnEdge.edgePoints;

            Handles.color = Color.blue;
            var position = (Vector2)spawnEdge.transform.position;
            for (int i = 0; i < points.Length; i++)
            {
                if (i == points.Length - 1)
                {
                    Handles.color = Color.yellow;
                }

                var handlePos = position + points[i];
                handlePos = Handles.FreeMoveHandle(handlePos, Quaternion.identity, 1f, Vector2.one * 0.1f, Handles.CubeHandleCap);
                points[i] = handlePos - position;
            }

            Vector3[] point3D = new Vector3[points.Length];
            for (int i = 0; i < point3D.Length; i++)
            {
                point3D[i] = position + points[i];
            }
            Handles.color = new Color(0, 1f, 0f, 0.25f);
            Handles.DrawAAPolyLine(10f, point3D);
        }
    }
}