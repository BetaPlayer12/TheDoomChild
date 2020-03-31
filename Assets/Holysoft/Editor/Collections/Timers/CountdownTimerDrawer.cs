using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Holysoft.Collections;

namespace HolysoftEditor.Collections
{
    
    public class CountdownTimerDrawer : OdinValueDrawer<CountdownTimer>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            ValueEntry.Property.FindChild(x => x.Name == "m_startTime", true).Draw(new GUIContent(label.text));
        }
    }
}