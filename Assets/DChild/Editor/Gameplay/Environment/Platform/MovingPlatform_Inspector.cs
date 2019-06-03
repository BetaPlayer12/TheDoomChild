using DChild.Gameplay.Environment.Platforms;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Environment.Platforms
{
    [CustomEditor(typeof(MovingPlatform))]
    public class MovingPlatform_Inspector : OdinEditor
    {
        private void OnSceneGUI()
        {
            var movingPlatform = this.target as MovingPlatform;
            if (movingPlatform.sequences != null)
            {
                if (movingPlatform.useCurrentPosition)
                {
                    if (movingPlatform.overrideSequenceIndex >= movingPlatform.sequences.Length)
                    {
                        movingPlatform.overrideSequenceIndex = movingPlatform.sequences.Length - 1;
                    }
                    movingPlatform.sequences[movingPlatform.overrideSequenceIndex].position = movingPlatform.transform.position;
                }

                var sequences = movingPlatform.sequences;
                List<Vector3> point3D = new List<Vector3>();
                for (int i = 0; i < sequences.Length; i++)
                {
                    var sequence = sequences[i];
                    Handles.Label(sequence.position, $"[{i}]Wait: {sequence.waitDuration}s");
                    point3D.Add(sequence.position);
                }

                if (movingPlatform.sequenceType == MovingPlatform.SequenceType.Loop)
                {
                    point3D.Add(sequences[0].position);
                }

                Handles.color = new Color(0, 1f, 0f, 0.25f);
                Handles.DrawAAPolyLine(10f, point3D.ToArray());
            }
        }
    }
}

