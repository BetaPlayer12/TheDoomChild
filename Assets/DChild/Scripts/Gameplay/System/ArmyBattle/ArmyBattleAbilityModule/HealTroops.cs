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
        var ownerCurrentValue = owner.troopCount;
        var opponentCurrentValue = opponent.troopCount;

        if (m_appliedEffectToOpponent)
        {
            opponent.AddTroopCount(opponentCurrentValue);
        }
        else
        {
            owner.AddTroopCount(ownerCurrentValue);
        }
    }
}
