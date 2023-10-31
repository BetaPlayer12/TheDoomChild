using DChild.Gameplay.Characters.Players.Modules;
using Spine.Unity;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace DChild.Gameplay.Characters.Players
{
    public class OnExitBehaviourState : StateMachineBehaviour
    {
        private enum Command
        {
            EndAttack,
            EndEarthShaker,
            EndLedgeGrab,
            EndShadowMorphCharge,
            EndComboAttack,
            EndCombo,
            EndWhipComboAttack,
            EndWhipCombo,
            ResetWhipComboGravity,
            EndSwordThrust,
            EndAirLunge,
            EndFireFist,
            EndReaperHarvest,
            EndKrakenRage,
            EndAirComboAttack,
            EndAirCombo,
            EndSovereignImpale,
            EndHellTrident,
            EndFoolsVerdict,
            EndSoulFireBlast,
            EndEdgedFury,
            EndNinthCircleSanction,
            EndDoomsdayKong,
            EndBackDiver,
            EndBarrier,
            EndFinalSlash,
            EndFencerFlash,
            EndDiagonalSwordDash,
            EndChampionsUprising,
            EndEelecktrick,
            EndLightningSpear,
            EndIcarusWings,
            EndProjectileThrow,
            EndAirSlashRange,
            EndTeleportingSkull,
        }

        [SerializeField]
        private Command m_toExecute;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            var player = animator.GetComponent<PlayerFunctions>();

            if (player != null)
            {
                switch (m_toExecute)
                {
                    case Command.EndAttack:
                        player.FinishAttackAnim();
                        break;
                    case Command.EndEarthShaker:
                        player.EarthShakerEnd();
                        break;
                    case Command.EndLedgeGrab:
                        player.EndLedgeGrab();
                        break;
                    case Command.EndShadowMorphCharge:
                        player.EndShadowMorphCharge();
                        break;
                    case Command.EndComboAttack:
                        player.ComboAttackEnd();
                        break;
                    case Command.EndCombo:
                        player.ComboEnd();
                        break;
                    case Command.EndWhipComboAttack:
                        player.ComboWhipAttackEnd();
                        break;
                    case Command.EndWhipCombo:
                        player.ComboWhipEnd();
                        break;
                    case Command.ResetWhipComboGravity:
                        player.ResetWhipComboGravity();
                        break;
                    case Command.EndSwordThrust:
                        player.SwordThrustEnd();
                        break;
                    case Command.EndAirLunge:
                        player.AirLungeEnd();
                        break;
                    case Command.EndFireFist:
                        player.FireFistEnd();
                        break;
                    case Command.EndReaperHarvest:
                        player.ReaperHarvestEnd();
                        break;
                    case Command.EndKrakenRage:
                        player.KrakenRageEnd();
                        break;
                    case Command.EndAirComboAttack:
                        player.AirComboAttackEnd();
                        break;
                    case Command.EndAirCombo:
                        player.AirComboEnd();
                        break;
                    case Command.EndSovereignImpale:
                        player.SovereignImpaleEnd();
                        break;
                    case Command.EndHellTrident:
                        player.HellTridentEnd();
                        break;
                    case Command.EndFoolsVerdict:
                        player.FoolsVerdictEnd();
                        break;
                    case Command.EndSoulFireBlast:
                        player.SoulFireBlastEnd();
                        break;
                    case Command.EndEdgedFury:
                        player.EdgedFuryEnd();
                        break;
                    case Command.EndNinthCircleSanction:
                        player.NinthCircleSanctionEnd();
                        break;
                    case Command.EndDoomsdayKong:
                        player.DoomsdayKongEnd();
                        break;
                    case Command.EndBackDiver:
                        player.BackDiverEnd();
                        break;
                    case Command.EndBarrier:
                        player.BarrierEnd();
                        break;
                    case Command.EndFinalSlash:
                        player.FinalSlashEnd();
                        break;
                    case Command.EndFencerFlash:
                        player.FencerFlashEnd();
                        break;
                    case Command.EndDiagonalSwordDash:
                        player.DiagonalSwordDashEnd();
                        break;
                    case Command.EndChampionsUprising:
                        player.ChampionsUprisingEnd();
                        break;
                    case Command.EndEelecktrick:
                        player.EelecktrickEnd();
                        break;
                    case Command.EndLightningSpear:
                        player.LightningSpearEnd();
                        break;
                    case Command.EndIcarusWings:
                        player.IcarusWingsEnd();
                        break;
                    case Command.EndProjectileThrow:
                        player.FinishProjectileThrow();
                        break;
                    case Command.EndAirSlashRange:
                        player.AirSlashRangeEnd();
                        break;
                    case Command.EndTeleportingSkull:
                        player.TeleportingSkullEnd();
                        break;
                }
            }
            else
            {
                var shadow = animator.GetComponent<ShadowCloneAttackFX>();

                switch (m_toExecute)
                {
                    case Command.EndAttack:
                        shadow.FinishAttackAnim();
                        break;
                    case Command.EndComboAttack:
                        shadow.FinishSlashComboAttackAnim();
                        break;
                    case Command.EndCombo:
                        shadow.ComboEnd();
                        break;
                }
            }
        }
    }

    //public class ForceEndAfterDurationBehaviourState : StateMachineBehaviour
    //{
    //    public AnimationReferenceAsset m_animation;
    //    private Coroutine m_routine;
    //    private PlayerFunctions m_reference;

    //    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //    {
    //        base.OnStateExit(animator, stateInfo, layerIndex);
    //        if (m_reference != null)
    //        {
    //            m_reference = animator.GetComponent<PlayerFunctions>();
    //        }
    //        m_reference.StartCoroutine()
    //    }

    //    private IEnumerator DurationRoutine()
    //    {
    //        yield return new m_animation.
    //    }
    //}
}