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
        private Collider2D m_movableObject;
        private int m_isGrabbingAnimationParameter;
        private int m_isPullingAnimationParameter;
        private int m_isPushingAnimationParameter;

        //void Update()
        //{
        //    var thereIsAMovableObject = IsThereAMovableObject();

        //    if (Input.GetKey(KeyCode.P))
        //    {
        //        if (thereIsAMovableObject == true)
        //        {
        //            Debug.Log("testr");

        //            var gameObject = m_cacheCollider.gameObject.transform.parent;
        //            gameObject.transform.parent = m_grabArea.transform;
        //            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        //        }
        //    }
        //    else
        //    {
        //        if (m_cacheCollider != null)
        //        {
        //            var gameObject = m_cacheCollider.gameObject.transform.parent;
        //            var parent = gameObject.GetComponent<MovableObject>().GetParentObject();
        //            gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        //            gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        //            gameObject.transform.parent = parent.transform;
        //        }
        //    }
        //}

        public void GrabIdle()
        {
            m_animator.SetBool(m_isPullingAnimationParameter, false);
            m_animator.SetBool(m_isPushingAnimationParameter, false);
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
                        if (m_movableObject.CompareTag("InvisibleWall") == false)
                        {
                            if (m_movableObject.gameObject.GetComponentInParent<MovableObject>() != null)
                            {

                                isValid = true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return isValid;
        }

        public void Execute()
        {
            m_state.isGrabbing = true;
            m_animator.SetBool(m_isGrabbingAnimationParameter, true);

            //var gameObject = m_movableObject.gameObject.transform.parent;
            //gameObject.transform.parent = m_grabArea.transform;
            //gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        }

        public void Cancel()
        {
            m_state.isGrabbing = false;
            m_animator.SetBool(m_isGrabbingAnimationParameter, false);
            m_animator.SetBool(m_isPullingAnimationParameter, false);
            m_animator.SetBool(m_isPushingAnimationParameter, false);

            if (m_movableObject != null)
            {
                //var gameObject = m_movableObject.gameObject.transform.parent;
                //var parent = gameObject.GetComponent<MovableObject>().GetParentObject();
                //gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                //gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, gameObject.GetComponent<Rigidbody2D>().velocity.y);
                //gameObject.transform.parent = parent.transform;
            }
        }

        public void MoveObject(float direction, HorizontalDirection facing)
        {
            bool isPulling = false;

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
                m_movableObject.GetComponentInParent<MovableObject>().MoveObject(direction, m_pullForce);
            }
            else
            {
                m_movableObject.GetComponentInParent<MovableObject>().MoveObject(direction, m_pushForce);
            }
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_animator = info.animator;
            m_isGrabbingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrabbing);
            m_isPullingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsPulling);
            m_isPushingAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsPushing);
        }
    }
}
