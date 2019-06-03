using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Environment.Platforms;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment.Platforms
{
    [CustomEditor(typeof(RepositionPlatform))]
    public class RepositionPlatform_Inspector : OdinEditor
    {
        private void OnSceneGUI()
        {
            var platform = target as RepositionPlatform;
            var positions = platform.positions;

            if (Application.isPlaying == false)
            {
                positions[0] = platform.transform.position;
            }
            Handles.color = Color.blue;
            for (int i = 1; i < positions.Length; i++)
            {
                positions[i] = Handles.FreeMoveHandle(positions[i], Quaternion.identity, 1f, Vector3.one * 0.1f, Handles.CubeHandleCap);
            }

            platform.positions = positions;
            Vector3[] positions3D = new Vector3[positions.Length + 1];
            for (int i = 0; i < positions.Length; i++)
            {
                positions3D[i] = positions[i];
            }
            positions3D[positions3D.Length - 1] = positions[0];

            Handles.color = new Color(0, 1f, 0f, 0.25f);
            Handles.DrawAAPolyLine(10f, positions3D);
        }
    }

}