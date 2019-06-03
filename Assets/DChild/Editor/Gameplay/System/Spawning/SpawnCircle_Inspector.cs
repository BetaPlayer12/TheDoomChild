using DChild.Gameplay;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay
{
    [CustomEditor(typeof(SpawnCircle))]
    public class SpawnCircle_Inspector : OdinEditor
    {
        private void OnSceneGUI()
        {
            var spawnCircle = target as SpawnCircle;
            Handles.color =  new Color(0, 1, 0, 0.1f);
            Handles.DrawSolidDisc(spawnCircle.transform.position, Vector3.forward, spawnCircle.radius);
        }
    }
}