using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace HolysoftEditor.Collections
{
    [OdinDrawer]
    public class RangeIntDrawer : OdinValueDrawer<Holysoft.Collections.RangeInt>
    {
        protected override void DrawPropertyLayout(IPropertyValueEntry<Holysoft.Collections.RangeInt> entry, GUIContent label)
        {
            var rangeFloat = entry.SmartValue;
            SirenixEditorGUI.BeginBox(label);
            var newMin = EditorGUILayout.IntField(label, rangeFloat.min);
            var newMax = EditorGUILayout.IntField(label, rangeFloat.max);
            SirenixEditorGUI.EndBox();


            if (newMin > newMax)
            {
                newMin = newMax;
            }
            else if (newMax < newMin)
            {
                newMax = newMin;
            }

            entry.SmartValue = new Holysoft.Collections.RangeInt(newMin, newMax);
        }
    }

}