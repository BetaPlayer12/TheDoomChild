using DChild.Gameplay.Characters.Players;
using Spine.Unity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Environment
{
    public class PlayerImitator : MonoBehaviour
    {
        private class AnimationParameterInfo
        {
            private float m_speedX;
            private float m_speedY;
            private bool m_isIdle;
            private bool isGrounded;
            private bool m_isJumping;
            private bool m_isCrouching;
            private bool m_isLevitating;
            private bool m_isDashing;
            private bool m_isInShadowMode;
            private bool m_isSliding;
            private bool m_wallStick;
            private bool m_isInCombatMode;
            private bool m_isAttacking;
            private int m_slashState;
            private bool m_isGrabbing;
            private bool m_ledgeGrab;
            private float m_YInput;
            private bool m_whipAttack;
            private bool m_isCharging;
            private bool m_EarthShake;
            private bool m_SwordThrust;

            public AnimationParameterInfo(Animator animator, AnimationParametersData animationParametersData)
            {
                m_isIdle = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsIdle));
                isGrounded = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrounded));
                m_isJumping = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump));
                m_isCrouching = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsCrouched));
                m_isLevitating = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsLevitating));
                m_isDashing = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsDashing));
                m_isInShadowMode = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ShadowMode));
                m_isSliding = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsSliding));
                m_wallStick = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WallStick));
                m_isInCombatMode = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.CombatMode));
                m_isAttacking = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsAttacking));
                m_isGrabbing = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrabbing));
                m_ledgeGrab = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.LedgeGrab));
                m_slashState = animator.GetInteger(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SlashState));
                m_YInput = animator.GetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.YInput));
                m_whipAttack = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WhipAttack));
                m_isCharging = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsCharging));
                m_EarthShake = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.EarthShaker));
                m_SwordThrust = animator.GetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SwordTrust));

                animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX);
                m_speedX = animator.GetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX));
                m_speedY = animator.GetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY));
            }

            public void Apply(Animator animator, AnimationParametersData animationParametersData)
            {
                animator.SetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedX), m_speedX);
                animator.SetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SpeedY), m_speedY);

                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsIdle), m_isIdle);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrounded), isGrounded);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.Jump), m_isJumping);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsCrouched), m_isCrouching);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsLevitating), m_isLevitating);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsDashing), m_isDashing);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.ShadowMode), m_isInShadowMode);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsSliding), m_isSliding);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WallStick), m_wallStick);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.CombatMode), m_isInCombatMode);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsAttacking), m_isAttacking);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsGrabbing), m_isGrabbing);
                animator.SetInteger(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SlashState), m_slashState);
                animator.SetFloat(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.YInput), m_YInput);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.WhipAttack), m_whipAttack);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.IsCharging), m_isCharging);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.EarthShaker), m_EarthShake);
                animator.SetBool(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.SwordTrust), m_SwordThrust);

                if (m_ledgeGrab == true)
                {
                    animator.SetTrigger(animationParametersData.GetParameterLabel(AnimationParametersData.Parameter.LedgeGrab));
                }
            }
        }

        public class FXAnimationParameterInfo
        {
            private bool m_groundOverhead;
            private bool m_crouch;
            private bool m_jumpOverhead;
            private bool m_jump;
            private bool m_slashCombo1;
            private bool m_slashCombo2;
            private bool m_slashCombo3;
            AnimatorStateInfo m_currentClipInfo;

            public FXAnimationParameterInfo(Animator animator)
            {
                //Bleeegh hard coded values >.<
                m_groundOverhead = animator.GetBool("GroundOverhead");
                m_crouch = animator.GetBool("Crouch");
                m_jumpOverhead = animator.GetBool("JumpOverhead");
                m_jump = animator.GetBool("Jump");

                m_currentClipInfo = animator.GetCurrentAnimatorStateInfo(0);

                if (m_currentClipInfo.IsName("SlashCombo1"))
                {
                    m_slashCombo1 = true;
                }
                if (m_currentClipInfo.IsName("SlashCombo2"))
                {
                    m_slashCombo2 = true;
                }
                if (m_currentClipInfo.IsName("SlashCombo3"))
                {
                    m_slashCombo3 = true;
                }
            }

            public void Apply(Animator animator)
            {
                //Bleeegh hard coded values >.<
                animator.SetBool("GroundOverhead", m_groundOverhead);
                animator.SetBool("Crouch", m_crouch);
                animator.SetBool("JumpOverhead", m_jumpOverhead);
                animator.SetBool("Jump", m_jump);

                if (m_slashCombo1 == true)
                {
                    animator.Play("SlashCombo1");
                }
                if (m_slashCombo2 == true)
                {
                    animator.Play("SlashCombo2");
                }
                if (m_slashCombo3 == true)
                {
                    animator.Play("SlashCombo3");
                }
            }
        }

        [SerializeField]
        private AnimationParametersData m_animationParametersData;
        [SerializeField]
        private LineRenderer m_lineConnection;
        [SerializeField]
        private float m_lineYOffset;

        [SerializeField]
        private Transform m_toImitate;
        [SerializeField]
        private float m_imitateDelay;

        private Animator m_animator;
        private Animator m_animatorToImitate;
        [SerializeField]
        private Animator m_attackFXAnimator;
        private Animator m_attackFXanimatorToImitate;

        private float m_imitationDelayTimer;
        private bool m_isDelayed;
        private List<Vector3> m_positionToImitate;
        private List<Vector3> m_scaleToImitate;
        private List<AnimationParameterInfo> m_animationToImitate;
        private List<FXAnimationParameterInfo> m_fxAnimationToImitate;

        public void StartImitating(Player toImitate)
        {
            m_toImitate = toImitate.character.transform;
            m_animator = GetComponent<Animator>();
            m_animatorToImitate = toImitate.character.centerMass.GetComponentInParent<Animator>();
            m_attackFXanimatorToImitate = toImitate.character.attackFX.GetComponent<Animator>();
            ResetImitation();
        }

        public void ResetImitation()
        {
            transform.position = m_toImitate.position;
            m_imitationDelayTimer = m_imitateDelay;
            m_isDelayed = true;

            InitializeRecords();
        }

        private void InitializeRecords()
        {
            if (m_positionToImitate == null)
            {
                m_positionToImitate = new List<Vector3>();
                m_scaleToImitate = new List<Vector3>();
                m_animationToImitate = new List<AnimationParameterInfo>();
                m_fxAnimationToImitate = new List<FXAnimationParameterInfo>();
            }
            m_positionToImitate.Clear();
            m_scaleToImitate.Clear();
            m_animationToImitate.Clear();
            m_fxAnimationToImitate.Clear();
        }

        private void UpdateImitation()
        {
            transform.position = m_positionToImitate[0];
            transform.localScale = m_scaleToImitate[0];
            m_animationToImitate[0].Apply(m_animator, m_animationParametersData);
            m_fxAnimationToImitate[0].Apply(m_attackFXAnimator);

            m_positionToImitate.RemoveAt(0);
            m_scaleToImitate.RemoveAt(0);
            m_animationToImitate.RemoveAt(0);
            m_fxAnimationToImitate.RemoveAt(0);
        }

        private void RecordInfoToImitate()
        {
            m_positionToImitate.Add(m_toImitate.position);
            var scale = m_toImitate.localScale;
            scale.x *= -1;
            m_scaleToImitate.Add(scale);

            m_animationToImitate.Add(new AnimationParameterInfo(m_animatorToImitate, m_animationParametersData));
            m_fxAnimationToImitate.Add(new FXAnimationParameterInfo(m_attackFXanimatorToImitate));
        }

        private void DrawLineConnection()
        {
            var yOffset = Vector3.up * m_lineYOffset;
            m_lineConnection.positionCount = m_positionToImitate.Count + 1;
            m_lineConnection.SetPosition(0, transform.position + yOffset);
            for (int i = 0; i < m_positionToImitate.Count; i++)
            {
                m_lineConnection.SetPosition(i + 1, m_positionToImitate[i] + yOffset);
            }
        }

        private void Start()
        {
            InitializeRecords();
            StartImitating(GameplaySystem.playerManager.player);
            var lineConnectionTransform = m_lineConnection.transform;
            lineConnectionTransform.SetParent(null);
            lineConnectionTransform.position = Vector3.zero;
        }

        private void Update()
        {
            RecordInfoToImitate();

            if (m_isDelayed)
            {
                m_imitationDelayTimer -= GameplaySystem.time.deltaTime;
                if (m_imitationDelayTimer <= 0)
                {
                    m_isDelayed = false;
                }
            }
            else
            {
                if (GameplaySystem.isGamePaused)
                {

                }
                else
                {
                    UpdateImitation();
                }
            }

            DrawLineConnection();
        }

        private void OnDestroy()
        {
            Destroy(m_lineConnection.gameObject);
        }
    }
}