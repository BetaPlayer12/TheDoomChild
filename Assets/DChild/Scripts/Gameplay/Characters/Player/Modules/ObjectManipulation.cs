using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Environment;
using Holysoft.Event;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class DetectedMovableObject : IEventActionArgs
    {
        private MovableObject m_moveableObject;

        public void Set(MovableObject moveableObject)
        {
            m_moveableObject = moveableObject;
        }

        public bool isEmpty => m_moveableObject == null;
        public MovableObject movableObject => m_moveableObject;
    }

    public class ObjectManipulation : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private RaySensor m_grabRangeSensor;
        [SerializeField]
        private float m_pushForce;
        [SerializeField]
        private float m_pullForce;
        [SerializeField]
        private float m_distanceCheck;

        private Animator m_animator;
        private IGrabState m_state;
        private IPlayerModifer m_modifier;
        private MovableObject m_movableObject;
        private Rigidbody2D m_rigidbody;
        private int m_isGrabbingAnimationParameter;
        private int m_isPullingAnimationParameter;
        private int m_isPushingAnimationParameter;

        public event EventAction<DetectedMovableObject> MovableObjectDetected;

        public void GrabIdle()
        {
            m_animator.SetBool(m_isPullingAnimationParameter, false);
            m_animator.SetBool(m_isPushingAnimationParameter, false);

            m_movableObject?.StopMovement();
        }

        public bool IsThereAMovableObject()
        {
            m_grabRangeSensor.Cast();
            bool isValid = false;

            if (m_grabRangeSensor.allRaysDetecting)
            {
                var hits = m_grabRangeSensor.GetUniqueHits();

                for (int i = 0; i < hits.Length; i++)
                {
                    var collider = hits[i].collider;
                    if (collider.isTrigger)
                    {
                        return false;
                    }
                    else
                    {
                        var newMovable = collider.gameObject.GetComponentInParent<MovableObject>();
                        if (newMovable != null)
                        {
                            if (m_movableObject != newMovable)
                            {
                                m_movableObject = newMovable;
                                InvokeDetectedMoveableObject();
                            }
                            isValid = true;
                        }
                        else
                        {
                            if (m_movableObject != null)
                            {
                                m_movableObject = null;
                                InvokeDetectedMoveableObject();
                            }

                            return false;
                        }

                        //if (m_movableObject.CompareTag("InvisibleWall") == false)
                        //{
                        //    if (m_movableObject.gameObject.GetComponentInParent<MovableObject>() != null)
                        //    {

                        //        isValid = true;
                        //    }
                        //    else
                        //    {
                        //        return false;
                        //    }
                        //}
                        //else
                        //{
                        //    return false;
                        //}
                    }
                }
            }

            return isValid;
        }

        public void Execute()
        {
            m_state.isGrabbing = true;
            m_animator.SetBool(m_isGrabbingAnimationParameter, true);
            m_movableObject?.SetGrabState(true);
        }

        public void Cancel()
        {
            m_state.isGrabbing = false;
            m_state.isPushing = false;
            m_state.isPulling = false;
            m_animator.SetBool(m_isGrabbingAnimationParameter, false);
            m_animator.SetBool(m_isPullingAnimationParameter, false);
            m_animator.SetBool(m_isPushingAnimationParameter, false);

            if (m_movableObject != null)
            {
                m_movableObject.source.SetParent(null);
                m_movableObject.SetGrabState(false);
            }
        }

        public void MoveObject(float direction, HorizontalDirection facing)
        {
            bool isPulling;
            Vector3 distance = transform.position - m_movableObject.transform.position;
            float dist = distance.magnitude;

            if (facing == HorizontalDirection.Left)
            {
                if (direction > 0)
                {
                    m_animator.SetBool(m_isPullingAnimationParameter, true);
                    m_animator.SetBool(m_isPushingAnimationParameter, false);
                    isPulling = true;
                }
                else
                {
                    m_animator.SetBool(m_isPushingAnimationParameter, true);
                    m_animator.SetBool(m_isPullingAnimationParameter, false);
                    isPulling = false;
                }
            }
            else
            {
                if (direction > 0)
                {
                    m_animator.SetBool(m_isPushingAnimationParameter, true);
                    m_animator.SetBool(m_isPullingAnimationParameter, false);
                    isPulling = false;
                }
                else
                {
                    m_animator.SetBool(m_isPullingAnimationParameter, true);
                    m_animator.SetBool(m_isPushingAnimationParameter, false);
                    isPulling = true;
                }
            }

            if (isPulling == true)
            {
                m_state.isPushing = false;
                m_state.isPulling = true;

                if (dist > m_distanceCheck)
                {
                    m_rigidbody.velocity = Vector2.zero;
                    m_movableObject.MoveObject(direction, m_modifier.Get(PlayerModifier.MoveSpeed) * m_pullForce + (m_pullForce / 2));
                }
                else
                {
                    m_movableObject.MoveObject(direction, m_modifier.Get(PlayerModifier.MoveSpeed) * m_pullForce);
                }
            }
            else
            {
                m_state.isPushing = true;
                m_state.isPulling = false;

                if (dist > m_distanceCheck)
                {
                    m_rigidbody.velocity = Vector2.zero;
                    m_movableObject.MoveObject(direction, m_modifier.Get(PlayerModifier.MoveSpeed) * m_pushForce / 2);
                }
                else
                {
                    m_movableObject.MoveObject(direction, m_modifier.Get(PlayerModifier.MoveSpeed) * m_pushForce);
                }
            }
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_modifier = info.modifier;
            m_animator = info.animator;
            m_rigidbody = info.rigidbody;
            m_isGrabbingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrabbing);
            m_isPullingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsPulling);
            m_isPushingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsPushing);
        }

        private void InvokeDetectedMoveableObject()
        {
            using (Cache<DetectedMovableObject> cacheEvent = Cache<DetectedMovableObject>.Claim())
            {
                cacheEvent.Value.Set(m_movableObject);
                MovableObjectDetected?.Invoke(this, cacheEvent.Value);
                cacheEvent.Release();
            }
        }
    }
}
