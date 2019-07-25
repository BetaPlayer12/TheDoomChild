using DChild.Gameplay.Characters.Players.Modules;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Behaviour
{
    public class LedgeGrab : MonoBehaviour, IPlayerExternalModule, IEventModule
    {
        private IFacing m_characterFacing;
        private RaySensor m_ledgeSensorCliff;
        private RaySensor m_ledgeSensorEdge;
        private RaySensor m_groundHeightSensor;
        private ILedgeGrabState m_state;
        private IPlatformDropState m_dropPlatform;
        private CharacterPhysics2D m_physics;
        [SerializeField]
        private Vector2 m_groundRayOriginOffset;


        [SerializeField]
        private Transform headObject;
        [SerializeField, MinValue(1)]
        private int distance;


        [Button]
        private void HeadCast()
        {
            Debug.Log("test cast");
            Raycaster.SetLayerMask(LayerMask.GetMask("Environment"));
            int hitcount;
            var hitBuffer = Raycaster.Cast(headObject.position, Vector2.up, distance, true, out hitcount);
            if (hitcount > 0)
            {

                Debug.Log("test hit");

                m_state.canLedgeGrab = false;
            }
            else
                m_state.canLedgeGrab = true;
        }




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
            wallContactPoint.x += m_groundRayOriginOffset.x * (int)m_characterFacing.currentFacingDirection;
            return wallContactPoint;
        }

        public void ConnectEvents()
        {
            //GetComponentInParent<ILandController>().LandCall += OnLandCall;
            GetComponentInParent<ILedgeController>().LedgeGrabCall += PullFromCliff;
        }


        public void Initialize(IPlayerModules player)
        {
            m_characterFacing = player;
            m_ledgeSensorCliff = player.sensors.ledgeSensorCliff;
            m_ledgeSensorEdge = player.sensors.ledgeSensorEdge;
            m_groundHeightSensor = player.sensors.groundHeightSensor;
            m_state = player.characterState;
            //m_playerAnimation = player.animation;
            m_physics = player.physics;
            m_dropPlatform = player.characterState;
        }

        public void PullFromCliff()
        {
            m_state.canLedgeGrab = false;
            HeadCast();

            m_ledgeSensorCliff.Cast();
            m_ledgeSensorEdge.Cast();
            m_groundHeightSensor.Cast();

            Debug.Log("Can LedgeGrab is " + m_state.canLedgeGrab);
            if (m_state.canLedgeGrab)
            {


                if (m_ledgeSensorCliff.isDetecting == true && m_ledgeSensorEdge.isDetecting == false && m_groundHeightSensor.isDetecting == false)
                {

                    Vector2 groundPosition;
                    if (FindGroundDestination(out groundPosition))
                    {

                        var currentPosition = m_physics.position;
                        currentPosition.y = groundPosition.y - 1;
                        currentPosition.x = currentPosition.x + (2 * (int)m_characterFacing.currentFacingDirection); //i addedd a forward force by 1 to make sure that the player avoids the edge slide so the player is standing on the platform
                        m_physics.position = currentPosition;
                        StartCoroutine(ChangeLedgingStateToFalse());
                    }
                }
            }

        }

        private void PullFromCliff(object sender, EventActionArgs eventArgs)
        {
            PullFromCliff();
        }

        private IEnumerator ChangeLedgingStateToFalse()
        {
            m_state.waitForBehaviour = true;
            m_state.isLedging = true;
            yield return new WaitForEndOfFrame();
            //m_playerAnimation.DoLedgeGrab(m_characterFacing.currentFacingDirection);
            //m_playerAnimation.EnableRootMotion(true, true);
            //m_playerAnimation.AnimationSet += OnAnimationSet;
            //m_playerAnimation.animationState.Complete += OnComplete;
            while (m_state.isLedging)
            {
                yield return null;
            }

            //m_playerAnimation.AnimationSet -= OnAnimationSet;
            //m_playerAnimation.animationState.Complete -= OnComplete;

        }

        private void OnComplete(TrackEntry trackEntry)
        {
            EndLedgeGrab();

        }

        private void OnAnimationSet(object sender, AnimationEventArgs eventArgs)
        {
            EndLedgeGrab();
        }

        private void EndLedgeGrab()
        {
            m_state.waitForBehaviour = false;
            m_state.isLedging = false;
            // m_playerAnimation.DisableRootMotion();
        }


        private void OnLandCall(object sender, EventActionArgs eventArgs)
        {

            m_state.canLedgeGrab = true;
        }

    }
}