using UnityEngine;

namespace Holysoft
{
    public class MonoBehaviourToggler : MonoBehaviour
    {
        [SerializeField]
        private MonoBehaviour m_component;

        public void Toggle() => m_component.enabled = !m_component.enabled;
    }
}