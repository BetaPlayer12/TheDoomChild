using System;
using System.Collections.Generic;
using DChild.Gameplay.Combat;
using DChild.Gameplay.Characters.Players;
using Sirenix.OdinInspector;
using UnityEngine;
using PlayerNew;
using System.Collections;
using Holysoft.Event;
using Random = UnityEngine.Random;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerFlinch : MonoBehaviour, IFlinch, IComplexCharacterModule
    {
        [SerializeField, HideLabel]
        private FlinchStatsInfo m_configuration;

        private CharacterState m_state;
        private Animator m_animator;

        public event EventAction<EventActionArgs> OnExecute;
        private Rigidbody2D m_rigidBody;
        private int m_animationParameter;
        private float playerGravityScale;
        private float m_defaultLinearDrag;
        private float m_flinchState;
        private int m_flinchStateAnimationParameter;

        private Coroutine m_flinchRoutine;

        private bool m_knockbackEnabled;

        public void EnableKnockback(bool enable)
        {
            m_knockbackEnabled = enable;
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            m_state = info.state;

            m_animator = info.animator;
            m_animationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Flinch);
            m_flinchStateAnimationParameter = info.animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.FlinchState);
            m_rigidBody = info.character.GetComponent<Rigidbody2D>();

            playerGravityScale = m_rigidBody.gravityScale;
            m_defaultLinearDrag = m_rigidBody.drag;

            m_knockbackEnabled = true;
        }

        public void SetConfiguration(FlinchStatsInfo info)
        {
            m_configuration.CopyInfo(info);
        }

        public void Flinch(Vector2 directionToSource, RelativeDirection damageSource, AttackSummaryInfo damageTypeRecieved)
        {
            if (m_state.canFlinch == true)
            {
                bool isAerialKnockback = false;
                OnExecute?.Invoke(this, EventActionArgs.Empty);
                m_rigidBody.velocity = Vector2.zero;
                Vector2 knockBackDirection = Vector2.zero;
                knockBackDirection.x = directionToSource.x > 0 ? -1 : 1;

                if (m_state.isGrounded)
                {
                    knockBackDirection.y = 1;
                }
                else
                {
                    knockBackDirection.y = 1;
                    isAerialKnockback = true;
                }

                m_flinchRoutine = StartCoroutine(FlinchRoutine(knockBackDirection, isAerialKnockback));
            }
            else
            {
                return;
            }
        }

        private IEnumerator FlinchRoutine(Vector2 direction, bool isAerialKnockBack)
        {
            float knockBackPower = m_configuration.xKnockBackPower;
            float aerialKnockBackPower = m_configuration.yKnockBackPower;
            int flinchState = Random.Range(1, m_configuration.numberOfFlinchStates + 1);

            m_state.waitForBehaviour = true;
            if (m_knockbackEnabled)
            {
                m_rigidBody.gravityScale = m_configuration.flinchGravityScale;
                m_rigidBody.drag = m_defaultLinearDrag;

                if (isAerialKnockBack == true)
                {
                    aerialKnockBackPower = m_configuration.yKnockBackPower * m_configuration.aerialKnockBackMultiplier;
                }

                m_rigidBody.velocity = Vector2.zero;
                m_rigidBody.AddForce(new Vector2(direction.x * knockBackPower, direction.y * aerialKnockBackPower), ForceMode2D.Impulse);
            }
            m_animator.SetBool(m_animationParameter, true);
            m_animator.SetInteger(m_flinchStateAnimationParameter, flinchState);

            yield return new WaitForSeconds(m_configuration.flinchDuration);
            m_animator.SetBool(m_animationParameter, false);
            m_state.waitForBehaviour = false;
            if (m_knockbackEnabled)
            {
                m_rigidBody.gravityScale = playerGravityScale;
            }
        }

        public void CancelFlinch()
        {
            if (m_flinchRoutine != null)
            {
                StopCoroutine(m_flinchRoutine);
                m_flinchRoutine = null;
                m_animator.SetBool(m_animationParameter, false);
                m_state.waitForBehaviour = false;
                m_rigidBody.gravityScale = playerGravityScale;
            }
        }
    }
}