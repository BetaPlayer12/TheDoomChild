using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace HolysoftEditor.Collections
{
    public class RangeIntDrawer : OdinValueDrawer<Holysoft.Collections.RangeInt>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var rangeFloat = ValueEntry.SmartValue;
            SirenixEditorGUI.BeginBox(label);
            var newMin = EditorGUILayout.IntField("Min", rangeFloat.min);
            var newMax = EditorGUILayout.IntField("Max", rangeFloat.max);
            SirenixEditorGUI.EndBox();


            if (newMin > newMax)
            {
                newMin = newMax;
            }
            else if (newMax < newMin)
            {
                newMax = newMin;
            }

            ValueEntry.SmartValue = new Holysoft.Collections.RangeInt(newMin, newMax);
        }
    }

}