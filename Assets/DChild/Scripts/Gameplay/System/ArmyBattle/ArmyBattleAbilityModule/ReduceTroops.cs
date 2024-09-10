using DChild.Gameplay.Combat;
using DChild.Gameplay.ArmyBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceTroops : IArmyAbilityEffect
{
    [SerializeField]
    private int m_troopDeduction;
    [SerializeField]
    private bool m_isAppliedToOpponent;

    public void ApplyEffect(Army owner, Army opponent)
    {
        float damage;
        float percentage = PercentageConverter.ConvertPercentage(m_troopDeduction);
        if (m_isAppliedToOpponent)
        {
            damage = opponent.troopCount.currentValue * percentage;
            opponent.troopCount.ReduceCurrentValue((int)damage);
        }
        else
        {
            damage = owner.troopCount.currentValue * percentage;
            owner.troopCount.ReduceCurrentValue((int)damage);
        }
        
    }

}
