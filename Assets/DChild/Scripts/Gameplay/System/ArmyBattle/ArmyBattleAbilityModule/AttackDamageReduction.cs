using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.ArmyBattle;
using System;

public class AttackDamageReduction : IArmyAbilityEffect
{
    [SerializeField]
    private float m_PowerReductionPercentage;
    [SerializeField]
    private bool m_appliedEffectToOpponent;
    [SerializeField]
    private UnitType m_unitType;
    public void ApplyEffect(Army owner, Army opponent)
    {
        var convertedPercentage = PercentageConverter.ConvertPercentage(m_PowerReductionPercentage);
        if (m_appliedEffectToOpponent)
        {
            //opponent.damageReductionModifier.SetModifier(m_unitType, convertedPercentage);
        }
        else
        {
            //owner.damageReductionModifier.SetModifier(m_unitType, convertedPercentage);
        }
        throw new NotImplementedException();
    }


}
