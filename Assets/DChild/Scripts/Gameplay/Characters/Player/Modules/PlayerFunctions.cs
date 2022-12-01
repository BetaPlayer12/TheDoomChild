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
        private WhipAttackCombo m_whipCombo;
        private ProjectileThrow m_projectileThrow;
        private LedgeGrab m_ledgeGrab;
        private ShadowMorph m_shadowMorph;
        private ShadowGaugeRegen m_shadowGaugeRegen;

        public void IdleStateFinished()
        {
            m_idleHandle?.BackToDefaultIdle();
        }

        public void SwordJumpSlashForwardFX()
        {
            m_basicSlashes?.PlayFXFor(BasicSlashes.Type.MidAir_Forward, true);
        }
        public void SwordJumpSlashForwardEnableCollision()
        {
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.MidAir_Forward, true);
        }

        public void JumpUpSlashFX()
        {
            m_basicSlashes?.PlayFXFor(BasicSlashes.Type.MidAir_Overhead, true);
        }
        public void SwordJumpUpSlashEnableCollision()
        {
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.MidAir_Overhead, true);
        }

        public void SwordUpSlashFX()
        {
            m_basicSlashes?.PlayFXFor(BasicSlashes.Type.Ground_Overhead, true);
        }
        public void SwordUpSlashEnableCollision()
        {
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.Ground_Overhead, true);
        }

        public void CrouchSlashFX()
        {
            m_basicSlashes?.PlayFXFor(BasicSlashes.Type.Crouch, true);
        }
        public void SwordCrouchSlashEnableCollision()
        {
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.Crouch, true);
        }

        public void SlashCombo()
        {
            //m_slashCombo?.PlayFX(true);
            m_slashCombo?.EnableCollision(true);
        }

        public void WhipCombo()
        {
            //Debug.Log("Do Whip Combo EVENT");
            m_whipCombo?.EnableCollision(true);
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
            m_slashCombo?.PlayFX(true);
            m_slashCombo?.EnableCollision(true);
        }

        public void FinishAttackAnim()
        {
            Debug.Log("FinishAttackAnim");
            m_basicSlashes?.AttackOver();
            m_basicSlashes?.ClearExecutedCollision();
            m_whip?.AttackOver();
            m_whip?.ClearExecutedCollision();
        }

        public void ComboAttackEnd()
        {
            m_slashCombo?.AttackOver();
        }

        public void ComboEnd()
        {
            m_slashCombo?.ComboEnd();
        }

        public void ComboWhipAttackEnd()
        {
            //Debug.Log("ComboWhipAttackEnd");
            m_whipCombo?.AttackOver();
        }

        public void ComboWhipEnd()
        {
            //Debug.Log("ComboWhipEnd");
            m_whipCombo?.ComboEnd();
        }

        public void FinishProjectileThrow()
        {
            m_projectileThrow?.AttackOver();
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

        public void SwordThrustPush()
        {
            m_swordThrust?.Push();
        }

        public void SkullThrowSpawnProjectile()
        {
            m_projectileThrow?.ThrowProjectile();
        }

        public void SpawnIdleProjectile()
        {
            m_projectileThrow?.SpawnIdleProjectile();
        }

        public void EndLedgeGrab()
        {
            m_ledgeGrab?.EndExecution();
        }

        public void EndShadowMorphCharge()
        {
            //m_shadowGaugeRegen?.Enable(true);
            m_shadowMorph.EndExecution();
        }

        public void LedgeGrabTeleport()
        {
            m_ledgeGrab?.Teleport();
        }

        public void LedgeGrabeEnableHitbox()
        {
            m_ledgeGrab?.EnableHitbox();
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
            m_whipCombo = character.GetComponentInChildren<WhipAttackCombo>();
            m_projectileThrow = character.GetComponentInChildren<ProjectileThrow>();
            m_ledgeGrab = character.GetComponentInChildren<LedgeGrab>();
            m_shadowMorph = character.GetComponentInChildren<ShadowMorph>();
            m_shadowGaugeRegen = character.GetComponentInChildren<ShadowGaugeRegen>();
        }

        #region TESTING
        public void StartSlashCombo()
        {

        }

        public void EndSlashCombo()
        {

        }
        #endregion
    }
}
