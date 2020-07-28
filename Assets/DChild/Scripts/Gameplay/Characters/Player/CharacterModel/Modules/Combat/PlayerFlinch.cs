using System;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;
using PlayerNew;
using System.Collections;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerFlinch : PlayerBehaviour, IFlinch, IComplexCharacterModule
    {
        [SerializeField, MinValue(0)]
        private float m_XknockBackPower;
        [SerializeField, MinValue(0)]
        private float m_YknockbackPower;
        [SerializeField]
        private float m_aerialKnockBackMultiplier;
        [SerializeField]
        private StateManager m_stateManager;
        [SerializeField]
        private Animator m_animator;

        private Rigidbody2D m_physics;
        private string m_flinch;
        private float playerGravityScale;
        private float m_defaultLinearDrag;

        [SerializeField]
        private PlayerMovement m_movement;
        [SerializeField]
        private float m_flinchDuration;
        [SerializeField]
        private float m_flinchGravityScale;

        private void Start()
        {
            m_physics = GetComponentInParent<Rigidbody2D>();
            playerGravityScale = rigidBody.gravityScale;
            m_defaultLinearDrag = rigidBody.drag;
        }

        public void Flinch(Vector2 directionToSource, RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved)
        {
            bool isAerialKnockback = false;
            m_physics.velocity = Vector2.zero;
            Vector2 knockBackDirection = Vector2.zero;
            knockBackDirection.x = directionToSource.x > 0 ? -1 : 1;

            Debug.Log("Direction: " + directionToSource);

            if (m_stateManager.isGrounded)
            {
                Debug.Log("isgrounded");
                knockBackDirection.y = 1;
            }
            else if (!m_stateManager.isGrounded)
            {
                Debug.Log("Not isgrounded");
                knockBackDirection.y = 1;
                //knockBackDirection.y = -directionToSource.y;
                isAerialKnockback = true;

                //Debug.Log("Not Grounded " + directionToSource.y);
                //if (directionToSource.y < 0)
                //{
                //    Debug.Log("Test");
                //    knockBackDirection.x = 0;
                //    knockBackDirection.y = 1;
                //    useAerialKnockback = true;
                //}
                //else
                //{
                //    knockBackDirection.y = -directionToSource.y;
                //}
            }

            StartCoroutine(FlinchRoutine(knockBackDirection, isAerialKnockback));
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            Debug.Log("Initialize Player Flinch");
            m_animator = info.animator;
            m_flinch = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Flinch);
            m_physics = info.character.GetComponent<Rigidbody2D>();
            //m_collionState = m_animator.GetComponent<CollisionState>();
            // info.state.canFlinch = true;
        }

        private IEnumerator FlinchRoutine(Vector2 direction, bool isAerialKnockBack)
        {
            Debug.Log(direction);
            float knockBackPower = m_XknockBackPower;
            float aerialKnockBackPower = m_YknockbackPower;

            m_movement.DisableMovement();
            m_stateManager.isFlinching = true;
            rigidBody.gravityScale = m_flinchGravityScale;
            rigidBody.drag = m_defaultLinearDrag;

            if (isAerialKnockBack == true)
            {
                aerialKnockBackPower = m_YknockbackPower * m_aerialKnockBackMultiplier;
            }

            if (m_stateManager.isDashing == true)
            {
                rigidBody.constraints = RigidbodyConstraints2D.FreezeRotation;
            }

            m_physics.velocity = Vector2.zero;
            m_physics.AddForce(new Vector2(direction.x * knockBackPower, direction.y * aerialKnockBackPower), ForceMode2D.Impulse);
            m_animator.SetTrigger("Flinch");

            yield return new WaitForSeconds(m_flinchDuration);

            m_movement.EnableMovement();
            m_stateManager.isFlinching = false;
            rigidBody.gravityScale = playerGravityScale;
        }
    }
}