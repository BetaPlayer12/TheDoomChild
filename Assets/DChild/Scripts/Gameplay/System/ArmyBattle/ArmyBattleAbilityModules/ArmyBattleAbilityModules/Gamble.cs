using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.ArmyBattle;


public class Gamble : ISpecialSkillImplementor, ISpecialSkillModule
{
    [SerializeField]
    private float m_gamblePercentage;
    [SerializeField]
    private bool m_appliedEffectToOpponent;


    public void ApplyEffect(ArmyController owner, ArmyController target)
    {
        float number = Random.Range(0f, 1f);
        var convertedPercentage = PercentageConverter.ConvertPercentage(m_gamblePercentage);
        var ownerCurrentValue = owner.controlledArmy.troopCount;
        var opponentCurrentValue = target.controlledArmy.troopCount;
        if (m_appliedEffectToOpponent)
        {

            if (number >= 0f && number <= convertedPercentage)
            {
                owner.controlledArmy.AddTroopCount(opponentCurrentValue);
                Debug.Log("Win");
            }
            else
            {
                target.controlledArmy.SubtractTroopCount(opponentCurrentValue);
                Debug.Log("Lose");
            }

        }
        else
        {
            if (number >= 0f && number <= convertedPercentage)
            {
                target.controlledArmy.AddTroopCount(ownerCurrentValue);
                Debug.Log("Win");
            }
            else
            {
                owner.controlledArmy.SubtractTroopCount(ownerCurrentValue);
                Debug.Log("Lose");
            }
        }
    }

    public void RemoveEffect(ArmyController owner, ArmyController target)
    {
        throw new System.NotImplementedException();
    }
}
