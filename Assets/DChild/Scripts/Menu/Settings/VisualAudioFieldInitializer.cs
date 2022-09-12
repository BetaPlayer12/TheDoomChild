using DChild.Configurations;
using DChild.UI;
using Holysoft.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Menu.Settings
{
    public class VisualAudioFieldInitializer : MonoBehaviour
    {
        private IValueUI[] m_fields;

        private void AssignReferencesToFields<T>(T reference) where T: class
        {
            var UIs = GetComponentsInChildren<IReferenceUI<T>>();
            foreach (var ui in UIs)
            {
                ui.SetReference(reference);
            }
        }

        private void UpdateFields()
        {
            foreach (var field in m_fields)
            {
                field.UpdateUI();
            }
        }

        private void Start()
        {
            AssignReferencesToFields(GameSystem.settings.visual);
            AssignReferencesToFields(GameSystem.settings.audio);
            AssignReferencesToFields(GameSystem.settings.gameplay);
            m_fields = GetComponentsInChildren<IValueUI>();
            UpdateFields();
        }
    }
}