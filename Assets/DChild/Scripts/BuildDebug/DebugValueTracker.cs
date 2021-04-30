using Holysoft.Event;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace DChildDebug.Window
{
    public class DebugValueTracker : SerializedMonoBehaviour
    {
        [SerializeField]
        private ITrackableValue m_reference;
        [SerializeField]
        private TextMeshProUGUI m_output;

        private void DisplayTrackedValue()
        {
            m_output.text = $"[{m_reference.value}]";
        }

        private void OnValueChange(object sender, EventActionArgs eventArgs)
        {
            DisplayTrackedValue();
        }

        private void Awake()
        {
            m_reference.ValueChange += OnValueChange;
        }

        private void Start()
        {
            DisplayTrackedValue();
        }
    }

}