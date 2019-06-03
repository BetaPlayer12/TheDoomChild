using DChild;
using Sirenix.OdinInspector.Editor;
using UnityEngine;

namespace DChildEditor
{
    [OdinDrawer]
    public sealed class RequiredVectors2Drawer : OdinAttributeDrawer<RequiredVectors2Attribute, Vector2[]>
    {
        protected override void DrawPropertyLayout(IPropertyValueEntry<Vector2[]> entry, RequiredVectors2Attribute attribute, GUIContent label)
        {
            var smartValue = entry.SmartValue;
            if (smartValue.Length < attribute.size)
            {
                Vector2[] newValue = new Vector2[attribute.size];
                for (int i = 0; i < smartValue.Length; i++)
                {
                    newValue[i] = smartValue[i];
                }

                entry.SmartValue = newValue;
            }
            this.CallNextDrawer(entry,label);
        }
    }

}