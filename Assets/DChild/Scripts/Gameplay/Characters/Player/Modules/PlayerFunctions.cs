using UnityEngine;

namespace DChild.Gameplay.Characters.Players.Modules
{
    public class PlayerFunctions : MonoBehaviour, IComplexCharacterModule
    {
        private IdleHandle m_idleHandle;
        private BasicSlashes m_basicSlashes;
        private SlashCombo m_slashCombo;
        private EarthShaker m_earthShaker;
        private SwordThrust m_swordThrust;
        private WhipAttack m_whip;
        private ProjectileThrow m_skullThrow;
        private LedgeGrab m_ledgeGrab;
        private ShadowMorph m_shadowMorph;

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

        public void SwordUpSlashFX()
        {
            m_basicSlashes?.PlayFXFor(BasicSlashes.Type.Ground_Overhead, true);
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.Ground_Overhead, true);
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

        public void GroundForwardWhipAttackFX()
        {
            //m_whip?.PlayFXFor(WhipAttack.Type.Ground_Forward, true);
            m_whip?.EnableCollision(WhipAttack.Type.Ground_Forward, true);
        }

        public void GroundOverheadWhipAttackFX()
        {
            //m_whip?.PlayFXFor(WhipAttack.Type.Ground_Overhead, true);
            m_whip?.EnableCollision(WhipAttack.Type.Ground_Overhead, true);
        }

        public void MidairForwardWhipAttackFX()
        {
            //m_whip?.PlayFXFor(WhipAttack.Type.MidAir_Forward, true);
            m_whip?.EnableCollision(WhipAttack.Type.MidAir_Forward, true);
        }

        public void MidairOverheadWhipAttackFX()
        {
            //m_whip?.PlayFXFor(WhipAttack.Type.MidAir_Overhead, true);
            m_whip?.EnableCollision(WhipAttack.Type.MidAir_Overhead, true);
        }

        public void CrouchForwardWhipAttackFX()
        {
            //m_whip?.PlayFXFor(WhipAttack.Type.Ground_Forward, true);
            m_whip?.EnableCollision(WhipAttack.Type.Crouch_Forward, true);
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
            m_basicSlashes?.ClearExecutedCollision();
            m_slashCombo?.AttackOver();
            m_whip?.AttackOver();
            m_whip?.ClearExecutedCollision();
            m_skullThrow?.AttackOver();

            Debug.Log("FinishAttack");
        }

        public void EarthShakerPreLoop()
        {
            m_earthShaker.HandlePreFall();
        }

        public void EarthShakerLoop()
        {
            m_earthShaker.HandleFall();
        }

        public void EarthShakerImpact()
        {
            m_earthShaker.Impact();
        }

        public void EarthShakerEnd()
        {
            m_earthShaker.EndExecution();
        }

        public void SwordThrustEnd()
        {
            m_swordThrust?.AttackOver();
            m_swordThrust.EndExecution();
        }

        public void SkullThrowSpawnProjectile()
        {
            m_skullThrow?.ThrowProjectile();
        }

        public void SpawnIdleProjectile()
        {
            m_skullThrow?.SpawnIdleProjectile();
        }

        public void EndLedgeGrab()
        {
            m_ledgeGrab?.EndExecution();
        }

        public void EndShadowMorphCharge()
        {
            Debug.Log("DONE SHADOW MORPH CHARGE");
            m_shadowMorph.EndExecution();
        }

        public void Initialize(ComplexCharacterInfo info)
        {
            var character = info.character;
            m_idleHandle = character.GetComponentInChildren<IdleHandle>();
            m_basicSlashes = character.GetComponentInChildren<BasicSlashes>();
            m_slashCombo = character.GetComponentInChildren<SlashCombo>();
            m_earthShaker = character.GetComponentInChildren<EarthShaker>();
            m_swordThrust = character.GetComponentInChildren<SwordThrust>();
            m_whip = character.GetComponentInChildren<WhipAttack>();
            m_skullThrow = character.GetComponentInChildren<ProjectileThrow>();
            m_ledgeGrab = character.GetComponentInChildren<LedgeGrab>();
            m_shadowMorph = character.GetComponentInChildren<ShadowMorph>();
        }
    }
}
