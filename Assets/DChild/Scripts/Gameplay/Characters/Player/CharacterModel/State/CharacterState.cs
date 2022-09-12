using Holysoft.Event;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.State
{
    [AddComponentMenu("DChild/Gameplay/Player/Character State")]
    public class CharacterState : MonoBehaviour, IPlayerState, IMoveState, IGroundednessState,
                                ICrouchState, IFlinchState, IWallStickState,
                                IDashState, IDoubleJumpState, IWallJumpState,
                                IHighJumpState, IBehaviourState, ICombatState, IPlatformDropState,
                                IWhipGrapple, IProjectileThrowState , ILedgeGrabState
    {
        public event EventAction<CombatStateEventArgs> CombatModeChanged;
         
        private bool m_waitForBehaviour;
        private bool m_isGrounded;
        private bool m_isFalling;
        private bool m_isCrouched;
        private bool m_isMoving;
        private bool m_isDashing;
        private bool m_canDash;
        private bool m_canHighJump;
        private bool m_hasJumped;
        private bool m_canDoubleJump;
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
        private bool m_isSliding;

        private bool m_isAttacking;
        private bool m_inCombat;
        private bool m_canAttack = true;
        private bool m_isAimingProjectile;

        public bool waitForBehaviour { get => m_waitForBehaviour; set => m_waitForBehaviour = value; }
        public bool isGrounded { get => m_isGrounded; set => m_isGrounded = value; }
        public bool isFalling { get => m_isFalling; set => m_isFalling = value; }
        public bool isCrouched { get => m_isCrouched; set => m_isCrouched = value; }
        public bool isMoving { get => m_isMoving; set => m_isMoving = value; }
        public bool isDashing { get => m_isDashing; set => m_isDashing = value; }
        public bool canDash { get => m_canDash; set => m_canDash = value; }
        public bool isHighJumping { get => m_canHighJump; set => m_canHighJump = value; }
        public bool hasJumped { get => m_hasJumped; set => m_hasJumped = value; }
        public bool canDoubleJump { get => m_canDoubleJump; set => m_canDoubleJump = value; }
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
        public bool isSliding { get => m_isSliding; set => m_isSliding = value; }

        public bool isAimingProjectile { get => m_isAimingProjectile; set => m_isAimingProjectile = value; }
        public bool inCombat
        {
            get => m_inCombat; set
            {
                m_inCombat = value;
                CombatModeChanged?.Invoke(this, new CombatStateEventArgs(m_inCombat));
            }
        }

        public bool canWallCrawl { get => canWallCrawl; set => canWallCrawl = value; }
    }
}