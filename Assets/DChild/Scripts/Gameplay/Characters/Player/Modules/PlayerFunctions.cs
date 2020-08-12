using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerFunctions : MonoBehaviour, IComplexCharacterModule
    {
        private IdleHandle m_idleHandle;
        private BasicSlashes m_basicSlashes;
        private SlashCombo m_slashCombo;

        public void DefaultIdleStateFinished()
        {
            m_idleHandle?.GenerateRandomState();
        }

        public void SwordJumpSlashForwardFX()
        {
            m_basicSlashes?.PlayFXFor(BasicSlashes.Type.MidAir_Forward, true);
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.MidAir_Forward, true);
        }

        public void JumpUpSlashFX()
        {
            m_basicSlashes?.PlayFXFor(BasicSlashes.Type.MidAir_Overhead, true);
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.MidAir_Overhead, true);
        }

        public void CrouchSlashFX()
        {
            m_basicSlashes?.PlayFXFor(BasicSlashes.Type.Crouch, true);
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.Crouch, true);
        }

        public void SlashCombo()
        {
            m_slashCombo?.PlayFX(true);
            m_slashCombo?.EnableCollision(true);
        }

        public void ContinueSlashCombo()
        {
            m_slashCombo?.ContinueCombo();
            m_slashCombo?.PlayFX(true);
            m_slashCombo?.EnableCollision(true);
        }

        public void FinishAttackAnim()
        {
            m_basicSlashes?.AttackOver();
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.MidAir_Forward, false);
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.MidAir_Overhead, false);
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.Crouch, false);

            m_slashCombo?.AttackOver();
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            var character = info.character;
            m_idleHandle = character.GetComponentInChildren<IdleHandle>();
            m_basicSlashes = character.GetComponentInChildren<BasicSlashes>();
            m_slashCombo = character.GetComponentInChildren<SlashCombo>();
        }
    }
}
