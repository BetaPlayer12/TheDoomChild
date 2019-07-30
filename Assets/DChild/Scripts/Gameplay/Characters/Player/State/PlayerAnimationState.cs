using Sirenix.OdinInspector;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players.State
{
    [System.Serializable]
    public class PlayerAnimationState : IPlayerAnimationState
    {
        private bool m_isFromFall;
        private bool m_isAnticPlayed;
        private bool m_hasDoubleJumped;
        private bool m_hasAttacked;
        private bool m_isLanding;
        private bool m_isFallingToJog;
        private bool m_isFromJog;
        private bool m_hasJumped;
        private bool m_isShortJumping;
        private bool m_isFromIdle;
        private bool m_isStaticJumping;
        private bool m_hasDashed;
        private bool m_hasWallSticked;
        private bool m_isWallJumping;
        private bool m_wallJumpEnd;
        private bool m_isThrowingBomb;
        private bool m_transitionToFall2;
        private bool m_isHardLanding;
        private bool m_isFallingFromWallJump;
        private bool m_isLedging;

        public bool isFromFall { get => m_isFromFall; set => m_isFromFall = value; }
        public bool isAnticPlayed { get => m_isAnticPlayed; set => m_isAnticPlayed = value; }
        public bool hasDoubleJumped { get => m_hasDoubleJumped; set => m_hasDoubleJumped = value; }
        public bool hasAttacked { get => m_hasAttacked; set => m_hasAttacked = value; }
        public bool isLanding { get => m_isLanding; set => m_isLanding = value; }
        public bool isFallingToJog { get => m_isFallingToJog; set => m_isFallingToJog = value; }
        public bool isFromJog { get => m_isFromJog; set => m_isFromJog = value; }
        public bool hasJumped { get => m_hasJumped; set => m_hasJumped = value; }
        public bool isShortJumping { get => m_isShortJumping; set => m_isShortJumping = value; }
        public bool isFromIdle { get => m_isFromIdle; set => m_isFromIdle = value; }
        public bool isStaticJumping { get => m_isStaticJumping; set => m_isStaticJumping = value; }
        public bool hasDashed { get => m_hasDashed; set => m_hasDashed = value; }
        public bool hasWallSticked { get => m_hasWallSticked; set => m_hasWallSticked = value; }
        public bool isWallJumping { get => m_isWallJumping; set => m_isWallJumping = value; }
        public bool onWallJumpEnd { get => m_wallJumpEnd; set => m_wallJumpEnd = value; }
        public bool isThrowingBomb { get => m_isThrowingBomb; set => m_isThrowingBomb = value; }
        public bool transitionToFall2 { get => m_transitionToFall2; set => m_transitionToFall2 = value; }
        public bool isHardLanding { get => m_isHardLanding; set => m_isHardLanding = value; }
        public bool isFallingFromWallJump { get => m_isFallingFromWallJump; set => m_isFallingFromWallJump = value; }
        public bool isLedging { get => m_isLedging; set => m_isLedging = value; }//

        public void ResetAnimations()
        {
            m_isFromFall = false;
            m_isAnticPlayed = false;
            m_hasDoubleJumped = false;
            m_hasAttacked = false;
            m_isLanding = false;
            m_isFallingToJog = false;
            m_isFromJog = false;
            m_hasJumped = false;
            m_isFromIdle = false;
            m_hasDashed = false;
            m_hasWallSticked = false;
            m_isWallJumping = false;
            m_wallJumpEnd = false;
            m_isThrowingBomb = false;
            m_transitionToFall2 = false;
            m_isHardLanding = false;
            m_isLedging = false;
            //m_isFallingFromWallJump = false;
        }
    }
}