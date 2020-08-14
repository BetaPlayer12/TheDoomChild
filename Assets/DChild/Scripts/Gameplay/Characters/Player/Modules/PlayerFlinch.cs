﻿using System;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;
using PlayerNew;
using System.Collections;
using Holysoft.Event;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerFlinch : MonoBehaviour, IFlinch, IComplexCharacterModule
    {
        [SerializeField, MinValue(0)]
        private float m_XknockBackPower;
        [SerializeField, MinValue(0)]
        private float m_YknockbackPower;
        [SerializeField]
        private float m_aerialKnockBackMultiplier;
        [SerializeField]
        private float m_flinchDuration;
        [SerializeField]
        private float m_flinchGravityScale;

        private CharacterState m_state;
        private Animator m_animator;

        public event EventAction<EventActionArgs> OnExecute;
        private Rigidbody2D m_rigidBody;
        private int m_animationParameter;
        private float playerGravityScale;
        private float m_defaultLinearDrag;

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;

            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Flinch);
            m_rigidBody = info.character.GetComponent<Rigidbody2D>();

            playerGravityScale = m_rigidBody.gravityScale;
            m_defaultLinearDrag = m_rigidBody.drag;
        }

        public void Flinch(Vector2 directionToSource, RelativeDirection damageSource, IReadOnlyCollection<AttackType> damageTypeRecieved)
        {
            bool isAerialKnockback = false;
            OnExecute?.Invoke(this, EventActionArgs.Empty);
            m_rigidBody.velocity = Vector2.zero;
            Vector2 knockBackDirection = Vector2.zero;
            knockBackDirection.x = directionToSource.x > 0 ? -1 : 1;

            Debug.Log("Direction: " + directionToSource);

            if (m_state.isGrounded)
            {
                Debug.Log("isgrounded");
                knockBackDirection.y = 1;
            }
            else
            {
                Debug.Log("Not isgrounded");
                knockBackDirection.y = 1;
                isAerialKnockback = true;
            }

            StartCoroutine(FlinchRoutine(knockBackDirection, isAerialKnockback));
        }

        private IEnumerator FlinchRoutine(Vector2 direction, bool isAerialKnockBack)
        {
            float knockBackPower = m_XknockBackPower;
            float aerialKnockBackPower = m_YknockbackPower;
            m_state.waitForBehaviour= true;
            m_rigidBody.gravityScale = m_flinchGravityScale;
            m_rigidBody.drag = m_defaultLinearDrag;

            if (isAerialKnockBack == true)
            {
                aerialKnockBackPower = m_YknockbackPower * m_aerialKnockBackMultiplier;
            }

            m_rigidBody.velocity = Vector2.zero;
            m_rigidBody.AddForce(new Vector2(direction.x * knockBackPower, direction.y * aerialKnockBackPower), ForceMode2D.Impulse);
            m_animator.SetBool(m_animationParameter,true);

            yield return new WaitForSeconds(m_flinchDuration);
            m_animator.SetBool(m_animationParameter, false);
            m_state.waitForBehaviour = false;
            m_rigidBody.gravityScale = playerGravityScale;
        }
    }
}