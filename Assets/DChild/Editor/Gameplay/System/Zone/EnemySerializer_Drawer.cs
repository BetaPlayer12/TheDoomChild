using DChild.Gameplay.Systems.Serialization;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Systems.Serialization
{
    [OdinDrawer]
    public class EnemySerializer_Drawer : OdinValueDrawer<EnemySerializer>
    {
        protected override void DrawPropertyLayout(IPropertyValueEntry<EnemySerializer> entry, GUIContent label)
        {
            var serializer = entry.SmartValue;
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Serialized Enemy:",serializer, typeof(EnemySerializer), true);
            GUI.enabled = true;
            serializer.isAlive = EditorGUILayout.ToggleLeft("Is Alive", serializer.isAlive);
        }
    }

}