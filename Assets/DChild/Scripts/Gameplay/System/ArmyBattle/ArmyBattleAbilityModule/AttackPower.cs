using UnityEngine;
using DChild.Gameplay.ArmyBattle;
using System.Collections;
using System.Collections.Generic;



[System.Serializable]
public class AttackPower : IArmyAbilityEffect
{

    [SerializeField]
    private float m_attackPowerPercentage;
    [SerializeField]
    private bool m_appliedEffectToOpponent;

    public void ApplyEffect(Army owner, Army opponent)
    {
        if (m_appliedEffectToOpponent)
        {
            SetAttackPower(opponent, m_attackPowerPercentage);
           
        }
        else
        {
            SetAttackPower(owner, m_attackPowerPercentage);
            
        }


    }

    private void SetAttackPower(Army army, float attackPercentage)
    {
        var convertedPercentage = PercentageConverter.ConvertPercentage(attackPercentage);
        Debug.Log(convertedPercentage);
        army.powerModifier.SetModifier(UnitType.Rock, convertedPercentage);
        army.powerModifier.SetModifier(UnitType.Paper, convertedPercentage);
        army.powerModifier.SetModifier(UnitType.Scissors, convertedPercentage);
    }

   

}
