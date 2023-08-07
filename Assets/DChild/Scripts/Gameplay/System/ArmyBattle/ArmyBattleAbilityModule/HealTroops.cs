using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DChild.Gameplay.ArmyBattle;

public class HealTroops : IArmyAbilityEffect
{
    [SerializeField]
    private bool m_appliedEffectToOpponent;
    public void ApplyEffect(Army owner, Army opponent)
    {
        Heal(owner, opponent);

    }

    private void Heal(Army owner, Army opponent)
    {
        var ownerCurrentValue = owner.troopCount.currentValue;
        var opponentCurrentValue = opponent.troopCount.currentValue;

        if (m_appliedEffectToOpponent)
        {
            opponent.troopCount.AddCurrentValue(opponentCurrentValue);
        }
        else
        {
            owner.troopCount.AddCurrentValue(ownerCurrentValue);
        }
    }
}
