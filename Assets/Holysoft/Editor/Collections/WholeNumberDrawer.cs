using Holysoft.Collections;
using Sirenix.OdinInspector.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HolysoftEditor.Collections
{
    public class WholeNumberDrawer : OdinValueDrawer<WholeNumber>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            int value = EditorGUILayout.IntField(label, ValueEntry.SmartValue.value);
            if(value < 0)
            {
                value = 0;
            }
            ValueEntry.SmartValue.Set(value);
        }
    }

}