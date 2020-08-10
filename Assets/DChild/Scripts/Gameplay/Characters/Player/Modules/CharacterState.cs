using DChild.Gameplay.Characters.Players.State;
using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class CharacterState : MonoBehaviour, ICrouchState, IGroundednessState, IDashState, IHighJumpState,
                                  IWallStickState, IWallJumpState, IAttackState
    {
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
        private bool m_isHighJumping;
        public bool isHighJumping { get => m_isHighJumping; set => m_isHighJumping = value; }

        [SerializeField, ReadOnly]
        private bool m_isStickingToWall;
        public bool isStickingToWall { get => m_isStickingToWall; set => m_isStickingToWall = value; }

        [SerializeField, ReadOnly]
        private bool m_canAttack;
        public bool canAttack { get => m_canAttack; set => m_canAttack = value; }

        [SerializeField, ReadOnly]
        private bool m_isAttacking;
        public bool isAttacking { get => m_isAttacking; set => m_isAttacking = value; }

        [SerializeField, ReadOnly]
        private bool m_waitForBehaviour;
        public bool waitForBehaviour { get => m_waitForBehaviour; set => m_waitForBehaviour = value; }

    }
}
