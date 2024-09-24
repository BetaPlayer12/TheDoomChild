using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.ArmyBattle;


public class Gamble : IArmyAbilityEffect
{
    [SerializeField]
    private float m_gamblePercentage;
    [SerializeField]
    private bool m_appliedEffectToOpponent;

    public void ApplyEffect(Army owner, Army opponent)
    {
        float number = Random.Range(0f, 1f);
        var convertedPercentage = PercentageConverter.ConvertPercentage(m_gamblePercentage);
        var ownerCurrentValue = owner.troopCount.currentValue;
        var opponentCurrentValue = opponent.troopCount.currentValue;
        if (m_appliedEffectToOpponent)
        {

            if (number >= 0f && number <= convertedPercentage)
            {
                owner.troopCount.ReduceCurrentValue(opponentCurrentValue);
                Debug.Log("Win");
            }
            else
            {
                opponent.troopCount.ReduceCurrentValue(opponentCurrentValue);
                Debug.Log("Lose");
            }

        }
        else
        {
            if (number >= 0f && number <= convertedPercentage)
            {
                opponent.troopCount.ReduceCurrentValue(ownerCurrentValue);
                Debug.Log("Win");
            }
            else
            {
                owner.troopCount.ReduceCurrentValue(ownerCurrentValue);
                Debug.Log("Lose");
            }
        }
    }


}
