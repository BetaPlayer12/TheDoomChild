using DChild.Menu.UI;
using Holysoft;
using Holysoft.Event;
using Holysoft.UI;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Menu
{
    public class SettingsFieldManager : MonoBehaviour
    {
        private SettingsField[] m_fields;

        private void OnFocusUnlock(object sender, FocusLockUIEventArgs eventArgs)
        {
            for (int i = 0; i < m_fields.Length; i++)
            {
                m_fields[i].SetFocusLock(false);
            }
        }

        private void OnFocusLock(object sender, FocusLockUIEventArgs eventArgs)
        {
            for (int i = 0; i < m_fields.Length; i++)
            {
                m_fields[i].SetFocusLock(true);
            }
        }

        private void OnEnable()
        {
            m_fields = GetComponentsInChildren<SettingsField>();
            for (int i = 0; i < m_fields.Length; i++)
            {
                m_fields[i].FocusLock += OnFocusLock;
                m_fields[i].FocusUnlock += OnFocusUnlock;
                m_fields[i].SetFocusLock(false);
            }
        }

        private void OnDisable()
        {
            for (int i = 0; i < m_fields.Length; i++)
            {
                m_fields[i].FocusLock -= OnFocusLock;
                m_fields[i].FocusUnlock -= OnFocusUnlock;
            }
        }
    }
}