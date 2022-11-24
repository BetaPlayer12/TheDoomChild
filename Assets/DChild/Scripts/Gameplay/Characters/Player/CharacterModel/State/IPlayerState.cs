using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.State
{
    public interface IPlayerState
    {
        bool waitForBehaviour { get; }
        bool isGrounded { get; }
        bool isFalling { get; }
        bool isCrouched { get; }
        bool isMoving { get; }
        bool isDashing { get; }
        bool canDash { get; }
        bool isHighJumping { get; }
        bool hasJumped { get;}
        bool canDoubleJump { get; }
        bool canWallJump { get; }
        bool isFlinching { get; }
        bool inCombat { get; }
        bool canAttack { get;}
        bool isAttacking { get;}
        bool isStickingToWall { get;}
        bool isSlidingToWall { get;}
        bool isWallJumping { get;}
        bool canPlatformDrop { get; }
        bool isDroppingFromPlatform { get; }
        bool isNearEdge { get; }
        bool isHookDashing { get; }
        bool isAimingProjectile { get; }
        bool isJogging { get; }
        bool isSprinting { get; }
        bool isSliding { get; }
    }
}