using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Environment;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class LedgeGrab : MonoBehaviour, IComplexCharacterModule
    {
        [SerializeField, HideLabel]
        private LedgeGrabStatsInfo m_configuration;

        [SerializeField]
        private RaySensor m_grabbableWallSensor;
        [SerializeField]
        private RaySensor m_overheadSensor;
        [SerializeField]
        private RaySensor m_destinationSensor;
        [SerializeField]
        private RaySensor m_clearingSensor;
        [SerializeField]
        private RaySensor m_footingSensor;
        [SerializeField]
        private GameObject m_playerShadow;
        [SerializeField]
        private Collider2D m_playerHitbox;
        [SerializeField]
        private float m_heightOffset;

        private int m_animation;
        private int m_jumpParameter;
        private Character m_character;
        private Rigidbody2D m_rigidbody;
        private Animator m_animator;
        private ILedgeGrabState m_state;
        private Vector2 m_destination;
        private Damageable m_damageable;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_damageable = info.damageable;
            m_character = info.character;
            m_rigidbody = info.rigidbody;
            m_state = info.state;
            m_animator = info.animator;
            m_animation = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.LedgeGrab);
            m_jumpParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump);
        }

        public void SetConfiguration(LedgeGrabStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public bool IsDoable()
        {
            m_grabbableWallSensor.Cast();
            if (m_grabbableWallSensor.allRaysDetecting)
            {
                var hits = m_grabbableWallSensor.GetUniqueHits();

                for (int i = 0; i < hits.Length; i++)
                {
                    if (hits[i].collider.isTrigger)
                    {
                        return false;
                    }
                    else if (hits[i].collider.CompareTag("InvisibleWall") == true)
                    {
                        return false;
                    }
                    else if (hits[i].collider.GetComponent<MovingPlatform>() != null)
                    {
                        return false;
                    }

                    m_overheadSensor.Cast();
                    if (m_overheadSensor.isDetecting == false)
                    {
                        var wallPoint = m_grabbableWallSensor.GetValidHits()[0].point;
                        var destinationPosition = m_destinationSensor.transform.position;
                        destinationPosition.x = wallPoint.x + (m_configuration.distanceFromWallOffset * (int)m_character.facing);
                        m_destinationSensor.transform.position = destinationPosition;
                        m_destinationSensor.Cast();
                        if (m_destinationSensor.isDetecting)
                        {
                            m_destination = m_destinationSensor.GetValidHits()[0].point;
                            m_destination.y += m_heightOffset;
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
            }
            return false;
        }

        public void Execute()
        {
            //m_damageable.SetInvulnerability(Invulnerability.MAX);
            m_playerShadow.SetActive(false);
            m_playerHitbox.enabled = false;

            m_state.waitForBehaviour = true;
            m_animator.SetTrigger(m_animation);
            m_animator.SetBool(m_jumpParameter, false);
            m_rigidbody.velocity = Vector2.zero;
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezePosition | RigidbodyConstraints2D.FreezeRotation;
            //Note: Animation Gitch is happening right now. Possible solution is to play animation first and on start teleport player.s
        }

        public void EndExecution()
        {
            m_rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            m_state.waitForBehaviour = false;
        }

        public void Teleport()
        {
            m_rigidbody.position = m_destination;
        }

        public void EnableHitbox()
        {
            //m_damageable.SetInvulnerability(Invulnerability.None);
            m_playerShadow.SetActive(true);
            m_playerHitbox.enabled = true;
        }
    }
}
