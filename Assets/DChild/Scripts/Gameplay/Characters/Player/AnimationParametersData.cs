using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [CreateAssetMenu(fileName = "AnimationParametersData", menuName = "DChild/Animation Parameters")]
    public class AnimationParametersData : SerializedScriptableObject
    {
        public enum Parameter
        {
            IsIdle,
            IdleState,
            IsGrounded,
            CombatMode,
            IsAttacking,
            SlashState,
            IsDashing,
            Jump,
            SpeedX,
            SpeedY,
            IsCrouched,
            YInput,
            IsDead,
            WallStick,
            Flinch,
            EarthShaker,
            SwordTrust,
            WhipAttack,
            IsLevitating,
            IsGrabbing,
            IsPulling,
            IsPushing,
            ShadowMode,
            IsSliding,
            ProjectileThrow,
            ProjectileThrowVariant,
            LedgeGrab,
            FlinchState,
            IsBlocking,
            IsCharging,
            StepClimb,
            AimingProjectile,
            IsWallCrawling,
            DoubleJump,
            WallJump,
            WhipState,
            XInput,
            ReaperHarvest,
            KrakenRage,
            AirSlashCombo,
            AirSlashState,
            SovereignImpale,
            HellTrident,
            FoolsVerdict,
            SoulFireBlast,
            EdgedFury,
            BackDiver,
            Barrier,
            DiagonalSwordDash,
            ChampionsUprising,
            LightningSpear,
            IcarusWings,
            AirSlashRange,
            TeleportingSkull,
        }

        [SerializeField]
        private Dictionary<Parameter, string> m_labels;

        public int GetParameterLabel(Parameter parameter) => Animator.StringToHash(m_labels[parameter]);
    }
}