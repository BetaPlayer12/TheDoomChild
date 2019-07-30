using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Refactor.DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class LedgeGrab : MonoBehaviour, IComplexCharacterModule, IControllableModule
    {
        private Character m_character;
        private CharacterPhysics2D m_physics;
        private RaySensor m_ledgeSensorCliff;
        private RaySensor m_ledgeSensorEdge;
        private ILedgeGrabState m_state;

        private Animator m_animator;
        private string m_ledgeGrabParameter;
        private string m_speedYDirectionParameter;
        private string m_midAirParameter;

        private GroundednessHandle m_groundednessHandle;

        [SerializeField]
        private Vector2 m_groundRayOriginOffset;
        [SerializeField]
        private Vector2 m_destinationOffset;

        private bool FindGroundDestination(out Vector2 groundPosition)
        {
            groundPosition = Vector2.zero;
            Raycaster.SetLayerCollisionMask(LayerMask.NameToLayer("Environment"));
            int hitcount;
            var hitBuffer = Raycaster.Cast(CalculateGroundRayOrigin(), Vector2.down, m_groundRayOriginOffset.y, true, out hitcount);
            if (hitcount > 0)
            {
                groundPosition = hitBuffer[0].point;
            }
            return hitcount > 0;
        }

        private Vector2 CalculateGroundRayOrigin()
        {
            var wallContactPoint = m_ledgeSensorCliff.GetHits()[0].point;
            wallContactPoint.y += m_groundRayOriginOffset.y;
            wallContactPoint.x += m_groundRayOriginOffset.x * (int)m_character.facing;
            return wallContactPoint;
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_character = info.character;
            m_ledgeSensorCliff = info.GetSensor(PlayerSensorList.SensorType.LedgeCliff);
            m_ledgeSensorEdge = info.GetSensor(PlayerSensorList.SensorType.LedgeEdge);
            m_state = info.state;
            m_physics = info.physics;
            m_animator = info.animator;
            m_ledgeGrabParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.LedgeGrab);
            m_speedYDirectionParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY);
            m_midAirParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsMidAir);
            m_groundednessHandle = info.groundednessHandle;
        }

        public void ConnectTo(IMainController controller)
        {
            // GetComponentInParent<ILandController>().LandCall += OnLandCall;
            controller.GetSubController<ILedgeController>().LedgeGrabCall += AttemptToLedgeGrab;
        }

        public void AttemptToLedgeGrab()
        {
            m_ledgeSensorCliff.Cast();
            m_ledgeSensorEdge.Cast();

            var canLedgeGrab = m_ledgeSensorCliff.isDetecting == true && m_ledgeSensorEdge.isDetecting == false;
            if (canLedgeGrab)
            {
                Vector2 groundPosition;
                if (FindGroundDestination(out groundPosition))
                {
                    StartCoroutine(PullFromCliff(groundPosition));
                }
            }
        }

        private IEnumerator PullFromCliff(Vector2 groundPosition)
        {
            m_state.waitForBehaviour = true;
            m_state.isFalling = false;
            m_state.isGrounded = true;
            m_animator.SetTrigger(m_ledgeGrabParameter);
            m_animator.SetInteger(m_speedYDirectionParameter, 0);
            m_animator.SetBool(m_midAirParameter, false);
            m_groundednessHandle.enabled = false;
            yield return new WaitForEndOfFrame();
            m_groundednessHandle.enabled = true;
            m_physics.SetVelocity(Vector2.zero);
            var currentPosition = m_physics.position;
            currentPosition.y = groundPosition.y + m_destinationOffset.y;
            currentPosition.x = currentPosition.x + (m_destinationOffset.x * (int)m_character.facing); //i addedd a forward force by 1 to make sure that the player avoids the edge slide so the player is standing on the platform
            m_physics.position = currentPosition;
        }

        private void AttemptToLedgeGrab(object sender, EventActionArgs eventArgs)
        {
            AttemptToLedgeGrab();
        }
    }
}