using DChild.Gameplay.Characters.Players.BattleAbilityModule;
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
        private ShadowSlide m_shadowSlide;

        #region Battle Abilities
        private AirLunge m_airLunge;
        private FireFist m_fireFist;
        private ReaperHarvest m_reaperHarvest;
        private KrakenRage m_krakenRage;
        private FinalSlash m_finalSlash;
        private AirSlashCombo m_airSlashCombo;
        private SovereignImpale m_sovereignImpale;
        private HellTrident m_hellTrident;
        private FoolsVerdict m_foolsVerdict;
        private SoulFireBlast m_soulFireBlast;
        private EdgedFury m_edgedFury;
        private NinthCircleSanction m_ninthCircleSanction;
        private DoomsdayKong m_doomsdayKong;
        private BackDiver m_backDiver;
        private Barrier m_barrier;
        private FencerFlash m_fencerFlash;
        private DiagonalSwordDash m_diagonalSwordDash;
        private ChampionsUprising m_championsUprising;
        private Eelecktrick m_eelecktrick;
        private LightningSpear m_lightningSpear;
        private IcarusWings m_icarusWings;
        private TeleportingSkull m_teleportingSkull;
        private AirSlashRange m_airSlashRange;
        #endregion

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

        public void SwordJumpUpSlashDisableCollision()
        {
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.MidAir_Overhead, false);
        }

        public void SwordUpSlashFX()
        {
            m_basicSlashes?.PlayFXFor(BasicSlashes.Type.Ground_Overhead, true);
        }

        public void SwordUpSlashEnableCollision()
        {
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.Ground_Overhead, true);
        }

        public void SwordUpSlashDisableCollision()
        {
            m_basicSlashes?.EnableCollision(BasicSlashes.Type.Ground_Overhead, false);
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

        public void AirSlashCombo()
        {
            //m_slashCombo?.PlayFX(true);
            m_airSlashCombo?.EnableCollision(true);
        }

        public void ContinueAirSlashCombo()
        {
            m_airSlashCombo?.PlayFX(true);
            m_airSlashCombo?.EnableCollision(true);
        }

        public void AirComboAttackEnd()
        {
            m_airSlashCombo?.AttackOver();
        }

        public void AirComboEnd()
        {
            m_airSlashCombo?.ComboEnd();
        }

        public void ResetWhipComboGravity()
        {
            //Debug.Log("ComboWhipEnd");
            m_whipCombo?.ResetGravity();
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

        public void ShadowSlideEndTransitionStart()
        {
            m_shadowSlide.EndTransitionStart();
        }

        public void ShadowSlideEndTransitionEnd()
        {
            m_shadowSlide.EndTransitionEnd();
        }

        #region BattleAbilities

        public void AirLungeEnableCollision()
        {
            m_airLunge.EnableCollision(true);
        }

        public void AirLungeDisableCollision()
        {
            m_airLunge.EnableCollision(false);
        }

        public void AirLungeEnd()
        {
            //m_airLunge?.AttackOver();
            m_airLunge.EndExecution();
        }

        public void FireFistSummon()
        {
            m_fireFist.Summon();
        }

        public void FireFistEnableCollision()
        {
            m_fireFist.EnableCollision(true);
        }

        public void FireFistDisableCollision()
        {
            m_fireFist.EnableCollision(false);
        }

        public void ReaperHarvestStartDash()
        {
            m_reaperHarvest.StartDash();
        }

        public void ReaperHarvestEnableCollision()
        {
            m_reaperHarvest.EnableCollision(true);
        }

        public void ReaperHarvestDisableCollision()
        {
            m_reaperHarvest.EnableCollision(false);
        }

        public void ReaperHarvestEnd()
        {
            //m_reaperHarvest?.AttackOver();
            m_reaperHarvest.EndExecution();
        }

        public void FinalSlashEnableDash()
        {
            m_finalSlash.EnableDash(true);
        }

        public void FinalSlashDisableDash()
        {
            m_finalSlash.EnableDash(false);
        }

        public void FinalSlashDash()
        {
            m_finalSlash.ExecuteDash();
        }

        public void FinalSlashEnableCollision()
        {
            m_finalSlash.EnableCollision(true);
        }

        public void FinalSlashDisableCollision()
        {
            m_finalSlash.EnableCollision(false);
        }

        public void FinalSlashEnd()
        {
            m_finalSlash.EndExecution();
        }

        #region FinalSlashFX
        public void FinalSlashEnableSwordGlowFX()
        {
            m_finalSlash.SetSwordGlowFXAnimator(true);
        }

        public void FinalSlashEnableDustChargeFX()
        {
            m_finalSlash.SetDustChargeFXAnimator(true);
        }

        public void FinalSlashDisableSwordGlowFX()
        {
            m_finalSlash.SetSwordGlowFXAnimator(false);
        }

        public void FinalSlashDisableDustChargeFX()
        {
            m_finalSlash.SetDustChargeFXAnimator(false);
        }

        public void FinalSlashPlayDustFeedbackFX()
        {
            m_finalSlash.SpawnDustFeedbackFX();
        }

        public void FinalSlash1FX()
        {
            m_finalSlash.SetFinalSlashHolderFX(FinalSlash.FinalSlashState.FinalSlash1);
        }

        public void FinalSlash2FX()
        {
            m_finalSlash.SetFinalSlashHolderFX(FinalSlash.FinalSlashState.FinalSlash2);
        }

        public void FinalSlash3FX()
        {
            m_finalSlash.SetFinalSlashHolderFX(FinalSlash.FinalSlashState.FinalSlash3);
        }
        #endregion

        public void KrakenRageEnableCollision()
        {
            m_krakenRage.EnableCollision(true);
        }

        public void KrakenRageDisableCollision()
        {
            m_krakenRage.EnableCollision(false);
        }

        public void KrakenRageEnd()
        {
            //m_krakenRage?.AttackOver();
            m_krakenRage.EndExecution();
        }

        public void FireFistEnd()
        {
            //m_fireFist?.AttackOver();
            m_fireFist.EndExecution();
        }

        public void SovereignImpaleSummon()
        {
            m_sovereignImpale.Summon();
        }

        public void SovereignImpaleEnd()
        {
            //m_sovereignImpale?.AttackOver();
            m_sovereignImpale.EndExecution();
        }

        public void HellTridentEnableCollision()
        {
            m_hellTrident.EnableCollision(true);
        }

        public void HellTridentDisableCollision()
        {
            m_hellTrident.EnableCollision(false);
        }

        public void HellTridentSummon()
        {
            m_hellTrident.Summon();
        }

        public void HellTridentPlaySpawnFX()
        {
            m_hellTrident.PlaySpawnFX();
        }

        public void HellTridentEnd()
        {
            //m_hellTrident?.AttackOver();
            m_hellTrident.EndExecution();
        }

        public void FoolsVerdictSummon()
        {
            m_foolsVerdict.Summon();
        }

        public void FoolsVerdictEnd()
        {
            //m_foolsVerdict?.AttackOver();
            m_foolsVerdict.EndExecution();
        }

        public void SoulFireBlastSummon()
        {
            m_soulFireBlast.Summon();
        }

        public void SoulFireBlastEnd()
        {
            //m_soulFireBlast?.AttackOver();
            m_soulFireBlast.EndExecution();
        }

        public void EdgedFuryEnableCollision()
        {
            m_edgedFury.EnableCollision(true);
        }

        public void EdgedFuryDisableCollision()
        {
            m_edgedFury.EnableCollision(false);
        }

        public void EdgedFuryEnd()
        {
            //m_edgedFury?.AttackOver();
            m_edgedFury.EndExecution();
        }

        public void NinthCircleSanctionSummon()
        {
            m_ninthCircleSanction.Summon();
        }

        public void NinthCircleSanctionEnd()
        {
            m_ninthCircleSanction.EndExecution();
        }

        public void DoomsdayKongSummon()
        {
            m_doomsdayKong.Summon();
        }

        public void DoomsdayKongEnd()
        {
            m_doomsdayKong.EndExecution();
        }

        public void BackDiverSummon()
        {
            //if (m_teleportingSkull.spawnedProjectile == null)
            //    m_backDiver.Summon();
        }

        public void BackDiverGetProjectile()
        {
            //m_teleportingSkull.Execute();
            //m_teleportingSkull.GetSpawnedProjectile(m_backDiver.spawnedProjectile);
        }

        public void BackDiverCheckGround()
        {
            m_backDiver.CheckGround();
        }

        public void BackDiverLandOnGround()
        {
            m_backDiver.LandOnGround();
        }

        public void BackDiverEnd()
        {
            m_backDiver.EndExecution();
        }

        public void BarrierEnableShield()
        {
            m_barrier.EnableShield(true);
        }

        public void BarrierDisableShield()
        {
            m_barrier.EnableShield(false);
        }

        public void BarrierEnd()
        {
            m_barrier.EndExecution();
        }

        public void FencerFlashStartDash()
        {
            m_fencerFlash.StartDash();
        }

        public void FencerFlashEnableCollision()
        {
            m_fencerFlash.EnableCollision(true);
        }

        public void FencerFlashDisableCollision()
        {
            m_fencerFlash.EnableCollision(false);
        }

        public void FencerFlashEnd()
        {
            //m_fencerFlash?.AttackOver();
            m_fencerFlash.EndExecution();
        }

        public void DiagonalSwordDashStartDash()
        {
            m_diagonalSwordDash.StartDash();
        }

        public void DiagonalSwordDashEnableCollision()
        {
            m_diagonalSwordDash.EnableCollision(true);
        }

        public void DiagonalSwordDashDisableCollision()
        {
            m_diagonalSwordDash.EnableCollision(false);
        }

        public void DiagonalSwordDashEnd()
        {
            //m_diagonalSwordDash?.AttackOver();
            m_diagonalSwordDash.EndExecution();
        }

        public void ChampionsUprisingStartDash()
        {
            m_championsUprising.StartDash();
        }

        public void ChampionsUprisingStartUppercut()
        {
            m_championsUprising.StartUppercut();
        }

        public void ChampionsUprisingEnableCollision()
        {
            m_championsUprising.EnableCollision(true);
        }

        public void ChampionsUprisingDisableCollision()
        {
            m_championsUprising.EnableCollision(false);
        }

        public void ChampionsUprisingEnd()
        {
            //m_championsUprising?.AttackOver();
            m_championsUprising.EndExecution();
        }

        public void EelecktrickSummon()
        {
            m_eelecktrick.SummonWhip();
        }

        public void EelecktrickEnd()
        {
            m_eelecktrick.EndExecution();
        }

        public void LightningSummon()
        {
            m_lightningSpear.SummonLightning();
        }

        public void LightningSpearEnd()
        {
            m_lightningSpear.EndExecution();
        }

        public void IcarusWingsJump()
        {
            m_icarusWings.Jump();
        }

        public void IcarusWingsEnd()
        {
            m_icarusWings.EndExecution();
        }

        public void TeleportingSkullGetProjectile()
        {
            m_teleportingSkull.GetSpawnedProjectile(m_projectileThrow.spawnedProjectile);
        }

        public void AirSlashRangeSummon()
        {
            m_airSlashRange.SummonAirSlash();
        }

        public void AirSlashRangeEnd()
        {
            m_airSlashRange.EndExecution();
        }

        public void TeleportingSkullEnd()
        {
            m_teleportingSkull.EndExecution();
        }

        public void TeleportingSkullEnableCollision()
        {
            m_teleportingSkull.EnableCollision(true);
        }

        public void TeleportingSkullDisableCollision()
        {
            m_teleportingSkull.EnableCollision(false);
        }
        #endregion

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

        public void Null() { }

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
            m_airLunge = character.GetComponentInChildren<AirLunge>();
            m_fireFist = character.GetComponentInChildren<FireFist>();
            m_reaperHarvest = character.GetComponentInChildren<ReaperHarvest>();
            m_krakenRage = character.GetComponentInChildren<KrakenRage>();
            m_finalSlash = character.GetComponentInChildren<FinalSlash>();
            m_airSlashCombo = character.GetComponentInChildren<AirSlashCombo>();
            m_sovereignImpale = character.GetComponentInChildren<SovereignImpale>();
            m_hellTrident = character.GetComponentInChildren<HellTrident>();
            m_foolsVerdict = character.GetComponentInChildren<FoolsVerdict>();
            m_soulFireBlast = character.GetComponentInChildren<SoulFireBlast>();
            m_edgedFury = character.GetComponentInChildren<EdgedFury>();
            m_ninthCircleSanction = character.GetComponentInChildren<NinthCircleSanction>();
            m_doomsdayKong = character.GetComponentInChildren<DoomsdayKong>();
            m_backDiver = character.GetComponentInChildren<BackDiver>();
            m_barrier = character.GetComponentInChildren<Barrier>();
            m_fencerFlash = character.GetComponentInChildren<FencerFlash>();
            m_diagonalSwordDash = character.GetComponentInChildren<DiagonalSwordDash>();
            m_championsUprising = character.GetComponentInChildren<ChampionsUprising>();
            m_eelecktrick = character.GetComponentInChildren<Eelecktrick>();
            m_lightningSpear = character.GetComponentInChildren<LightningSpear>();
            m_icarusWings = character.GetComponentInChildren<IcarusWings>();
            m_teleportingSkull = character.GetComponentInChildren<TeleportingSkull>();
            m_airSlashRange = character.GetComponentInChildren<AirSlashRange>();
            m_shadowSlide = character.GetComponentInChildren<ShadowSlide>();
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
