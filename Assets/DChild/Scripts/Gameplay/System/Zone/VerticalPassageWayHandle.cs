using DChild.Gameplay.Characters;
using PlayerNew;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    [System.Serializable]
    public struct VerticalPassageWayHandle : ISwitchHandle
    {
        private enum TravelDirection
        {
            Up,
            Down
        }

        [SerializeField, OnValueChanged("OnDirectionChange")]
        private TravelDirection m_entranceDirection;
        [SerializeField, ShowIf("m_customTravelDirections")]
        private TravelDirection m_exitDirection;
        [SerializeField]
        private Vector2 m_upVelocity;
        [SerializeField]
        private bool m_forceExitFacing;
        [SerializeField, ShowIf("m_forceExitFacing")]
        private HorizontalDirection m_exitFacing;

        public float transitionDelay => 0.2f;

        public bool needsButtonInteraction => false;

        public Vector3 promptPosition => Vector3.zero;

        public void DoSceneTransition(Character character, TransitionType type)
        {
            var characterPhysics = character.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidBody = character.GetComponent<Rigidbody2D>();
            CollisionState collisionState = character.GetComponentInChildren<CollisionState>();
            switch (type)
            {
                case TransitionType.Enter:
                    if (m_entranceDirection == TravelDirection.Up)
                    {
                        character.StartCoroutine(UpEntranceRoutine(characterPhysics));
                    }
                    else
                    {
                        rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    }
                    break;
                case TransitionType.Exit:
                    character.StopCoroutine("UpEntranceRoutine");
                    if (m_exitDirection == TravelDirection.Up)
                    {
                        characterPhysics.velocity = Vector2.zero;
                        var exitVelocity = m_upVelocity;
                        exitVelocity.x *= m_forceExitFacing ? (int)m_exitFacing : (int)character.facing;
                        characterPhysics.AddForce(exitVelocity, ForceMode2D.Impulse);
                    }
                    else
                    {
                        characterPhysics.velocity = (Vector2.zero);
                    }
                    rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
                    collisionState.forceGrounded = false;
                    break;
                case TransitionType.PostEnter:
                    rigidBody.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
                    collisionState.forceGrounded = false;
                    break;

            }

        }

        private IEnumerator UpEntranceRoutine(Rigidbody2D physics)
        {
            var waitFor = new WaitForFixedUpdate();
            var time = transitionDelay;
            while (time > 0)
            {
                physics.velocity = Vector2.up * m_upVelocity;
                yield return waitFor;
                time -= Time.deltaTime;
            }
        }

#if UNITY_EDITOR
        [SerializeField, PropertyOrder(-1)]
        private bool m_customTravelDirections;

        private void OnDirectionChange()
        {
            if (m_customTravelDirections == false)
            {
                if (m_entranceDirection == TravelDirection.Up)
                {
                    m_exitDirection = TravelDirection.Down;
                }
                else
                {
                    m_exitDirection = TravelDirection.Up;
                }
            }
        }
#endif
    }
}
