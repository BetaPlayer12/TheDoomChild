using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ClockTimeAttribute_Drawer : OdinAttributeDrawer<ClockTimeAttribute, float>
{
    private int m_seconds;
    private int m_minutes;
    private int m_hours;

    private int m_fieldWidth = 50;
    private int m_colonLabelWidth = 10;

    protected override void DrawPropertyLayout(GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.BeginHorizontal();
        TimeSpan timeSpan = TimeSpan.FromSeconds(ValueEntry.SmartValue);
        m_hours = (int)timeSpan.TotalHours;
        m_minutes = timeSpan.Minutes;
        m_seconds = timeSpan.Seconds;

        GUILayout.Label(label);
        m_hours = SirenixEditorFields.IntField(m_hours, GUILayout.Width(m_fieldWidth));
        GUILayout.Label(":", GUILayout.Width(m_colonLabelWidth));
        m_minutes = SirenixEditorFields.IntField(m_minutes, GUILayout.Width(m_fieldWidth));
        GUILayout.Label(":", GUILayout.Width(m_colonLabelWidth));
        m_seconds = SirenixEditorFields.IntField(m_seconds, GUILayout.Width(m_fieldWidth));
        EditorGUILayout.EndHorizontal();
        if (EditorGUI.EndChangeCheck())
        {
            AlignTime();
            var updatedTime = new TimeSpan(m_hours, m_minutes, m_seconds);
            ValueEntry.SmartValue = (float)updatedTime.TotalSeconds;
            Property.Tree.ApplyChanges();
        }
    }

    public void AlignTime()
    {
        if (m_seconds >= 60)
        {
            var addedMinutes = Mathf.FloorToInt(m_seconds / 60);
            m_seconds = m_seconds % 60;
            m_minutes += addedMinutes;

        }
        else if (m_seconds < 0)
        {
            if (m_minutes > 0)
            {
                var positiveSeconds = Mathf.Abs(m_seconds);
                var deductedMinutes = Mathf.FloorToInt(positiveSeconds / 60) + 1;
                m_minutes -= deductedMinutes;
                m_seconds = 60 - (positiveSeconds % 60);
                if (m_seconds == 60)
                {
                    m_seconds = 0;
                }
            }
            else
            {
                m_seconds = 0;
            }
        }

        if (m_minutes >= 60)
        {
            var addedhours = Mathf.FloorToInt(m_minutes / 60);
            m_minutes = m_minutes % 60;
            m_hours += addedhours;
        }
        else if (m_minutes < 0)
        {
            if (m_hours > 0)
            {
                var positiveMinutes = Mathf.Abs(m_minutes);
                var deductedHours = Mathf.FloorToInt(positiveMinutes / 60) + 1;
                m_hours -= deductedHours;
                m_minutes = 60 - (positiveMinutes % 60);
                if (m_minutes == 60)
                {
                    m_minutes = 0;
                }
            }
            else
            {
                m_minutes = 0;
            }
        }
        if (m_hours < 0)
        {
            m_hours = 0;
        }
    }
}
