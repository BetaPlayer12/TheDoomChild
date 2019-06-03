using Holysoft.Collections;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HolysoftEditor.Collections
{
    [OdinDrawer]
    public class RangeFloatDrawer : OdinValueDrawer<RangeFloat>
    {
        protected override void DrawPropertyLayout(IPropertyValueEntry<RangeFloat> entry, GUIContent label)
        {
            var rangeFloat = entry.SmartValue;
            SirenixEditorGUI.BeginBox(label);
            var newMin = EditorGUILayout.FloatField("Min:", rangeFloat.min);
            var newMax = EditorGUILayout.FloatField("Max:", rangeFloat.max);
            SirenixEditorGUI.EndBox();


            if (newMin > newMax)
            {
                newMin = newMax;
            }
            else if (newMax < newMin)
            {
                newMax = newMin;
            }

            entry.SmartValue = new RangeFloat(newMin, newMax);
        }
    }
}