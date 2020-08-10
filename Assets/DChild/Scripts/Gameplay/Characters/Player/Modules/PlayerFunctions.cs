using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerFunctions : MonoBehaviour, IComplexCharacterModule
    {
        private IdleHandle m_idleHandle;
        private BasicSlashes m_basicSlashes;

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

        public void FinishAttackAnim()
        {
            m_basicSlashes?.AttackOver();
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.MidAir_Forward, false);
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.MidAir_Overhead, false);
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            var character = info.character;
            m_idleHandle = character.GetComponentInChildren<IdleHandle>();
            m_basicSlashes = character.GetComponentInChildren<BasicSlashes>();
        }
    }
}
