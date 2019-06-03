using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Holysoft.Collections;

namespace HolysoftEditor.Collections
{
    [OdinDrawer]
    public class CountdownTimerDrawer : OdinValueDrawer<CountdownTimer>
    {
        protected override void DrawPropertyLayout(IPropertyValueEntry<CountdownTimer> entry, GUIContent label)
        {
            var timer = entry.SmartValue;
            timer.startTime = EditorGUILayout.FloatField(label, timer.startTime);
        }
    }
}