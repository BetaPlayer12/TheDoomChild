using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class ShadowImitationFunctions : MonoBehaviour
    {
        private ShadowCloneAttackFX m_basicSlashes;
        private Animator m_animator;
        AnimatorStateInfo m_currentClipInfo;

        public void SwordJumpSlashForwardFX()
        {
            m_basicSlashes?.PlayFXFor(ShadowCloneAttackFX.Type.MidAir_Forward, true);
        }
        public void SwordJumpSlashForwardEnableCollision()
        {
            m_basicSlashes?.EnableCollision(ShadowCloneAttackFX.Type.MidAir_Forward, true);
        }

        public void SwordUpSlashFX()
        {
            m_basicSlashes?.PlayFXFor(ShadowCloneAttackFX.Type.Ground_Overhead, true);
        }
        public void SwordUpSlashEnableCollision()
        {
            m_basicSlashes?.EnableCollision(ShadowCloneAttackFX.Type.Ground_Overhead, true);
        }

        public void JumpUpSlashFX()
        {
            m_basicSlashes?.PlayFXFor(ShadowCloneAttackFX.Type.MidAir_Overhead, true);
        }
        public void SwordJumpUpSlashEnableCollision()
        {
            m_basicSlashes?.EnableCollision(ShadowCloneAttackFX.Type.MidAir_Overhead, true);
        }

        public void CrouchSlashFX()
        {
            m_basicSlashes?.PlayFXFor(ShadowCloneAttackFX.Type.Crouch, true);
        }
        public void SwordCrouchSlashEnableCollision()
        {
            m_basicSlashes?.EnableCollision(ShadowCloneAttackFX.Type.Crouch, true);
        }

        public void SlashCombo()
        {
            m_currentClipInfo = m_animator.GetCurrentAnimatorStateInfo(0);

            //Bleeegh hard coded values >.<
            if (m_currentClipInfo.IsName("Slash Combo 1"))
            {
                m_basicSlashes?.EnableSlashComboCollision(true, 0);
            }
            if (m_currentClipInfo.IsName("Slash Combo 2"))
            {
                m_basicSlashes?.EnableSlashComboCollision(true, 1);
            }
            if (m_currentClipInfo.IsName("Slash Combo 3"))
            {
                m_basicSlashes?.EnableSlashComboCollision(true, 2);
            }
        }

        public void FinishAttackAnim()
        {
            m_basicSlashes.FinishAttackAnim();
        }

        private void Awake()
        {
            m_basicSlashes = GetComponentInChildren<ShadowCloneAttackFX>();
            m_animator = GetComponentInChildren<Animator>();
        }
    }
}
