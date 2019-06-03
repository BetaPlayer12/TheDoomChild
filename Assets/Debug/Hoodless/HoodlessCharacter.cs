using Holysoft.Event;
using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class HoodlessCharacter : MonoBehaviour, IHoodless
    {
        [SerializeField]
        private bool m_isHoodless;

        public bool isHoodless => m_isHoodless;
        public Vector2 position => transform.position;

        public event EventAction<IHoodless> HoodlessToggled;

        public void ToggleHoodless()
        {
            m_isHoodless = !m_isHoodless;
            HoodlessToggled?.Invoke(this, this);
        }
    }
}