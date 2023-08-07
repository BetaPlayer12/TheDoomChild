using DChild.Gameplay.ArmyBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullifyDamage : IArmyAbilityEffect
{
    [SerializeField]
    private UnitType m_unitType;
    [SerializeField]
    private int m_powerPercentage;
    [SerializeField]
    private bool m_isAppliedToOpponent;
    public void ApplyEffect(Army owner, Army opponent)
    {
        var percentage = PercentageConverter.ConvertPercentage(m_powerPercentage);
        if (m_isAppliedToOpponent)
        {
            opponent.powerModifier.SetModifier(m_unitType, percentage);
        }
        else
        {
            owner.powerModifier.SetModifier(m_unitType, percentage);
        }
    }
}
