using UnityEngine;

namespace DChild.Gameplay.Characters.Players
{
    public enum PlayerModifier
    {
        Dash_Distance,
        Cooldown_Dash,
        Critical_Damage,
        Magic_Requirement,
        ShadowMagic_Requirement,
        Jump_Power,
        WallStick_Duration,
        AttackDamage,
        MoveSpeed,
        AttackSpeed,
        Item_Effectivity,
        SoulAbsorb_Range,
        [HideInInspector]
        _COUNT
    }
}