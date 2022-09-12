using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DChildDebug
{
    public class VersionText : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI m_text;

        private void OnValidate()
        {
            if (m_text)
            {
                m_text.text = $"Version {Application.version}";
            }
        }
    }
}