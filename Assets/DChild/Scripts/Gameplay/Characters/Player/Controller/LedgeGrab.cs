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
        private ILedgeGrabState m_state;
        private CharacterPhysics2D m_physics;
        [SerializeField]
        private Vector2 m_groundRayOriginOffset;

        [SerializeField, MinValue(1)]
        private float rayDistance;

        
        private bool FindGroundDestination(out Vector2 groundPosition)
        {
            groundPosition = Vector2.zero;
            Raycaster.SetLayerCollisionMask(LayerMask.NameToLayer("Environment"));
            int hitcount;
            var hitBuffer = Raycaster.Cast(CalculateGroundRayOrigin(),Vector2.down,rayDistance ,true, out hitcount);
            if(hitcount > 0)
            {
                groundPosition =  hitBuffer[0].point;
            }
            return hitcount > 0;
        }


        private Vector2 CalculateGroundRayOrigin()
        {
            var wallContactPoint = m_ledgeSensorCliff.GetHits()[0].point;
            wallContactPoint.y += m_groundRayOriginOffset.y;
            wallContactPoint.x += m_groundRayOriginOffset.x*(int)m_characterFacing.currentFacingDirection;
            return wallContactPoint;
        }

        private PlayerAnimation m_playerAnimation;

        public void ConnectEvents() => GetComponentInParent<ILedgeController>().LedgeGrabCall += PullFromCliff;

        public void Initialize(IPlayerModules player)
        {
            m_characterFacing = player;
            m_ledgeSensorCliff = player.sensors.ledgeSensorCliff;
            m_ledgeSensorEdge = player.sensors.ledgeSensorEdge;
            m_state = player.characterState;
            m_playerAnimation = player.animation;
            m_physics = player.physics;
        }

        public void PullFromCliff()
        {
            Debug.Log("before Cast");
            m_ledgeSensorCliff.Cast();
            m_ledgeSensorEdge.Cast();

            Debug.Log("after Cast");

            if (m_ledgeSensorCliff.isDetecting == true && m_ledgeSensorEdge.isDetecting == false)
            {
                Debug.Log("Edge detected");
                Vector2 groundPosition;
                if(FindGroundDestination(out groundPosition))
                {
                    Debug.Log("Destination  detected");
                    var currentPosition = m_physics.position;
                    currentPosition.y = groundPosition.y;
                    currentPosition.x = currentPosition.x + 1; //i addedd a forward force by 1 to make sure that the player avoids the edge slide so the player is standing on the platform
                    m_physics.position = currentPosition;
                }
                StartCoroutine(ChangeLedgingStateToFalse());
            }
        }

        private void PullFromCliff(object sender, EventActionArgs eventArgs)
        {
            PullFromCliff();
        }

        private IEnumerator ChangeLedgingStateToFalse()
        {
            yield return new WaitForEndOfFrame();
            m_playerAnimation.DoLedgeGrab(m_characterFacing.currentFacingDirection);
            m_playerAnimation.EnableRootMotion(true, true);
            m_state.waitForBehaviour = true;
            m_state.isLedging = true;
            m_playerAnimation.AnimationSet += OnAnimationSet;
            m_playerAnimation.animationState.Complete += OnComplete;
            while (m_state.isLedging)
            {
                yield return null;
            }

            m_playerAnimation.AnimationSet -= OnAnimationSet;
            m_playerAnimation.animationState.Complete -= OnComplete;

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
            m_playerAnimation.DisableRootMotion();
        }
    }
}