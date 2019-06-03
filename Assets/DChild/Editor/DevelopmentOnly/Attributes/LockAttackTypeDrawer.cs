using DChild.Gameplay.Combat;
using Sirenix.OdinInspector.Editor;
using System;
using UnityEditor;
using UnityEngine;

namespace DChildEditor.Gameplay.Combat
{
    public sealed class LockAttackTypeDrawer : OdinAttributeDrawer<LockAttackTypeAttribute, AttackDamage>
    {
        protected override void DrawPropertyLayout(GUIContent label)
        {
            var smartValue = ValueEntry.SmartValue;
            if (Attribute.type != AttackType._COUNT)
            {
                smartValue.type = Attribute.type;
            }
            var valueProp = ValueEntry.Property.Children.Get(Convention.ATTACKDAMAGE_VALUE_VARNAME);
            valueProp.Draw(new GUIContent($"{smartValue.type} Damage:"));
        }
    }
}