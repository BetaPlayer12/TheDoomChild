using Holysoft.Event;
using UnityEngine;

namespace DChildDebug.Gameplay
{
    public class StealthCharacter : MonoBehaviour, IStealth
    {
        [SerializeField]
        private bool m_isStealth;

        public bool isStealth => m_isStealth;
        public Vector2 position => transform.position;

        public event EventAction<IStealth> StealthToggled;

        public void ToggleStealth()
        {
            m_isStealth = !m_isStealth;
            StealthToggled?.Invoke(this, this);
        }
    }
}
