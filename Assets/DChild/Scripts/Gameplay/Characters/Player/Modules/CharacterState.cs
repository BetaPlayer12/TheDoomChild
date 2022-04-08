using DChild.Gameplay.Characters.Players.State;
using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class CharacterState : MonoBehaviour, ICrouchState, IGroundednessState, IDashState, IHighJumpState,
                                  IWallStickState, IWallJumpState, IAttackState, ICombatReadinessState, IDeathState,
                                  ILevitateState, IGrabState, ISlideState, ILedgeGrabState, IProjectileThrowState, IShadowModeState,
                                  IEnragedState, IBlockingState, ISwordThrustState, IExposedPlayerState
    {
        [SerializeField, ReadOnly]
        private bool m_isCombatReady;
        public bool isCombatReady { get => m_isCombatReady; set => m_isCombatReady = value; }

        [SerializeField, ReadOnly]
        private bool m_isGrounded;
        public bool isGrounded { get => m_isGrounded; set => m_isGrounded = value; }

        [SerializeField, ReadOnly]
        private bool m_isCrouched;
        public bool isCrouched { get => m_isCrouched; set => m_isCrouched = value; }

        [SerializeField, ReadOnly]
        private bool m_canDash;
        public bool canDash { get => m_canDash; set => m_canDash = value; }

        [SerializeField, ReadOnly]
        private bool m_isDashing;
        public bool isDashing { get => m_isDashing; set => m_isDashing = value; }

        [SerializeField, ReadOnly]
        private bool m_canSlide;
        public bool canSlide { get => m_canSlide; set => m_canSlide = value; }

        [SerializeField, ReadOnly]
        private bool m_isSliding;
        public bool isSliding { get => m_isSliding; set => m_isSliding = value; }

        [SerializeField, ReadOnly]
        private bool m_isHighJumping;
        public bool isHighJumping { get => m_isHighJumping; set => m_isHighJumping = value; }

        [SerializeField, ReadOnly]
        private bool m_isStickingToWall;
        public bool isStickingToWall { get => m_isStickingToWall; set => m_isStickingToWall = value; }

        [SerializeField, ReadOnly]
        private bool m_isLevitating;
        public bool isLevitating { get => m_isLevitating; set => m_isLevitating = value; }

        [SerializeField, ReadOnly]
        private bool m_canAttack;
        public bool canAttack { get => m_canAttack; set => m_canAttack = value; }

        [SerializeField, ReadOnly]
        private bool m_isAttacking;
        public bool isAttacking { get => m_isAttacking; set => m_isAttacking = value; }

        [SerializeField, ReadOnly]
        private bool m_isChargingAttack;
        public bool isChargingAttack { get => m_isChargingAttack; set => m_isChargingAttack = value; }

        [SerializeField, ReadOnly]
        private bool m_isGrabbing;
        public bool isGrabbing { get => m_isGrabbing; set => m_isGrabbing = value; }
        [SerializeField, ReadOnly]
        private bool m_isPulling;
        public bool isPulling { get => m_isPulling; set => m_isPulling = value; }
        [SerializeField, ReadOnly]
        private bool m_isPushing;
        public bool isPushing { get => m_isPushing; set => m_isPushing = value; }

        [SerializeField, ReadOnly]
        private bool m_isAimingProjectile;
        public bool isAimingProjectile { get => m_isAimingProjectile; set => m_isAimingProjectile = value; }

        [SerializeField, ReadOnly]
        private bool m_isInShadowMode;
        public bool isInShadowMode { get => m_isInShadowMode; set => m_isInShadowMode = value; }

        [SerializeField, ReadOnly]
        private bool m_waitForBehaviour;
        public bool waitForBehaviour { get => m_waitForBehaviour; set => m_waitForBehaviour = value; }

        [SerializeField, ReadOnly]
        private bool m_canFlinch = true;
        public bool canFlinch { get => m_canFlinch; set => m_canFlinch = value; }

        [SerializeField, ReadOnly]
        private bool m_isDead;
        public bool isDead { get => m_isDead; set => m_isDead = value; }

        public event EventAction<EnrageEventArgs> EnrageChange;
        [SerializeField, ReadOnly]
        private bool m_isEnraged;
        public bool isEnraged
        {
            get
            {
                return m_isEnraged;
            }

            set
            {
                if (m_isEnraged != value)
                {
                    EnrageChange?.Invoke(this, new EnrageEventArgs(value));
                }

                m_isEnraged = value;
            }
        }

        [SerializeField, ReadOnly]
        private bool m_forcedCurrentGroundedness;

        public bool forcedCurrentGroundedness { get => m_forcedCurrentGroundedness; set => m_forcedCurrentGroundedness = value; }

        [SerializeField, ReadOnly]
        private bool m_allowExtendedIdle = true;
        public bool allowExtendedIdle { get => m_allowExtendedIdle; set => m_allowExtendedIdle = value; }
        [SerializeField, ReadOnly]
        private bool m_isBlocking;
        public bool isBlocking { get => m_isBlocking; set => m_isBlocking = value; }
        [SerializeField, ReadOnly]
        private bool m_isDoingCombo;
        public bool isDoingCombo { get => m_isDoingCombo; set => m_isDoingCombo = value; }
        [SerializeField, ReadOnly]
        private bool m_isDoingSwordThrust;
        public bool isDoingSwordThrust { get => m_isDoingSwordThrust; set => m_isDoingSwordThrust = value; }
    }
}
