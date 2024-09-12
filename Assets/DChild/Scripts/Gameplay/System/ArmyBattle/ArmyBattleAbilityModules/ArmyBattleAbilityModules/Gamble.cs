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
        var ownerCurrentValue = owner.troopCount;
        var opponentCurrentValue = opponent.troopCount;
        if (m_appliedEffectToOpponent)
        {

            if (number >= 0f && number <= convertedPercentage)
            {
                owner.AddTroopCount(opponentCurrentValue);
                Debug.Log("Win");
            }
            else
            {
                opponent.SubtractTroopCount(opponentCurrentValue);
                Debug.Log("Lose");
            }

        }
        else
        {
            if (number >= 0f && number <= convertedPercentage)
            {
                opponent.AddTroopCount(ownerCurrentValue);
                Debug.Log("Win");
            }
            else
            {
                owner.SubtractTroopCount(ownerCurrentValue);
                Debug.Log("Lose");
            }
        }
    }


}
