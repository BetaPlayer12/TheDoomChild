using DChild.Gameplay.ArmyBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePower : IArmyAbilityEffect
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
            opponent.powerModifier.AddModifier(m_unitType, percentage);
        }
        else
        {
            owner.powerModifier.AddModifier(m_unitType, percentage);
        }
    }
}
