using DChild.Gameplay;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay
{
    [CustomEditor(typeof(SpawnBox))]
    public class SpawnBox_Inspector : OdinEditor
    {
        private void OnSceneGUI()
        {
            var spawnBox = target as SpawnBox;
            Color color = new UnityEngine.Color(0, 1, 0, 0.1f);

            var position = spawnBox.transform.position;
            position.x -= spawnBox.widthExtent;
            position.y -= spawnBox.lengthExtent;

            var size = new Vector2(spawnBox.widthExtent * 2, spawnBox.lengthExtent * 2);

            Rect rect = new Rect(position, size);
            Handles.DrawSolidRectangleWithOutline(rect, color, color);
        }
    }
}