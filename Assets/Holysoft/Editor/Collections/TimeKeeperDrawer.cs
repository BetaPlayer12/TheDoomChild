using Holysoft.Collections;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace HolysoftEditor.Collections
{
    public class TimeKeeperDrawer : OdinValueDrawer<TimeKeeper>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            CallNextDrawer(label);
            var entry = ValueEntry.SmartValue;
            entry.AlignTime();
            ValueEntry.SmartValue = entry;
        }
    }
}