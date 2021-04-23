using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Environment;
using Holysoft.Gameplay;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ObjectManipulation : MonoBehaviour, ICancellableBehaviour, IComplexCharacterModule
    {
        [SerializeField]
        private RaySensor m_grabRangeSensor;
        [SerializeField]
        private float m_pushForce;
        [SerializeField]
        private float m_pullForce;

        private Animator m_animator;
        private IGrabState m_state;
        private IPlayerModifer m_modifier;
        private Collider2D m_movableObject;
        private int m_isGrabbingAnimationParameter;
        private int m_isPullingAnimationParameter;
        private int m_isPushingAnimationParameter;

        public void GrabIdle()
        {
            m_animator.SetBool(m_isPullingAnimationParameter, false);
            m_animator.SetBool(m_isPushingAnimationParameter, false);

            if (m_movableObject != null)
            {
                m_movableObject.gameObject?.GetComponentInParent<MovableObject>().StopMovement();
            }
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
                    m_movableObject = hits[i].collider;
                    if (m_movableObject.isTrigger)
                    {
                        return false;
                    }
                    else
                    {
                        if (m_movableObject.gameObject.GetComponentInParent<MovableObject>() != null)
                        {
                            isValid = true;
                        }
                        else
                        {
                            m_movableObject = null;

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

            if (m_movableObject != null)
            {
                m_movableObject.gameObject?.GetComponentInParent<MovableObject>().SetGrabState(true);
            }
        }

        public void Cancel()
        {
            m_state.isGrabbing = false;
            m_animator.SetBool(m_isGrabbingAnimationParameter, false);
            m_animator.SetBool(m_isPullingAnimationParameter, false);
            m_animator.SetBool(m_isPushingAnimationParameter, false);

            if (m_movableObject != null)
            {
                m_movableObject.gameObject?.GetComponentInParent<MovableObject>().SetGrabState(false);
            }
        }

        public void MoveObject(float direction, HorizontalDirection facing)
        {
            bool isPulling;

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
                m_movableObject.GetComponentInParent<MovableObject>().MoveObject(direction, m_modifier.Get(PlayerModifier.MoveSpeed) * m_pullForce);
            }
            else
            {
                m_movableObject.GetComponentInParent<MovableObject>().MoveObject(direction, m_modifier.Get(PlayerModifier.MoveSpeed) * m_pushForce);
            }
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_modifier = info.modifier;
            m_animator = info.animator;
            m_isGrabbingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrabbing);
            m_isPullingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsPulling);
            m_isPushingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsPushing);
        }
    }
}
