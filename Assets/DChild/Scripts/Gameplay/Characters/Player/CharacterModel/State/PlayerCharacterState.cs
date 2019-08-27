using Holysoft.Event;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;

namespace DChild.Gameplay.Characters.Players.State
{
    [System.Serializable]
    public class PlayerCharacterState : IPlayerState, IMoveState, IGroundednessState,
                                ICrouchState, IFlinchState, IWallStickState,
                                IDashState, IDoubleJumpState, IWallJumpState,
                                IHighJumpState, IBehaviourState, ICombatState, IPlatformDropState,
                                IWhipGrapple, IProjectileThrowState
    {
        public event EventAction<CombatStateEventArgs> CombatModeChanged;

        private bool m_waitForBehaviour;
        private bool m_hasLanded;
        private bool m_isGrounded;
        private bool m_isFalling;
        private bool m_isCrouched;
        private bool m_isMoving;
        private bool m_isDashing;
        private bool m_canDash;
        private bool m_canHighJump;
        [ShowInInspector]
        private bool m_canDoubleJump;
        private bool m_hasDoubleJumped;
        private bool m_canWallJump;
        private bool m_isFlinching;
        private bool m_canFlinch;
        private bool m_isSlidingToWall;
        private bool m_isStickingToWall;
        private bool m_isWallJumping;
        private bool m_canPlatformDrop;
        private bool m_isDroppingFromPlatform;
        private bool m_isNearEdge;
        private bool m_isHookDashing;
        private bool m_isJogging;
        private bool m_isSprinting;
        private bool m_isLedging;///
        private bool m_canledgeGrab;

        private bool m_isAttacking;
        private bool m_inCombat;
        private bool m_canAttack = true;
        private bool m_isAimingProjectile;

        public PlayerCharacterState()
        {
            m_canFlinch = true;
        }

        public bool waitForBehaviour { get => m_waitForBehaviour; set => m_waitForBehaviour = value; }
        public bool hasLanded { get => m_hasLanded; set => m_hasLanded = value; }
        public bool isGrounded { get => m_isGrounded; set => m_isGrounded = value; }
        public bool isFalling { get => m_isFalling; set => m_isFalling = value; }
        public bool isCrouched { get => m_isCrouched; set => m_isCrouched = value; }
        public bool isMoving { get => m_isMoving; set => m_isMoving = value; }
        public bool isDashing { get => m_isDashing; set => m_isDashing = value; }
        public bool canDash { get => m_canDash; set => m_canDash = value; }
        public bool canHighJump { get => m_canHighJump; set => m_canHighJump = value; }
        public bool canDoubleJump { get => m_canDoubleJump; set => m_canDoubleJump = value; }
        public bool hasDoubleJumped { get => m_hasDoubleJumped; set => m_hasDoubleJumped = value; }
        public bool canWallJump { get => m_canWallJump; set => m_canWallJump = value; }
        public bool isFlinching { get => m_isFlinching; set => m_isFlinching = value; }
        public bool canFlinch { get => m_canFlinch; set => m_canFlinch = value; }
        public bool isStickingToWall { get => m_isStickingToWall; set => m_isStickingToWall = value; }
        public bool isSlidingToWall { get => m_isSlidingToWall; set => m_isSlidingToWall = value; }
        public bool isWallJumping { get => m_isWallJumping; set => m_isWallJumping = value; }
        public bool canPlatformDrop { get => m_canPlatformDrop; set => m_canPlatformDrop = value; }
        public bool isDroppingFromPlatform { get => m_isDroppingFromPlatform; set => m_isDroppingFromPlatform = value; }
        public bool isNearEdge { get => m_isNearEdge; set => m_isNearEdge = value; }
        public bool isAttacking { get => m_isAttacking; set => m_isAttacking = value; }
        public bool canAttack { get => m_canAttack; set => m_canAttack = value; }
        public bool isHookDashing { get => m_isHookDashing; set => m_isHookDashing = value; }
        public bool isJogging { get => m_isJogging; set => m_isJogging = value; }
        public bool isSprinting { get => m_isSprinting; set => m_isSprinting = value; }
        public bool isLedging { get => m_isLedging; set => m_isLedging = value; }
        public bool canLedgeGrab { get => m_canledgeGrab; set => m_canledgeGrab = value; }

        public bool isAimingProjectile { get => m_isAimingProjectile; set => m_isAimingProjectile = value; }
        public bool inCombat
        {
            get => m_inCombat; set
            {
                m_inCombat = value;
                CombatModeChanged?.Invoke(this, new CombatStateEventArgs(m_inCombat));
            }
        }

        public bool hasJumped { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}