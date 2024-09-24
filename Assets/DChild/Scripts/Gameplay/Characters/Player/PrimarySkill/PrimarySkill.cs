using DChild.Gameplay.Characters.Players.Modules;
using System;
using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    [Flags]
    public enum PrimarySkill
    {
        DoubleJump = 1 << 0,
        WallMovement = 1 << 1,
        Dash = 1 << 2,
        Slide = 1 << 3,
        BlackBloodImmunity = 1 << 4,
        SwordThrust = 1 << 5,
        EarthShaker = 1 << 6,
        Whip = 1 << 7,
        DevilWings = 1 << 8,
        ShadowDash = 1 << 9,
        ShadowSlide = 1 << 10,
        SkullThrow = 1 << 11,
        ShadowMorph = 1 << 12,

        [HideInInspector]
        All = DoubleJump | WallMovement | Dash | Slide | BlackBloodImmunity | SwordThrust | EarthShaker | Whip | DevilWings | ShadowDash | ShadowSlide | SkullThrow | ShadowMorph,
        [HideInInspector]
        None = 0
    }
}
