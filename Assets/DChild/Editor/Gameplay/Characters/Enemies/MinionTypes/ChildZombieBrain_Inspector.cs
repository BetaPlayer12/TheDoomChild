using System.Collections;
using System.Collections.Generic;
using DChild.Gameplay.Characters.Enemies;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Enemies
{
    [CustomEditor(typeof(ChildZombieBrain),true)]
    public class ChildZombieBrain_Inspector : OdinEditor
    {
        protected void OnSceneGUI()
        {
            var brain = target as ChildZombieBrain;
            var sign = Mathf.Sign(brain.transform.localScale.x);
            var from = new Vector3(1, 100f, 0);
            Rect rect = new Rect(brain.transform.position, new Vector2(brain.scratchDistance * sign, 5));
            var rectColor = new Color(1, 0, 0, 0.1f);
            Handles.DrawSolidRectangleWithOutline(rect, rectColor, rectColor);
            Handles.color = new Color(0, 1, 1, 0.1f);
            Handles.DrawSolidArc(brain.transform.position, Vector3.forward, from * sign, -180f, brain.projectileVomitDistance);
        }
    }
}