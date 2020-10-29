using Holysoft.Event;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class CelestialCube : MonoBehaviour
    {
        private bool m_isInASlot;
        public event EventAction<EventActionArgs> OnStateChange;

        public bool isInASlot => m_isInASlot;

        public void SetState(bool isInASlot)
        {
            if (m_isInASlot != isInASlot)
            {
                m_isInASlot = isInASlot;
                OnStateChange?.Invoke(this, EventActionArgs.Empty);
            }
        }

        public void SetInteraction(bool canBeInteracted)
        {
            if (canBeInteracted)
            {

            }
            else
            {

            }
        }
    }
}
