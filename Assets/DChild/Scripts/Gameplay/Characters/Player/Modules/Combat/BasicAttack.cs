using System;
using DChild.Gameplay.Characters.Players.Behaviour;
using DChild.Gameplay.Characters.Players.State;
using DChild.Gameplay.Systems.WorldComponents;
using Holysoft.Collections;
using Holysoft.Event;
using Sirenix.OdinInspector;
using Spine;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class BasicAttack : MonoBehaviour, IEventModule, IPlayerExternalModule
    {
        [SerializeField]
        [MinValue(1)]
        private int m_maxCombos;
        [SerializeField]
        private CountdownTimer m_comboResetTimer;
        [SerializeField]
        private CountdownTimer m_coolOffDuration;
        private int m_currentComboIndex;
        private bool m_isCoolingOff;

        private IPlayerAnimationState m_animationState;
        private IBasicAttackAnimation m_animation;
        private IBehaviourState m_behaviourState;
        private IPlayerState m_characterState;
        private ICombatState m_combatState;
        private IIsolatedTime m_time;

        public void Initialize(IPlayerModules player)
        {
            m_animationState = player.animationState;
            m_characterState = player.characterState;
            m_combatState = player.characterState;
            m_animation = player.animation;
            m_time = player.isolatedObject;
            m_behaviourState = player.characterState;
        }

        public void ConnectEvents()
        {
            GetComponentInParent<IBasicAttackController>().BasicAttackCall += OnBasicAttackCall;
            GetComponentInParent<IBasicAttackController>().UpwardAttackCall += OnUpwardAttackCall;
            GetComponentInParent<IBasicAttackController>().JumpAttackUpwardCall += OnJumpAttackUpwardCall;
            GetComponentInParent<IBasicAttackController>().JumpAttackDownwardCall += OnJumpAttackDownwardCall;
            GetComponentInParent<IBasicAttackController>().JumpAttackForwardCall += OnJumpAttackForwardCall;
            GetComponentInParent<IMainController>().ControllerDisabled += OnControllerDisabled;
        }

        private void OnControllerDisabled(object sender, EventActionArgs eventArgs)
        {
            m_currentComboIndex = 0;
            m_isCoolingOff = false;
            m_combatState.canAttack = true;
            m_comboResetTimer.Reset();
        }

        public void HandleBasicAttack(HorizontalDirection direction)
        {
            //if (m_isCoolingOff == false)
            //{
            if (m_currentComboIndex > 2) m_comboResetTimer.Reset();

            if (m_characterState.isCrouched)
            {
                m_animation.DoCrouchAttack(m_currentComboIndex, direction);
            }
            else
            {
                m_animation.DoBasicAttack(m_currentComboIndex, direction);
            }

            if (m_currentComboIndex < m_maxCombos)
            {
                m_currentComboIndex++;
            }
            else
            {
                m_comboResetTimer.EndTime(false);
                m_coolOffDuration.Reset();
                //m_isCoolingOff = true;
            }
            //}
        }

        public void HandleJumpUpwardAttack(HorizontalDirection direction)
        {
            if (m_isCoolingOff == false)
            {
                m_animation.DoJumpAttackUpward(m_currentComboIndex, direction);
                if (m_currentComboIndex < 1)
                {
                    m_currentComboIndex++;
                }
                else
                {
                    m_comboResetTimer.EndTime(false);
                    m_coolOffDuration.Reset();
                    //m_isCoolingOff = true;
                }
            }
        }

        private void HandleJumpDownwardAttack(HorizontalDirection direction)
        {
            if (m_isCoolingOff == false)
            {
                m_animation.DoJumpAttackDownward(m_currentComboIndex, direction);
                if (m_currentComboIndex < 1)
                {
                    m_currentComboIndex++;
                }
                else
                {
                    m_comboResetTimer.EndTime(false);
                    m_coolOffDuration.Reset();
                    //m_isCoolingOff = true;
                }
            }

        }

        private void HandleJumpForwardAttack(HorizontalDirection direction)
        {
            if (m_isCoolingOff == false)
            {
                m_animation.DoJumpAttackForward(m_currentComboIndex, direction);
                if (m_currentComboIndex < 1)
                {
                    m_currentComboIndex++;
                }
                else
                {
                    m_comboResetTimer.EndTime(false);
                    m_coolOffDuration.Reset();
                    //m_isCoolingOff = true;
                }
            }

        }

        public void HandleUpwardAttack(HorizontalDirection direction)
        {
            //if (m_isCoolingOff == false)
            //{
            //m_behaviourState.waitForBehaviour = true;
            //m_combatState.canAttack = false;
            //m_combatState.isAttacking = true;

            m_animation.DoUpwardAttack(m_currentComboIndex, direction);
            //    if (m_currentComboIndex < 1)
            //    {
            //        m_currentComboIndex++;
            //    }
            //    else
            //    {
            //        m_comboResetTimer.EndTime(false);
            //        m_coolOffDuration.Reset();
            //        m_isCoolingOff = true;
            //    }
            //}
        }

        private void OnBasicAttackCall(object sender, CombatEventArgs eventArgs)
        {
            m_animationState.hasAttacked = true;
            HandleBasicAttack(eventArgs.facing.currentFacingDirection);
        }

        private void OnJumpAttackUpwardCall(object sender, CombatEventArgs eventArgs)
        {
            HandleJumpUpwardAttack(eventArgs.facing.currentFacingDirection);
        }

        private void OnJumpAttackDownwardCall(object sender, CombatEventArgs eventArgs)
        {
            HandleJumpDownwardAttack(eventArgs.facing.currentFacingDirection);
        }

        private void OnJumpAttackForwardCall(object sender, CombatEventArgs eventArgs)
        {
            HandleJumpForwardAttack(eventArgs.facing.currentFacingDirection);
        }

        private void OnUpwardAttackCall(object sender, CombatEventArgs eventArgs)
        {
            m_animationState.hasAttacked = true;
            HandleUpwardAttack(eventArgs.facing.currentFacingDirection);
        }

        private void OnComplete(TrackEntry trackEntry)
        {
            if (trackEntry.Animation.Name == m_animation.currentAttackAnimation)
            {
                m_combatState.isAttacking = false;
                enabled = true;
                m_comboResetTimer.Reset();
            }
        }

        private void OnEvent(TrackEntry trackEntry, Spine.Event e)
        {
            if (e.Data.Name == "Combo_Attach")
            {
                m_combatState.canAttack = true;
            }
        }

        private void OnComboReset(object sender, EventActionArgs eventArgs)
        {
            m_currentComboIndex = 1;
            if (m_isCoolingOff == false)
            {
                enabled = false;
            }
        }

        private void OnCoolOffEnd(object sender, EventActionArgs eventArgs)
        {
            m_currentComboIndex = 1;
            m_isCoolingOff = false;
            enabled = false;
        }

        private void Awake()
        {
            m_currentComboIndex = 1;
        }

        private void Start()
        {
            m_animation.skeletonAnimation.state.Event += OnEvent;
            m_animation.skeletonAnimation.state.Complete += OnComplete;
            m_comboResetTimer.CountdownEnd += OnComboReset;
            m_coolOffDuration.CountdownEnd += OnCoolOffEnd;
            enabled = false;
        }

        private void Update()
        {
            m_comboResetTimer.Tick(m_time.deltaTime);
            if (m_isCoolingOff)
            {
                m_coolOffDuration.Tick(m_time.deltaTime);
            }
        }

#if UNITY_EDITOR
        public void Initialize(int maxCombos, float comboResetTime, float coolOffTime)
        {
            m_maxCombos = maxCombos;
            m_comboResetTimer = new CountdownTimer(comboResetTime);
            m_coolOffDuration = new CountdownTimer(coolOffTime);
        }
#endif
    }
}