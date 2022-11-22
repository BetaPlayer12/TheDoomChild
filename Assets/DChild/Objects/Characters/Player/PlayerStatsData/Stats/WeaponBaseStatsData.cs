using DChild.Gameplay.Combat;
using DChild.Gameplay.Combat.StatusAilment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "PlayerWeaponData", menuName = "DChild/Gameplay/Combat/Player Weapon Data")]
public class WeaponBaseStatsData : ScriptableObject
{
    [SerializeField]
    private Damage m_damage;

    public Damage damage => m_damage;

    [SerializeField]
    private List<StatusEffectChance> m_statusInflictions;
    public List<StatusEffectChance> statusInflictions => m_statusInflictions;
}
