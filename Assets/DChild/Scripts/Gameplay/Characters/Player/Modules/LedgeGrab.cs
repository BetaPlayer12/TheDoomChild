using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class LedgeGrab : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private RaySensor m_grabbableWallSensor;
        [SerializeField]
        private RaySensor m_overheadSensor;
        [SerializeField]
        private RaySensor m_destinationSensor;
        [SerializeField, MinValue(0f)]
        private float m_destinationFromWallOffset;
        [SerializeField]
        private RaySensor m_clearingSensor;
        [SerializeField]
        private RaySensor m_footingSensor;

        private int m_animation;
        private Character m_character;
        private Rigidbody2D m_rigidbody;
        private Animator m_animator;
        private ILedgeGrabState m_state;
        private Vector2 m_destination;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            m_state = info.state;
            m_animator = info.animator;
            m_animation = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.LedgeGrab);
        }

        public bool IsDoable()
        {
            m_grabbableWallSensor.Cast();
            if (m_grabbableWallSensor.allRaysDetecting)
            {
                m_overheadSensor.Cast();
                if (m_overheadSensor.isDetecting == false)
                {
                    var wallPoint = m_grabbableWallSensor.GetValidHits()[0].point;
                    var destinationPosition = m_destinationSensor.transform.position;
                    destinationPosition.x = wallPoint.x + (m_destinationFromWallOffset * (int)m_character.facing);
                    m_destinationSensor.transform.position = destinationPosition;
                    m_destinationSensor.Cast();
                    if (m_destinationSensor.isDetecting)
                    {
                        m_destination = m_destinationSensor.GetValidHits()[0].point;
                        var clearingPos = m_clearingSensor.transform.position;
                        clearingPos.x = destinationPosition.x;
                        m_clearingSensor.transform.position = clearingPos;
                        m_clearingSensor.Cast();
                        if (m_clearingSensor.isDetecting == false)
                        {
                            m_footingSensor.Cast();
                            if (m_footingSensor.isDetecting == false)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public void Execute()
        {
            m_state.waitForBehaviour = true;
            m_animator.SetTrigger(m_animation);
            m_rigidbody.position = m_destination;
            m_rigidbody.velocity = Vector2.zero;
            //Note: Animation Gitch is happening right now. Possible solution is to play animation first and on start teleport player.s
        }

        public void EndExecution()
        {
            m_state.waitForBehaviour = false;
        }
    }
}
