using DChild.Gameplay.Characters.Players.State;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class LedgeGrab : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private RaySensor m_wallSensor;
        [SerializeField]
        private RaySensor m_overheadSensor;
        [SerializeField]
        private RaySensor m_destinationSensor;

        private int m_animation;
        private Character m_character;
        private Animator m_animator;
        private ILedgeGrabState m_state;
        private Vector2 m_destination;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            //m_state = info.state;
        }

        public bool IsDoable()
        {
            m_wallSensor.Cast();
            if (m_wallSensor.isDetecting)
            {
                m_overheadSensor.Cast();
                if (m_overheadSensor.isDetecting == false)
                {
                    m_destinationSensor.Cast();
                    if (m_destinationSensor.isDetecting)
                    {
                        m_destination = m_destinationSensor.GetValidHits()[0].point;
                        return true;
                    }
                }
            }
            return false;
        }

        public void Execute()
        {
            m_character.transform.position = m_destination;
            m_animator.SetTrigger(m_animation);
            m_state.waitForBehaviour = true;
        }

        public void EndExecution()
        {
            m_state.waitForBehaviour = false;
        }
    }
}
