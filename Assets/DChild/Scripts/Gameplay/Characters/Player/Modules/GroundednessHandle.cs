using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class GroundednessHandle : MonoBehaviour, IComplexCharacterModule
    {
        //[SerializeField, MinValue(0.1)]
        //private float m_collisionRadius = 10.0f;
        [SerializeField]
        private Vector2 m_origin;
        [SerializeField]
        private LayerMask m_collisionLayer;
        [SerializeField, MinValue(0f)]
        private float m_coyoteTime;
        [SerializeField]
        private Vector2 boxSize;
        [SerializeField]
        private float angle;
        [SerializeField]
        private float m_groundCheckOffset;

        private bool m_previouslyGrounded;
        private IGroundednessState m_state;
        private Animator m_animator;
        private int m_groundedAnimationParameter;
        private ContactFilter2D m_filter;
        private List<Collider2D> m_colliderList;
        private bool m_isUsingCoyote;
        
        public event EventAction<EventActionArgs> StateChange;
        public bool isUsingCoyote => m_isUsingCoyote;
        public float groundCheckOffset => m_groundCheckOffset;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;
            m_animator = info.animator;
            m_groundedAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrounded);
            m_filter = new ContactFilter2D();
            m_filter.useTriggers = false;
            m_filter.useLayerMask = true;
            m_filter.layerMask = m_collisionLayer;
            m_colliderList = new List<Collider2D>();
        }

        public void ChangeValue(bool groundedness)
        {
            StopAllCoroutines();
            m_isUsingCoyote = false;
            m_animator.SetBool(m_groundedAnimationParameter, groundedness);
            m_state.isGrounded = groundedness;
            if (m_previouslyGrounded != groundedness)
            {
                StateChange?.Invoke(this, EventActionArgs.Empty);
            }
            m_previouslyGrounded = groundedness;
        }

        public void Evaluate()
        {
            //int groundColliderResult = Physics2D.OverlapCircle(m_origin + (Vector2)transform.position, m_collisionRadius, m_filter, m_colliderList);
            //var isGrounded = groundColliderResult > 0 ? true : false;

            int groundColliderResult = Physics2D.OverlapBox(m_origin + (Vector2)transform.position, boxSize, angle, m_filter, m_colliderList);
            var isGrounded = groundColliderResult > 0 ? true : false;

            if (isGrounded)
            {
                ChangeValue(true);
            }
            else if (m_previouslyGrounded)
            {
                if (m_coyoteTime > 0)
                {
                    StartCoroutine(CoyoteRoutine());
                }
                else
                {
                    ChangeValue(false);
                }
            }
            else if (m_isUsingCoyote)
            {
                //AllowCoyoteToDoThe Thing
                //What thing?
                //You know. The THING
            }
            
            else
            {
                ChangeValue(false);
            }

        }

        private IEnumerator CoyoteRoutine()
        {
            Debug.Log("CoyoteRoutine");
            m_isUsingCoyote = true;
            yield return new WaitForSeconds(m_coyoteTime);
            m_isUsingCoyote = false;
            ChangeValue(false);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            //Gizmos.DrawWireSphere(m_origin + (Vector2)transform.position, m_collisionRadius);

            Gizmos.DrawWireCube(m_origin + (Vector2)transform.position, boxSize);
        }
    }
}
